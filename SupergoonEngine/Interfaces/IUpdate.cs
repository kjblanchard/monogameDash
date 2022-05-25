using System;
using Microsoft.Xna.Framework;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Interfaces;

public interface IUpdate
{
    public void Update(GameTime gameTime);

    public bool Enabled { get; }
    public int UpdateOrder { get; }
    public event EventHandler<EventArgs> EnabledChanged;
    public event EventHandler<EventArgs> UpdateOrderChanged;
}