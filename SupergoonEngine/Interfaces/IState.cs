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
    }

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

    public void AddTag(params int[] tag)
    {
        Tags.AddRange(tag);
    }

    public virtual void AddTag(int tag)
    {
        Tags.Add(tag);
    }

    bool ITags.RemoveTag(int tag)
    {
        throw new NotImplementedException();
    }

    public bool HasTag(int tag) => Tags.Contains(tag);

    public virtual void RemoveTag(int tag)
    {
        throw new NotImplementedException();
    }

    public List<int> Tags { get; set; } = new();

    public virtual void StartState()
    {
        //TODO get this working differently.
        Initialize();
    }

    public virtual void EndState()
    {
    }
}