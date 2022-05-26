using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Interfaces;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

public class GameObject : ITags, IUpdate, IDraw, IInitialize
{
    public T GetComponent<T>(int tag) where T : Component
    {
        var matchedComponent = _components.FirstOrDefault(component => component.Tags.Contains(tag));
        return (T)matchedComponent;
    }

    public void AddComponent(Component component)
    {
        _components.Add(component);
        _components.OrderBy(comp => comp.UpdateOrder);
    }

    protected static GameWorld _gameWorld;
    public Vector2 _location;
    protected List<Component> _components = new();

    public GameObject(Vector2 location = new())
    {
        _location = location;
    }

    public static void SetGameWorld(GameWorld gameWorld) => _gameWorld = gameWorld;
    public virtual void AddTag(int tag) => Tags.Add(tag);
    bool ITags.RemoveTag(int tag) => Tags.Remove(tag);
    public bool HasTag(int tag) => Tags.Contains(tag);
    public virtual void RemoveTag(int tag) => Tags.Remove(tag);
    public List<int> Tags { get; set; }
    public virtual void Update(GameTime gameTime) => _components.ForEach(component => component.Update(gameTime));
    public bool Enabled { get; set; }
    public int UpdateOrder { get; set; }
    public event EventHandler<EventArgs> EnabledChanged;
    public event EventHandler<EventArgs> UpdateOrderChanged;
    public virtual void Draw(SpriteBatch spriteBatch) => _components.ForEach(component => component.Draw(spriteBatch));
    public int DrawOrder { get; }
    public bool Visible { get; }
    public event EventHandler<EventArgs> DrawOrderChanged;
    public event EventHandler<EventArgs> VisibleChanged;
    public bool IsInitialized { get; set; }
    public virtual void Initialize() => _components.ForEach(comp => comp.Initialize());
}