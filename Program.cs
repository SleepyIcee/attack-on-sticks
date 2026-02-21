using AntsShooter.Systems;
using Raylib_cs;

namespace AntsShooter;

class Program
{
    public static void Main()
    {
        Raylib.InitWindow(Globals.SCREEN_WIDTH, Globals.SCREEN_HEIGHT, "AntsShooter");
        Raylib.ToggleFullscreen();
        Raylib.InitAudioDevice();
        Game game = new Game();

        Raylib.SetTargetFPS(60);
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            game.Update();
            game.Draw();

            if (Raylib.IsKeyPressed(KeyboardKey.F11))
            {
                Raylib.ToggleFullscreen();
            }

            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }
}
