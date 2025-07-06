using System.Collections.Generic;

namespace Descensus
{
    /// <summary>
    /// A descendant of the State machine class that creates all possible states of player and enters to idling on start
    /// </summary>
    public class PlayerStateMachine : StateMachine
    {
        public PlayerStateMachine(Player player)
        {
            _states = new List<IState>()
            {
                new IdlingState(this, player),
                new MovingState(this, player),
                new DashingState(this, player),
                new WallSlideState(this, player),
                new FallState(this, player),
                new DeathState(this, player)
            };

            _currentState = _states[0];
            _currentState.Enter();
        }
    }
}
