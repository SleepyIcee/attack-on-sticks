using System.Numerics;
using AntsShooter.Entities;
using AntsShooter.Entities.UI;
using AntsShooter.Systems;
using Raylib_cs;

namespace AntsShooter.Entities;

public class Player : Entity
{
    private Texture2D texture;
    private Gun gun = new Gun();
    private Dictionary<string, Animation> animations = new Dictionary<string, Animation>
    {
        {"idle" , new Animation("assets/player/idle")},
        {"run" , new Animation("assets/player/run")},
        {"jump" , new Animation("assets/player/jump")}
    };
    private Vector2 velocity;
    private const float speed = 50f;
    private const float friction = 10f;
    private readonly float MaxSpeed = 5f;
    private int facing = 1;
    public int health = 3;
    public int maxHealth = 3;
    private bool isDead = false;
    private const float timeBetweenDamages = 1.0f;
    public float damageTimer = timeBetweenDamages;

    private Sound runningSound;

    private const float jumpSpeed = 12.0f;
    private bool isJumping = false;
    private bool isFalling = false;

    public LifeBar lifeBar;
    public DashBar dashBar;

    private const float dashingSpeed = 10f;
    private bool dashing = false;
    private const float DashTime = 0.1f;
    private float dashTimer = DashTime;

    private bool canDash = true;
    private const float tilDashEnableTime = 3f;
    private float tilDashEnableTimer = tilDashEnableTime;

    // private Texture2D texture = Raylib.LoadTexture("assets/player/idle/player-idle.png");

    public Player() : base()
    {
        Width = Globals.PLAYER_WIDTH;
        Height = Globals.PLAYER_HEIGHT;
        Position.X = Globals.OriginPlayerPos.X;
        Position.Y = Globals.OriginPlayerPos.Y;
        velocity = Vector2.Zero;

        lifeBar = new LifeBar(Globals.VECTUAL_SCREEN_WIDTH / 5, Globals.VECTUAL_SCREEN_HEIGHT / 30);
        dashBar = new DashBar(Globals.VECTUAL_SCREEN_WIDTH / 5, Globals.VECTUAL_SCREEN_HEIGHT / 30);

        runningSound = Raylib.LoadSound("assets/sounds/running-sound.wav");
    }

    public void HandleMovement()
    {
        if (!dashing)
        {
            if (Raylib.IsKeyDown(KeyboardKey.D) && Position.X < Globals.MAP_WIDTH)
            {
                facing = 1;
                velocity.X += speed * Raylib.GetFrameTime();
                if (velocity.X > MaxSpeed) velocity.X = MaxSpeed;
                texture = animations["run"].Play(10);
                if (!Raylib.IsSoundPlaying(runningSound) && !isJumping && !isFalling) Raylib.PlaySound(runningSound);
            }
            else if (Raylib.IsKeyDown(KeyboardKey.A) && Position.X > 0)
            {
                facing = 0;
                velocity.X -= speed * Raylib.GetFrameTime();
                if (velocity.X < -MaxSpeed) velocity.X = -MaxSpeed;
                texture = animations["run"].Play(10);
                if (!Raylib.IsSoundPlaying(runningSound) && !isJumping && !isFalling) Raylib.PlaySound(runningSound);
            }
            else
            {
                SlowDown();
                texture = animations["idle"].Play(10);
            }
        }
    }

    public void HandleDash()
    {
        if (dashing)
        {
            if (facing == 1)
            {
                velocity.X += dashingSpeed;
            }
            else
            {
                velocity.X -= dashingSpeed;
            }

            if (dashTimer <= 0)
            {
                dashing = false;
                velocity.X = 0;
            }

            dashTimer -= Raylib.GetFrameTime();
        }
        else
        {
            if (Raylib.IsKeyPressed(KeyboardKey.Space) && canDash)
            {
                dashing = true;
                dashTimer = DashTime;
                canDash = false;
                tilDashEnableTimer = 0;
            }
        }

        if (!canDash)
        {
            if (tilDashEnableTimer >= tilDashEnableTime)
            {
                canDash = true;
            }
            else
            {
                tilDashEnableTimer += Raylib.GetFrameTime();
            }
        }
    }

    private void SlowDown()
    {
        if (Math.Abs(velocity.X) > 1)
        {
            velocity.X -= Math.Sign(velocity.X) * friction * Raylib.GetFrameTime();
        }
        else
        {
            velocity.X = 0;
        }
    }

    public void HandleJump()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.W) && !isJumping && !isFalling)
        {
            isJumping = true;
            velocity.Y = -jumpSpeed; // start upward velocity
        }

        if (isJumping)
        {
            if (Position.Y <= Globals.OriginPlayerPos.Y - 1 - Height)
            {
                isJumping = false;
                isFalling = true;
            }
        }

        if (isFalling)
        {
            velocity.Y += Globals.GRAVITY * Raylib.GetFrameTime();

            if (Position.Y >= Globals.OriginPlayerPos.Y - Height)
            {
                Position.Y = Globals.OriginPlayerPos.Y;
                velocity.Y = 0;
                isFalling = false;
            }
        }

        if (isJumping || isFalling)
        {
            texture = animations["jump"].Play(10);
        }
    }

    public override void Update()
    {
        base.Update();

        HandleMovement();
        HandleJump();
        HandleDash();
        Position += velocity;
        HandleDeath();
        gun.LookAtMouse(Position, Globals.mouseWorldPos);
        lifeBar.lifeBarHealthWidth = (int)(lifeBar.Width * (health / (float)maxHealth));
        dashBar.dashBarWidth = (int)(dashBar.Width * (tilDashEnableTimer / (float)tilDashEnableTime));

        // Console.WriteLine("player y pos: " + position.Y + " player origin y pos: " + (Globals.OriginPlayerPos.Y - height));
    }

    public void HandleDeath()
    {
        if (isDead == true)
        {
            States.StatesManager.currentState = "DeathState";
        }

        if (damageTimer > 0)
        {
            damageTimer -= Raylib.GetFrameTime();
        }
        // Console.WriteLine("Health: " + health);
    }

    public bool GetDamage(Ant ant)
    {
        if (Raylib.CheckCollisionRecs(new Rectangle(Position, new Vector2(Width, Height)),
            new Rectangle(ant.Position, new Vector2(ant.Width, ant.Height))) && damageTimer <= 0)
        {
            if (health > 0)
            {
                health -= 1;
            }
            else
            {
                isDead = true;
            }
            damageTimer = timeBetweenDamages;

            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Draw()
    {
        base.Draw();

        if (facing == 1)
        {
            Raylib.DrawTexture(texture, (int)MathF.Round(Position.X), (int)MathF.Round(Position.Y), Color.White);
        }
        else
        {
            //Raylib.DrawRectangle((int)MathF.Round(Position.X), (int)MathF.Round(Position.Y), Width, Height, Color.Red);
            Raylib.DrawTexturePro(texture, new Rectangle(0f, 0f, -Width, Height),
            new Rectangle(Position.X, Position.Y, Width, Height), Vector2.Zero, 0f, Color.White);
        }
        gun.Draw();
    }
}
