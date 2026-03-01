using System.Numerics;
using System.Threading.Channels;
using AntsShooter.Entities.UI;
using AntsShooter.Systems;
using Raylib_cs;

namespace AntsShooter.Entities;

public class Ant : Entity
{
    public float VelocityX = 0f;
    private Texture2D texture;
    private Dictionary<string, Animation> animations = new Dictionary<string, Animation>
    {
        {"run", ResourceManager.GetAnimation("enemy_run", "assets/enemy/run")}
    };
    private const float speed = 1500f;
    private const float maxSpeed = 200f;
    private int facing = 1;
    private Random random = new Random();
    private int spawnDirection;
    private int health = 10;
    private int maxHealth = 10;
    public bool IsDead = false;
    public LifeBar LifeBar;

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

        texture = animations["run"].Play(0);

        LifeBar = new LifeBar(50, 10);
    }

    public void Follow(Player player)
    {
        if (player.Position.X - player.Width > Position.X)
        {
            VelocityX += speed * Raylib.GetFrameTime();
            if (VelocityX > maxSpeed) VelocityX = maxSpeed;
            facing = 1;
            texture = animations["run"].Play(10);
        }
        else if (player.Position.X + player.Width < Position.X)
        {
            VelocityX -= speed * Raylib.GetFrameTime();
            if (VelocityX < -maxSpeed) VelocityX = -maxSpeed;
            facing = 0;
            texture = animations["run"].Play(10);
        }
    }

    public bool GetShot(Bullet bullet)
    {
        if (Raylib.CheckCollisionRecs(bullet.rectangle, new Rectangle(Position, new Vector2(Width, Height))))
        {
            if (health > 0)
            {
                health -= 1;
                LifeBar.LifeBarHealthWidth = (int)(LifeBar.Width * (health / (float)maxHealth));
            }
            else
            {
                IsDead = true;
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
        Position.X += VelocityX * Raylib.GetFrameTime();
        LifeBar.Position = Position + new Vector2(-25, -15);
    }

    public override void Draw()
    {
        if (facing == 1)
        {
            Raylib.DrawTexture(texture, (int)MathF.Round(Position.X), (int)MathF.Round(Position.Y), Color.White);
        }
        else
        {
            Raylib.DrawTexturePro(texture, new Rectangle(0f, 0f, -Width, Height),
            new Rectangle(Position.X, Position.Y, Width, Height), Vector2.Zero, 0f, Color.White);
        }
    }
}
