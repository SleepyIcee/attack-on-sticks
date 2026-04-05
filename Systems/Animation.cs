using Raylib_cs;


namespace AttackOnSticks.Systems;

public class Animation
{
    private List<Texture2D> images = new();
    private const float FrameTime = 1.0f;
    private float frameTimer = FrameTime;
    private int currentFrame = 0;
    public bool Replay = true;
    private bool _playOnce = false;

    public Animation(string imagesDir, bool playOnce = false)
    {
        foreach(var image in Directory.GetFiles(imagesDir))
        {
            images.Add(Raylib.LoadTexture(image));
        }

        _playOnce = playOnce;
    }

    public Texture2D Play(float animationSpeed)
    {
        if (frameTimer <= 0 && Replay == true)
        {
            if (currentFrame < images.Count() - 1)
            {
                currentFrame++;
            }
            else
            {
                if (_playOnce)
                {
                    Replay = false;
                }

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
