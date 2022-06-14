using Microsoft.Xna.Framework;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;

namespace SupergoonDashCrossPlatform.Actors;

public class LevelEnd : StaticActor
{
    public LevelEnd(ActorParams actorParams) : base(actorParams)
    {
    }
    public new static GameObject FactoryFunction(ActorParams actorParams)
    {
        var actor = new LevelEnd(actorParams);

        return actor;

    }

    public override void Initialize()
    {
        base.Initialize();
        _boxColliderComponent._size = new Point(32, 32);
        _boxColliderComponent.OverlapEvent += OnOverlapBegin;
    }
    public void OnOverlapBegin(GameObject overlapee)
    {
        if (!overlapee.HasTag(EngineTags.GameObjectTags.Player)) return;
        var player = (Player)overlapee;
        if (player != null)
        {
            player.PlayerWin();
        }
    }
}