using Microsoft.Xna.Framework;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using SupergoonEngine.Tiled;

namespace SupergoonDashCrossPlatform
{
    public class SupergoonDashGameWorld : GameWorld
    {
        private TiledGameComponent _tiledGameComponent;
        protected override void Initialize()
        {
            base.Initialize();
            //Add the tiled Component
            _tiledGameComponent = new TiledGameComponent(this);
            Components.Add(_tiledGameComponent);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
           _spriteBatch.Begin(); 
           _tiledGameComponent.LoadedTmxContent.DrawTilemap(_spriteBatch);
           _spriteBatch.End();
        }
    }
}