using System;
using ImGuiNET.SampleProgram.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Animation;
using SupergoonDashCrossPlatform.SupergoonEngine.Components;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using SupergoonDashCrossPlatform.SupergoonEngine.Input;
using SupergoonDashCrossPlatform.Tags;
using TiledCS;

namespace SupergoonDashCrossPlatform.Actors;

public class Player : Actor
{
    private const string IdleAnimString = "Idle";
    private const string FallingAnimString = "Fall";
    private const string JumpingAnimString = "Jump";
    private const string RunningAnimString = "Run";

    private const string JumpSfxString = "playerJump";
    private const float JumpSfxSoundLevel = 2;

    private SoundComponent _soundComponent;

    private float slowRunTreshold = 100;

    private float slowRunAnimSpeed = 0.7f;

    private float fastRunTreshold = 180;

    private float fastRunAnimSpeed = 1.30f;

    [ImGuiWrite(typeof(float), true, "Run speed", Min = 220, Max = 400)]
    private float runSpeed = 320;

    [ImGuiWrite(typeof(float), true, "Jump addition", Min = 520, Max = 800)]
    private float jumpAddition = 620;

    [ImGuiWrite(typeof(float), true, "Jump length max", Min = 0, Max = 5)]
    private float _jumpLengthMax = 0.245f;

    [ImGuiWrite(typeof(float), true, "jump length", Min = 0, Max = 200)]
    private float _jumpLength;

    private CameraComponent _cameraComponent;

    private bool _isDead;

    private float _minXVel = 70;
    private bool playerStartedMoving;

    private byte _coinsCollected;
    private const int _coinSpeedAddition = 20;

    private Player(ActorParams actorParams) : base(actorParams)
    {
        AddTag(EngineTags.GameObjectTags.Player);
    }

    // public new static GameObject FactoryFunction(Vector2 location, TiledProperty[] tags = null, Rectangle textureRect = new Rectangle(), Texture2D texture = null)
    public new static GameObject FactoryFunction(ActorParams actorParams)
    {
        actorParams.AsepriteDocString = "player";
        actorParams.BoxColliderOffset = new Vector2(6, 10);
        actorParams.BoxSize = new Point(19, 20);
        var player = new Player(actorParams);
        player.Initialize();
        player.jumpHeight = 300;
        return player;
    }

    public override void Initialize()
    {
        _cameraComponent = new CameraComponent(this);
        _soundComponent = new SoundComponent(this);
        // _cameraComponent.Initialize();
        AddComponent(_cameraComponent);
        _rigidbodyComponent.GravityEnabled = true;
        _rigidbodyComponent.RightCollisionJustStartedEvent += PlayerDeath;
        _rigidbodyComponent.TopCollisionJustStartedEvent += PlayerDeath;
        _spriteComponent.DrawOrder = 0.7f;
        AddAnimationTransitions();
        _rigidbodyComponent.BottomCollisionJustStartedEvent += OnJustHitGround;
        playerStartedMoving = false;

        SupergoonDashGameWorld.Attempts++;
        SupergoonDashGameWorld.MaxSpeed = 200;

        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        if (_isDead)
        {
            if (_playerControllerComponent.PlayerController.IsButtonPressed(ControllerButtons.A))
            {
                var currentLevel = _gameWorld.LevelStateMachine.GetState(LevelTags.Level1);
                _gameWorld.Reset();
                currentLevel.Reset();
            }

            return;
        }

        base.Update(gameTime);
        if (_playerControllerComponent.PlayerController.IsButtonPressed(ControllerButtons.Right) ||
            _playerControllerComponent.PlayerController.IsButtonHeld(ControllerButtons.Right))
        {
            playerStartedMoving = true;
            var movementForce = new Vector2(runSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);

            //If you arent moving, get a boost in speed
            if (_rigidbodyComponent._velocity.X == 0)
            {
                movementForce.X *= 5;
            }


            _rigidbodyComponent.AddForce(movementForce);
        }

        if (playerStartedMoving &&
            (_playerControllerComponent.PlayerController.IsButtonPressed(ControllerButtons.Left) ||
             _playerControllerComponent.PlayerController.IsButtonHeld(ControllerButtons.Left)))
        {
            var movementForce = new Vector2(-runSpeed* (float)gameTime.ElapsedGameTime.TotalSeconds, 0);

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
                _rigidbodyComponent.AddForce(new Vector2(0, -jumpAddition* (float)gameTime.ElapsedGameTime.TotalSeconds));
                _jumpLength += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        if (playerStartedMoving && !_isDead)
            SupergoonDashGameWorld.TimeThisLevel += gameTime.ElapsedGameTime;

        if (playerStartedMoving)
            _rigidbodyComponent._velocity.X =
                MathHelper.Clamp(_rigidbodyComponent._velocity.X, _minXVel, float.MaxValue);

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
        _soundComponent.PlaySfx(JumpSfxString, JumpSfxSoundLevel);
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
            new AnimationTransition(FallingAnimString,
                () => _playerControllerComponent.PlayerController.IsButtonReleased(ControllerButtons.A) ||
                      _jumpLength >= _jumpLengthMax);
        jumpingAnimation.Transitions.Add(jumpingToFallingTransition);

        //Falling
        var fallingToIdleTransition = new AnimationTransition(IdleAnimString, () => !isFalling);
        fallingAnimation.Transitions.Add(fallingToIdleTransition);

        //Running
        var runningToIdleTransition =
            // new AnimationTransition(IdleAnimString, () => _rigidbodyComponent._velocity.X == 0);
            new AnimationTransition(IdleAnimString, () =>
                (_rigidbodyComponent._velocity.X < 55 && _rigidbodyComponent._velocity.X > 0)
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

    public void PlayerDeath()
    {
        _soundComponent.PlaySfx("playerDeath");
        Visible = false;
        _isDead = true;
    }

    public void PlayerWin()
    {
        _soundComponent.PlayBgm("levelWin");
        _isDead = true;
    }

    public void OnCoinOverlap()
    {
        _coinsCollected++;
        _rigidbodyComponent.OverrideGravityMax(_coinsCollected * _coinSpeedAddition);
        _rigidbodyComponent._velocity.X += _coinSpeedAddition;
        SupergoonDashGameWorld.CoinAmount = _coinsCollected;
        SupergoonDashGameWorld.MaxSpeed = 200 + (_coinsCollected * _coinSpeedAddition);
    }
}