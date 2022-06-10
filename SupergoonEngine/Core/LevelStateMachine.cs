using Microsoft.Xna.Framework;
using SupergoonEngine.Tiled;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

public class LevelStateMachine : StateMachine<Level>
{

    public LevelStateMachine(TiledGameComponent tiledGameComponent)
    {
        Level.SetTiledGameComponent(tiledGameComponent);
    }

    public void AddLevel(Level level)
    {
        AddState(level);
        
    }
    
    
}