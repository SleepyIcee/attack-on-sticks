using AntsShooter.Entities;
using AntsShooter.Systems;
using Raylib_cs;
using System.Net;
using System.Numerics;

namespace AntsShooter.Entities.UI;

public class Button : Entity
{
    private string Text;
    private Texture2D texture;
    private Dictionary<string, Animation> animations = new Dictionary<string, Animation>
    {
        {"idle" , ResourceManager.GetAnimation("button_idle", "assets/ui/button/idle")},
        {"hovered" , ResourceManager.GetAnimation("button_hovered", "assets/ui/button/hovered")},
    };
    private int fontSize;
    private Vector2 textPosition;

    public bool IsHovered = false;
    public bool MouseHovered = false;

    public Button(string text, Vector2 position, int width, int height) : base()
    {
        Position = position;
        Width = width;
        Height = height;
        Text = text;
        CalculateTextSize();

        texture = animations["idle"].Play(0);
    }

    private void CalculateTextSize()
    {
        fontSize = (int)(Height * 0.6f);

        int padding = 10;
        int maxTextWidth = Width - (padding * 2);

        while (fontSize > 10)
        {
            int textWidth = Raylib.MeasureText(Text, fontSize);
            if (textWidth <= maxTextWidth)
            {
                break;
            }
            fontSize -= 2;
        }

        int textWidthFinal = Raylib.MeasureText(Text, fontSize);
        textPosition = new Vector2(
            Position.X + (Width - textWidthFinal) / 2,
            Position.Y + (Height - fontSize) / 2
        );
    }

    public bool IsClicked()
    {
        if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition()/Globals.VECTUAL_SCREEN_SCALING,
            new Rectangle(Position, new Vector2(Width, Height))))
        {
            MouseHovered = true;
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            MouseHovered = false;
        }

        return false;
    }

    public override void Update()
    {
        base.Update();

        if (IsHovered)
        {
            texture = animations["hovered"].Play(0);
        }
        else
        {
            texture = animations["idle"].Play(0);
        }
    }

    public override void Draw()
    {
        base.Draw();
        

        Raylib.DrawTexturePro(texture,
        new Rectangle(0, 0, texture.Width, texture.Height),
        new Rectangle(Position.X, Position.Y, Width, Height),
        Vector2.Zero, 0, Color.White);
        
        Raylib.DrawText(Text, (int)Math.Round(textPosition.X), (int)Math.Round(textPosition.Y), fontSize, Color.White);
        // Raylib.DrawTextEx(Globals.GameFont, Text, Position, fontSize, 2, Color.White);
    }
}
