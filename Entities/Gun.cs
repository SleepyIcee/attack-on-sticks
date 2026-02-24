using System.Numerics;
using AntsShooter.Entities;
using AntsShooter.Systems;
using Raylib_cs;

namespace AntsShooter.Entities
{
    public class Gun : Entity
    {
        private Texture2D texture;
        private Dictionary<string, Animation> animations = new Dictionary<string, Animation>
        {
            {"idle", ResourceManager.GetAnimation("gun_idle", "assets/gun/idle")},
            {"shoot", ResourceManager.GetAnimation("gun_shoot", "assets/gun/shoot")}
        };
        private Vector2 origin = Vector2.Zero;
        private Vector2 direction = Vector2.Zero;
        private float angle = 0;
        private int facing = 1;
        public bool shooting = false;
        private Sound gunSound;

        public Gun() : base()
        {
            texture = animations["idle"].Play(0);
            // gunSound = ResourceManager.GetSound("gun_sound", "assets/sounds/gun-sound.wav");
        }

        public void LookAtMouse(Vector2 playerPosition, Vector2 mousePosition)
        {
            Position = playerPosition + new Vector2(Width/2, Height/2);
            origin = new Vector2((float)Width/2, (float)Height/2);
            Vector2 gunCenter = Position + origin;
            direction = Vector2.Normalize(mousePosition - Position);
            angle = MathF.Atan2(direction.Y, direction.X) * 180f / MathF.PI;

            if (mousePosition.X < Position.X)
            {
                facing = -1;
            }
            else
            {
                facing = 1;
            }
        }

        public override void Update()
        {
            base.Update();

            if (shooting)
            {
                texture = animations["shoot"].Play(1);
            }
            else
            {
                texture = animations["idle"].Play(1);
            }
        }

        public override void Draw()
        {
            base.Draw();
            Raylib.DrawTexturePro(texture, new Rectangle(0f, 0f, texture.Width, texture.Height * facing),
            new Rectangle(Position.X, Position.Y, Width, Height), origin, angle, Color.White);
        }
    }
}
