using System;
using ImGuiNET.SampleProgram.XNA;
using Microsoft.Xna.Framework;
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

    [ImGuiWrite(typeof(float), true, "slow run vel", Min = 0, Max = 200)]
    private float slowRunTreshold = 100;

    [ImGuiWrite(typeof(float), true, "Slow run anim speed", Min = 0, Max = 5)]
    private float slowRunAnimSpeed = 0.7f;
    [ImGuiWrite(typeof(float), true, "fast run vel", Min = 0, Max = 100)]
    private float fastRunTreshold = 200;

    private float fastRunAnimSpeed = 1.30f;

    [ImGuiWrite(typeof(float), true, "Run speed", Min = 0, Max = 20)]
    private float runSpeed = 6.5f;

    private float jumpAddition = 10;

    private float _jumpLengthMax = 0.245f;

    private float _jumpLength;

    private CameraComponent _cameraComponent;

    public Player(string asepriteDocString, Vector2 location, Vector2 boxColliderOffset = new Vector2(),
        Point boxSize = new Point()) : base(asepriteDocString, location, boxColliderOffset, boxSize)
    {
    }

    public new static Actor FactoryFunction(Vector2 loc, TiledProperty[] tags)
    {
        var player = new Player("player", loc, new Vector2(6, 10), new Point(20, 22));
        player.Debug = true;
        player.Initialize();

        return player;
    }

    public override void Initialize()
    {
        _cameraComponent = new CameraComponent(this);
        // _cameraComponent.Initialize();
        AddComponent(_cameraComponent);
        _rigidbodyComponent.GravityEnabled = true;
        _spriteComponent.DrawOrder = 0.7f;
        AddAnimationTransitions();
        _rigidbodyComponent.BottomCollisionJustStartedEvent += OnJustHitGround;
        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (_playerControllerComponent.PlayerController.IsButtonPressed(ControllerButtons.Right) ||
            _playerControllerComponent.PlayerController.IsButtonHeld(ControllerButtons.Right))
        {
            var movementForce = new Vector2(runSpeed, 0);

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
            var movementForce = new Vector2(-runSpeed, 0);

            //If you arent moving, get a boost in speed
            if (_rigidbodyComponent._velocity.X == 0)
            {
                movementForce.X *= 5;
            }

            _rigidbodyComponent.AddForce(movementForce);
        }

        if (_playerControllerComponent.PlayerController.IsButtonPressed(ControllerButtons.A))
        {
            if (!isFalling)
            {
                Jump();
                isFalling = true;
            }
        }
        else if (_playerControllerComponent.PlayerController.IsButtonHeld(ControllerButtons.A))
        {
            if (_jumpLength <= _jumpLengthMax)
            {
                _rigidbodyComponent.AddForce(new Vector2(0, -jumpAddition));
                _jumpLength += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        if (_rigidbodyComponent._velocity.X > fastRunTreshold || _rigidbodyComponent._velocity.X < -fastRunTreshold)
            _animationComponent._animationSpeed = fastRunAnimSpeed;
        else if (_rigidbodyComponent._velocity.X < slowRunTreshold ||
                 _rigidbodyComponent._velocity.X > -slowRunTreshold)
            _animationComponent._animationSpeed = slowRunAnimSpeed;
        else
        {
            _animationComponent._animationSpeed = 1.0f;
        }
    }

    //Events
    public void OnJustHitGround()
    {
        isFalling = false;
    }

    public override void Jump()
    {
        base.Jump();
        _animationComponent.ChangeAnimation(JumpingAnimString);
        _jumpLength = 0;
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
        var jumpingAnimation = new AnimationProperties(JumpingAnimString, false);
        //Create and add the transitions

        //Idle
        var idleToFallingTransition = new AnimationTransition(FallingAnimString, () => isFalling);
        var idleToRunTransition =
            // new AnimationTransition(RunningAnimString, () => _rigidbodyComponent._velocity.X != 0);
            new AnimationTransition(RunningAnimString, RunningToIdle);
        idleAnimation.Transitions.Add(idleToFallingTransition);
        idleAnimation.Transitions.Add(idleToRunTransition);
        
        //Jumping
        var jumpingToFallingTransition =
            new AnimationTransition(FallingAnimString, () => _playerControllerComponent.PlayerController.IsButtonReleased(ControllerButtons.A) || _jumpLength >= _jumpLengthMax);
        jumpingAnimation.Transitions.Add(jumpingToFallingTransition);

        //Falling
        var fallingToIdleTransition = new AnimationTransition(IdleAnimString, () => !isFalling);
        fallingAnimation.Transitions.Add(fallingToIdleTransition);

        //Running
        var runningToIdleTransition =
            // new AnimationTransition(IdleAnimString, () => _rigidbodyComponent._velocity.X == 0);
            new AnimationTransition(IdleAnimString, () => (_rigidbodyComponent._velocity.X < 55 &&  _rigidbodyComponent._velocity.X>0) 
                                                          || (_rigidbodyComponent._velocity.X > -55 && _rigidbodyComponent._velocity.X < 0) ||
                                                          _rigidbodyComponent._velocity.X == 0);
        var runningToJumpingTransition = new AnimationTransition(FallingAnimString, () => isFalling);
        runningAnimation.Transitions.Add(runningToIdleTransition);
        runningAnimation.Transitions.Add(runningToJumpingTransition);


        //Add the animations to the animation component.
        _animationComponent.AddAnimationTransition(idleAnimation, fallingAnimation, runningAnimation, jumpingAnimation);
    }

    public bool RunningToIdle()
    {
        // if (_rigidbodyComponent._velocity.X != 0)
        //Handle staying in animation too long.
        if (_rigidbodyComponent._velocity.X > 55 && _rigidbodyComponent._velocity.X > 0 ||
            _rigidbodyComponent._velocity.X < -55 && _rigidbodyComponent._velocity.X < 0) 
            return true;
        return false;
    }
}