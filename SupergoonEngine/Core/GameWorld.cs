using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SupergoonDashCrossPlatform.Sound;
using SupergoonEngine.Tiled;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

public class GameWorld : Game
{
    private GraphicsDeviceManager _graphics;
    protected SpriteBatch _spriteBatch;
    protected SoundGameComponent _soundGameComponent;
    protected LevelGameComponent _levelGameComponent;
    protected TiledGameComponent _tiledGameComponent;


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
        _soundGameComponent.PlayBgm();
        _tiledGameComponent.LoadedTmxContent.CreateTileGameObjectsFromContent();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        //Handle input
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _tiledGameComponent.LoadedTmxContent.BackgroundTiles.ForEach(tile => tile.Update(gameTime));
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();
        _tiledGameComponent.LoadedTmxContent.DrawTilemap(_spriteBatch);
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }


    private void AttachAllGameComponents()
    {
        _soundGameComponent = new SoundGameComponent(this);
        _levelGameComponent = new LevelGameComponent(this);
        _tiledGameComponent = new TiledGameComponent(this);
        
        Components.Add(_tiledGameComponent);
        Components.Add(_soundGameComponent);
        Components.Add(_levelGameComponent);
    }

    #region Configuration

    #endregion


    #region Methods

    #endregion
}