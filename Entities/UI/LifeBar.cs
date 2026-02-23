using AntsShooter.Entities;
using AntsShooter.Systems;
using Raylib_cs;
using System.Numerics;

namespace AntsShooter.Entities.UI;

public class LifeBar : Entity
{
    private Vector2 screenPosition = new Vector2(Globals.VECTUAL_SCREEN_WIDTH / 20, Globals.VECTUAL_SCREEN_HEIGHT / 10);
    private Color color = new Color(255, 0, 0, 255);
    public int lifeBarHealthWidth { get; set; }

    public LifeBar(int lifeBarWidth, int lifeBarHeight) : base()
    {
        Width = lifeBarWidth;
        Height = lifeBarHeight;

        Position.X = 10;
        Position.Y = Globals.VECTUAL_SCREEN_HEIGHT - Height * 4 - 5;

        this.lifeBarHealthWidth = Width;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Draw()
    {
        base.Draw();
        // Raylib.DrawRectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), Width, Height, Color.DarkGray);
        Raylib.DrawRectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), lifeBarHealthWidth, Height, color);
    }
}
