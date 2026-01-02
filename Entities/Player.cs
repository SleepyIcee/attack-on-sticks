using System.Numerics;
using AntsShooter.Enities.UI;
using AntsShooter.Systems;
using Raylib_cs;

namespace AntsShooter.Entities;

public class Player : Entity
{
    private Vector2 velocity;
    private const float speed = 50f;
    private const float friction = 0.1f;
    private readonly float MaxSpeed = 5f;
    private int facing = 1;
    public int health = 3;
    public int maxHealth = 3;
    private bool isDead = false;
    private const float timeBetweenDamages = 1.0f;
    public float damageTimer = timeBetweenDamages;

    private const float jumpSpeed = 20f;
    private bool isJumping = false;
    private bool isFalling = false;
    private const int jumpHight = 2;

    public LifeBar lifeBar;

    public Player() : base()
    {
        width = Globals.PLAYER_WIDTH;
        height = Globals.PLAYER_HEIGHT;
        position.X = Globals.OriginPlayerPos.X;
        position.Y = Globals.OriginPlayerPos.Y;
        
        velocity = Vector2.Zero;

        lifeBar = new LifeBar(Globals.SCREEN_WIDTH/5, Globals.SCREEN_HEIGHT/30);;
    }
    
    public void HandelMovement()
    {
        if (Raylib.IsKeyDown(KeyboardKey.D) && position.X < Globals.MAP_WIDTH)
        {
            velocity.X += speed * Raylib.GetFrameTime();
            if (velocity.X > MaxSpeed) velocity.X = MaxSpeed;
            facing = 1;
        }
        else if (Raylib.IsKeyDown(KeyboardKey.A) && position.X > 0)
        {
            velocity.X -= speed * Raylib.GetFrameTime();
            if (velocity.X < -MaxSpeed) velocity.X = -MaxSpeed;
            facing = 0;
        }
        else
        {
            if (Math.Abs(velocity.X) > 10)
            {
                velocity.X -= Math.Sign(velocity.X) * friction * Raylib.GetFrameTime();
            }
            else
            {
                velocity.X = 0;
            }
        }
        
        // Console.WriteLine(isJumping);
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
            if (position.Y <= Globals.OriginPlayerPos.Y - 1 - height)
            {
                isJumping = false;
                isFalling = true;
            }
        }

        if (isFalling)
        {
            velocity.Y += Globals.GRAVITY * Raylib.GetFrameTime();

            if (position.Y >= Globals.OriginPlayerPos.Y - height)
            {
                position.Y = Globals.OriginPlayerPos.Y;
                velocity.Y = 0;
                isFalling = false;
            }
        }
    }
    
    public override void Update()
    {
        base.Update();
        
        HandelMovement();
        HandleJump();
        position += velocity;
        HandelDeath();
        lifeBar.lifeBarHealthWidth = (int)(lifeBar.width * (health / (float)maxHealth));

        // Console.WriteLine("player y pos: " + position.Y + " player origin y pos: " + (Globals.OriginPlayerPos.Y - height));
    }

    public void HandelDeath()
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
        if (Raylib.CheckCollisionRecs(new Rectangle(position, new Vector2(width, height)),
            new Rectangle(ant.position, new Vector2(ant.width, ant.height))) && damageTimer <= 0)
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
            Raylib.DrawRectangle((int)MathF.Round(position.X), (int)MathF.Round(position.Y), width, height, Color.Red);
        }
        else
        {
            Raylib.DrawRectangle((int)MathF.Round(position.X), (int)MathF.Round(position.Y), width, height, Color.Red);
            // draw the flipped version of the texture
        }
    }
}
