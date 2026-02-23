using AntsShooter.Entities;
using AntsShooter.Systems;
using Raylib_cs;
using System.Net;
using System.Numerics;

namespace AntsShooter.Entities.UI;

public class Button : Entity
{
    private string Text;
    private int fontSize;
    private Vector2 textPosition;
    private Color buttonColor = Color.Black;

    public bool isHovered = false;
    public bool mouseHovered = false;

    public Button(string text, Vector2 position, int width, int height) : base()
    {
        Position = position;
        Width = width;
        Height = height;
        Text = text;
        CalculateTextSize();
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
            mouseHovered = true;
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
            mouseHovered = false;
        }

        return false;
    }

    public override void Update()
    {
        base.Update();

        if (isHovered)
        {
            buttonColor = Color.DarkGray;
        }
        else
        {
            buttonColor = Color.Black;
        }
    }

    public override void Draw()
    {
        base.Draw();

        Raylib.DrawRectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), Width, Height, buttonColor);
        Raylib.DrawText(Text, (int)Math.Round(textPosition.X), (int)Math.Round(textPosition.Y), fontSize, Color.White);
        // Raylib.DrawTextEx(Globals.GameFont, Text, Position, fontSize, 2, Color.White);
    }
}
