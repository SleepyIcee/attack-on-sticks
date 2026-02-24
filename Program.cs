using AntsShooter.Systems;
using Raylib_cs;

namespace AntsShooter;

class Program
{
    public static void Main()
    {
        Raylib.ToggleFullscreen();
        Raylib.InitWindow(Globals.SCREEN_WIDTH, Globals.SCREEN_HEIGHT, "AntsShooter");
        Raylib.InitAudioDevice();
        Game game = new Game();

        Raylib.SetTargetFPS(60);
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            game.Update();
            game.Draw();
            Raylib.EndDrawing();
        }
        ResourceManager.UnloadAll();
        Raylib.CloseWindow();
    }
}
