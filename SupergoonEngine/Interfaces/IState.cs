using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Interfaces;

public class IState : IInitialize, IUpdate, IDraw, ITags
{
    #region Configuration

    #endregion

    

    #region Methods

    #endregion

    public bool IsInitialized { get; set; }
    public virtual void Initialize()
    {
        throw new NotImplementedException();
    }

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
    public virtual void AddTag(int tag)
    {
        throw new NotImplementedException();
    }

    bool ITags.RemoveTag(int tag)
    {
        throw new NotImplementedException();
    }

    public bool HasTag(int tag)
    {
        throw new NotImplementedException();
    }

    public virtual void RemoveTag(int tag)
    {
        throw new NotImplementedException();
    }

    public List<int> Tags { get; set; }
}