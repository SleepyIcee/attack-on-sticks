using AntsShooter.Systems;
using Raylib_cs;
using System.Numerics;

namespace AntsShooter.States;

public class StatesManager
{
    private static MenuState menuState = new MenuState();
    private static PlayState playState = new PlayState();
    private static DeathState deathState = new DeathState();
    private static ScoresState scoresState = new ScoresState();

    public static string currentState = "PlayState";
    private static string lastState = currentState;

    private const int VirtualScreenScaling = 2;
    private static RenderTexture2D renderTexture = Raylib.LoadRenderTexture(Globals.SCREEN_WIDTH/VirtualScreenScaling, Globals.SCREEN_HEIGHT/VirtualScreenScaling);

    public static void Load()
    {
        menuState = new MenuState();
        playState = new PlayState();
        deathState = new DeathState();
        scoresState = new ScoresState();

        Raylib.SetTextureFilter(renderTexture.Texture, TextureFilter.Point);
    }

    public static void Update()
    {
        if (lastState != currentState)
        {
            Load();
            lastState = currentState;
        }

        switch (currentState)
        {
            case "MenuState":
                menuState.Update();
                break;
            case "PlayState":
                playState.Update();
                break;
            case "DeathState":
                deathState.Update();
                break;
            case "ScoresState":
                scoresState.Update();
                break;
        }

        // scale mouse position to virtual screen
        Globals.mousePosition = Raylib.GetMousePosition() / VirtualScreenScaling;
    }

    public static void Draw()
    {
        Raylib.BeginTextureMode(renderTexture);
        Raylib.ClearBackground(Color.Black);
        switch (currentState)
        {
            case "MenuState":
                menuState.Draw();
                break;
            case "PlayState":
                playState.Draw();
                break;
            case "DeathState":
                deathState.Draw();
                break;
            case "ScoresState":
                scoresState.Draw();
                break;
        }
        Raylib.EndTextureMode();

        Raylib.DrawTexturePro(renderTexture.Texture,
        new Rectangle(0, 0, renderTexture.Texture.Width, -renderTexture.Texture.Height),
        new Rectangle(0, 0, Globals.SCREEN_WIDTH, Globals.SCREEN_HEIGHT),
        Vector2.Zero,
        0.0f,
        Color.White);
    }
}