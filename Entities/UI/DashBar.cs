using AntsShooter.Systems;
using AntsShooter.Entities;
using System.Numerics;
using Raylib_cs;

namespace AntsShooter.Entities.UI;

public class DashBar : Entity
{
    private Vector2 screenPosition = new Vector2(Globals.VECTUAL_SCREEN_WIDTH / 20, Globals.VECTUAL_SCREEN_HEIGHT / 10);
    private Color color = Color.Orange;
    public int dashBarWidth { get; set; }

    public DashBar(int dashBarWidth, int dashBarHeight) : base()
    {
        Position = Vector2.Zero;
        Width = dashBarWidth;
        Height = dashBarHeight;

        this.dashBarWidth = Width;
    }

    public void UpdateScreenPosition(Camera camera)
    {
        Position.X = camera.camera.Target.X + (screenPosition.X - Globals.VECTUAL_SCREEN_WIDTH / 4);
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
        Raylib.DrawRectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), dashBarWidth, Height, color);
    }
}
