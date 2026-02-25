using AntsShooter.Entities;
using AntsShooter.Systems;
using Raylib_cs;
using System.Numerics;

namespace AntsShooter.Entities.UI;

public class KillsScore : Entity
{
    private Vector2 screenPosition = new Vector2(Globals.VECTUAL_SCREEN_WIDTH / 20, Globals.VECTUAL_SCREEN_HEIGHT / 10);
    public int Kills = 0;
    private readonly int fontSize;
    private Color fontColor;

    public KillsScore() : base()
    {
        fontSize = 5;

        Position.X = 10;
        Position.Y = Globals.VECTUAL_SCREEN_HEIGHT - 75;

        fontColor = Color.White;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Draw()
    {
        base.Draw();
        // Raylib.DrawText(Convert.ToString(kills), Globals.VECTUAL_SCREEN_WIDTH - Globals.VECTUAL_SCREEN_WIDTH/2,
        // Globals.VECTUAL_SCREEN_HEIGHT - Globals.VECTUAL_SCREEN_HEIGHT/2, fontSize, fontColor);

        Raylib.DrawText("kills: " + Convert.ToString(Kills), (int)Math.Round(Position.X), (int)Math.Round(Position.Y), fontSize, fontColor);
        // Raylib.DrawTextEx(Globals.GameFont, "kills: " + Convert.ToString(kills), Position, fontSize, 2, Color.White);
    }
}
