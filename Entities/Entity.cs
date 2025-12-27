using System.Numerics;
using Raylib_cs;

namespace AntsShooter.Entities;

public class Entity
{
    public Vector2 position;
    public Texture2D texture;
    public int height { get; set; } = 50;
    public int width { get; set; } = 50;

    public Entity()
    {
        position = new Vector2(0, 0);
        texture = new Texture2D();
    }

    public virtual void Update()
    {

    }

    public virtual void Draw()
    {
        
    }
}