using Microsoft.Xna.Framework;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Interfaces;

public interface IInitialize
{

    public bool IsInitialized { get; set; }
    public void Initialize();

}