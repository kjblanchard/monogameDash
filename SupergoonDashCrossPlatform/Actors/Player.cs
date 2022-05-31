using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using TiledCS;

namespace SupergoonDashCrossPlatform.Actors;

public class Player : Actor
{
    public Player(string asepriteDocString, Vector2 location, Vector2 boxColliderOffset = new Vector2(), Point boxSize = new Point()) : base(asepriteDocString, location, boxColliderOffset, boxSize)
    {
    }
    


    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _spriteComponent.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        _spriteComponent.Draw(spriteBatch);
    }

    public new static Actor FactoryFunction(Vector2 loc, TiledProperty[] tags)
    {
        var player = new Player("idle", loc, new Vector2(), new Point(32, 32));
        player._boxColliderComponent.Debug = true;
        // player._rigidbodyComponent.GravityEnabled = false;
        player._spriteComponent.DrawOrder = 0.7f;
        return player;
        
    }

}