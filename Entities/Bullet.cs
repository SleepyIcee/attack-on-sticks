using System.Numerics;
using System.Reflection.Emit;
using Raylib_cs;
using System.Runtime.InteropServices.Marshalling;


namespace AntsShooter.Entities;

public class Bullet : Entity
{
    private Vector2 direction;
    private Vector2 velocity;
    private float speed = 2000.0f;
    private float maxSpeed = 2000.0f;
    public Rectangle rectangle;

    public Bullet(Vector2 pos, Vector2 target) : base()
    {
        Position = pos;
        direction = Vector2.Normalize(target - Position);
        Width = 20;
        Height = 10;
        rectangle = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), Width, Height);
    }

    public override void Update()
    {
        base.Update();

        velocity = direction * maxSpeed;
        velocity += -direction * speed * Raylib.GetFrameTime();
        Position += velocity * Raylib.GetFrameTime();
        rectangle.X = (int)Math.Round(Position.X);
        rectangle.Y = (int)Math.Round(Position.Y);
        // Console.WriteLine(direction);
    }

    public override void Draw()
    {
        base.Draw();
        // Raylib.DrawCircle((int)Math.Round(position.X), (int)Math.Round(position.Y), radius, Color.Gold);
        float angle = (float)(Math.Atan2(direction.Y, direction.X) * 180 / Math.PI);
        Rectangle bulletAura = new Rectangle(rectangle.X - rectangle.Width/4, rectangle.Y - rectangle.Height/4, rectangle.Width*1.5f, rectangle.Height*1.5f);
        Raylib.DrawRectanglePro(bulletAura, new Vector2(bulletAura.Width/2, bulletAura.Height/2), angle, Color.Yellow);
        Raylib.DrawRectanglePro(rectangle, new Vector2(rectangle.Width/2, rectangle.Height/2), angle, Color.Gold);
    }
}