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
        private int playerBullets = 10;

        private List<Pickable> pickables = new();
        private float timeToSpawnBullet;
        private Random random = new Random();
        
        public PlayState()
        {
            player = new Player();
            camera = new Camera(player);
            
            ants = new List<Ant>();
            
            testBlockPosition = new Vector2(0, 0);

            timeToSpawnBullet = 0.1f;
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

                ant.lifeBar.position.X = ant.position.X;
                ant.lifeBar.position.Y = ant.position.Y - 20;
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
            if (Raylib.IsMouseButtonDown(MouseButton.Left) && bulletTimer <= 0 && playerBullets > 0)
            {
                Vector2 mouseScreenPos = Raylib.GetMousePosition();
                Vector2 mouseWorldPos = Raylib.GetScreenToWorld2D(mouseScreenPos, camera.camera);

                bullets.Add(new Bullet(
                    new Vector2(player.position.X + player.width / 2,
                                player.position.Y + player.height / 2),
                    mouseWorldPos));

                bulletTimer = timeBetweenBullets;
                playerBullets -= 1;

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

            float centerX = player.position.X;
            float centerY = player.position.Y;

            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update();

                if (bullets[i].position.X > centerX + bulletRange ||
                    bullets[i].position.X < centerX - bulletRange ||
                    bullets[i].position.Y > centerY + bulletRange ||
                    bullets[i].position.Y < centerY - bulletRange)
                {
                    bullets.RemoveAt(i);
                    continue;
                }

                for (int j = ants.Count - 1; j >= 0; j--)
                {
                    if (ants[j].isDead)
                    {
                        ants.RemoveAt(j);
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
            if (timeToSpawnBullet <= 0)
            {
                string[] pickableTypes = {"bullet", "health"};
                Pickable pickable = new Pickable(pickableTypes[random.Next(pickableTypes.Length)]);
                // Console.WriteLine(pickable.type);
                pickables.Add(pickable);
                timeToSpawnBullet = random.Next(3, 7);
            }
            else
            {
                timeToSpawnBullet -= 1 * Raylib.GetFrameTime();
            }
        }

        void UpdatePickables()
        {
            for (int i = pickables.Count - 1; i >= 0; i--)
            {
                pickables[i].Update();
                
                if (pickables[i].IsPickedByPlayer(player))
                {
                    if (pickables[i].type == "bullet")
                    {
                        playerBullets += 10;
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
        }

        public void Draw()
        {
            Raylib.BeginMode2D(camera.camera);
            player.Draw();
            DrawAnts();
            DrawBullets();
            DrawBulletsToPick();
            Raylib.DrawRectangle((int)MathF.Round(testBlockPosition.X), (int)MathF.Round(testBlockPosition.Y), 50, 50, Color.Blue);
            // draw UI elements
            player.lifeBar.Draw();
            foreach (var ant in ants)
            {
                ant.lifeBar.Draw();
            }
            Raylib.EndMode2D();
        }
    }
}