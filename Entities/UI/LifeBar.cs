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
        position = Vector2.Zero;
        width = lifeBarWidth;
        height = lifeBarHeight;

        lifeBarHealthWidth = width;
    }

    public void UpdateScreenPosition(Camera2D camera, Vector2 screenPos)
    {
        position.X = camera.Target.X + (screenPos.X - Globals.SCREEN_WIDTH / 2);
        position.Y = camera.Target.Y + (screenPos.Y - Globals.SCREEN_HEIGHT / 2);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawRectangle((int)Math.Round(position.X), (int)Math.Round(position.Y), width, height, Color.Gray);
        Raylib.DrawRectangle((int)Math.Round(position.X), (int)Math.Round(position.Y), lifeBarHealthWidth, height, color);
    }
}