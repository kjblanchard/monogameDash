using Microsoft.Xna.Framework;
using SupergoonDashCrossPlatform.SupergoonEngine.Animation;
using SupergoonDashCrossPlatform.SupergoonEngine.Components;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;

namespace SupergoonDashCrossPlatform.Actors;

public class Coin : Actor
{
    private SoundComponent _soundComponent;
    
    
    public new static GameObject FactoryFunction(ActorParams actorParams)
    {

        actorParams.AsepriteDocString = "coin";
        actorParams.BoxSize = new Point(32, 32);

        //coin animations
        var entryTransition = new AnimationTransition("entry", () => true);
        var actor = new Coin(actorParams);
        var spinAnimation = new AnimationProperties("entry");
        actor._rigidbodyComponent.GravityEnabled = false;
        actor._animationComponent.SetAnimationEntryTransmission(entryTransition);
        actor._animationComponent.AddAnimationTransition(spinAnimation);
        actor.Initialize();
        return actor;
    }

    public override void Initialize()
    {
        _spriteComponent.DrawOrder = 0.6f;
        _soundComponent = new SoundComponent(this);
        AddComponent(_soundComponent);
        _boxColliderComponent.OverlapEvent += OnOverlap;
        base.Initialize();
    }


    private Coin(ActorParams actorParams) : base(actorParams)
    {
    }

    public void OnOverlap(GameObject overlapee)
    {
        if (overlapee.HasTag(EngineTags.GameObjectTags.Player))
        {
            _soundComponent.PlaySfx();
            
        }
    }
}