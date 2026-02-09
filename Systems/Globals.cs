using System.Numerics;
using Raylib_cs;

namespace AntsShooter.Systems;

public static class Globals
{
    // screen and map settings
    public const int SCREEN_WIDTH = 1080;
    public const int SCREEN_HEIGHT = 600;
    public const int VECTUAL_SCREEN_SCALING = 2;
    public const int VECTUAL_SCREEN_WIDTH = SCREEN_WIDTH/VECTUAL_SCREEN_SCALING;
    public const int VECTUAL_SCREEN_HEIGHT = SCREEN_HEIGHT/VECTUAL_SCREEN_SCALING;
    public const int MAP_WIDTH = 1600;
    public const int GROUND_LEVEL = VECTUAL_SCREEN_HEIGHT - 200;
    public static Vector2 mousePosition = Vector2.Zero;

    // player settings
    public const int PLAYER_WIDTH = 43;
    public const int PLAYER_HEIGHT = 41;
    public static readonly Vector2 OriginPlayerPos = new Vector2(VECTUAL_SCREEN_WIDTH / 2, GROUND_LEVEL - PLAYER_HEIGHT);

    // physics constants
    public const float GRAVITY = 50f;

    // game timers and spawns
    public static float SpawnAntTimer = 5f;

    // UI settings
    public const int BUTTONS_WIDTH = 100;
    public const int BUTTONS_HEIGHT = 20;
}