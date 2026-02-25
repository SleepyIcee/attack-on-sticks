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

    public static string CurrentState = "MenuState";
    private static string lastState = CurrentState;

    private const int VirtualScreenScaling = 2;
    private static Color backgroundColor = new Color(14, 20, 61);
    private static Texture2D mouseCursor = Raylib.LoadTexture("assets/cursor/cursor.png");
    private const int MouseCursorWidth = 16;
    private static RenderTexture2D renderTexture = Raylib.LoadRenderTexture(Globals.SCREEN_WIDTH / VirtualScreenScaling, Globals.SCREEN_HEIGHT / VirtualScreenScaling);

    public static void Load()
    {
        menuState = new MenuState();
        playState = new PlayState();
        deathState = new DeathState();
        scoresState = new ScoresState();

        if (CurrentState == "PlayState")
        {
            Globals.Score = 0;
        }

        lastState = CurrentState;

        Raylib.SetTextureFilter(renderTexture.Texture, TextureFilter.Point);
    }

    public static void Update()
    {
        if (lastState != CurrentState)
        {
            Load();
        }

        switch (CurrentState)
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
        Globals.MousePosition = Raylib.GetMousePosition() / VirtualScreenScaling;
    }

    public static void Draw()
    {
        Raylib.BeginTextureMode(renderTexture);
        Raylib.ClearBackground(backgroundColor);
        switch (CurrentState)
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

        Raylib.DrawTexture(mouseCursor, (int)MathF.Round(Globals.MousePosition.X - MouseCursorWidth/2), (int)MathF.Round(Globals.MousePosition.Y - MouseCursorWidth/2), Color.White);
        Raylib.EndTextureMode();

        Raylib.DrawTexturePro(renderTexture.Texture,
        new Rectangle(0, 0, renderTexture.Texture.Width, -renderTexture.Texture.Height),
        new Rectangle(0, 0, Globals.SCREEN_WIDTH, Globals.SCREEN_HEIGHT),
        Vector2.Zero,
        0.0f,
        Color.White);
    }
}
