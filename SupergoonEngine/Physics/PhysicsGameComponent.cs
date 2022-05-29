using Microsoft.Xna.Framework;
using SupergoonDashCrossPlatform.SupergoonEngine.Components;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Physics;

public class PhysicsGameComponent : GameComponent
{
    public PhysicsGameComponent(Game game) : base(game)
    {
    }

    public Gravity Gravity = new ();
    
    
}