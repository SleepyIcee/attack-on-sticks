using Raylib_cs;
using System.Numerics;
using AntsShooter.Enities.UI;
using AntsShooter.Systems;

namespace AntsShooter.States;

public class MenuState : IState
{
    private Button playButton;
    private Button scoresButton;

    public MenuState()
    {
        playButton = new Button("play",
        new Vector2(
        Globals.SCREEN_WIDTH/2,
        Globals.SCREEN_HEIGHT/2),
        200, 50);

        scoresButton = new Button("scores",
        new Vector2(Globals.SCREEN_WIDTH/3, Globals.SCREEN_HEIGHT/3),
        200, 50);
    }

    private void UpdateUI()
    {
        if (playButton.IsClicked())
        {
            StatesManager.currentState = "PlayState";
        }
        else if (scoresButton.IsClicked())
        {
            StatesManager.currentState = "ScoresState";
        }
    }
    
    public void Update()
    {
        UpdateUI();
    }

    public void Draw()
    {
        playButton.Draw();
        scoresButton.Draw();
    }
}