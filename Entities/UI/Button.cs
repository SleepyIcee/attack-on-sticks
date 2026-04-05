using AttackOnSticks.Entities;
using AttackOnSticks.Systems;
using Raylib_cs;
using System.Net;
using System.Numerics;

namespace AttackOnSticks.Entities.UI;

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

    private int textureWidth;
    private int textureHeight;
    private Vector2 texturePosition;

    public Button(string text, Vector2 position, int width, int height) : base()
    {
        Position = position;
        texturePosition = Position;
        Width = width;
        Height = height;
        textureWidth = Width;
        textureHeight = Height;
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
        if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition()/Globals.VERTUAL_SCREEN_SCALING,
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

        // Keep hover state in sync with the latest mouse collision check (IsClicked).
        // This makes the button visually respond to mouse-over even if the user is not clicking.
        // Preserve any hover state set by keyboard navigation in the parent state.
        IsHovered = IsHovered || MouseHovered;

        if (IsHovered)
        {
            texture = animations["hovered"].Play(0);
            textureWidth = Width + 4;
            textureHeight = Height + 4;
            texturePosition = new Vector2(Position.X - 2, Position.Y - 2);
        }
        else
        {
            texture = animations["idle"].Play(0);
            textureWidth = Width;
            textureHeight = Height;
            texturePosition = Position;
        }
    }

    public override void Draw()
    {
        base.Draw();
        

        Raylib.DrawTexturePro(texture,
        new Rectangle(0, 0, texture.Width, texture.Height),
        new Rectangle(texturePosition.X, texturePosition.Y, textureWidth, textureHeight),
        Vector2.Zero, 0, Color.White);
        
        Raylib.DrawText(Text, (int)Math.Round(textPosition.X), (int)Math.Round(textPosition.Y), fontSize, Color.White);
        // Raylib.DrawTextEx(Globals.GameFont, Text, Position, fontSize, 2, Color.White);
    }
}
