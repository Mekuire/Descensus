namespace Descensus
{
    /// <summary>
    /// Every state should implement this interface so you can add this state to state machine 
    /// </summary>
    public interface IState
    {
        void Enter();
        void HandleInput();
        void Update();
        void FixedUpdate();
        void Exit();
    }
}

