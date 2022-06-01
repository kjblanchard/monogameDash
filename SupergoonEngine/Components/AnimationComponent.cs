using Microsoft.Xna.Framework;
using MonoGame.Aseprite.Documents;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Components;

public class AnimationComponent: Component
{
    private SpriteComponent _spriteComponent;
    private AsepriteDocument _asepriteDocument;

    private Point _texturePointToDisplay = Point.Zero;


    private double _secondsThisFrame = 0;
    private int currentFrame = 0;
    
    public AnimationComponent(GameObject parent, SpriteComponent spriteComponent, AsepriteDocument asepriteDocument): base(parent)
    {
        _spriteComponent = spriteComponent;
        _asepriteDocument = asepriteDocument;
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _secondsThisFrame += gameTime.ElapsedGameTime.TotalSeconds;
        if (_secondsThisFrame >= _asepriteDocument.Frames[currentFrame].Duration)
        {
            _secondsThisFrame -= _asepriteDocument.Frames[currentFrame].Duration;
            var newFrame = currentFrame + 1;
            if (newFrame >= _asepriteDocument.Frames.Count)
                newFrame = 0;
            currentFrame = newFrame;
            _texturePointToDisplay = new Point(_asepriteDocument.Frames[currentFrame].X,
                _asepriteDocument.Frames[currentFrame].Y);
        }
        _spriteComponent.UpdateFromAnimationComponent(_asepriteDocument.Texture, _texturePointToDisplay);
    }

}