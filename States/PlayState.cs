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
        private float spawnAntTimer = Globals.SpawnAntTimer;
        private List<Bullet> bullets = new();
        private const float timeBetweenBullets = 0.1f;
        private float bulletTimer = timeBetweenBullets;
        private const int bulletRange = 1000;
        private Ammo ammo;
        private KillsScore killsScore;
        private int killsNumberToMakeTheGameHarder = 3;

        private List<Pickable> pickables = new();
        private float timeToSpawnPickable;
        private Random random = new Random();

        // sounds
        private Sound gunSound = Raylib.LoadSound("assets/sounds/gun-sound.wav");
        
        public PlayState()
        {
            player = new Player();
            camera = new Camera(player);
            
            ants = new List<Ant>();
            
            testBlockPosition = new Vector2(0, 0);

            timeToSpawnPickable = 0.1f;

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

                ant.lifeBar.Position.X = ant.Position.X;
                ant.lifeBar.Position.Y = ant.Position.Y - 20;
            }
        }

        private void DrawAnts()
        {
            foreach (var ant in ants)
            {
                ant.Draw();
            }
        }

        public void HandleShooting()
        {
            if (Raylib.IsMouseButtonDown(MouseButton.Left) && bulletTimer <= 0 && ammo.ammo > 0)
            {
                Vector2 mouseScreenPos = Raylib.GetMousePosition();
                Vector2 mouseWorldPos = Raylib.GetScreenToWorld2D(mouseScreenPos, camera.camera);

                bullets.Add(new Bullet(
                    new Vector2(player.Position.X + player.Width / 2,
                                player.Position.Y + player.Height / 2),
                    mouseWorldPos));

                bulletTimer = timeBetweenBullets;
                ammo.ammo -= 1;

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
                    continue;
                }

                for (int j = ants.Count - 1; j >= 0; j--)
                {
                    if (ants[j].isDead)
                    {
                        ants.RemoveAt(j);
                        killsScore.kills++;

                        // make the game harder
                        if (killsScore.kills%killsNumberToMakeTheGameHarder == 0 && Globals.SpawnAntTimer > 1)
                        {
                            Globals.SpawnAntTimer -= 0.5f;
                            killsNumberToMakeTheGameHarder+=1;
                        }
                        break;
                    }
                    // Console.WriteLine(ants[j].isDead);
                    if (ants[j].GetShot(bullets[i]))
                    {
                        bullets.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public void DrawBullets()
        {
            foreach (var bullet in bullets)
            {
                bullet.Draw();
            }
        }

        void SpawnPickables()
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

        void UpdatePickables()
        {
            for (int i = pickables.Count - 1; i >= 0; i--)
            {
                pickables[i].Update();
                
                if (pickables[i].IsPickedByPlayer(player))
                {
                    if (pickables[i].type == "ammo")
                    {
                        ammo.ammo += 10;
                    }
                    else if (pickables[i].type == "health")
                    {
                        if (player.health < player.maxHealth)
                        {
                            player.health += 1;
                        }
                    }
                    pickables.Remove(pickables[i]);
                }
            }
        }

        void DrawBulletsToPick()
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
            // update UI position with camera
            player.lifeBar.UpdateScreenPosition(camera);
            ammo.UpdateScreenPosition(camera);
        }

        public void Draw()
        {
            Raylib.BeginMode2D(camera.camera);
            player.Draw();
            DrawAnts();
            DrawBullets();
            DrawBulletsToPick();
            // Raylib.DrawRectangle((int)MathF.Round(testBlockPosition.X), (int)MathF.Round(testBlockPosition.Y), 50, 50, Color.Blue);

            // draw UI elements
            foreach (var ant in ants)
            {
                ant.lifeBar.Draw();
            }
            player.lifeBar.Draw();
            ammo.Draw();

            Raylib.EndMode2D();
        }
    }
}