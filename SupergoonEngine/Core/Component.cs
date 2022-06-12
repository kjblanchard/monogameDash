using System;
using System.Collections.Generic;
using ImGuiNET.SampleProgram.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Interfaces;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

public abstract class Component : ITags, IInitialize, IUpdate, IDraw, ILoadContent, IBeginRun
{
    /// <summary>
    /// Used to set the gameworld of the gameobject.
    /// </summary>
    public static GameWorld GameWorld
    {
        set => _gameWorld = value;
    }
    /// <summary>
    /// The gameworld of the game.
    /// </summary>
   protected static GameWorld _gameWorld;
    
    //TODO is this needed here?
    public static ImGuiGameComponent ImGuiGameComponent;
    
    public readonly GameObject Parent;
    protected Vector2 _offset;
    
    public bool Debug = false;

    public Component(GameObject parent, Vector2 offset = new())
    {
        Parent = parent;
        _offset = offset;
    }

    public void AddTag(params int[] tag)
    {
        throw new NotImplementedException();
    }

    public void AddTag(int tag)
    {
        Tags.Add(tag);
    }

    bool ITags.RemoveTag(int tag)
    {
        throw new NotImplementedException();
    }

    public bool HasTag(int tag)
    {
        throw new NotImplementedException();
    }

    public void RemoveTag(int tag)
    {
        Tags.Remove(tag);
    }

    public List<int> Tags { get; set; } = new();

    public virtual void Update(GameTime gameTime)
    {
    }

    public bool Enabled { get; set; }
    public int UpdateOrder { get; set; }
    public event EventHandler<EventArgs> EnabledChanged;
    public event EventHandler<EventArgs> UpdateOrderChanged;

    public virtual void Draw(SpriteBatch spriteBatch)
    {
    }

    public float DrawOrder { get; set; }
    public bool Visible { get; }
    public event EventHandler<EventArgs> DrawOrderChanged;
    public event EventHandler<EventArgs> VisibleChanged;
    public bool IsInitialized { get; set; }

    public virtual void Initialize()
    {
        //Handle checking for vec2 debugs.
        if (Debug)
        {
           ImGuiGameComponent.CheckComponentForDebugAttributes(this); 
           ImGuiGameComponent.CheckObjectForDebugAttributes(this);
        }
    }

    public virtual void LoadContent()
    {
    }

    public virtual void BeginRun()
    {
    }
}