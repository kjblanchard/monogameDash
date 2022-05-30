using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using SupergoonDashCrossPlatform.Tags;

namespace SupergoonDashCrossPlatform
{
    public class SupergoonDashGameWorld : GameWorld
    {
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