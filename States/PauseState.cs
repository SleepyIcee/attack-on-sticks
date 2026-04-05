using Raylib_cs;
using System.Numerics;
using AttackOnSticks.Entities.UI;
using AttackOnSticks.Systems;


namespace AttackOnSticks.States;

public class PauseState : IState
{
    private List<Button> buttons;
    public int KeyboardPoitingToButtonNumber = 0;
    private static readonly float keyboardPoitingTime = 0.2f;
    private float keyboardPoitingToTimer = keyboardPoitingTime;

    private Color backgroundColor = new Color(0, 0, 0, 100);

    public PauseState()
    {
        buttons = new List<Button>
        {
            new Button("resume",
            new Vector2(Globals.VERTUAL_SCREEN_WIDTH/2 - Globals.BUTTONS_WIDTH/2,
            Globals.VERTUAL_SCREEN_HEIGHT/2 - Globals.BUTTONS_HEIGHT),
            Globals.BUTTONS_WIDTH, Globals.BUTTONS_HEIGHT),
            new Button("main menu",
            new Vector2(Globals.VERTUAL_SCREEN_WIDTH/2 - Globals.BUTTONS_WIDTH/2,
            Globals.VERTUAL_SCREEN_HEIGHT/2 + Globals.BUTTONS_HEIGHT/2),
            Globals.BUTTONS_WIDTH, Globals.BUTTONS_HEIGHT)
        };
    }

    private void UpdateUI()
    {
        if (buttons[0].IsClicked())
        {
            StatesManager.PauseGame = false;
            Globals.InputLock = 0.2f; // ignore input immediately after clicking resume
        }
        else if (buttons[1].IsClicked())
        {
            StatesManager.CurrentState = "MenuState";
            StatesManager.PauseGame = false;
        }

        if (keyboardPoitingToTimer <= 0)
        {
            if (Raylib.IsKeyDown(KeyboardKey.Up) || Raylib.IsKeyDown(KeyboardKey.W))
            {
                if (KeyboardPoitingToButtonNumber <= 0)
                {
                    KeyboardPoitingToButtonNumber = buttons.Count() - 1;
                }
                else
                {
                    KeyboardPoitingToButtonNumber -= 1;
                }

                keyboardPoitingToTimer = keyboardPoitingTime;
            }
            else if (Raylib.IsKeyDown(KeyboardKey.Down) || Raylib.IsKeyDown(KeyboardKey.S))
            {
                if (KeyboardPoitingToButtonNumber >= buttons.Count() - 1)
                {
                    KeyboardPoitingToButtonNumber = 0;
                }
                else
                {
                    KeyboardPoitingToButtonNumber += 1;
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
            if (KeyboardPoitingToButtonNumber == 0)
            {
                StatesManager.PauseGame = false;
                Globals.InputLock = 0.2f;
            }
            else if (KeyboardPoitingToButtonNumber == 1)
            {
                StatesManager.CurrentState = "MenuState";
            }
        }
        else
        {
            buttons[KeyboardPoitingToButtonNumber].IsHovered = true;
        }
    }

    public void Update()
    {
        UpdateUI();

        for (int i = 0; i < buttons.Count(); i++)
        {
            buttons[i].Update();

            if (buttons[i].MouseHovered)
            {
                KeyboardPoitingToButtonNumber = i;
            }

            if (i == KeyboardPoitingToButtonNumber)
            {
                continue;
            }

            buttons[i].IsHovered = false;
        }
    }

    public void Draw()
    {
        Raylib.DrawRectangle(0, 0, Globals.VERTUAL_SCREEN_WIDTH, Globals.VERTUAL_SCREEN_HEIGHT, backgroundColor);

        foreach (var button in buttons)
        {
            button.Draw();
        }
    }
}
