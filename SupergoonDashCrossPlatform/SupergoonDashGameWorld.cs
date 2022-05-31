using SupergoonDashCrossPlatform.Actors;
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
        }

        protected override void BeginRun()
        {
            base.BeginRun();
            _graphicsGameComponent.ApplyResolutionSettings(false);

            var level1 = new Level("level1");
            level1.AddTag(LevelTags.Level1);
            AddLevels(level1);
            InitializeLevels();
            ChangeLevel(LevelTags.Level1);
        }

    }
}