using Raylib_cs;
using System.Numerics;
using AntsShooter.Entities.UI;
using AntsShooter.Systems;

namespace AntsShooter.States;

public class MenuState : IState
{
    private List<Button> buttons;
    private int keyboardPoitingToButtonNumber = 0;
    private static readonly float keyboardPoitingTime = 0.2f;
    private float keyboardPoitingToTimer = keyboardPoitingTime;

    private int[] topScores = Score.LoadHighestScores();

    public MenuState()
    {
        buttons = new List<Button>
        {
            // new Button("play",
            // new Vector2(Globals.VECTUAL_SCREEN_WIDTH/2 - Globals.BUTTONS_WIDTH/2,
            // Globals.VECTUAL_SCREEN_HEIGHT/2 - Globals.BUTTONS_HEIGHT),
            // Globals.BUTTONS_WIDTH, Globals.BUTTONS_HEIGHT),
            // new Button("scores",
            // new Vector2(Globals.VECTUAL_SCREEN_WIDTH/2 - Globals.BUTTONS_WIDTH/2,
            // Globals.VECTUAL_SCREEN_HEIGHT/2 + Globals.BUTTONS_HEIGHT/2),
            // Globals.BUTTONS_WIDTH, Globals.BUTTONS_HEIGHT)

            new Button("play",
            new Vector2(10,
            Globals.VECTUAL_SCREEN_HEIGHT - Globals.BUTTONS_HEIGHT * 3 - 20),
            Globals.BUTTONS_WIDTH, Globals.BUTTONS_HEIGHT),
            new Button("exit",
            new Vector2(10,
            Globals.VECTUAL_SCREEN_HEIGHT - Globals.BUTTONS_HEIGHT * 2 - 10),
            Globals.BUTTONS_WIDTH, Globals.BUTTONS_HEIGHT)
        };
    }

    private void UpdateUI()
    {
        if (buttons[0].IsClicked())
        {
            StatesManager.currentState = "PlayState";
        }
        else if (buttons[1].IsClicked())
        {
            Raylib.CloseWindow();
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
                StatesManager.currentState = "PlayState";
            }
            else if (keyboardPoitingToButtonNumber == 1)
            {
                Raylib.CloseWindow();
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
        Raylib.DrawRectangle(10, 180 - 20, Globals.BUTTONS_WIDTH, Globals.VECTUAL_SCREEN_HEIGHT/2 - 80, Raylib.Fade(Color.Black, 0.5f));

        Raylib.DrawText("highest kills", Globals.BUTTONS_WIDTH/5 - 2, 170, 15, Color.White);

        for (int i = 0; i < topScores.Length; i++)
        {
            Raylib.DrawText(topScores[i].ToString(), Globals.BUTTONS_WIDTH / 2, 190 + i * 20, 20, Color.White);
        }

        foreach (var button in buttons)
        {
            button.Draw();
        }
    }
}
