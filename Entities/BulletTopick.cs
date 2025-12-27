using System;
using Raylib_cs;
using AntsShooter.Entities;
using AntsShooter.Systems;
using System.Numerics;


public class BulletToPick : Entity
{
    private Vector2 velocity;
    public float radius = 10.0f;
    Random random = new Random();

    public BulletToPick() : base()
    {
        width = 16;
        height = 16;

        position = Globals.BulletsSpawnPositions[random.Next(Globals.BulletsSpawnPositions.Count)];
        velocity = Vector2.Zero;
    }

    public override void Update()
    {
        base.Update();
        ApplyGravity();
    }

    public void ApplyGravity()
    {
        velocity.Y += Globals.gravity * Raylib.GetFrameTime();

        if (position.Y >= Globals.originPlayerPos.Y)
        {
            position.Y = Globals.originPlayerPos.Y;
            velocity.Y = 0;
        }

        position += velocity * Raylib.GetFrameTime();
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
        Raylib.DrawCircle((int)Math.Round(position.X), (int)Math.Round(position.Y), radius, Color.Gold);
    }
}