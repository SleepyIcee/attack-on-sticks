using System.Numerics;
using AttackOnSticks.States;
using AttackOnSticks.Systems;
using Raylib_cs;


namespace AttackOnSticks;

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
