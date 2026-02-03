using AntsShooter.Entities;
using AntsShooter.Systems;
using Raylib_cs;
using System.Numerics;

namespace AntsShooter.Enities.UI;

public class LifeBar : Entity
{
    private Vector2 screenPosition;
    private Color color = new Color(255, 0, 0, 255);
    public int lifeBarHealthWidth { get; set; }

    public LifeBar(int lifeBarWidth, int lifeBarHeight) : base()
    {
        Position = Vector2.Zero;
        screenPosition = new Vector2(Globals.VECTUAL_SCREEN_WIDTH/20, Globals.VECTUAL_SCREEN_HEIGHT/10);
        Width = lifeBarWidth;
        Height = lifeBarHeight;

        lifeBarHealthWidth = Width;
    }

    public void UpdateScreenPosition(Camera camera)
    {
        Position.X = camera.camera.Target.X + (screenPosition.X - Globals.VECTUAL_SCREEN_WIDTH / 2);
        Position.Y = camera.camera.Target.Y + (screenPosition.Y - Globals.VECTUAL_SCREEN_HEIGHT / 2);
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