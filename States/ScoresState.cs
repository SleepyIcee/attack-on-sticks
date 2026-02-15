using Raylib_cs;
using System.Numerics;
using AntsShooter.Systems;
using AntsShooter.Enities.UI;

namespace AntsShooter.States;

public class ScoresState : IState
{
    private List<Button> buttons;
    private int keyboardPoitingToButtonNumber = 0;
    private static readonly float keyboardPoitingTime = 0.2f;
    private float keyboardPoitingToTimer = keyboardPoitingTime;

    private int[] topScores = Score.LoadHighestScores();

    public ScoresState()
    {
        buttons = new List<Button>
        {
            new Button("back",
            new Vector2(Globals.VECTUAL_SCREEN_WIDTH/2 - Globals.BUTTONS_WIDTH/2,
            Globals.VECTUAL_SCREEN_HEIGHT/2 - Globals.BUTTONS_HEIGHT/2),
            Globals.BUTTONS_WIDTH, Globals.BUTTONS_HEIGHT)
        };
    }

    private void UpdateUI()
    {
        if (buttons[0].IsClicked())
        {
            StatesManager.currentState = "MenuState";
        }

        if (keyboardPoitingToTimer <= 0)
        {
            if (Raylib.IsKeyDown(KeyboardKey.Up) || Raylib.IsKeyDown(KeyboardKey.W))
            {
                if (keyboardPoitingToButtonNumber <= 0)
                {
                    keyboardPoitingToButtonNumber = buttons.Count() - 1;
                }
                else
                {
                    keyboardPoitingToButtonNumber -= 1;
                }

                keyboardPoitingToTimer = keyboardPoitingTime;
            }
            else if (Raylib.IsKeyDown(KeyboardKey.Down) || Raylib.IsKeyDown(KeyboardKey.S))
            {
                if (keyboardPoitingToButtonNumber >= buttons.Count() - 1)
                {
                    keyboardPoitingToButtonNumber = 0;
                }
                else
                {
                    keyboardPoitingToButtonNumber += 1;
                }

                keyboardPoitingToTimer = keyboardPoitingTime;
            }
        }
        else
        {
            keyboardPoitingToTimer -= Raylib.GetFrameTime();
        }

        if (Raylib.IsKeyPressed(KeyboardKey.Enter) || Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            if (keyboardPoitingToButtonNumber == 0)
            {
                StatesManager.currentState = "MenuState";
            }
        }
        else
        {
            buttons[keyboardPoitingToButtonNumber].isHovered = true;
        }
    }

    public void Update()
    {
        UpdateUI();

        for (int i = 0; i < buttons.Count(); i++)
        {
            buttons[i].Update();

            if (buttons[i].mouseHovered)
            {
                keyboardPoitingToButtonNumber = i;
            }

            if (i == keyboardPoitingToButtonNumber)
            {
                continue;
            }

            buttons[i].isHovered = false;
        }
    }

    public void Draw()
    {
        for (int i = 0; i < topScores.Length; i++)
        {
            Raylib.DrawText(topScores[i].ToString(), (int)MathF.Round(Globals.VECTUAL_SCREEN_WIDTH / 2 - 10), 50 + i * 20, 20, Color.White);
        }

        foreach (var button in buttons)
        {
            button.Draw();
        }
    }
}
