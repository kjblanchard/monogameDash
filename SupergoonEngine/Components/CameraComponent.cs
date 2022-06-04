using Microsoft.Xna.Framework;
using SupergoonDashCrossPlatform.SupergoonEngine.Cameras;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Components;

public class CameraComponent : Component
{
    private CameraBoom _cameraBoom;
    public CameraComponent(GameObject parent, Vector2 offset = new Vector2()) : base(parent, offset)
    {
        
        
    }


    public override void Initialize()
    {
        base.Initialize();
        _cameraBoom = new CameraBoom(Parent);
        
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _cameraBoom.KeepCameraAtHalfScreen();
    }
}