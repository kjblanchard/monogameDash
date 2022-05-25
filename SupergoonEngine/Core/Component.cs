using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Interfaces;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;


public class Component : ITags, IInitialize, IUpdate, IDraw
{
    public GameObject Parent;
    public Vector2 offset;

    public Component(GameObject parent)
    {
        Parent = parent;


    }

    public void AddTag(int tag)
    {
        Tags.Add(tag);
    }

    public void RemoveTag(int tag)
    {
        Tags.Remove(tag);
    }

    public List<int> Tags { get; set; }
    public virtual void Update(GameTime gameTime)
    {
        throw new NotImplementedException();
    }

    public bool Enabled { get; }
    public int UpdateOrder { get; }
    public event EventHandler<EventArgs> EnabledChanged;
    public event EventHandler<EventArgs> UpdateOrderChanged;

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        throw new NotImplementedException();
    }

    public int DrawOrder { get; }
    public bool Visible { get; }
    public event EventHandler<EventArgs> DrawOrderChanged;
    public event EventHandler<EventArgs> VisibleChanged;
    public bool IsInitialized { get; set; }
    public virtual void Initialize()
    {
        throw new NotImplementedException();
    }
}