namespace Descensus
{
    public interface IState
    {
        void Enter();
        void HandleInput();
        void Update();
        void FixedUpdate();
        void Exit();
    }
}

