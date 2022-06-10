using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Components;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using TiledCS;

namespace SupergoonDashCrossPlatform.Actors;

public class Spike : StaticActor
{

    
    public new static GameObject FactoryFunction(ActorParams actorParams)
    {
        var actor = new Spike(actorParams);
        actor.Initialize();
        
        return actor;
    }

    private Spike(ActorParams actorParams) : base(actorParams)
    {
        _spriteComponent.DrawOrder = 0.6f;
        _boxColliderComponent.OverlapEvent += OnOverlapBegin;
    }
    public void OnOverlapBegin(GameObject overlapee)
    {
        if (!overlapee.HasTag(EngineTags.GameObjectTags.Player)) return;
        var player = (Player)overlapee;
        if (player != null)
        {
            player.PlayerDeath();
        }
    }
}