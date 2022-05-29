using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SupergoonDashCrossPlatform.Sound;
using SupergoonDashCrossPlatform.SupergoonEngine.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Physics;
using SupergoonEngine.Tiled;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

public class GameWorld : Game
{
    private GraphicsDeviceManager _graphics;
    protected SpriteBatch _spriteBatch;
    protected SoundGameComponent _soundGameComponent;
    protected LevelGameComponent _levelGameComponent;
    protected TiledGameComponent _tiledGameComponent;
    protected GraphicsGameComponent _graphicsGameComponent;
    public PhysicsGameComponent PhysicsGameComponent;

    public static bool moveUp;
    public static bool moveDown;
    public static bool moveRight;
    public static bool moveLeft;


    public GameWorld()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
        AttachAllGameComponents();
        //Add in the statics for the cameras to be used in the game.
       Camera.Camera.InitializeCamera(GraphicsDevice, _graphicsGameComponent); 
       _graphicsGameComponent.Initialize();
        _soundGameComponent.PlayBgm();
        _tiledGameComponent.LoadedTmxContent.CreateTileGameObjectsFromContent();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        moveDown = moveLeft = moveRight = moveUp = false;
        //Handle input
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (Keyboard.GetState().IsKeyDown(Keys.Right))
            moveRight = true;
        if (Keyboard.GetState().IsKeyDown(Keys.Down))
            moveDown = true;
        if (Keyboard.GetState().IsKeyDown(Keys.Up))
            moveUp = true;
        if (Keyboard.GetState().IsKeyDown(Keys.Left))
            moveLeft = true;

        _tiledGameComponent.LoadedTmxContent.BackgroundTiles.ForEach(tile => tile.Update(gameTime));
        _tiledGameComponent.LoadedTmxContent.SolidTiles.ForEach(tile => tile.Update(gameTime));
        _tiledGameComponent.LoadedTmxContent.Actors.ForEach(actor => actor.Update(gameTime));
        
        Camera.Camera.Update();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null,
            _graphicsGameComponent.SpriteScale * Camera.Camera.GetCameraTransformMatrix());

        // _spriteBatch.Begin();
        _tiledGameComponent.LoadedTmxContent.BackgroundTiles.ForEach(tile => tile.Draw(_spriteBatch));
        _tiledGameComponent.LoadedTmxContent.SolidTiles.ForEach(tile => tile.Draw(_spriteBatch));
        _tiledGameComponent.LoadedTmxContent.Actors.ForEach(actor => actor.Draw(_spriteBatch));
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }


    private void AttachAllGameComponents()
    {
        _soundGameComponent = new SoundGameComponent(this);
        _levelGameComponent = new LevelGameComponent(this);
        _tiledGameComponent = new TiledGameComponent(this);
        _graphicsGameComponent = new GraphicsGameComponent(this, _graphics, GraphicsDevice);
        PhysicsGameComponent = new PhysicsGameComponent(this);
        
        Components.Add(_tiledGameComponent);
        Components.Add(_soundGameComponent);
        Components.Add(_levelGameComponent);
        Components.Add(_graphicsGameComponent);
        Components.Add(PhysicsGameComponent);

        GameObject._gameWorld = this;
    }

    #region Configuration

    #endregion


    #region Methods

    #endregion
}