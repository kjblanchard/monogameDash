using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SupergoonDashCrossPlatform.SupergoonEngine.Interfaces;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;


public class GameObject : ITags, IUpdateable, IDrawable
{
    public T GetComponent<T>(int tag) where T: Component
    {
        var matchedComponent = _components.FirstOrDefault(component => component.Tags.Contains(tag));
        return (T)matchedComponent;
    }
    
    protected static GameWorld _gameWorld;
    public Vector2 _location;
    protected List<Component> _components = new();

    public GameObject(Vector2 location = new())
    {
        _location = location;

    }

    public static void SetGameWorld(GameWorld gameWorld)
    {
        _gameWorld = gameWorld;
    }

    public virtual void AddTag(int tag)
    {
        throw new NotImplementedException();
    }

    public virtual void RemoveTag(int tag)
    {
        throw new NotImplementedException();
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
    public virtual void Draw(GameTime gameTime)
    {
        throw new NotImplementedException();
    }

    public int DrawOrder { get; }
    public bool Visible { get; }
    public event EventHandler<EventArgs> DrawOrderChanged;
    public event EventHandler<EventArgs> VisibleChanged;
}