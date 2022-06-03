using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SupergoonDashCrossPlatform.SupergoonEngine.Interfaces;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Animation;

public class Animator : IUpdate
{
    private List<AnimationProperties> Animations = new();
    public AnimationProperties CurrentAnimation;
    public string CurrentAnimationTag;

    public event AnimationChangeEventArgs AnimationTransitionEvent;

    public delegate void AnimationChangeEventArgs();

    public AnimationTransition EntryTransition;


    public void AddAnimationTransition(params AnimationProperties[] transition)
    {
        Animations.AddRange(transition);
    }

    public void ChangeAnimation(string animationTag)
    {
        CurrentAnimationTag = animationTag;
        AnimationTransitionEvent.Invoke();
        CurrentAnimation = Animations.FirstOrDefault(anim => anim.Name == animationTag);
    }

    public void Update(GameTime gameTime)
    {
        var transitionMadeThisUpdate = false;
        if (CurrentAnimation != null)
        {
            CurrentAnimation.Transitions.ForEach(transition =>
            {
                if (!transition.TransitionCondition.Invoke()) return;
                CurrentAnimationTag = transition.TransitionAnimationTag;
                transitionMadeThisUpdate = true;
                AnimationTransitionEvent.Invoke();
                CurrentAnimation = Animations.FirstOrDefault(anim => anim.Name == transition.TransitionAnimationTag);
            });
        }
        //Try the entry transition to move to the default animation.
        else
        {
            if (EntryTransition == null)
                throw new Exception("You didn't specify an entry transition animation");
            if (!EntryTransition.TransitionCondition.Invoke()) return;
            CurrentAnimationTag = EntryTransition.TransitionAnimationTag;
            AnimationTransitionEvent.Invoke();
            CurrentAnimation = Animations.FirstOrDefault(anim => anim.Name == EntryTransition.TransitionAnimationTag);
        }
    }

    public bool Enabled { get; set; }
    public int UpdateOrder { get; set; }
    public event EventHandler<EventArgs> EnabledChanged;
    public event EventHandler<EventArgs> UpdateOrderChanged;
}