namespace Descensus
{
    /// <summary>
    /// Holds some usefull information so states will work in a right way
    /// </summary>
    public class StatesReusableData
    {
        public MinimalTimer CanDashTimer;
        public bool IsFallingToNewLevel;
        public bool PlayerLandedPreviously;
        public float YAxisWhenFallStarted;
    }
}
