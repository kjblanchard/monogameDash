using System.Collections.Generic;
using ImGuiNET.SampleProgram.XNA;
using Microsoft.Xna.Framework;
using SupergoonDashCrossPlatform.SupergoonEngine.Components;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using SupergoonDashCrossPlatform.SupergoonEngine.Interfaces;
using SupergoonEngine.Tiled;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Physics;

public class Gravity : IDebug, IInitialize
{
    public TiledGameComponent _tiledGameComponent;
    public bool GravityEnabled = true;

    [ImGuiWrite(typeof(float), true, "Gravity", Min = 0, Max = 2000)]
    private float gravity = 550;

    [ImGuiWrite(typeof(float), true, "Friction", Min = 0, Max = 1000)]
    private float friction = 150;

    [ImGuiWrite(typeof(float), true, "Min Y velocity", Min = 0, Max = 1)]
    private float minYVelocity = 0.1f;

    [ImGuiWrite(typeof(float), true, "max Y velocity", Min = 0, Max = 1)]
    private float maxYVelocity = 400;

    [ImGuiWrite(typeof(float), true, "Max x velocity", Min = 0, Max = 1)]
    private float minXVelocity = 0.1f;

    [ImGuiWrite(typeof(float), true, "Max x velocity", Min = 0, Max = 1)]
    private float maxXVelocity = 200;

    public Gravity(TiledGameComponent tiledGameComponent)
    {
        Debug = true;
        _tiledGameComponent = tiledGameComponent;
    }

    public void ApplyGravity(RigidbodyComponent rigidbodyComponent, GameTime gameTime)
    {
        if (!GravityEnabled || !rigidbodyComponent.GravityEnabled)
            return;
        var gravityStep = gravity * gameTime.ElapsedGameTime.TotalSeconds;
        var frictionStep = 0.0;
        if (rigidbodyComponent.FrictionEnabled)
            frictionStep = friction * gameTime.ElapsedGameTime.TotalSeconds;
        
        var overrideSpeed = 0f;
        if (rigidbodyComponent._gravityXOverride)
        {
            overrideSpeed += rigidbodyComponent._gravityXSpeedOverride;

        }
        GravityConstraintY(rigidbodyComponent, gravityStep);
        GravityConstraintX(rigidbodyComponent, frictionStep, overrideSpeed);
    }

    private void GravityConstraintX(RigidbodyComponent rigidbodyComponent, double fric, float overrideMax)
    {
        if (rigidbodyComponent._velocity.X == 0)
            return;
        double step;
        double max = maxXVelocity + overrideMax;

        if (rigidbodyComponent._velocity.X > 0)
        {
            step = rigidbodyComponent._velocity.X - fric;
            if (step > max)
                step = max;
            if (step < minXVelocity)
                step = 0;
        }
        else
        {
            step = rigidbodyComponent._velocity.X + fric;
            if (step < -max)
                step = -max;
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

    public bool Debug { get; set; }

    public void SendAttributesToDebugger(object parent)
    {
        if (Debug)
        {
            ImGuiGameComponent.Instance.CheckObjectForDebugAttributes(parent);
        }
    }

    public bool IsInitialized { get; set; }

    public void Initialize()
    {
        SendAttributesToDebugger(this);
    }
}