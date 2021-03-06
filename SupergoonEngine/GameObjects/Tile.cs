using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Components;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;

namespace SupergoonDashCrossPlatform.SupergoonEngine.GameObjects;

public class Tile : GameObject
{
    #region Configuration

    #endregion

    private SpriteComponent _spriteComponent;

    public Tile(Vector2 location, Rectangle sourceTextureRect, Texture2D texture, float drawOrder)
    {
        _spriteComponent = new SpriteComponent(sourceTextureRect, texture, this);
        Location = location;
        _spriteComponent.DrawOrder = drawOrder;
        AddComponent(_spriteComponent);
    }

    #region Methods

    #endregion
}