using System;
using Raylib_cs;
using AntsShooter.Entities;
using AntsShooter.Systems;
using System.Numerics;


public class Pickable : Entity
{
    private Vector2 velocity;
    public float radius = 10.0f;
    Random random = new Random();
    public string type = "";
    private Color color;

    public float timeToRemove;
    public float removeTimer;

    public Pickable(string pickableType) : base()
    {
        Width = 16;
        Height = 16;

        Position.X = random.Next(0, 1600);
        Position.Y = -30; 
        velocity = Vector2.Zero;

        timeToRemove = 10.0f;
        removeTimer = timeToRemove;

        type = pickableType;
        if (type == "ammo")
        {
            color = Color.Gold;
        }
        else if (type == "health")
        {
            color = new Color(255, 0, 0, 255);
        }
    }

    public override void Update()
    {
        base.Update();
        ApplyGravity();
    }

    public void ApplyGravity()
    {
        velocity.Y += Globals.GRAVITY * Raylib.GetFrameTime();

        if (Position.Y >= Globals.GROUND_LEVEL - Height)
        {
            Position.Y = Globals.GROUND_LEVEL - Height;
            velocity.Y = 0;
        }
        else 
        {
            Position += velocity * Raylib.GetFrameTime();
        }
    }

    public bool IsPickedByPlayer(Player player)
    {
        if (Raylib.CheckCollisionCircleRec(Position, radius, new Rectangle(player.Position, new Vector2(player.Width, player.Height))))
        {
            return true;
        }
            return false;
    }

    public override void Draw()
    {
        Raylib.DrawCircle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), radius, color);
    }
}