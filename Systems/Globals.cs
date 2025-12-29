using System.Numerics;
using Raylib_cs;

namespace AntsShooter.Systems;

public static class Globals
{
    // screen and map settings
    public const int SCREEN_WIDTH = 1067;
    public const int SCREEN_HEIGHT = 600;
    public const int MAP_WIDTH = 1600;
    public const int GROUND_LEVEL = SCREEN_HEIGHT - 100;

    // player settings
    public const int PLAYER_WIDTH = 50;
    public const int PLAYER_HEIGHT = 50;
    public static readonly Vector2 OriginPlayerPos = new Vector2(SCREEN_WIDTH / 2, GROUND_LEVEL - PLAYER_HEIGHT);

    // physics constants
    public const float GRAVITY = 50f;

    // game timers and spawns
    public static float SpawnAntTimer = 5f;
    public static readonly List<Vector2> BulletsSpawnPositions = [new Vector2(200, -30), new Vector2(400, -30), new Vector2(600, -30)];
}