using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET.SampleProgram.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Interfaces;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

public abstract class GameObject : ITags, IUpdate, IDraw, IInitialize, IBeginRun, ILoadContent, IDebug
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

    /// <summary>
    /// <summary>
    /// Used on gameobject Initialization to determine ID
    /// </summary>
    private static uint lastId;

    /// <summary>
    /// The ID of this gameobject, this is unique to the gameobject and used for lookups and comparisons occasionally.
    /// </summary>
    public uint Id;

    /// <summary>
    /// The location of this gameobject
    /// </summary>
    public Vector2 Location;
    
    /// <summary>
    /// The tags on this gameobject.
    /// </summary>
    public List<int> Tags { get; set; } = new();

    /// <summary>
    /// Debug mode will send this through the Imgui Debugger to try and get debug values to display.
    /// </summary>
    public bool Debug { get; set; }

    /// <summary>
    /// If the gameobject is enabled.  If not enabled, this object will not update or draw, but will not be destroyed.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// The update order for this gameobject.  Currently doesn't affect anything on Gameobjects.
    /// </summary>
    public int UpdateOrder { get; set; }

    /// <summary>
    /// Not implemented currently.
    /// </summary>
    public event EventHandler<EventArgs> EnabledChanged;

    /// <summary>
    /// Not implemented currently.
    /// </summary>
    public event EventHandler<EventArgs> UpdateOrderChanged;

    /// <summary>
    /// No effect currently on gameobjects.
    /// </summary>
    public float DrawOrder { get; protected set; }

    /// <summary>
    /// Determines if this gameobject's draw call gets called.
    /// </summary>
    public bool Visible { get; protected set; }

    /// <summary>
    /// Not currently implemented.
    /// </summary>
    public event EventHandler<EventArgs> DrawOrderChanged;

    /// <summary>
    /// Not currently implemented
    /// </summary>
    public event EventHandler<EventArgs> VisibleChanged;

    /// <summary>
    /// Has this gameobject been initialized
    /// </summary>
    public bool IsInitialized { get; set; }

    /// <summary>
    /// Components attached to this GameObject.  These will be called during this gameobjects Update and draw step in their update order.
    /// </summary>
    private List<Component> _components = new();

    /// <summary>
    /// Create a gameobject.
    /// </summary>
    /// <param name="location">The location of this GameObject, defaults to Vector2 0</param>
    protected GameObject(Vector2 location = new())
    {
        Location = location;
        Enabled = true;
        Visible = true;
        Id = ++lastId;
    }

    /// Gets a component from the gameobject, by the tags attached to the components.  Requires a loop through components.
    /// </summary>
    /// <param name="tag">The tag to match on in the components</param>
    /// <typeparam name="T">The type of component to return back, this is for casting.</typeparam>
    /// <returns>The class if the cast was successful in finding and casting, otherwise null</returns>
    public T GetComponent<T>(int tag) where T : Component
    {
        foreach (var component in _components)
        {
            if (component.Tags.Contains(tag))
                return component as T;
        }

        return null;
    }

    /// <summary>
    /// Adds a component or multiple components to this gameobject.  Orders the components by update order as well.  This uses params, so try not to run this in the update loop.
    /// </summary>
    /// <param name="components"></param>
    public void AddComponent(params Component[] components)
    {
        foreach (var component in components)
        {
            _components.Add(component);
        }

        _components.OrderBy(comp => comp.UpdateOrder);
    }

    public void AddTag(params int[] tag) => Tags.AddRange(tag);
    public bool RemoveTag(int tag) => Tags.Remove(tag);
    public bool HasTag(int tag) => Tags.Contains(tag);

    /// <summary>
    /// Loads content on this gameobject and on all components
    /// </summary>
    public virtual void LoadContent()
    {
        foreach (var component in _components)
        {
            component.LoadContent();
        }
    }

    /// <summary>
    /// Calls begin run on this Gameobject and on all components.
    /// </summary>
    public virtual void BeginRun()
    {
        foreach (var component in _components)
        {
            component.BeginRun();
        }
    }

    /// <summary>
    /// Updates this gameobject and all of its components if it is enabled
    /// </summary>
    /// <param name="gameTime">The total elapsed gametime in seconds</param>
    public virtual void Update(GameTime gameTime)
    {
        if (!Enabled)
            return;
        foreach (var component in _components)
        {
            component.Update(gameTime);
        }
    }


    /// <summary>
    /// Draws this gameobject if it is enabled and visible
    /// </summary>
    /// <param name="spriteBatch">The spritebatch to draw to.</param>
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (!Enabled || !Visible)
            return;
        foreach (var component in _components)
        {
            component.Draw(spriteBatch);
        }
    }


    /// <summary>
    /// Initialize all the components, and Send attributes to the debugger.
    /// </summary>
    public virtual void Initialize()
    {
        IsInitialized = true;
        _components.ForEach(comp => comp.Initialize());
        SendAttributesToDebugger(this);
    }

    //TODO Cache the texture in a variable probably don't do it like this.
    public void DrawDebug(SpriteBatch spriteBatch, Rectangle rectangleToDraw, int borderSize = 2)
    {
        var color = Color.Red;
        var debugLayer = 1.0f;
        var t = GraphicsGameComponent._debugTexture;
        spriteBatch.Draw(t,
            new Rectangle(rectangleToDraw.Left, rectangleToDraw.Top, borderSize, rectangleToDraw.Height), null, color,
            0f, new Vector2(), SpriteEffects.None, debugLayer); // Left
        spriteBatch.Draw(t,
            new Rectangle(rectangleToDraw.Right, rectangleToDraw.Top, borderSize, rectangleToDraw.Height + borderSize),
            null, color, 0f, new Vector2(), SpriteEffects.None, debugLayer); // Left
        spriteBatch.Draw(t, new Rectangle(rectangleToDraw.Left, rectangleToDraw.Top, rectangleToDraw.Width, borderSize),
            null, color, 0f, new Vector2(), SpriteEffects.None, debugLayer); // Left
        spriteBatch.Draw(t,
            new Rectangle(rectangleToDraw.Left, rectangleToDraw.Bottom, rectangleToDraw.Width, borderSize), null, color,
            0f, new Vector2(), SpriteEffects.None, debugLayer); // Left
    }


    public void SendAttributesToDebugger(object parent)
    {
        if (!Debug)
            return;
        ImGuiGameComponent.Instance.CheckObjectForDebugAttributes(this);
    }
}