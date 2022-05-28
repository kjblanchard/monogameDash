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

    public RigidbodyComponent(GameObject parent, BoxColliderComponent collider, bool gravityEnabled = true) : base(parent, new Vector2())
    {
        GravityEnabled = gravityEnabled;
        UpdateOrder = EngineTags.ComponentTags.RigidBody;
        _gravity ??= GameObject._gameWorld.PhysicsGameComponent.Gravity;
        _collider = collider;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if(GravityEnabled)
            _gravity.ApplyGravity(this,gameTime);
        ApplyVelocity(gameTime);
        
    }

    private void ApplyVelocity(GameTime gameTime)
    {
        var yStep = _velocity.Y * gameTime.ElapsedGameTime.TotalSeconds;
        if (yStep >= 1)
        {
            //Handle movement / collision
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
            //Handle movement / collision
        }

    }
}