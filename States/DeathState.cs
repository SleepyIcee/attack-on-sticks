using Raylib_cs;
using System.Numerics;
using AntsShooter.Systems;
using AntsShooter.Enities.UI;

namespace AntsShooter.States;


public class DeathState : IState
{
    private Button playAgainButton;
    private Button menuScreenButton;

    public DeathState()
    {
        playAgainButton = new Button("play again",
        new Vector2(
        Globals.SCREEN_WIDTH/2,
        Globals.SCREEN_HEIGHT/2),
        200, 50);

        menuScreenButton = new Button("menu",
        new Vector2(Globals.SCREEN_WIDTH/3, Globals.SCREEN_HEIGHT/3),
        200, 50);
    }

    private void UpdateUI()
    {
        if (playAgainButton.IsClicked())
        {
            StatesManager.currentState = "PlayState";
        }
        else if (menuScreenButton.IsClicked())
        {
            StatesManager.currentState = "MenuState";
        }
    }
    
    public void Update()
    {
        UpdateUI();
    }
    public void Draw()
    {
        playAgainButton.Draw();
        menuScreenButton.Draw();
    }
}