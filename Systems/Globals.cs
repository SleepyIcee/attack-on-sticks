using System.Numerics;
using Raylib_cs;

namespace AntsShooter.Systems;

public static class Globals
{
    // screen and map settings
    public const int SCREEN_WIDTH = 1280;
    public const int SCREEN_HEIGHT = 720;
    public const int VECTUAL_SCREEN_SCALING = 2;
    public const int VECTUAL_SCREEN_WIDTH = SCREEN_WIDTH / VECTUAL_SCREEN_SCALING;
    public const int VECTUAL_SCREEN_HEIGHT = SCREEN_HEIGHT / VECTUAL_SCREEN_SCALING;
    public const int MAP_WIDTH = 1600;
    public const int GROUND_LEVEL = VECTUAL_SCREEN_HEIGHT - 200;
    public static Vector2 MousePosition = Vector2.Zero;
    public static Vector2 MouseWorldPos = Vector2.Zero;
    public static Texture2D frontBackgroundTexture = Raylib.LoadTexture("assets/backgrounds/front.png");
    public static Texture2D backBackgroundTexture = Raylib.LoadTexture("assets/backgrounds/back.png");
    public static float backBackgroundScrolling = 0f;

    // player settings
    public const int PLAYER_WIDTH = 64;
    public const int PLAYER_HEIGHT = 64;
    public static readonly Vector2 OriginPlayerPos = new Vector2(VECTUAL_SCREEN_WIDTH / 2, GROUND_LEVEL - PLAYER_HEIGHT);

    // physics constants
    public const float GRAVITY = 2500f;

    // game timers and spawns
    public static float SpawnAntTimer = 5f;

    // UI settings
    // public static readonly Font GameFont = Raylib.LoadFontEx("assets/font/font.ttf", 50, null, 0);
    public const int BUTTONS_WIDTH = 100;
    public const int BUTTONS_HEIGHT = 20;
    public static int Score;
    public static float InputLock = 0f;
}
