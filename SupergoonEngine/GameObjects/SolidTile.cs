using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Components;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;

namespace SupergoonDashCrossPlatform.SupergoonEngine.GameObjects;

public class SolidTile : GameObject
{

    
    private SpriteComponent _spriteComponent;
    private BoxColliderComponent _boxColliderComponent;

    public SolidTile(Vector2 location, Rectangle sourceTextureRect, Texture2D texture, float drawOrder, Point boxSize, Vector2 offset = new Vector2() )
    {
        _location = location;
        _spriteComponent = new SpriteComponent(sourceTextureRect, texture, this);
        _spriteComponent.DrawOrder = drawOrder;
        //Use half height for now. TODO this needs to be altered.
        // _boxColliderComponent = new BoxColliderComponent(this, new Point(32,16));
        _boxColliderComponent = new BoxColliderComponent(this, boxSize, offset);
        AddComponent(_spriteComponent,_boxColliderComponent);
        
    }


}