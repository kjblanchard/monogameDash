using Microsoft.Xna.Framework;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

public class LevelGameComponent : DrawableGameComponent
{

    public Level CurrentLevel;
    
    #region Configuration

    #endregion

    

    #region Methods

    #endregion

    public LevelGameComponent(Game game) : base(game)
    {
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
    }
}