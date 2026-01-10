using AntsShooter.Entities;
using AntsShooter.Systems;
using Raylib_cs;
using System.Net.Http.Headers;
using System.Numerics;


public class Ammo : Entity
{
    private Vector2 screenPosition;
    public int ammo = 60;
    public int loadedAmmo = 30;
    public readonly int HowMuchAmmoCouldBeLoaded = 30;

    private readonly float TimeToReload = 3f;
    private float reloadDelayTimer = 0;
    private bool reloadGun = false;

    public Ammo() : base()
    {
        Position = Vector2.Zero;
        screenPosition = new Vector2(Globals.SCREEN_WIDTH-200, 45);

        reloadDelayTimer = TimeToReload;
    }

    public void UpdateScreenPosition(Camera camera)
    {
        Position.X = camera.camera.Target.X + (screenPosition.X - Globals.SCREEN_WIDTH / 2);
        Position.Y = camera.camera.Target.Y + (screenPosition.Y - Globals.SCREEN_HEIGHT / 2);
    }

    public void Reload()
    {
        if (reloadDelayTimer <= 0)
        {
            int needs = HowMuchAmmoCouldBeLoaded - loadedAmmo;
            int transfer = Math.Min(needs, ammo);

            loadedAmmo += transfer;
            ammo -= transfer;

            reloadGun = false;
            reloadDelayTimer = TimeToReload;
        }
        else
        {
            reloadDelayTimer -= Raylib.GetFrameTime();
        }
    }

    public override void Update()
    {
        base.Update();

        if (loadedAmmo == 0 || Raylib.IsMouseButtonPressed(MouseButton.Right) && ammo > 0)
        {
            reloadGun = true;
        }

        if (reloadGun) Reload();
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawText("loaded ammo: " + Convert.ToString(loadedAmmo), (int)Math.Round(Position.X-400), (int)Math.Round(Position.Y), 50, Color.Black);
        Raylib.DrawText("ammo: " + Convert.ToString(ammo), (int)Math.Round(Position.X), (int)Math.Round(Position.Y), 50, Color.Black);
    }
}