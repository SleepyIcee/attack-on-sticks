using Raylib_cs;
using System.Numerics;
using AttackOnSticks.Entities.UI;
using AttackOnSticks.Systems;

namespace AttackOnSticks.States;

public class MenuState : IState
{
    private List<Button> buttons;
    private int keyboardPoitingToButtonNumber = 0;
    private static readonly float keyboardPoitingTime = 0.2f;
    private float keyboardPoitingToTimer = keyboardPoitingTime;
    private Texture2D Logo = Raylib.LoadTexture("assets/logo.png");

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
            Globals.VERTUAL_SCREEN_HEIGHT - Globals.BUTTONS_HEIGHT * 3 - 20),
            Globals.BUTTONS_WIDTH, Globals.BUTTONS_HEIGHT),
            new Button("exit",
            new Vector2(10,
            Globals.VERTUAL_SCREEN_HEIGHT - Globals.BUTTONS_HEIGHT * 2 - 10),
            Globals.BUTTONS_WIDTH, Globals.BUTTONS_HEIGHT)
        };
    }

    private void UpdateUI()
    {
        if (buttons[0].IsClicked())
        {
            StatesManager.CurrentState = "PlayState";
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
                StatesManager.CurrentState = "PlayState";
            }
            else if (keyboardPoitingToButtonNumber == 1)
            {
                Raylib.CloseWindow();
            }
        }
        else
        {
            buttons[keyboardPoitingToButtonNumber].IsHovered = true;
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
                keyboardPoitingToButtonNumber = i;
            }

            if (i == keyboardPoitingToButtonNumber)
            {
                continue;
            }

            buttons[i].IsHovered = false;
        }
    }

    public void Draw()
    {
        Raylib.DrawTexture(Globals.backBackgroundTexture, 0, 0, Color.White);
        Raylib.DrawTexture(Globals.frontBackgroundTexture, -10, 0, Color.White);

        Raylib.DrawRectangle(10, 180 - 20, Globals.BUTTONS_WIDTH, Globals.VERTUAL_SCREEN_HEIGHT/2 - 80, Color.Black);

        Raylib.DrawText("highest kills", Globals.BUTTONS_WIDTH/5 - 2, 170, 15, Color.White);

        for (int i = 0; i < topScores.Length; i++)
        {
            int posX = Globals.BUTTONS_WIDTH/2;

            if (topScores[i] > 1000 && topScores[i] < 10000)
            {
                posX -= 10;
            }
            if (topScores[i] > 10000 && topScores[i] < 100000)
            {
                posX -= 20;
            }

            Raylib.DrawText(topScores[i].ToString(), posX, 190 + i * 20, 20, Color.White);
        }

        Raylib.DrawTexture(Logo, Globals.VERTUAL_SCREEN_WIDTH/2 - 140, Globals.VERTUAL_SCREEN_HEIGHT/4, Color.White);
        Raylib.DrawText("made by Icee", Globals.VERTUAL_SCREEN_WIDTH - Globals.VERTUAL_SCREEN_WIDTH/6, 
        Globals.VERTUAL_SCREEN_HEIGHT - 50, 15, Color.White);

        foreach (var button in buttons)
        {
            button.Draw();
        }
    }
}
