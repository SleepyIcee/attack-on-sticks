using System;
using Raylib_cs;
using AntsShooter.Entities;
using AntsShooter.Systems;
using System.Numerics;

namespace AntsShooter.Entities;

public class Pickable : Entity
{
    private Vector2 velocity;
    public float Radius = 5.0f;
    Random random = new Random();
    public string Type = "";
    private Color color;

    public float TimeToRemove;
    public float RemoveTimer;

    public Pickable(string pickableType) : base()
    {
        Width = 16;
        Height = 16;

        Position.X = random.Next(0, 1600);
        Position.Y = -30;
        velocity = Vector2.Zero;

        TimeToRemove = 10.0f;
        RemoveTimer = TimeToRemove;

        Type = pickableType;
        if (Type == "ammo")
        {
            color = Color.Gold;
        }
        else if (Type == "health")
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
        if (Raylib.CheckCollisionCircleRec(Position, Radius, new Rectangle(player.Position, new Vector2(player.Width, player.Height))))
        {
            return true;
        }
            return false;
    }

    public override void Draw()
    {
        Raylib.DrawCircle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), Radius, color);
    }
}
