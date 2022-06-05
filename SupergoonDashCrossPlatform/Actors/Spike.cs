using Microsoft.Xna.Framework;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using TiledCS;

namespace SupergoonDashCrossPlatform.Actors;

public class Spike : Actor
{

    public Spike(string asepriteDocString, Vector2 location, Vector2 boxColliderOffset = new Vector2(), Point boxSize = new Point(), float jump = 150) : base(asepriteDocString, location, boxColliderOffset, boxSize, jump)
    {
    }
    
    public new static Actor FactoryFunction(Vector2 loc, TiledProperty[] tags)
    {
        var actor = new Spike("spikel", loc, new Vector2(6, 10), new Point(20, 22));
        actor.Initialize();

        return actor;
    }
    
}