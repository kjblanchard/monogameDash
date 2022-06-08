using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using TiledCS;

namespace SupergoonDashCrossPlatform.Actors;

public class Coin : Actor
{
    public new static GameObject FactoryFunction(ActorParams actorParams)
    {

        actorParams.AsepriteDocString = "coin";
        var actor = new Coin(actorParams);
        actor.Initialize();
        return actor;
    }


    private Coin(ActorParams actorParams) : base(actorParams)
    {
    }
}