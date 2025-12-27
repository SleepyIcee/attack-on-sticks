using System.Collections.Specialized;
using System.Numerics;
using Raylib_cs;

namespace AntsShooter.Systems;

public class Globals
{
    public const int SCREEN_WIDTH = 1067;
    public const int SCREEN_HEIGHT = 600;
    public const int MAP_WIDTH = 1600;
    public const float gravity = 50f;
    public static float spawnAntTimer = 5;
    public static Vector2 originPlayerPos = new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2);
    public static List<Vector2> BulletsSpawnPositions = [new Vector2(200, -30), new Vector2(400, -30), new Vector2(600, -30)];
}