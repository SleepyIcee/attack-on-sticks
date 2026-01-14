namespace AntsShooter.States;

public class StatesManager
{
    private static MenuState menuState = new MenuState();
    private static PlayState playState = new PlayState();
    private static DeathState deathState = new DeathState();
    private static ScoresState scoresState = new ScoresState();

    public static string currentState = "MenuState";
    private static string lastState = currentState;

    public static void Load()
    {
        menuState = new MenuState();
        playState = new PlayState();
        deathState = new DeathState();
        scoresState = new ScoresState();
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
    }

    public static void Draw()
    {
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
    }
}