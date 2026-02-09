using System.Numerics;
using System.Runtime.CompilerServices;
using AntsShooter.Editor;
using AntsShooter.Enities.UI;
using AntsShooter.Entities;
using AntsShooter.Systems;
using Raylib_cs;

namespace AntsShooter.States
{
    public class PlayState : IState
    {
        private Player player;
        private Camera camera;
        private Vector2 testBlockPosition;
        private List<Ant> ants;
        private readonly int antPushForce = 30;
        private float spawnAntTimer = Globals.SpawnAntTimer;
        private List<Bullet> bullets = new();
        private const float timeBetweenBullets = 0.1f;
        private float bulletTimer = timeBetweenBullets;
        private const int bulletRange = 1000;
        private Ammo ammo;
        private KillsScore killsScore;
        private Vector2 lastMouseWorldPos = Vector2.Zero;
        private int killsNumberToMakeTheGameHarder = 3;

        private List<Pickable> pickables = new();
        private float timeToSpawnPickable = 0.1f;
        private Random random = new Random();

        private const int killsPerDifficultyStep = 10;
        private const float spawnReductionPerStep = 0.5f;
        private const float minSpawnTime = 1.0f;
        private int lastDifficultyStep = 0;

        // sounds
        private Sound gunSound = Raylib.LoadSound("assets/sounds/gun-sound.wav");
        
        public PlayState()
        {
            player = new Player();
            camera = new Camera(player);
            
            ants = new List<Ant>();
            
            testBlockPosition = new Vector2(0, 0);

            ammo = new Ammo();
            killsScore = new KillsScore();
        }

        private void SpawnAnt()
        {
            Ant ant = new Ant();
            ants.Add(ant);
        }

        private void UpdateAnts()
        {
            if (spawnAntTimer <= 0)
            {
                SpawnAnt();
                spawnAntTimer = Globals.SpawnAntTimer;
            }
            else
            {
                spawnAntTimer -= 1 * Raylib.GetFrameTime();
            }

            foreach (var ant in ants)
            {
                ant.Follow(player);
                ant.Update();

                if (player.GetDamage(ant))
                {
                    // turn on ant attack animation
                }

                for (int i = 0; i < ants.Count(); i++)
                {
                    ResolveAntCollisions();
                }

                ant.lifeBar.Position.X = ant.Position.X;
                ant.lifeBar.Position.Y = ant.Position.Y - 20;
            }
        }

        private void ResolveAntCollisions()
        {
            for (int i = 0; i < ants.Count; i++)
            {
                for (int j = i + 1; j < ants.Count; j++)
                {
                    Ant a = ants[i];
                    Ant b = ants[j];

                    if (!Raylib.CheckCollisionRecs(new Rectangle(a.Position, new Vector2(a.Width, a.Height)),
                        new Rectangle(b.Position, new Vector2(b.Width, b.Height))))
                        continue;

                    Vector2 delta = a.Position - b.Position;

                    if (delta.Length() < 0.001f)
                        delta = Vector2.UnitX;
                    else
                        delta /= delta.Length();

                    a.Position += delta * antPushForce * Raylib.GetFrameTime();;
                    b.Position -= delta * antPushForce * Raylib.GetFrameTime();;
                }
            }
        }

        private void DrawAnts()
        {
            foreach (var ant in ants)
            {
                ant.Draw();
            }
        }

