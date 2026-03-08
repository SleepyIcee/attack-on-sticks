using AntsShooter.Entities;
using AntsShooter.Systems;
using Raylib_cs;
using System.Numerics;

namespace AntsShooter.Entities.UI;

public class Ammo : Entity
{
    private Vector2 screenPosition;
    public int HowMuchAmmo = 60;
    public int LoadedAmmo = 30;
    public readonly int HowMuchAmmoCouldBeLoaded = 30;

    private readonly float TimeToReload = 3f;
    private float reloadDelayTimer = 0;
    public bool ReloadGun = false;

    private Vector2 reloadingLinePosition;
    private Vector2 reloadingLineScreenPosition;
    private int normalReloadingLineWidth = 30;
    private int reloadingLineWidth;
    private int reloadingLineHeight = 10;

    private Sound gunReloadSound = new();
    private bool reloadSoundPlayed = false;

    private int fontSize = 5;
    private Gun _gun;

    public Ammo(ref Gun gun) : base()
    {
        _gun = gun;

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
            int needs = HowMuchAmmoCouldBeLoaded - LoadedAmmo;
            int transfer = Math.Min(needs, HowMuchAmmo);

            LoadedAmmo += transfer;
            HowMuchAmmo -= transfer;

            ReloadGun = false;
            _gun.ReloadAnimationPlay = false;
            reloadDelayTimer = 0;
            reloadingLineWidth = 0;
            reloadSoundPlayed = false;
        }
        else
        {
            if (!reloadSoundPlayed)
            {
                Raylib.PlaySound(gunReloadSound);
                reloadSoundPlayed = true;
            }
            
            reloadDelayTimer += Raylib.GetFrameTime();
            reloadingLineWidth = (int)(normalReloadingLineWidth/TimeToReload*reloadDelayTimer);
        }
    }

    public override void Update()
    {
        base.Update();

        // Start reload once when needed (don't restart every frame while reloading)
        // Only start a reload if we have reserve ammo to pull from
        if ((LoadedAmmo == 0 || (Raylib.IsKeyPressed(KeyboardKey.R) && LoadedAmmo < HowMuchAmmoCouldBeLoaded))
            && !ReloadGun
            && HowMuchAmmo > 0)
        {
            ReloadGun = true;
            _gun.ReloadAnimationPlay = true;
        }

        // If we run out of reserve ammo mid-reload, stop the reload animation too.
        if (ReloadGun && HowMuchAmmo <= 0)
        {
            ReloadGun = false;
            _gun.ReloadAnimationPlay = false;
        }

        if (ReloadGun && HowMuchAmmo > 0)
        {
            Reload();
        }
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawText(Convert.ToString(LoadedAmmo) + " / " + Convert.ToString(HowMuchAmmo), (int)Math.Round(Position.X), (int)Math.Round(Position.Y), 5, Color.White);
        // Raylib.DrawTextEx(Globals.GameFont, "loaded ammo: " + Convert.ToString(loadedAmmo), Position, fontSize, 2, Color.White);
        // Raylib.DrawTextEx(Globals.GameFont, "ammo: " + Convert.ToString(ammo), Position, fontSize, 2, Color.White);

        // reload bar
        if (ReloadGun)
        {
            Raylib.DrawRectangle((int)Math.Round(reloadingLinePosition.X), (int)Math.Round(reloadingLinePosition.Y), reloadingLineWidth, reloadingLineHeight, Color.Gray);
        }
    }
}
