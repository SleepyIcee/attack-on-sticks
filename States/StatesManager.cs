namespace AntsShooter.States;

public class StatesManager
{
    public static MenuState menuState = new MenuState();
    public static PlayState playState = new PlayState();
    public static DeathState deathState = new DeathState();

    public static string currentState = "PlayState";

    public static void Update()
    {
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
        }
    }
}