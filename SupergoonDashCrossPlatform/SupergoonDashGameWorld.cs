using Microsoft.Xna.Framework;
using SupergoonDashCrossPlatform.Actors;
using SupergoonDashCrossPlatform.SupergoonEngine.Cameras;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using SupergoonDashCrossPlatform.Tags;
using SupergoonEngine.Tiled;

namespace SupergoonDashCrossPlatform
{
    public class SupergoonDashGameWorld : GameWorld
    {
        protected override void Initialize()
        {
            base.Initialize();
            TiledActorFactory.NameToSpawnFunction["player"] = Player.FactoryFunction;
            TiledActorFactory.NameToSpawnFunction["coin"] = Coin.FactoryFunction;
            TiledActorFactory.NameToSpawnFunction["spikel"] = Spike.FactoryFunction;
            TiledActorFactory.NameToSpawnFunction["end"] = LevelEnd.FactoryFunction;
        }

        protected override void BeginRun()
        {
            base.BeginRun();
            _graphicsGameComponent.ApplyResolutionSettings(false);

            var level1 = new Level("level1");
            level1.AddTag(LevelTags.Level1);
            AddLevels(level1);
            InitializeLevels();
            //TODO remove this, just there to start the music on restart and start currently.
            Reset();
            ChangeLevel(LevelTags.Level1);
        }

        public override void Reset()
        {
            base.Reset();
            CameraGameComponent.MainCamera.Location = Vector3.Zero;
            _soundGameComponent.PlayBgm("level1");
        }
    }
}