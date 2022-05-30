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
    protected T GetState(int tag) => States.FirstOrDefault(state => state.HasTag(tag));
    protected void AddState(T stateToAdd) => States.Add(stateToAdd);

    protected List<T> States = new();

    public void InitializeStates()
    {
        States.ForEach(state => state.Initialize());
    }

    public override void Update(GameTime gameTime)
    {
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
        var newState = GetState(tag);
        if (_currentState != null)
        {
            _currentState?.EndState();
            _previousState = _currentState;
        }

        _currentState = newState;
        _currentState.StartState();
    }
}