using AttackOnSticks.Entities;
using AttackOnSticks.Systems;
using Raylib_cs;
using System.Numerics;

namespace AttackOnSticks.Entities.UI;

public class LifeBar : Entity
{
    private Vector2 screenPosition = new Vector2(Globals.VERTUAL_SCREEN_WIDTH / 20, Globals.VERTUAL_SCREEN_HEIGHT / 10);
    private Color color = new Color(234, 79, 54, 255);
    private Texture2D boarderTexture = Raylib.LoadTexture("assets/ui/bar/barBoarder.png");
    public int LifeBarHealthWidth { get; set; }

    public LifeBar(int lifeBarWidth, int lifeBarHeight) : base()
    {
        Width = lifeBarWidth;
        Height = lifeBarHeight;

        Position.X = 10;
        Position.Y = Globals.VERTUAL_SCREEN_HEIGHT - Height * 4 - 5;

        this.LifeBarHealthWidth = Width;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Draw()
    {
        base.Draw();
        // Raylib.DrawRectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), Width, Height, Color.DarkGray);
        Raylib.DrawRectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), LifeBarHealthWidth, Height, color);
        Raylib.DrawTexture(boarderTexture, (int)Position.X - 1, (int)Position.Y - 1, Color.White);
    }
}
