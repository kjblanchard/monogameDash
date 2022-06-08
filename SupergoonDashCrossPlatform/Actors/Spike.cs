using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

    public Spike(ActorParams actorParams) : base(actorParams)
    {
    }
}