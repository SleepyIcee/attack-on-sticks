using System.Numerics;
using System.Reflection.Emit;
using Raylib_cs;


namespace AntsShooter.Entities;

public class Bullet : Entity
{
    private Vector2 direction;
    private Vector2 velocity;
    private float speed = 2000.0f;
    private float maxSpeed = 2000.0f;
    public float radius = 10.0f;

    public Bullet(Vector2 pos, Vector2 target) : base()
    {
        position = pos;
        direction = Vector2.Normalize(target - position);
    }

    public override void Update()
    {
        base.Update();

        velocity = direction * maxSpeed;
        velocity += -direction * speed * Raylib.GetFrameTime();
        position += velocity * Raylib.GetFrameTime();
        // Console.WriteLine(direction);
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawCircle((int)Math.Round(position.X), (int)Math.Round(position.Y), radius, Color.Gold);
    }
}