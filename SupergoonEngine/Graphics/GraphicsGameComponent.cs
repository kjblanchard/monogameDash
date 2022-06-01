using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Graphics;

public class GraphicsGameComponent : GameComponent
{
    public Point WorldSize => _worldSize;
    public Point WindowSize => _windowSize;
    public Matrix SpriteScale => _spriteScale;
    private Point _windowSize;
    private Point _worldSize;
    private readonly GraphicsDeviceManager _graphics;
    private readonly GraphicsDevice _graphicsDevice;
    private Matrix _spriteScale;
    
    //Testing out Drawing Debug boxes with this
    public static Texture2D _debugTexture;

    public GraphicsGameComponent(Game game, GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice) : base(game)
    {
        _graphics = graphics;
        _graphicsDevice = graphicsDevice;
    }

    public override void Initialize()
    {
        base.Initialize();
        // _windowSize = new Point(1280,960);
        _windowSize = new Point(1920,1080);
        // _worldSize = new Point(640,380);
        _worldSize = new Point(512,288);
        
    _debugTexture = new Texture2D(_graphicsDevice, 1, 1);
    _debugTexture.SetData(new[] { Color.White });
    }

    /// <summary>
    /// Sets the proper resolution changes and applies them
    /// </summary>
    /// <param name="fullScreen"></param>
    public void ApplyResolutionSettings(bool fullScreen)
    {
        _graphics.IsFullScreen = fullScreen;
        var screenSize = fullScreen
            ? new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            : _windowSize;
        _graphics.PreferredBackBufferWidth = screenSize.X;
        _graphics.PreferredBackBufferHeight = screenSize.Y;
        _graphics.ApplyChanges();
        _graphicsDevice.Viewport = CalculateViewport(screenSize);
        _spriteScale = Matrix.CreateScale(
            (float)_graphicsDevice.Viewport.Width / _worldSize.X,
            (float)_graphicsDevice.Viewport.Height / _worldSize.Y, 1);
    }

    /// <summary>
    /// Calculates and returns the viewport to use, so that the game world fits on the screen while preserving its aspect ratio.
    /// </summary>
    /// <param name="windowSize">The size of the screen on which the world should be drawn.</param>
    /// <returns>A Viewport object that will show the game world as large as possible while preserving its aspect ratio.</returns>
    Viewport CalculateViewport(Point windowSize)
    {
        var viewport = new Viewport();
        var gameAspectRatio = (float)_worldSize.X / _worldSize.Y;
        var windowAspectRatio = (float)windowSize.X / windowSize.Y;
        if (windowAspectRatio > gameAspectRatio)
        {
            viewport.Width = (int)(windowSize.Y * gameAspectRatio);
            viewport.Height = windowSize.Y;
        }
        else
        {
            viewport.Width = windowSize.X;
            viewport.Height = (int)(windowSize.X / gameAspectRatio);
        }

        viewport.X = (windowSize.X - viewport.Width) / 2;
        viewport.Y = (windowSize.Y - viewport.Height) / 2;
        return viewport;
    }
}