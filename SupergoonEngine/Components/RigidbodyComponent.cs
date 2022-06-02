using System;
using ImGuiNET.SampleProgram.XNA;
using Microsoft.Xna.Framework;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using SupergoonDashCrossPlatform.SupergoonEngine.Physics;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Components;

public class RigidbodyComponent : Component
{
    public bool GravityEnabled;
    private static Gravity _gravity;
    private BoxColliderComponent _collider;
    
    //How fast it is actually moving now.
    [ImGuiReadProperty("PlayerVelocity")]
    public Vector2 _velocity;
    //How much it is going to start moving (force)
    public Vector2 _acceleration;

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
        Debug = true;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        UpdateCollisionsThisFrame();
            _gravity.ApplyGravity(this, gameTime);
        ApplyVelocity(gameTime);
    }

    private void UpdateCollisionsThisFrame()
    {
        for (int i = 0; i < _collisionsThisFrame.Length; i++)
        {
            _collisionsLastFrame[i] = _collisionsThisFrame[i];
        }
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
        if (_collisionsLastFrame[directionInt])
            return false;
        return true;
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
                BottomCollisionJustStartedEvent?.Invoke();
                break;
            case Directions.Down when !collisionJustStarted:
                BottomCollisionEvent?.Invoke();
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
        var xStep = _velocity.X * gameTime.ElapsedGameTime.TotalSeconds;
        ApplyYVelocity(gameTime, yStep);
        ApplyXVelocity(gameTime, xStep);
    }

    private void ApplyYVelocity(GameTime gameTime, double yStep)
    {
        if (yStep >= 1)
        {
            while (yStep >= 1)
            {
                
                //Temporarily add 1 to Y and check for collisions
                Parent._location.Y++;
                bool collision = false;
                var tilesToCheck = _gravity._tiledGameComponent.LoadedTmxContent.SolidTiles;
                tilesToCheck.ForEach(solidTile =>
                {
                    if(collision)
                        return;
                    var tileCollider =
                        solidTile.GetComponent<BoxColliderComponent>(EngineTags.ComponentTags.BoxCollider);
                    var sourceRect = _collider.Bounds;
                    if (sourceRect.Intersects(tileCollider.Bounds))
                    {
                        yStep = 0;
                        collision = true;
                        Parent._location.Y--;
                        _velocity.Y = 0;
                        CollisionEvent(Directions.Down);
                    }
                });
                if(collision)
                    return;
                yStep--;
                // Parent._location.Y++;
            }
        }
        else if (yStep <= -1)
        {
            yStep++;
            Parent._location.Y--;
        }
    }

    private void ApplyXVelocity(GameTime gametime, double xStep)
    {
        if (xStep >= 1)
        {
            while (xStep >= 1)
            {
                // var tempLocation = Parent._location + offset;
                // tempLocation.X += 1;
                // bool collision = false;
                // var tilesToCheck = _gravity._tiledGameComponent.LoadedTmxContent.SolidTiles;
                // tilesToCheck.ForEach(solidTile =>
                // {
                //     if(collision)
                //         return;
                //     var tileCollider =
                //         solidTile.GetComponent<BoxColliderComponent>(EngineTags.ComponentTags.BoxCollider);
                //     var sourceRect = new Rectangle(tempLocation.ToPoint(), _collider._size);
                //     if (sourceRect.Intersects(tileCollider.Bounds))
                //     {
                //         xStep = 0;
                //         collision = true;
                //         CollisionEvent(Directions.Right);
                //     }
                //
                //
                // });
                // if(collision)
                //     return;
                xStep--;
                Parent._location.X++;
            }
        }
        else if (xStep <= -1)
        {
            xStep++;
            Parent._location.X--;
        }
        
    }

    public void AddForce(Vector2 force)
    {
        _velocity += force;
    }
}