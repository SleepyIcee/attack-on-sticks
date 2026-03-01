using System.Numerics;
using AntsShooter.States;
using AntsShooter.Systems;
using Raylib_cs;


namespace AntsShooter;

public class Game
{
    public Game()
    {
        Raylib.ToggleFullscreen();
        Raylib.HideCursor();
    }

    public void Update()
    {
        StatesManager.Update();

        if (Raylib.IsKeyPressed(KeyboardKey.F11))
        {
            Raylib.ToggleFullscreen();
        }
        // else if (Raylib.IsKeyPressed(KeyboardKey.Escape))
        // {
        //     Raylib.CloseWindow();
        // }
    }

    public void Draw()
    {
        StatesManager.Draw();
    }
}
