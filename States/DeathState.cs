using Raylib_cs;
using AntsShooter.Systems;
using AntsShooter.Enities.UI;

namespace AntsShooter.States;


public class DeathState : IState
{
    Button playAgainButton;
    public DeathState()
    {
        playAgainButton = new Button("play again",
        new System.Numerics.Vector2(
        Globals.SCREEN_WIDTH/2,
        Globals.SCREEN_HEIGHT/2),
        200, 50);
    }
    
    public void Update()
    {
        if (playAgainButton.IsClicked())
        {
            StatesManager.currentState = "PlayState";
        }

        playAgainButton.Update();
    }

    public void Draw()
    {
        playAgainButton.Draw();
    }
}