namespace Descensus
{
    /// <summary>
    /// This interface is not used in project
    /// </summary>
    public interface IStateSwitcher
    {
        void SwitchState<T>() where T : IState;
    }
}

