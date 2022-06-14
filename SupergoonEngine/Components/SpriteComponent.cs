using System;
using System.ComponentModel.Design.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Cameras;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Components;

public class SpriteComponent : Component
{
    public static GameWorld _gameWorld;

    public bool Mirror
    {
        set
        {
            switch (value)
            {
                case true:
                    DrawSpriteEffect = SpriteEffects.FlipHorizontally;
                    break;
                case false:
                    DrawSpriteEffect = SpriteEffects.None;
                    break;
                
            }
        }
    }
    
    

    private SpriteEffects DrawSpriteEffect = SpriteEffects.None;

    private Texture2D _texture;
    public Point DrawDestinationLocation => new Point((int)Math.Floor(Parent.Location.X + _offset.X), (int)Math.Floor(Parent.Location.Y + _offset.Y));
    private Point _drawDestinationLocation;
    private Point _drawDestinationSize;
    private Point _textureSourceLocation;
    private Point _textureSourceSize;


    public SpriteComponent(GameObject parent) : base(parent)
    {

    }
    
    public SpriteComponent(Rectangle sourceRect, Texture2D texture, GameObject parent) : base(parent)
    {
        _textureSourceLocation = sourceRect.Location;
        _textureSourceSize = sourceRect.Size;
        //TODO Do this some other way.
        _drawDestinationSize = sourceRect.Size;
        _texture = texture;

        //Add the sprite tag to the update order, and to the list of tags.
        var tag = EngineTags.ComponentTags.Sprite;
        UpdateOrder = tag;
        AddTag(tag);
    }

    public void UpdateFromAnimationComponent(Texture2D texture,Point sourceLocation )
    {
        _texture = texture;
        _textureSourceLocation = sourceLocation;
        _textureSourceSize = new Point(32, 32);
        _drawDestinationSize = new Point(32, 32);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        
        // _drawDestinationLocation = CameraGameComponent.MainCamera.CalculateCameraOffset(Parent.Location + _offset).ToPoint();
        _drawDestinationLocation = Parent.Location.ToPoint();
        spriteBatch.Draw(_texture,
            // new Rectangle(_drawDestinationLocation, _drawDestinationSize),
            new Rectangle(DrawDestinationLocation, _drawDestinationSize),
            new Rectangle(_textureSourceLocation, _textureSourceSize),
            Color.White,
           0.0f,new Vector2(),DrawSpriteEffect,DrawOrder);
    }
}