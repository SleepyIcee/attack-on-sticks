using AntsShooter.Entities;
using AntsShooter.Systems;
using Raylib_cs;
using System.Net.Http.Headers;
using System.Numerics;

namespace AntsShooter.Entities.UI;

public class Ammo : Entity
{
    private Vector2 screenPosition;
    public int ammo = 60;
    public int loadedAmmo = 30;
    public readonly int HowMuchAmmoCouldBeLoaded = 30;

    private readonly float TimeToReload = 3f;
    private float reloadDelayTimer = 0;
    public bool reloadGun = false;

    private Vector2 reloadingLinePosition;
    private Vector2 reloadingLineScreenPosition;
    private int normalReloadingLineWidth = 30;
    private int reloadingLineWidth;
    private int reloadingLineHeight = 10;

    private Sound gunReloadSound = new();

    public Ammo() : base()
    {
        Position = Vector2.Zero;
        screenPosition = new Vector2(Globals.VECTUAL_SCREEN_WIDTH-200, 45);

        reloadDelayTimer = 0;

        reloadingLinePosition = Vector2.Zero;
        reloadingLineWidth = 0;
        reloadingLineScreenPosition = new Vector2(Globals.VECTUAL_SCREEN_WIDTH/2 + normalReloadingLineWidth/2, Globals.VECTUAL_SCREEN_HEIGHT/2 - reloadingLineHeight * 3);

        gunReloadSound = Raylib.LoadSound("assets/sounds/ak47-reload-sound.wav");
    }

    public void UpdateScreenPosition(Camera camera)
    {
        Position.X = camera.camera.Target.X + (screenPosition.X - Globals.VECTUAL_SCREEN_WIDTH / 2);
        Position.Y = camera.camera.Target.Y + (screenPosition.Y - Globals.VECTUAL_SCREEN_HEIGHT / 2);

        reloadingLinePosition.X = camera.camera.Target.X + (reloadingLineScreenPosition.X - Globals.VECTUAL_SCREEN_WIDTH / 2);
        reloadingLinePosition.Y = camera.camera.Target.Y + (reloadingLineScreenPosition.Y - Globals.VECTUAL_SCREEN_HEIGHT / 2);
    }

    public void Reload()
    {
        if (reloadDelayTimer >= TimeToReload)
        {
            int needs = HowMuchAmmoCouldBeLoaded - loadedAmmo;
            int transfer = Math.Min(needs, ammo);

            loadedAmmo += transfer;
            ammo -= transfer;

            reloadGun = false;
            reloadDelayTimer = 0;
            reloadingLineWidth = 0;
        }
        else
        {
            reloadDelayTimer += Raylib.GetFrameTime();
            reloadingLineWidth = (int)(normalReloadingLineWidth/TimeToReload*reloadDelayTimer);
        }

        Raylib.PlaySound(gunReloadSound);
    }

    public override void Update()
    {
        base.Update();

        if (loadedAmmo == 0 || Raylib.IsKeyPressed(KeyboardKey.R) && loadedAmmo < HowMuchAmmoCouldBeLoaded)
        {
            reloadGun = true;
        }

        if (reloadGun & ammo > 0)
        {
            Reload();
        }
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawText("loaded ammo: " + Convert.ToString(loadedAmmo), (int)Math.Round(Position.X-80), (int)Math.Round(Position.Y), 5, Color.White);
        Raylib.DrawText("ammo: " + Convert.ToString(ammo), (int)Math.Round(Position.X), (int)Math.Round(Position.Y), 5, Color.White);

        // reload bar
        if (reloadGun)
        {
            Raylib.DrawRectangle((int)Math.Round(reloadingLinePosition.X), (int)Math.Round(reloadingLinePosition.Y), reloadingLineWidth, reloadingLineHeight, Color.Gray);
        }
    }
}
