using System;
using Microsoft.Xna.Framework;
using SupergoonDashCrossPlatform.SupergoonEngine.Animation;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using SupergoonDashCrossPlatform.SupergoonEngine.Input;
using TiledCS;

namespace SupergoonDashCrossPlatform.Actors;

public class Player : Actor
{
    private const string IdleAnimString = "Idle";
    private const string FallingAnimString = "Fall";
    private const string JumpingAnimString = "Jump";
    private const string RunningAnimString = "Run";

    private bool isFalling = true;

    public Player(string asepriteDocString, Vector2 location, Vector2 boxColliderOffset = new Vector2(),
        Point boxSize = new Point()) : base(asepriteDocString, location, boxColliderOffset, boxSize)
    {
    }

    public new static Actor FactoryFunction(Vector2 loc, TiledProperty[] tags)
    {
        var player = new Player("player", loc, new Vector2(6, 10), new Point(20, 22));
        player.Initialize();
        return player;
    }

    public override void Initialize()
    {
        base.Initialize();
        _rigidbodyComponent.GravityEnabled = true;
        _spriteComponent.DrawOrder = 0.7f;
        AddAnimationTransitions();
        _rigidbodyComponent.BottomCollisionJustStartedEvent += OnJustHitGround;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (_playerControllerComponent.PlayerController.IsButtonPressed(ControllerButtons.Right) ||
            _playerControllerComponent.PlayerController.IsButtonHeld(ControllerButtons.Right))
        {
            var movementForce = new Vector2(10, 0);
            
            //If you arent moving, get a boost in speed
            if (_rigidbodyComponent._velocity.X == 0)
            {
                movementForce.X *= 5;
            }

            _rigidbodyComponent.AddForce(movementForce);
        }
        if (_playerControllerComponent.PlayerController.IsButtonPressed(ControllerButtons.Left) ||
            _playerControllerComponent.PlayerController.IsButtonHeld(ControllerButtons.Left))
        {
            var movementForce = new Vector2(-10, 0);
            
            //If you arent moving, get a boost in speed
            if (_rigidbodyComponent._velocity.X == 0)
            {
                movementForce.X *= 5;
            }

            _rigidbodyComponent.AddForce(movementForce);
        }

        if (_playerControllerComponent.PlayerController.IsButtonPressed(ControllerButtons.A))
        {
            _rigidbodyComponent.AddForce(new Vector2(0, -100));
        }
        else if (_playerControllerComponent.PlayerController.IsButtonHeld(ControllerButtons.A))
        {
            _rigidbodyComponent.AddForce(new Vector2(0, -20));
        }
    }

    //Events
    public void OnJustHitGround()
    {
        isFalling = false;
    }

    //Animations
    private void AddAnimationTransitions()
    {
        //Create the entry transition so we know where to start in the animation
        var entryTransition = new AnimationTransition(IdleAnimString, () => true);
        _animationComponent.SetAnimationEntryTransmission(entryTransition);

        //Create the animation
        var idleAnimation = new AnimationProperties(IdleAnimString);
        var fallingAnimation = new AnimationProperties(FallingAnimString, false);
        var runningAnimation = new AnimationProperties(RunningAnimString);
        //Create and add the transitions
        
        //Idle
        var idleToFallingTransition = new AnimationTransition(FallingAnimString, () => isFalling);
        var idleToRunTransition =
            // new AnimationTransition(RunningAnimString, () => _rigidbodyComponent._velocity.X != 0);
            new AnimationTransition(RunningAnimString,RunningToIdle);
        idleAnimation.Transitions.Add(idleToFallingTransition);
        idleAnimation.Transitions.Add(idleToRunTransition);

        //Falling
        var fallingToIdleTransition = new AnimationTransition(IdleAnimString, () => !isFalling);
        fallingAnimation.Transitions.Add(fallingToIdleTransition);
        
        //Running
        var runningToIdleTransition = new AnimationTransition(IdleAnimString, () => _rigidbodyComponent._velocity.X == 0);
        runningAnimation.Transitions.Add(runningToIdleTransition);

        //Add the animations to the animation component.
        _animationComponent.AddAnimationTransition(idleAnimation, fallingAnimation,runningAnimation);
    }

    public bool RunningToIdle()
    {
        if (_rigidbodyComponent._velocity.X != 0)
            return true;
        return false;
    }

}