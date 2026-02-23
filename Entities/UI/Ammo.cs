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

    private int fontSize = 5;

    public Ammo() : base()
    {
        Position.X = 10;
        Position.Y = Globals.VECTUAL_SCREEN_HEIGHT - 65;

        screenPosition = new Vector2(Globals.VECTUAL_SCREEN_WIDTH-200, 45);

        reloadDelayTimer = 0;

        reloadingLinePosition = new Vector2(Globals.VECTUAL_SCREEN_WIDTH/2 - normalReloadingLineWidth/2, Globals.VECTUAL_SCREEN_HEIGHT/2 - reloadingLineHeight * 3);
        reloadingLineWidth = 0;

        gunReloadSound = Raylib.LoadSound("assets/sounds/ak47-reload-sound.wav");
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
        Raylib.DrawText(Convert.ToString(loadedAmmo) + " / " + Convert.ToString(ammo), (int)Math.Round(Position.X), (int)Math.Round(Position.Y), 5, Color.White);
        // Raylib.DrawTextEx(Globals.GameFont, "loaded ammo: " + Convert.ToString(loadedAmmo), Position, fontSize, 2, Color.White);
        // Raylib.DrawTextEx(Globals.GameFont, "ammo: " + Convert.ToString(ammo), Position, fontSize, 2, Color.White);

        // reload bar
        if (reloadGun)
        {
            Raylib.DrawRectangle((int)Math.Round(reloadingLinePosition.X), (int)Math.Round(reloadingLinePosition.Y), reloadingLineWidth, reloadingLineHeight, Color.Gray);
        }
    }
}
