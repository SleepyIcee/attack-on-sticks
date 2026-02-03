using System.Numerics;
using System.Threading.Channels;
using AntsShooter.Enities.UI;
using AntsShooter.Systems;
using Raylib_cs;

namespace AntsShooter.Entities;

public class Ant : Entity
{
    public float velocityX = 0f;
    private const float speed = 250f;
    private const float maxSpeed = 3f;
    private int facing = 1;
    private Random random = new Random();
    private int spawnDirection;
    private int health = 10;
    private int maxHealth = 10; 
    public bool isDead = false; 
    public LifeBar lifeBar;
    
    public Ant() : base()
    {
        Width = Globals.PLAYER_WIDTH;
        Height = Globals.PLAYER_HEIGHT;
        Position.Y = Globals.GROUND_LEVEL - Height;
        spawnDirection = random.Next(0, 2);
        if (spawnDirection == 0)
        {
            Position.X = 0 - 100;
        }
        else
        {
            Position.X = Globals.MAP_WIDTH + 100;
        }

        lifeBar = new LifeBar(50, 10);
    }

    public void Follow(Player player)
    {
        if (player.Position.X - player.Width > Position.X)
        {
            velocityX += speed * Raylib.GetFrameTime();
            if (velocityX > maxSpeed) velocityX = maxSpeed;
            facing = 1;
        }
        else if (player.Position.X + player.Width < Position.X)
        {
            velocityX -= speed * Raylib.GetFrameTime();
            if (velocityX < -maxSpeed) velocityX = -maxSpeed;
            facing = 0;
        }
    }

    public bool GetShot(Bullet bullet)
    {
        if (Raylib.CheckCollisionRecs(bullet.rectangle, new Rectangle(Position, new Vector2(Width, Height))))
        {
            if (health > 0)
            {
                health -= 1;
                lifeBar.lifeBarHealthWidth = (int)(lifeBar.Width * (health / (float)maxHealth));
            }
            else
            {
                isDead = true;
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Update()
    {
        Position.X += velocityX;
        lifeBar.Position = Position + new Vector2(-25, -15);
    }

    public override void Draw()
    {
        Raylib.DrawRectangle((int)MathF.Round(Position.X), (int)MathF.Round(Position.Y), Width, Height, Color.Blue);
    }
}