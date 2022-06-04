using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Graphics;
using SupergoonEngine.Tiled;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Cameras;

public class CameraGameComponent : GameComponent
{
    public static Camera MainCamera;
    private GraphicsGameComponent _graphicsGameComponent;
    private GraphicsDevice _graphicsDevice;
    private TiledGameComponent _tiledTmxContent;

    public CameraGameComponent(Game game, GraphicsGameComponent graphicsGameComponent, GraphicsDevice graphicsDevice, TiledGameComponent tiledTmxContent) : base(game)
    {
        _graphicsGameComponent = graphicsGameComponent;
        _graphicsDevice = graphicsDevice;
        _tiledTmxContent = tiledTmxContent;
    }

    public override void Initialize()
    {
        base.Initialize();
        MainCamera = new Camera(this);
    }

    public Vector2 GetTopLeftOfViewport()
    {
        return new Vector2(_graphicsDevice.Viewport.X, _graphicsDevice.Viewport.Y);
    }

    public float GetScreenToWorldScale()
    {
        return _graphicsGameComponent.WorldSize.X / (float)_graphicsDevice.Viewport.Width;
    }

    public Point GetWorldSize()
    {
        return _graphicsGameComponent.WorldSize;
    }

    public Point GetWindowSize()
    {
        return _graphicsGameComponent.WindowSize;
    }

    public int GetCurrentLevelWidth()
    {
        return _tiledTmxContent.LoadedTmxContent.TileMap.Width * _tiledTmxContent.LoadedTmxContent.TileMap.TileWidth;
    }
    
}