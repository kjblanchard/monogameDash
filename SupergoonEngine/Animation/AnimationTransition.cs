using System;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Animation;

public class AnimationTransition
{
    public string TransitionAnimationTag;
    public Func<bool> TransitionCondition;

    public AnimationTransition(string transitionAnimationTag, Func<bool> transitionCondition)
    {
        TransitionAnimationTag = transitionAnimationTag;
        TransitionCondition = transitionCondition;
    }
}