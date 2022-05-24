using Microsoft.Xna.Framework;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using SupergoonDashCrossPlatform.Tiled;

namespace SupergoonDashCrossPlatform
{
    public class SupergoonDashGameWorld : GameWorld
    {
        private TiledComponent _tiledComponent;
        protected override void Initialize()
        {
            base.Initialize();
            //Add the tiled Component
            _tiledComponent = new TiledComponent(this);
            Components.Add(_tiledComponent);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
           _spriteBatch.Begin(); 
           _tiledComponent.LoadedTmxContent.DrawTilemap(_spriteBatch);
           _spriteBatch.End();
        }
    }
}