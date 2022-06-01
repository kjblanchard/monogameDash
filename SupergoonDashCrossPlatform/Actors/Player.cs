using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Animation;
using SupergoonDashCrossPlatform.SupergoonEngine.Components;
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
    public Player(string asepriteDocString, Vector2 location, Vector2 boxColliderOffset = new Vector2(), Point boxSize = new Point()) : base(asepriteDocString, location, boxColliderOffset, boxSize)
    {
    }
    
    public new static Actor FactoryFunction(Vector2 loc, TiledProperty[] tags)
    {
        var player = new Player("player", loc, new Vector2(), new Point(32, 32));
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
        if (_playerControllerComponent.PlayerController.IsButtonPressed(ControllerButtons.A))
        {
            Console.WriteLine("Pressed");
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
        var entryTransition = new AnimationTransition( IdleAnimString, IdleTransition);
        _animationComponent.SetAnimationEntryTransmission(entryTransition);
        
        //Create the animation
        var idleAnimation = new AnimationProperties(IdleAnimString);
        var fallingAnimation = new AnimationProperties(FallingAnimString, false);
        //Create and add the transitions
        var idleToFallingTransition = new AnimationTransition( FallingAnimString, FallingTransition);
        idleAnimation.Transitions.Add(idleToFallingTransition);

        var fallingToIdleTransition = new AnimationTransition(IdleAnimString, FallingToIdleTransition);
        fallingAnimation.Transitions.Add(fallingToIdleTransition);
        
        //Add the animations to the animation component.
        _animationComponent.AddAnimationTransition(idleAnimation, fallingAnimation);
    }

    private bool IdleTransition()
    {
        return true;
    }

    private bool FallingTransition()
    {
        return isFalling;
    }
    private bool FallingToIdleTransition()
    {
        return !isFalling;
    }

}