using System.Numerics;
using AntsShooter.States;
using AntsShooter.Systems;
using Raylib_cs;


namespace AntsShooter;

public class Game
{
    RenderTexture2D renderTexture = Raylib.LoadRenderTexture(Globals.SCREEN_WIDTH/2, Globals.SCREEN_HEIGHT/2);

    public Game()
    {
        Raylib.SetTextureFilter(renderTexture.Texture, TextureFilter.Point);
    }
    
    public void Update()
    {
        StatesManager.Update();
        if (Raylib.IsKeyPressed(KeyboardKey.Escape))
        {
            Raylib.CloseWindow();
        }
    }

    public void Draw()
    {
        StatesManager.Draw();
    }
}
