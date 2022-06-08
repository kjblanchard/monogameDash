using Microsoft.Xna.Framework;
using SupergoonDashCrossPlatform.Sound;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Components;

public class SoundComponent : Component
{
    public static SoundGameComponent _soundGameComponent;

    public void PlaySfx()
    {
        _soundGameComponent.PlaySfx();
        
        
    }

    public SoundComponent(GameObject parent, Vector2 offset = new Vector2()) : base(parent, offset)
    {
        UpdateOrder = 10;
    }
}