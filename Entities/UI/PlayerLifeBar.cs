using AntsShooter.Entities;
using AntsShooter.Systems;
using Raylib_cs;
using System.Numerics;

namespace AntsShooter.Enities.UI;

public class PlayerLifeBar : Entity
{
    private Color color = Color.Red;
    public int lifeBarWidth { get; set; }

    public PlayerLifeBar(Camera2D camera) : base()
    {
        position = new Vector2(camera.Target.X - Globals.SCREEN_WIDTH/2 + 2, camera.Target.Y + 10);
        width = Globals.SCREEN_WIDTH/5;
        height = Globals.SCREEN_HEIGHT/30;

        lifeBarWidth = width;
    }

    public void UpdatePositionWithCamera(Camera2D camera)
    {
        position.X = camera.Target.X - Globals.SCREEN_WIDTH/2 + Globals.SCREEN_WIDTH/20;
        position.Y = camera.Target.Y - Globals.SCREEN_HEIGHT/2 + Globals.SCREEN_HEIGHT/10;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawRectangle((int)Math.Round(position.X), (int)Math.Round(position.Y), width, height, Color.Gray);
        Raylib.DrawRectangle((int)Math.Round(position.X), (int)Math.Round(position.Y), lifeBarWidth, height, color);
    }
}