        private void HandleShooting()
        {
            Vector2 mouseWorldPos = Raylib.GetScreenToWorld2D(Globals.mousePosition, camera.camera);
            lastMouseWorldPos = mouseWorldPos;

            if (Raylib.IsMouseButtonDown(MouseButton.Left) && ammo.reloadGun == false && bulletTimer <= 0 && ammo.loadedAmmo > 0)
            {
                bullets.Add(new Bullet(
                    new Vector2(player.Position.X + player.Width / 2,
                                player.Position.Y + player.Height / 2),
                    mouseWorldPos));

                bulletTimer = timeBetweenBullets;
                ammo.loadedAmmo -= 1;

                // start shaking the camera
                List<Vector2> cameraShakePoints = new();
                for (int i = 0; i < 3; i++)
                {
                    cameraShakePoints.Add(new Vector2(random.Next(-3, 3), random.Next(-3, 3)));
                }
                camera.Shake(cameraShakePoints);
                Raylib.PlaySound(gunSound);
            }

            // Console.Write(Raylib.GetMousePosition() + " ... ");
            // Console.WriteLine(player.position);

            if (bulletTimer > 0)
            {
                bulletTimer -= Raylib.GetFrameTime();
            }

            float centerX = player.Position.X;
            float centerY = player.Position.Y;

            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update();

                if (bullets[i].Position.X > centerX + bulletRange ||
                    bullets[i].Position.X < centerX - bulletRange ||
                    bullets[i].Position.Y > centerY + bulletRange ||
                    bullets[i].Position.Y < centerY - bulletRange)
                {
                    bullets.RemoveAt(i);
                    break;
                }

                for (int j = ants.Count - 1; j >= 0; j--)
                {
                    if (ants[j].isDead)
                    {
                        killsScore.kills++;
                        ants.RemoveAt(j);
                        break;
                    }
                    
                    if (ants[j].GetShot(bullets[i]))
                    {
                        bullets.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private void DrawBullets()
        {
            foreach (var bullet in bullets)
            {
                bullet.Draw();
            }
        }

        private void SpawnPickables()
        {
            if (timeToSpawnPickable <= 0)
            {
                string[] pickableTypes = {"ammo", "health"};
                Pickable pickable = new Pickable(pickableTypes[random.Next(pickableTypes.Length)]);
                // Console.WriteLine(pickable.type);
                pickables.Add(pickable);
                timeToSpawnPickable = random.Next(3, 7);
            }
            else
            {
                timeToSpawnPickable -= 1 * Raylib.GetFrameTime();
            }
        }

        private void UpdatePickables()
        {
            for (int i = pickables.Count - 1; i >= 0; i--)
            {
                pickables[i].Update();

                if (pickables[i].IsPickedByPlayer(player))
                {
                    if (pickables[i].type == "ammo")
                    {
                        ammo.ammo += 10;
                        pickables.Remove(pickables[i]);
                        break;
                    }
                    else if (pickables[i].type == "health" && player.health < player.maxHealth)
                    {
                        player.health += 1;
                        pickables.Remove(pickables[i]);
                        break;
                    }
                }

                if (pickables[i].removeTimer < 0)
                {
                    pickables[i].removeTimer = pickables[i].timeToRemove;
                    pickables.RemoveAt(i);
                }
                else
                {
                    pickables[i].removeTimer -= Raylib.GetFrameTime();
                }
            }
        }

        private void UpdateDifficulty()
        {
            int step = killsScore.kills / killsPerDifficultyStep;

            if (step <= lastDifficultyStep)
                return;

            lastDifficultyStep = step;

            Globals.SpawnAntTimer = MathF.Max(
                Globals.SpawnAntTimer - spawnReductionPerStep,
                minSpawnTime
            );
        }

        private void DrawPickables()
        {
            foreach (var pickable in pickables)
            {
                pickable.Draw();
            }
        }

        public void Update()
        {
            player.Update();
            camera.Update();
            HandleShooting();
            UpdateAnts();
            SpawnPickables();
            UpdatePickables();
            UpdateDifficulty();
            // update UI position with camera
            player.lifeBar.UpdateScreenPosition(camera);
            ammo.UpdateScreenPosition(camera);
            ammo.Update();
        }

        public void Draw()
        {
            Raylib.BeginMode2D(camera.camera);
            player.Draw();
            DrawAnts();
            DrawBullets();
            DrawPickables();

            Raylib.DrawCircleV(lastMouseWorldPos, 3, Color.Red);
            Vector2 playerCenter = new Vector2(player.Position.X + player.Width/2, player.Position.Y + player.Height/2);
            Raylib.DrawLineV(playerCenter, lastMouseWorldPos, Color.Red);

            // draw UI elements
            foreach (var ant in ants)
            {
                ant.lifeBar.Draw();
            }
            player.lifeBar.Draw();
            ammo.Draw();

            Raylib.DrawRectangle(0 - Globals.MAP_WIDTH/2, (int)MathF.Round(Globals.OriginPlayerPos.Y + player.Height), Globals.MAP_WIDTH + Globals.MAP_WIDTH, Globals.VECTUAL_SCREEN_HEIGHT - Globals.GROUND_LEVEL, Color.Yellow);

            Raylib.EndMode2D();
        }
    }
}