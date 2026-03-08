using AntsShooter.Entities;
using AntsShooter.Systems;
using Raylib_cs;

namespace AntsShooter.Entities.UI;

public class DashBar : Entity
{
    private Color color = new Color(77, 155, 230);
    private Texture2D boarderTexture = Raylib.LoadTexture("assets/ui/bar/barBoarder.png");
    public int DashBarWidth { get; set; }

    public DashBar(int dashBarWidth, int dashBarHeight) : base()
    {
        Width = dashBarWidth;
        Height = dashBarHeight;

        Position.X = 10;
        Position.Y = Globals.VECTUAL_SCREEN_HEIGHT - Height * 3;

        this.DashBarWidth = Width;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Draw()
    {
        base.Draw();
        // Raylib.DrawRectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), Width, Height, Color.DarkGray);
        Raylib.DrawRectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), DashBarWidth, Height, color);
        Raylib.DrawTexture(boarderTexture, (int)Position.X - 1, (int)Position.Y - 1, Color.White);
    }
}
