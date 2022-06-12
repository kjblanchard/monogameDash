using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Documents;
using SupergoonDashCrossPlatform.SupergoonEngine.Components;
using TiledCS;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

public class Actor : GameObject
{
    protected RigidbodyComponent _rigidbodyComponent;
    protected SpriteComponent _spriteComponent;
    protected BoxColliderComponent _boxColliderComponent;
    protected AnimationComponent _animationComponent;
    protected AsepriteDocument _asepriteDocument;
    protected PlayerControllerComponent _playerControllerComponent;
    

    protected bool isFalling = true;
    protected bool _isFacingRight;
    
    protected int jumpHeight = 160;


    /// <summary>
    /// Base actor class, this is used for things that have a rigidbody and collisions/gravity, and animate with aseprite animations.  Also has a controller component so that it can be controlled.
    /// </summary>
    /// <param name="actorParams"></param>
    public Actor(ActorParams actorParams)
    {
        Location = actorParams.Location;
        _spriteComponent = new SpriteComponent(this);
        _boxColliderComponent = new BoxColliderComponent(this, actorParams.BoxSize ,actorParams.BoxColliderOffset);
        _rigidbodyComponent = new RigidbodyComponent(this, _boxColliderComponent, jumpHeight: jumpHeight);
        _asepriteDocument = _gameWorld.Content.Load<AsepriteDocument>($"Aseprite/{actorParams.AsepriteDocString}");
        _animationComponent = new AnimationComponent(this, _spriteComponent, _asepriteDocument);

        _playerControllerComponent = new PlayerControllerComponent(this, 0);
        AddComponent(_boxColliderComponent, _rigidbodyComponent, _animationComponent, _spriteComponent,
            _playerControllerComponent);
    }

    /// <summary>
    /// The factory function that is called on them, this is overrode/new on all derived actors.
    /// </summary>
    /// <param name="location"></param>
    /// <param name="tags"></param>
    /// <param name="textureRect"></param>
    /// <param name="texture"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static GameObject FactoryFunction(ActorParams actorParams)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Jump()
    {
        _rigidbodyComponent.Jump();
    }

    public override void Update(GameTime gameTime)
    {
        IsFacingRight();
        base.Update(gameTime);
    }

    private bool IsFacingRight()
    {
        if (_rigidbodyComponent._velocity.X != 0)
            if (!isFalling)
            {
                _isFacingRight = _rigidbodyComponent._velocity.X > 0;
                _spriteComponent.Mirror = !_isFacingRight;
            }

        return _isFacingRight;
    }
}