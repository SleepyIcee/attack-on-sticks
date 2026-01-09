using AntsShooter.Entities;
using AntsShooter.Systems;
using Raylib_cs;
using System.Numerics;


public class KillsScore : Entity
{
    public int kills = 0;
    private readonly int fontSize;
    private Color fontColor;


    public KillsScore() : base()
    {
        Position = Vector2.Zero;
        fontSize = 50;
        fontColor = Color.Black;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawText(Convert.ToString(kills), Globals.MAP_WIDTH - Globals.MAP_WIDTH/2, Globals.SCREEN_HEIGHT - Globals.SCREEN_HEIGHT/2, fontSize, fontColor);
    }
}