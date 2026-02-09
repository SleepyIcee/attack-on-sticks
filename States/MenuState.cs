using Raylib_cs;
using System.Numerics;
using AntsShooter.Enities.UI;
using AntsShooter.Systems;

namespace AntsShooter.States;

public class MenuState : IState
{
    private List<Button> buttons;
    private int keyboardPoitingToButtonNumber = 0;
    private static readonly float keyboardPoitingTime = 0.2f;
    private float keyboardPoitingToTimer = keyboardPoitingTime;

    public MenuState()
    {
        buttons = new List<Button>
        {
            new Button("play",
            new Vector2(Globals.VECTUAL_SCREEN_WIDTH/2 - Globals.BUTTONS_WIDTH/2,
            Globals.VECTUAL_SCREEN_HEIGHT/2 - Globals.BUTTONS_HEIGHT),
            Globals.BUTTONS_WIDTH, Globals.BUTTONS_HEIGHT),
            new Button("scores",
            new Vector2(Globals.VECTUAL_SCREEN_WIDTH/2 - Globals.BUTTONS_WIDTH/2,
            Globals.VECTUAL_SCREEN_HEIGHT/2 + Globals.BUTTONS_HEIGHT/2),
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
            StatesManager.currentState = "ScoresState";
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
                StatesManager.currentState = "ScoresState";
            }
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Enter) || Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            if (keyboardPoitingToButtonNumber == 0)
            {
                StatesManager.currentState = "PlayState";
            }
            else if (keyboardPoitingToButtonNumber == 1)
            {
                StatesManager.currentState = "ScoresState";
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
        foreach (var button in buttons)
        {
            button.Draw();
        }
    }
}