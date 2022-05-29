using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using SupergoonDashCrossPlatform.SupergoonEngine.Physics;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Components;

public class RigidbodyComponent : Component
{
    public bool GravityEnabled;
    private static Gravity _gravity;
    private BoxColliderComponent _collider;
    public Vector2 _velocity;

    public event BottomCollisionEventArgs BottomCollisionJustStartedEvent;
    public event BottomCollisionEventArgs BottomCollisionEvent;

    public delegate void BottomCollisionEventArgs();

    private static readonly int _directionsToCheck = Enum.GetNames(typeof(Directions)).Length;
    private bool[] _collisionsLastFrame = new bool[_directionsToCheck];
    private bool[] _collisionsThisFrame = new bool[_directionsToCheck];

    public RigidbodyComponent(GameObject parent, BoxColliderComponent collider, bool gravityEnabled = true) : base(
        parent, new Vector2())
    {
        GravityEnabled = gravityEnabled;
        UpdateOrder = EngineTags.ComponentTags.RigidBody;
        _gravity ??= GameObject._gameWorld.PhysicsGameComponent.Gravity;
        _collider = collider;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        UpdateCollisionsThisFrame();
        if (GravityEnabled)
            _gravity.ApplyGravity(this, gameTime);
        ApplyVelocity(gameTime);
    }

    private void UpdateCollisionsThisFrame()
    {
        _collisionsLastFrame = _collisionsThisFrame;
        for (int i = 0; i < _directionsToCheck; i++)
            _collisionsThisFrame[i] = false;
    }

    private void CollisionEvent(Directions direction)
    {
        var directionInt = (int)direction;
        _collisionsThisFrame[directionInt] = true;
        var collisionJustStarted = CheckIfCollisionJustStarted(directionInt);
        ThrowDirectionCollisionEvent(direction, collisionJustStarted);
    }
    
    private bool CheckIfCollisionJustStarted(int directionInt)
    {
        return !_collisionsLastFrame[directionInt];
    }

    private void ThrowDirectionCollisionEvent(Directions direction, bool collisionJustStarted)
    {
        switch (direction)
        {
            case Directions.Top:
                break;
            case Directions.Right:
                break;
            case Directions.Down when collisionJustStarted:
                BottomCollisionJustStartedEvent.Invoke();
                break;
            case Directions.Down when !collisionJustStarted:
                BottomCollisionEvent.Invoke();
                break;
            case Directions.Left:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }



    private void ApplyVelocity(GameTime gameTime)
    {
        var yStep = _velocity.Y * gameTime.ElapsedGameTime.TotalSeconds;
        if (yStep >= 1)
        {
            while (yStep >= 1)
            {
                yStep--;
                Parent._location.Y++;
            }
        }
        else if (yStep <= -1)
        {
            yStep++;
            Parent._location.Y--;
        }
    }
}