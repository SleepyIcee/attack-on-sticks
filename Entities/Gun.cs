using System.Numerics;
using AntsShooter.Entities;
using Raylib_cs;


namespace AntsShooter.Enities
{
    public class Gun : Entity
    {
        Texture2D texture = Raylib.LoadTexture("assets/gun/idle/gun.png");
        private Vector2 origin = Vector2.Zero;
        private Vector2 direction = Vector2.Zero;
        private float angle = 0;
        private int facing = 1;

        public Gun() : base()
        {
            
        }

        public void LookAtMouse(Vector2 playerPosition, Vector2 mousePosition)
        {
            Position = playerPosition + new Vector2(Width/2, Height/2);
            origin = new Vector2((float)Width/2, (float)Height/2);
            Vector2 gunCenter = Position + origin;
            direction = Vector2.Normalize(mousePosition - gunCenter);
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
        }

        public override void Draw()
        {
            base.Draw();
            Raylib.DrawTexturePro(texture, new Rectangle(0f, 0f, new Vector2(texture.Width, texture.Height * facing)),
            new Rectangle(Position.X, Position.Y, new Vector2(Width, Height)), origin, angle, Color.White);
        }
    }
}