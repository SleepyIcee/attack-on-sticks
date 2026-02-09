using Raylib_cs;


namespace AntsShooter.Systems;

public class Animation
{
    private List<Texture2D> images = new();
    private const float FrameTime = 1.0f;
    private float frameTimer = FrameTime;
    private int currentFrame = 0;

    public Animation(string imagesDir)
    {
        foreach(var image in Directory.GetFiles(imagesDir))
        {
            images.Add(Raylib.LoadTexture(image));
        }
    }

    public Texture2D Play(float animationSpeed)
    {
        if (frameTimer <= 0)
        {
            if (currentFrame < images.Count() - 1)
            {
                currentFrame++;
            }
            else
            {
                currentFrame = 0;
            }

            frameTimer = FrameTime;
        }
        else
        {
            frameTimer -= animationSpeed * Raylib.GetFrameTime();
        }

        return images[currentFrame];
    }
}