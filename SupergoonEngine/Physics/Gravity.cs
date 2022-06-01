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
    private float gravity = 800;
    private float friction = 80;
    private float minYVelocity = 0.01f;
    private float maxYVelocity = 135;
    private float minXVelocity = 0.9f;
    private float maxXVelocity = 135;

    public Gravity(TiledGameComponent tiledGameComponent)
    {
        _tiledGameComponent = tiledGameComponent;
    }

    public void ApplyGravity(RigidbodyComponent rigidbodyComponent, GameTime gameTime)
    {
        if (!GravityEnabled || !rigidbodyComponent.GravityEnabled)
            return;
        var gravityStep = gravity * gameTime.ElapsedGameTime.TotalSeconds;
        var frictionStep = friction * gameTime.ElapsedGameTime.TotalSeconds;
        GravityConstraintY(rigidbodyComponent, gravityStep);
        GravityConstraintX(rigidbodyComponent,frictionStep);
    }

    private void GravityConstraintX(RigidbodyComponent rigidbodyComponent, double fric)
    {
        if (rigidbodyComponent._velocity.X == 0)
            return;
        double step;

        if (rigidbodyComponent._velocity.X > 0)
        {
            step = rigidbodyComponent._velocity.X - fric;
            if (step > maxXVelocity)
                step = maxXVelocity;
            if (step < minXVelocity)
                step = 0;
        }
        else
        {
            step = rigidbodyComponent._velocity.X + fric;
            if (step < -maxXVelocity)
                step = -maxXVelocity;
            if (step > -minXVelocity)
                step = 0;
        }

        rigidbodyComponent._velocity.X = (float)step;
    }

    private void GravityConstraintY(RigidbodyComponent rigidbodyComponent, double gravityStep)
    {
        var velocityY = rigidbodyComponent._velocity.Y;
        double step;
        if (velocityY >= 0)
        {
            step = velocityY + gravityStep;
            if (step > maxYVelocity)
                step = maxYVelocity;
            if (step < minYVelocity)
                step = 0;
        }
        else
        {
            step = velocityY + gravityStep;
            if (step < -maxYVelocity)
                step = -maxYVelocity;
            if (step > -minYVelocity)
                step = 0;
        }

        rigidbodyComponent._velocity.Y = (float)step;
    }
}