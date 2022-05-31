using Microsoft.Xna.Framework;
using SupergoonEngine.Tiled;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Physics;

public class PhysicsGameComponent : GameComponent
{
    private TiledGameComponent _tiledGameComponent;
    public Gravity Gravity;
    public PhysicsGameComponent(Game game, TiledGameComponent tiledGameComponent) : base(game)
    {
        _tiledGameComponent = tiledGameComponent;
        Gravity = new Gravity(_tiledGameComponent);
    }



}