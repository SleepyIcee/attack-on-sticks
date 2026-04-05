using Raylib_cs;

namespace AttackOnSticks.Systems;

public static class ResourceManager
{
    private static Dictionary<string, Animation> animations = new();
    private static Dictionary<string, Sound> sounds = new();

    // Load animation once and cache it
    public static Animation GetAnimation(string name, string path, bool playOnce = false)
    {
        if (!animations.ContainsKey(name))
        {
            animations[name] = new Animation(path, playOnce);
        }
        return animations[name];
    }

    // Load sound once and cache it
    public static Sound GetSound(string name, string path)
    {
        if (!sounds.ContainsKey(name))
        {
            sounds[name] = Raylib.LoadSound(path);
        }
        return sounds[name];
    }

    // Unload all resources when closing
    public static void UnloadAll()
    {
        foreach (var sound in sounds.Values)
        {
            Raylib.UnloadSound(sound);
        }
        sounds.Clear();
        animations.Clear();
    }
}
