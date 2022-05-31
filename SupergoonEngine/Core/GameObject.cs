using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Interfaces;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

public class GameObject : ITags, IUpdate, IDraw, IInitialize, IBeginRun, ILoadContent
{
    public T GetComponent<T>(int tag) where T : Component
    {
        var matchedComponent = _components.FirstOrDefault(component => component.Tags.Contains(tag));
        return (T)matchedComponent;
    }

    public void AddComponent(params Component[] components)
    {
        foreach (var component in components)
        {
            _components.Add(component);
        }
        _components.OrderBy(comp => comp.UpdateOrder);
    }
    public void AddComponent(Component component)
    {
        _components.Add(component);
        _components.OrderBy(comp => comp.UpdateOrder);
    }

    public static GameWorld _gameWorld;
    public Vector2 _location;
    protected List<Component> _components = new();

    public GameObject(Vector2 location = new())
    {
        _location = location;
    }

    public static void SetGameWorld(GameWorld gameWorld) => _gameWorld = gameWorld;
    public void AddTag(params int[] tag)
    {
        throw new NotImplementedException();
    }

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
    public float DrawOrder { get; set; }
    public bool Visible { get; }
    public event EventHandler<EventArgs> DrawOrderChanged;
    public event EventHandler<EventArgs> VisibleChanged;
    public bool IsInitialized { get; set; }
    public virtual void Initialize() => _components.ForEach(comp => comp.Initialize());
    
    //TODO get a texture differently, probably don't do it like this.
    public void DrawDebug(SpriteBatch spriteBatch, Rectangle rectangleToDraw, int borderSize = 2)
    {
        var color = Color.Red;
        var debugLayer = 1.0f;
        var t = GraphicsGameComponent._debugTexture;
        spriteBatch.Draw(t, new Rectangle(rectangleToDraw.Left, rectangleToDraw.Top, borderSize, rectangleToDraw.Height),null, color,0f,new Vector2(),SpriteEffects.None,debugLayer); // Left
        spriteBatch.Draw(t, new Rectangle(rectangleToDraw.Right, rectangleToDraw.Top, borderSize, rectangleToDraw.Height + borderSize),null, color,0f,new Vector2(),SpriteEffects.None,debugLayer); // Left
        spriteBatch.Draw(t, new Rectangle(rectangleToDraw.Left, rectangleToDraw.Top, rectangleToDraw.Width, borderSize), null,color,0f,new Vector2(),SpriteEffects.None,debugLayer); // Left
        spriteBatch.Draw(t, new Rectangle(rectangleToDraw.Left, rectangleToDraw.Bottom, rectangleToDraw.Width, borderSize), null,color,0f,new Vector2(),SpriteEffects.None,debugLayer); // Left
    }

    public void BeginRun()
    {
    }

    public virtual void LoadContent()
    {
    }
}