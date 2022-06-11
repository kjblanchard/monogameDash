using Microsoft.Xna.Framework;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using Component = SupergoonDashCrossPlatform.SupergoonEngine.Core.Component;

namespace SupergoonDashCrossPlatform.Components;

public class CoinLevelComponent : Component
{
    public CoinLevelComponent(GameObject parent, Vector2 offset = new Vector2()) : base(parent, offset)
    {
    }
}