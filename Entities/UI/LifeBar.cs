using AntsShooter.Entities;
using AntsShooter.Systems;
using Raylib_cs;
using System.Numerics;

namespace AntsShooter.Enities.UI;

public class LifeBar : Entity
{
    private Color color = new Color(255, 0, 0, 255);
    public int lifeBarHealthWidth { get; set; }

    public LifeBar(int lifeBarWidth, int lifeBarHeight) : base()
    {
        Position = Vector2.Zero;
        Width = lifeBarWidth;
        Height = lifeBarHeight;

        lifeBarHealthWidth = Width;
    }

    public void UpdateScreenPosition(Camera2D camera, Vector2 screenPos)
    {
        Position.X = camera.Target.X + (screenPos.X - Globals.SCREEN_WIDTH / 2);
        Position.Y = camera.Target.Y + (screenPos.Y - Globals.SCREEN_HEIGHT / 2);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawRectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), Width, Height, Color.Gray);
        Raylib.DrawRectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), lifeBarHealthWidth, Height, color);
    }
}