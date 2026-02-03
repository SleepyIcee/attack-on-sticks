using Raylib_cs;


namespace AntsShooter.Systems;

public class Animation
{
    private Texture2D image = new();
    private const float FrameTime = 10.0f;
    private float frameTimer = FrameTime;
    private int currentFrame = 1;
    private int framesCount = 1;
    private Rectangle frameRect = new();

    public Animation(Texture2D framesImage, Rectangle rect, int rectsCount)
    {
        image = framesImage;
        frameRect = rect;
        framesCount = rectsCount;
    }

    public Rectangle Play(float animationSpeed)
    {
        if (frameTimer <= 0)
        {
            if (currentFrame < framesCount)
            {
                currentFrame++;
            }
        }
        else
        {
            frameTimer = FrameTime;
        }

        return new Rectangle {};
    }
}