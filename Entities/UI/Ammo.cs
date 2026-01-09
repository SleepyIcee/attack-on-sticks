using AntsShooter.Entities;
using AntsShooter.Systems;
using Raylib_cs;
using System.Numerics;


public class Ammo : Entity
{
    private Vector2 screenPosition;
    public int ammo = 30;

    public Ammo() : base()
    {
        Position = Vector2.Zero;
        screenPosition = new Vector2(Globals.SCREEN_WIDTH-200, 45);
    }

    public void UpdateScreenPosition(Camera camera)
    {
        Position.X = camera.camera.Target.X + (screenPosition.X - Globals.SCREEN_WIDTH / 2);
        Position.Y = camera.camera.Target.Y + (screenPosition.Y - Globals.SCREEN_HEIGHT / 2);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawText("ammo: " + Convert.ToString(ammo), (int)Math.Round(Position.X), (int)Math.Round(Position.Y), 50, Color.Black);
    }
}