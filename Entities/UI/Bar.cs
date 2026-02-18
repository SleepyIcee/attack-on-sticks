using AntsShooter.Systems;
using System.Numerics;
using AntsShooter.Entities;

namespace AntsShooter.Entities.UI;

public class Bar : Entity
{
    private Vector2 screenPosition = new Vector2(Globals.VECTUAL_SCREEN_WIDTH / 20, Globals.VECTUAL_SCREEN_HEIGHT / 10);

    public Bar() : base()
    {
        Position = Vector2.Zero;
        screenPosition = new Vector2(Globals.VECTUAL_SCREEN_WIDTH / 20, Globals.VECTUAL_SCREEN_HEIGHT / 10);
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
    }
}
