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
        return actor;
    }

    public override void Initialize()
    {
        _spriteComponent.DrawOrder = 0.6f;
        _soundComponent = new SoundComponent(this);
        AddComponent(_soundComponent);
        _boxColliderComponent.OverlapEvent += OnOverlapBegin;
        base.Initialize();
    }


    private Coin(ActorParams actorParams) : base(actorParams)
    {
    }

    public void OnOverlapBegin(GameObject overlapee)
    {
        if (overlapee.HasTag(EngineTags.GameObjectTags.Player))
        {
            var player = (Player)overlapee;
            if (player != null)
                player.OnCoinOverlap();
            _soundComponent.PlaySfx("coin");
            Enabled = false;
        }
    }
}