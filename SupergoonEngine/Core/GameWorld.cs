using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SupergoonDashCrossPlatform.Sound;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

public class GameWorld : Game
{
    private GraphicsDeviceManager _graphics;
    protected SpriteBatch _spriteBatch;
    protected SoundGameComponent soundGameComponent;
    protected LevelGameComponent _levelGameComponent;


    public GameWorld()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        AttachAllGameComponents();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        base.Initialize();
        soundGameComponent.PlayBgm();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        //Handle input
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();


        _spriteBatch.End();

        base.Draw(gameTime);
    }


    private void AttachAllGameComponents()
    {
        soundGameComponent = new SoundGameComponent(this);
        _levelGameComponent = new LevelGameComponent(this);

        Components.Add(soundGameComponent);
        Components.Add(_levelGameComponent);
    }

    #region Configuration

    #endregion


    #region Methods

    #endregion
}