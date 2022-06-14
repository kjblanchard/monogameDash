using System;
using System.Collections.Generic;
using ImGuiNET.SampleProgram.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.Sound;
using SupergoonDashCrossPlatform.SupergoonEngine.Cameras;
using SupergoonEngine.Tiled;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

public class Level : State
{
    public static void SetTiledGameComponent(TiledGameComponent tiledGameComponent) =>
        _tiledGameComponent = tiledGameComponent;

    public static TiledGameComponent _tiledGameComponent;
    public static SoundGameComponent _soundGameComponent;
    
    public string _tmxLevelToLoad;
    public string _bgmToPlay;
    public bool _shouldReset = false;
    public TiledTmxContent LoadedContents;

    public List<Component> LevelComponents = new();

    public Level(string tmxLevelToLoad = null, string soundToPlay = null)
    {
        _tmxLevelToLoad = tmxLevelToLoad;
        _bgmToPlay = soundToPlay;
    }

    protected void PlayBgm()
    {
        _soundGameComponent.PlayBgm(_bgmToPlay);
    }


    public override void Update(GameTime gameTime)
    {
        if(_shouldReset)
            InternalReset();
        _tiledGameComponent.LoadedTmxContent.Update(gameTime);
    }

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
    
    
    public override void StartState()
    {
        base.StartState();
       LoadedContents = _tiledGameComponent.LoadTilesets(_tmxLevelToLoad);
       Initialize();
       LoadContent();
       BeginRun();
    }

    public override void Initialize()
    {
        base.Initialize();
        _tiledGameComponent.LoadedTmxContent.Actors.ForEach(actor => actor.Initialize());
        CameraGameComponent.MainCamera.Location = Vector3.Zero;
    }

    public override void LoadContent()
    {
        base.LoadContent();
        _tiledGameComponent.LoadedTmxContent.Actors.ForEach(actor => actor.LoadContent());
    }

    public override void BeginRun()
    {
        base.BeginRun();
        PlayBgm();
        _tiledGameComponent.LoadedTmxContent.Actors.ForEach(actor => actor.BeginRun());
        
    }

    public void Reset()
    {
        _shouldReset = true;
    }

    private void InternalReset()
    {
        _shouldReset = false;
        LoadedContents.Reset();
        ImGuiGameComponent.Instance.Reset();
        Initialize();
        LoadContent();
        BeginRun();
        GC.Collect();
    }
}