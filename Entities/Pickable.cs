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

    public Pickable(string pickableType) : base()
    {
        width = 16;
        height = 16;

        position = Globals.BulletsSpawnPositions[random.Next(Globals.BulletsSpawnPositions.Count)];
        velocity = Vector2.Zero;

        type = pickableType;
        if (type == "bullet")
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

        if (position.Y >= Globals.GROUND_LEVEL - height)
        {
            position.Y = Globals.GROUND_LEVEL - height;
            velocity.Y = 0;
        }
        else 
        {
            position += velocity * Raylib.GetFrameTime();
        }
    }

    public bool IsPickedByPlayer(Player player)
    {
        if (Raylib.CheckCollisionCircleRec(position, radius, new Rectangle(player.position, new Vector2(player.width, player.height))))
        {
            return true;
        }
            return false;
    }

    public override void Draw()
    {
        Raylib.DrawCircle((int)Math.Round(position.X), (int)Math.Round(position.Y), radius, color);
    }
}