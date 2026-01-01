using Raylib_cs;
using System.Numerics;
using AntsShooter.Systems;

namespace AntsShooter.Entities;

public class Camera : Entity
{
    private Player player;
    public Camera2D camera;
    private Vector2 cameraTarget;
    private Vector2 cameraOffset = new();

    public bool isTimeToShake { get; set; } = false;
    private readonly float ShakeTime = 0.1f;
    private float shakeTimer = 0.1f;
    private List<Vector2> shakePoints = new();
    private int shakeIndex = 0;

    public Camera(Player GamePlayer) : base()
    {
        player = GamePlayer;

        cameraTarget = new Vector2();    
        camera = new Camera2D();
        camera.Target = cameraTarget;
        cameraOffset = new Vector2(Globals.SCREEN_WIDTH/2 - player.width/2, Globals.SCREEN_HEIGHT/2 - player.height/2);
        camera.Offset = cameraOffset;
        camera.Rotation = 0.0f;
        camera.Zoom = 1.0f;
    }

    public override void Update()
    {
        base.Update();

        cameraTarget.X = player.position.X;
        cameraTarget.Y = Globals.OriginPlayerPos.Y;
            
        float leftBound = camera.Target.X - camera.Offset.X;
        float rightBound = camera.Target.X + camera.Offset.X;

        if (leftBound < 0) camera.Target.X = camera.Offset.X;
        if (rightBound > Globals.MAP_WIDTH) camera.Target.X = Globals.MAP_WIDTH - camera.Offset.X;
            
        camera.Target = Vector2.Lerp(camera.Target, cameraTarget, 0.1f);

        // update UI position with camera
        player.lifeBar.UpdateScreenPosition(camera,
        new Vector2(Globals.SCREEN_WIDTH/20, Globals.SCREEN_HEIGHT/10));

        // shake logic
        if (isTimeToShake)
        {
            if (shakeTimer > 0)
            {
                camera.Offset = cameraOffset + shakePoints[shakeIndex];
                shakeTimer -= Raylib.GetFrameTime();
            }
            else
            {
                shakeIndex++;
                if (shakeIndex >= shakePoints.Count)
                {
                    isTimeToShake = false;
                    shakeIndex = 0;
                    camera.Offset = cameraOffset;
                }
                else
                {
                    shakeTimer = ShakeTime;
                }
            }
        }
    }

    public void Shake(List<Vector2> cameraShakePoints)
    {
        isTimeToShake = true;
        shakePoints = cameraShakePoints;
    }

    public override void Draw()
    {
        base.Draw();
    }
}