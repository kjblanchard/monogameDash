using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Components;

public class SpriteComponent : Component
{
    public static GameWorld _gameWorld;
    private Texture2D _texture;
    private Point _drawDestinationLocation;
    private Point _drawDestinationSize;
    private Point _textureSourceLocation;
    private Point _textureSourceSize;

    public SpriteComponent(Rectangle sourceRect, Texture2D texture, GameObject parent) : base(parent)
    {
        _textureSourceLocation = sourceRect.Location;
        _textureSourceSize = sourceRect.Size;
        _texture = texture;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _drawDestinationLocation = Parent._location.ToPoint() + offset.ToPoint();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        spriteBatch.Draw(_texture,
            new Rectangle(_drawDestinationLocation, _drawDestinationSize),
            new Rectangle(_textureSourceLocation, _textureSourceSize),
            Color.White);
    }


}