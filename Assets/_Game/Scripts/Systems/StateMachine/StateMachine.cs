using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Descensus
{
    public abstract class StateMachine : IStateSwitcher
    {
        protected IState _currentState;
        protected List<IState> _states;

        public void SwitchState<T>() where T : IState
        {
            var state = _states?.FirstOrDefault(state => state is T);

            if (state == null)
            {
                Debug.Log("State is null");
                return;
            }

            _currentState.Exit();
            _currentState = state;
            _currentState.Enter();
        }

        public void HandleInput()
        {
            _currentState.HandleInput();
        }

        public void Update()
        {
            _currentState.Update();
        }

        public void FixedUpdate()
        {
            _currentState.FixedUpdate();
        }
    }
}