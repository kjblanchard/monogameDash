using Microsoft.Xna.Framework;
using MonoGame.Aseprite.Documents;
using SupergoonDashCrossPlatform.SupergoonEngine.Components;
using TiledCS;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

public class Actor : GameObject
{
    protected RigidbodyComponent _rigidbodyComponent;
    protected SpriteComponent _spriteComponent;
    protected BoxColliderComponent _boxColliderComponent;
    protected AnimationComponent _animationComponent;
    protected AsepriteDocument _asepriteDocument;
    

    public Actor(string asepriteDocString,Vector2 location, Vector2 boxColliderOffset = new Vector2() , Point boxSize = new Point())
    {

        _location = location;
        _spriteComponent = new SpriteComponent(this);
        _boxColliderComponent = new BoxColliderComponent(this,boxSize);
        _rigidbodyComponent = new RigidbodyComponent(this,_boxColliderComponent);
        _asepriteDocument = _gameWorld.Content.Load<AsepriteDocument>($"Aseprite/{asepriteDocString}");
        _animationComponent = new AnimationComponent(this, _spriteComponent, _asepriteDocument);
        AddComponent(_boxColliderComponent,_rigidbodyComponent, _animationComponent, _spriteComponent);
        
    }

    public static Actor FactoryFunction(Vector2 location, TiledProperty[] tags)
    {
        throw new System.NotImplementedException();
    }
}