using SupergoonDashCrossPlatform.SupergoonEngine.Components;
using SupergoonEngine.Tiled;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Physics;

public class Collision
{
    private static TiledGameComponent _tiledGameComponent;
    public Collision(TiledGameComponent tiledGameComponent)
    {
        _tiledGameComponent = tiledGameComponent;

    }
    public static bool CheckIfCollisionForAllSolidTiles(BoxColliderComponent movingObject)
    {
       // _tiledGameComponent.LoadedTmxContent 
       return true;

    }
    public static bool CheckIfCollision(BoxColliderComponent a, BoxColliderComponent b)
    {
        return false;
    }
}