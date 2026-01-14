using Raylib_cs;
using System.Numerics;
using AntsShooter.Systems;
using AntsShooter.Enities.UI;

namespace AntsShooter.States;

public class ScoresState : IState
{
    private Button backButton;

    public ScoresState()
    {
        backButton = new Button("back",
        new Vector2(
        Globals.SCREEN_WIDTH/2,
        Globals.SCREEN_HEIGHT/2),
        200, 50);
    }

    private void UpdateUI()
    {
        if (backButton.IsClicked())
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
        backButton.Draw();
    }
}