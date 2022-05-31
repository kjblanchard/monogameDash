using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Components;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;

namespace SupergoonDashCrossPlatform.SupergoonEngine.GameObjects;

public class SolidTile : GameObject
{

    
    private SpriteComponent _spriteComponent;
    private BoxColliderComponent _boxColliderComponent;

    public SolidTile(Vector2 location, Rectangle sourceTextureRect, Texture2D texture, float drawOrder)
    {
        _location = location;
        _spriteComponent = new SpriteComponent(sourceTextureRect, texture, this);
        _spriteComponent.DrawOrder = drawOrder;
        _boxColliderComponent = new BoxColliderComponent(this, sourceTextureRect.Size);
        _boxColliderComponent.Debug = true;
        AddComponent(_spriteComponent,_boxColliderComponent);
        
    }


}