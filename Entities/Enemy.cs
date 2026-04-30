using System.Numerics;
using System.Threading.Channels;
using AttackOnSticks.Entities.UI;
using AttackOnSticks.Systems;
using Raylib_cs;

namespace AttackOnSticks.Entities;

public class Enemy : Entity
{
    public float VelocityX = 0f;
    private Texture2D texture;
    private Dictionary<string, Animation> animations = new Dictionary<string, Animation>
    {
        {"run", ResourceManager.GetAnimation("enemy_run", "assets/enemy/run")},
        {"dying", ResourceManager.GetAnimation("enemy_dying", "assets/enemy/dying")}
    };
    private const float speed = 1500f;
    private const float maxSpeed = 200f;
    private int facing = 1;
    private Random random = new Random();
    private int spawnDirection;
    public int Health = 10;
    public int MaxHealth = 10;
    public Color Color = Color.White;
    public bool IsDead = false;
    public bool IsDying = false;
    private const float timeToDie = 30.0f;
    private float dyingTimer = timeToDie;

    private Sound dyingSound = ResourceManager.GetSound("dying_sound", "assets/sounds/throat.mp3");
    private Sound bloodySound = ResourceManager.GetSound("bloody_sound", "assets/sounds/combat-bloody.wav");

    // public LifeBar LifeBar;

    public Enemy() : base()
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

        // LifeBar = new LifeBar(50, 10);
    }

    public void Follow(Player player)
    {
        if (IsDying || IsDead)
        {
            return;
        }

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
        if (IsDying || IsDead)
        {
            return false;
        }

        if (!Raylib.CheckCollisionRecs(bullet.rectangle, new Rectangle(Position, new Vector2(Width, Height))))
        {
            return false;
        }

        Health -= 1;
        if (Health <= 0)
        {
            Health = 0;
            IsDying = true;
            dyingTimer = timeToDie;
            Raylib.PlaySound(dyingSound);
        }
        else
        {
            int greenAndBlueValue = (int)(Globals.maxColorValue * (Health / (float)MaxHealth));
            Color = new Color(Globals.maxColorValue, greenAndBlueValue, greenAndBlueValue);
        }

        Raylib.PlaySound(bloodySound);

        return true;
    }

    public override void Update()
    {
        Position.X += VelocityX * Raylib.GetFrameTime();
        // LifeBar.Position = Position + new Vector2(-25, -15);

        if (IsDying)
        {
            // LifeBar.LifeBarHealthWidth = 0;
            VelocityX = 0;
            texture = animations["dying"].Play(0);

            if (dyingTimer > 0)
            {
                dyingTimer -= Raylib.GetFrameTime();
            }
            else
            {
                IsDead = true;
            }
        }
    }

    public override void Draw()
    {
        if (facing == 1)
        {
            Raylib.DrawTexture(texture, (int)MathF.Round(Position.X), (int)MathF.Round(Position.Y), Color);
        }
        else
        {
            Raylib.DrawTexturePro(texture, new Rectangle(0f, 0f, -Width, Height),
            new Rectangle(Position.X, Position.Y, Width, Height), Vector2.Zero, 0f, Color);
        }
    }
}
