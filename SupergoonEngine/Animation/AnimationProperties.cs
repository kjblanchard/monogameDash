using System.Collections.Generic;
using System.Data.SqlTypes;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Animation;

public class AnimationProperties
{
    public string Name;
    public bool Looping;
    public List<AnimationTransition> Transitions = new List<AnimationTransition>();

    public AnimationProperties(string name, bool looping = true)
    {
        Name = name;
        Looping = looping;
    }
}