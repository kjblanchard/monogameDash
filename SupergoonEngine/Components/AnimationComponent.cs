using Microsoft.Xna.Framework;
using MonoGame.Aseprite.Documents;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Components;

public class AnimationComponent: Component
{
    private SpriteComponent _spriteComponent;
    private AsepriteDocument _asepriteDocument;
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
        var sourceLoc = new Point(_asepriteDocument.Frames[0].X, _asepriteDocument.Frames[0].Y);
        _spriteComponent.UpdateFromAnimationComponent(_asepriteDocument.Texture, sourceLoc);
    }
}