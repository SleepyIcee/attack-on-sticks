using System.Numerics;
using System.Threading.Channels;
using AntsShooter.Enities.UI;
using AntsShooter.Systems;
using Raylib_cs;

namespace AntsShooter.Entities;

public class Ant : Entity
{
    private float velocityX = 0f;
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
        position.Y = Globals.GROUND_LEVEL - height;
        spawnDirection = random.Next(0, 2);
        if (spawnDirection == 0)
        {
            position.X = 0 - 100;
        }
        else
        {
            position.X = Globals.MAP_WIDTH + 100;
        }

        lifeBar = new LifeBar(50, 10);
    }

    public void Follow(Player player)
    {
        if (player.position.X - player.width > position.X)
        {
            velocityX += speed * Raylib.GetFrameTime();
            if (velocityX > maxSpeed) velocityX = maxSpeed;
            facing = 1;
        }
        else if (player.position.X + player.width < position.X)
        {
            velocityX -= speed * Raylib.GetFrameTime();
            if (velocityX < -maxSpeed) velocityX = -maxSpeed;
            facing = 0;
        }
    }

    public bool GetShot(Bullet bullet)
    {
        if (Raylib.CheckCollisionCircleRec(bullet.position, bullet.radius, new Rectangle(position, new Vector2(width, height))))
        {
            if (health > 0)
            {
                health -= 1;
                lifeBar.lifeBarHealthWidth = (int)(lifeBar.width * (health / (float)maxHealth));
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
        position.X += velocityX;
        lifeBar.position = position + new Vector2(-25, -15);
    }

    public override void Draw()
    {
        Raylib.DrawRectangle((int)MathF.Round(position.X), (int)MathF.Round(position.Y), 50, 50, Color.Blue);
    }
}