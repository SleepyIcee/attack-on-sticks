using System.Numerics;
using System.Runtime.CompilerServices;
using AntsShooter.Entities.UI;
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
        private Tilemap tilemap = new Tilemap();
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

        private Sound gunSound = ResourceManager.GetSound("gun_sound", "assets/sounds/gun-sound.wav");

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
                ant.Update();
                ant.Follow(player);

                if (ant.IsDying)
                {
                    continue;
                }
                
                if (player.GetDamage(ant))
                {
                    // turn on ant attack animation
                }

                ant.LifeBar.Position.X = ant.Position.X;
                ant.LifeBar.Position.Y = ant.Position.Y - 20;
            }

            for (int i = 0; i < ants.Count(); i++)
            {
                ResolveAntCollisions();
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

                    if (a.IsDying || a.IsDead || b.IsDying || b.IsDead)
                        continue;

                    if (!Raylib.CheckCollisionRecs(new Rectangle(a.Position, new Vector2(a.Width, a.Height)),
                        new Rectangle(b.Position, new Vector2(b.Width, b.Height))))
                        continue;

                    Vector2 delta = a.Position - b.Position;

                    if (delta.Length() < 0.001f)
                        delta = Vector2.UnitX;
                    else
                        delta /= delta.Length();

                    a.Position += delta * antPushForce * Raylib.GetFrameTime(); ;
                    b.Position -= delta * antPushForce * Raylib.GetFrameTime(); ;
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
            Globals.MouseWorldPos = Raylib.GetScreenToWorld2D(Globals.MousePosition, camera.camera);
            lastMouseWorldPos = Globals.MouseWorldPos;

            // don't allow firing if inputs are locked right after unpausing
            if (Globals.InputLock <= 0f && Raylib.IsMouseButtonDown(MouseButton.Left) && ammo.ReloadGun == false && bulletTimer <= 0 && ammo.LoadedAmmo > 0)
            {
                bullets.Add(new Bullet(
                    new Vector2(player.Position.X + player.Width / 2,
                                player.Position.Y + player.Height / 2),
                    Globals.MouseWorldPos));

                bulletTimer = timeBetweenBullets;
                ammo.LoadedAmmo -= 1;

                player.Gun.ShootingAnimationPlay = true;
                Raylib.PlaySound(gunSound);

                // start shaking the camera
                List<Vector2> cameraShakePoints = new();
                for (int i = 0; i < 3; i++)
                {
                    cameraShakePoints.Add(new Vector2(random.Next(-3, 3), random.Next(-3, 3)));
                }
                camera.Shake(cameraShakePoints);
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
                    if (ants[j].IsDead)
                    {
                        killsScore.Kills++;
                        Globals.Score = killsScore.Kills;
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
                string[] pickableTypes = { "ammo", "health"};
                string pickableType = pickableTypes[random.Next(pickableTypes.Length)];
                if (random.Next(5) == 1)
                {
                    pickableType = "nuke";
                }
                Pickable pickable = new Pickable(pickableType);
                // Console.WriteLine(pickable.Type);
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
                    if (pickables[i].Type == "ammo")
                    {
                        ammo.HowMuchAmmo += 10;
                        pickables.Remove(pickables[i]);
                        break;
                    }
                    else if (pickables[i].Type == "health" && player.Health < player.MaxHealth)
                    {
                        player.Health += 1;
                        pickables.Remove(pickables[i]);
                        break;
                    }
                    else if (pickables[i].Type == "nuke")
                    {
                        pickables.Remove(pickables[i]);
                        foreach (var ant in ants)
                        {
                            ant.Health = 0;
                            ant.IsDying = true;
                            ant.LifeBar.LifeBarHealthWidth = 0;
                        }
                        killsScore.Kills += ants.Count;
                        Globals.Score = killsScore.Kills;
                        break;
                    }
                }

                if (pickables[i].RemoveTimer < 0)
                {
                    pickables[i].RemoveTimer = pickables[i].TimeToRemove;
                    pickables.RemoveAt(i);
                }
                else
                {
                    pickables[i].RemoveTimer -= Raylib.GetFrameTime();
                }
            }
        }

        private void UpdateDifficulty()
        {
            int step = killsScore.Kills / killsPerDifficultyStep;

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
            Globals.backBackgroundScrolling = camera.camera.Target.X * 0.2f;
            // update UI elements
            ammo.Update();
        }

        public void Draw()
        {
            Raylib.BeginMode2D(camera.camera);

            Raylib.DrawTexture(Globals.backBackgroundTexture, (int)MathF.Round(Globals.backBackgroundScrolling - Globals.MAP_WIDTH/5), -Globals.SCREEN_HEIGHT/4, Color.White);
            Raylib.DrawTexture(Globals.frontBackgroundTexture, 0, -Globals.VECTUAL_SCREEN_HEIGHT/2, Color.White);

            // Raylib.DrawCircleV(lastMouseWorldPos, 3, Color.Red);
            // Vector2 playerCenter = new Vector2(player.Position.X + player.Width / 2, player.Position.Y + player.Height / 2);
            // Raylib.DrawLineV(playerCenter, lastMouseWorldPos, Color.Red);

            DrawBullets();
            player.Draw();
            DrawAnts();
            DrawPickables();

            foreach (var ant in ants)
            {
                ant.LifeBar.Draw();
            }

            // Raylib.DrawRectangle(0 - Globals.MAP_WIDTH / 2,
            // (int)MathF.Round(Globals.OriginPlayerPos.Y + player.Height),
            // Globals.MAP_WIDTH + Globals.MAP_WIDTH,
            // Globals.VECTUAL_SCREEN_HEIGHT - Globals.GROUND_LEVEL,
            // new Color(46, 34, 47));
            tilemap.Draw();

            Raylib.EndMode2D();

            // draw UI elements
            player.LifeBar.Draw();
            player.DashBar.Draw();
            ammo.Draw();
            killsScore.Draw();
        }
    }
}
