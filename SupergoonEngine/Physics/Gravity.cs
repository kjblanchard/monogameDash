using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SupergoonDashCrossPlatform.SupergoonEngine.Components;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using SupergoonEngine.Tiled;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Physics;

public class Gravity
{
    public TiledGameComponent _tiledGameComponent;
    public bool GravityEnabled = true;
    private float gravity = 550;
    private float minYVelocity = 0.01f;
    private float maxYVelocity = 135;

    public Gravity(TiledGameComponent tiledGameComponent)
    {
        _tiledGameComponent = tiledGameComponent;

    }

    public void ApplyGravity(RigidbodyComponent rigidbodyComponent, GameTime gameTime)
    {
        if (!GravityEnabled || !rigidbodyComponent.GravityEnabled)
            return;
        var gravityStep = gravity * gameTime.ElapsedGameTime.TotalSeconds;
        GravityConstraintY(rigidbodyComponent,gravityStep);
    }

    private void GravityConstraintY(RigidbodyComponent rigidbodyComponent, double gravityStep)
    {
        //Handle Y
        var velocityY = rigidbodyComponent._velocity.Y;
        var step = velocityY + gravityStep;
        if (velocityY >= 0)
        {
            if (step > maxYVelocity)
                step = maxYVelocity;
            if (step < minYVelocity)
                step = 0;
        }
        else
        {
            if (step < -maxYVelocity)
                step = -maxYVelocity;
            if (step > -minYVelocity)
                step = 0;
        }

        rigidbodyComponent._velocity.Y = (float)step;
    }
}