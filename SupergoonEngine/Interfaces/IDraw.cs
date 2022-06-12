using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Interfaces;


public interface IDraw
{
    public void Draw(SpriteBatch spriteBatch);
    public float DrawOrder { get; }
    public bool Visible { get; }
    public event EventHandler<EventArgs> DrawOrderChanged;
    public event EventHandler<EventArgs> VisibleChanged;
}