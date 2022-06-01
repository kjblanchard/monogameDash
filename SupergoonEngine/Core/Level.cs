﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Interfaces;
using SupergoonEngine.Tiled;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

public class Level : IState
{
    public static void SetTiledGameComponent(TiledGameComponent tiledGameComponent) =>
        _tiledGameComponent = tiledGameComponent;

    public static TiledGameComponent _tiledGameComponent;
    public string _tmxLevelToLoad;
    public TiledTmxContent LoadedContents;

    public Level(string tmxLevelToLoad = null)
    {
        _tmxLevelToLoad = tmxLevelToLoad;
    }


    public override void Update(GameTime gameTime)
    {
        _tiledGameComponent.LoadedTmxContent.Update(gameTime);
    }

    public static int ticks;
    public bool Enabled { get; }
    public int UpdateOrder { get; }
    public event EventHandler<EventArgs> EnabledChanged;
    public event EventHandler<EventArgs> UpdateOrderChanged;

    public override void Draw(SpriteBatch spriteBatch)
    
    {
        _tiledGameComponent.LoadedTmxContent.Draw(spriteBatch);
    }

    public int DrawOrder { get; }
    public bool Visible { get; }
    public event EventHandler<EventArgs> DrawOrderChanged;
    public event EventHandler<EventArgs> VisibleChanged;
    public bool IsInitialized { get; set; }

    public override void Initialize()
    {
        base.Initialize();
       LoadedContents = _tiledGameComponent.LoadTilesets(_tmxLevelToLoad);
    }
}