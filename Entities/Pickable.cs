using System;
using Raylib_cs;
using AntsShooter.Entities;
using AntsShooter.Systems;
using System.Numerics;

namespace AntsShooter.Entities;

public class Pickable : Entity
{
    private Vector2 velocity;
    Random random = new Random();
    public string Type = "";
    private Color color;

    public float TimeToRemove;
    public float RemoveTimer;

    private Texture2D texture = new Texture2D();
    private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>
    {
        {"ammo", Raylib.LoadTexture("assets/pickables/ammo.png")},
        {"health", Raylib.LoadTexture("assets/pickables/health.png")},
        {"nuke", Raylib.LoadTexture("assets/pickables/nuke.png")}
    };

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
            texture = textures["ammo"];
        }
        else if (Type == "health")
        {
            texture = textures["health"];
        }
        else if (Type == "nuke")
        {
            texture = textures["nuke"];
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

        if (Position.Y >= Globals.GROUND_LEVEL - Height - Height/2)
        {
            Position.Y = Globals.GROUND_LEVEL - Height - Height/2;
            velocity.Y = 0;
        }
        else
        {
            Position += velocity * Raylib.GetFrameTime();
        }
    }

    public bool IsPickedByPlayer(Player player)
    {
        if (Raylib.CheckCollisionRecs(new Rectangle(Position.X, Position.Y, Width, Height),
        new Rectangle(player.Position.X, player.Position.Y, player.Width, player.Height)))
        {
            return true;
        }
        else
        {
            return false;   
        }
    }

    public override void Draw()
    {
        Raylib.DrawTexture(texture, (int)Position.X, (int)Position.Y, Color.White);
    }
}
