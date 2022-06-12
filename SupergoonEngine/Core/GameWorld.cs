using System;
using ImGuiNET;
using ImGuiNET.SampleProgram.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SupergoonDashCrossPlatform.Sound;
using SupergoonDashCrossPlatform.SupergoonEngine.Cameras;
using SupergoonDashCrossPlatform.SupergoonEngine.Components;
using SupergoonDashCrossPlatform.SupergoonEngine.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Input;
using SupergoonDashCrossPlatform.SupergoonEngine.Physics;
using SupergoonEngine.Tiled;


namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

public class GameWorld : Game
{
    private GraphicsDeviceManager _graphics;
    protected SpriteBatch _spriteBatch;
    protected SoundGameComponent _soundGameComponent;
    protected TiledGameComponent _tiledGameComponent;
    protected GraphicsGameComponent _graphicsGameComponent;
    protected CameraGameComponent _cameraGameComponent;
    public PhysicsGameComponent PhysicsGameComponent;
    public static InputGameComponent InputGameComponent;
    public ImGuiGameComponent _imGuiGameComponent;
    



    public LevelStateMachine LevelStateMachine => _levelStateMachine;
    protected LevelStateMachine _levelStateMachine;


    public GameWorld()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        AttachAllGameComponents();
        //Add in the statics for the cameras to be used in the game.
        _graphicsGameComponent.Initialize();
        _levelStateMachine = new LevelStateMachine(_tiledGameComponent);
        
        //Try 30fps
        // TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        //Handle Exit
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _levelStateMachine.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.SkyBlue);
        _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null,
            // _graphicsGameComponent.SpriteScale * CameraGameComponent.MainCamera.GetCameraTransformMatrix());
            _graphicsGameComponent.SpriteScale);
        _levelStateMachine.Draw(_spriteBatch);
        
        
        
        
        
        
        
        _spriteBatch.End();


        //Draw imgui render target
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
            DepthStencilState.Default, RasterizerState.CullNone, null, null);
        _imGuiGameComponent.Draw(_spriteBatch);
        _spriteBatch.End();
        

        base.Draw(gameTime);
    }


    private void AttachAllGameComponents()
    {
        _soundGameComponent = new SoundGameComponent(this);
        _tiledGameComponent = new TiledGameComponent(this);
        _graphicsGameComponent = new GraphicsGameComponent(this, _graphics, GraphicsDevice);
        PhysicsGameComponent = new PhysicsGameComponent(this, _tiledGameComponent);
        InputGameComponent = new InputGameComponent(this);
        _imGuiGameComponent = new ImGuiGameComponent(this, GraphicsDevice);
        _cameraGameComponent = new CameraGameComponent(this, _graphicsGameComponent, GraphicsDevice, _tiledGameComponent);
        

        AddGameComponent(_tiledGameComponent, _soundGameComponent, _graphicsGameComponent, PhysicsGameComponent,
            InputGameComponent, _imGuiGameComponent, _cameraGameComponent);

        //Set the gameobject static somewhere TODO put this in a better place.
        GameObject.GameWorld = this;
        Component.GameWorld = this;
        Component.ImGuiGameComponent = _imGuiGameComponent;
        SoundComponent._soundGameComponent = _soundGameComponent;
    }

    protected void AddLevels(params Level[] levels)
    {
        foreach (var level in levels)
        {
            _levelStateMachine.AddLevel(level);
        }

        _levelStateMachine.Initialize();
    }

    protected void InitializeLevels()
    {
        _levelStateMachine.InitializeStates();
    }

    protected void ChangeLevel(int levelTag)
    {
        _levelStateMachine.ChangeState(levelTag);
    }

    private void AddGameComponent(params GameComponent[] component)
    {
        foreach (var gameComponent in component)
        {
            Components.Add(gameComponent);
        }
    }

    public virtual void Reset()
    {
        
    }
}