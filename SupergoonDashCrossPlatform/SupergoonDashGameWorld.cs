using SupergoonDashCrossPlatform.SupergoonEngine.Core;

namespace SupergoonDashCrossPlatform
{
    public class SupergoonDashGameWorld : GameWorld
    {
        protected override void BeginRun()
        {
            base.BeginRun();
            _graphicsGameComponent.ApplyResolutionSettings(false);
        }
    }
}