using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Interfaces;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Core;

public abstract class StateMachine<T> : IState where T : IState
{
    protected T _currentState;
    protected T _previousState;
    public T GetState(int tag) => States.FirstOrDefault(state => state.HasTag(tag));
    protected void AddState(T stateToAdd) => States.Add(stateToAdd);

    protected List<T> States = new();

    //TODO do this differently.
    private bool stateChanging = false;
    private int nextLevelTag = 0;

    public void InitializeStates()
    {
        States.ForEach(state => state.Initialize());
    }

    public override void Update(GameTime gameTime)
    {
        if(stateChanging)
            InternalChangeState();
        base.Update(gameTime);
        _currentState?.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        _currentState?.Draw(spriteBatch);
    }

    public virtual void ChangeState(int tag)
    {
        stateChanging = true;
        nextLevelTag = tag;
        if(_currentState == null)
            InternalChangeState();

    }

    private void InternalChangeState()
    {
        stateChanging = false;
        var newState = GetState(nextLevelTag);
        if (_currentState != null)
        {
            _currentState?.EndState();
            _previousState = _currentState;
        }

        _currentState = newState;
        _currentState.StartState();
        
    }
}