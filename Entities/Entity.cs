using System.Numerics;
using Raylib_cs;

namespace AttackOnSticks.Entities;

public class Entity
{
    public Vector2 Position;
    public Texture2D Texture;
    public int Height { get; set; } = 50;
    public int Width { get; set; } = 50;

    public Entity()
    {
        Position = new Vector2(0, 0);
        Texture = new Texture2D();
    }

    public virtual void Update()
    {

    }

    public virtual void Draw()
    {
        
    }
}