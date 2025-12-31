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
        private Camera2D camera;
        private Vector2 cameraTarget;
        private Vector2 testBlockPosition;
        private List<Ant> ants;
        private float spawnAntTimer = Globals.SpawnAntTimer;
        private List<Bullet> bullets = new();
        private const float timeBetweenBullets = 0.1f;
        private float bulletTimer = timeBetweenBullets;
        private const int bulletRange = 1000;
        private int playerBullets = 10;

        private List<BulletToPick> bulletsToPick = new();
        private float timeToSpawnBullet;
        private Random random = new Random();
        
        public PlayState()
        {
            player = new Player();
            LoadCamera();
            
            ants = new List<Ant>();
            
            testBlockPosition = new Vector2(0, 0);

            timeToSpawnBullet = 0.1f;
        }
        
        private void LoadCamera()
        {
            cameraTarget = new Vector2();
            
            camera = new Camera2D();
            camera.Target = cameraTarget;
            camera.Offset = new Vector2(Globals.SCREEN_WIDTH/2 - player.width/2, Globals.SCREEN_HEIGHT/2 - player.height/2);
            camera.Rotation = 0.0f;
            camera.Zoom = 1.0f;
        }
        
        private void UpdateCamera() 
        {
            cameraTarget.X = player.position.X;
            cameraTarget.Y = Globals.OriginPlayerPos.Y;
            
            float leftBound = camera.Target.X - camera.Offset.X;
            float rightBound = camera.Target.X + camera.Offset.X;

            if (leftBound < 0) camera.Target.X = camera.Offset.X;
            if (rightBound > Globals.MAP_WIDTH) camera.Target.X = Globals.MAP_WIDTH - camera.Offset.X;
            
            camera.Target = Vector2.Lerp(camera.Target, cameraTarget, 0.1f);

            // update UI position with camera
            player.lifeBar.UpdateScreenPosition(camera,
            new Vector2(Globals.SCREEN_WIDTH/20, Globals.SCREEN_HEIGHT/10));
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
                Vector2 mouseWorldPos = Raylib.GetScreenToWorld2D(mouseScreenPos, camera);

                bullets.Add(new Bullet(
                    new Vector2(player.position.X + player.width / 2,
                                player.position.Y + player.height / 2),
                    mouseWorldPos));

                bulletTimer = timeBetweenBullets;
                playerBullets -= 1;
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

        void SpawnBulletsToPick()
        {
            if (timeToSpawnBullet <= 0)
            {
                BulletToPick bulletToPick = new BulletToPick();
                bulletsToPick.Add(bulletToPick);
                timeToSpawnBullet = random.Next(3, 7);
            }
            else
            {
                timeToSpawnBullet -= 1 * Raylib.GetFrameTime();
            }
        }

        void UpdateBulletsToPick()
        {
            for (int i = bulletsToPick.Count - 1; i >= 0; i--)
            {
                bulletsToPick[i].Update();
                
                if (bulletsToPick[i].IsPickedByPlayer(player))
                {
                    playerBullets += 10;
                    bulletsToPick.Remove(bulletsToPick[i]);
                }
            }
        }

        void DrawBulletsToPick()
        {
            foreach (var bullet in bulletsToPick)
            {
                bullet.Draw();
            }
        }

        public void Update()
        {
            player.Update();
            HandleShooting();
            UpdateCamera();
            UpdateAnts();
            SpawnBulletsToPick();
            UpdateBulletsToPick();
        }

        public void Draw()
        {
            Raylib.BeginMode2D(camera);
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