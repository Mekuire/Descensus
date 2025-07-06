using UnityEngine;

namespace Descensus
{
    public readonly struct MinimalTimer
    {
        public static MinimalTimer Start(float duration) => new(duration);
        private MinimalTimer(float duration) => _triggerTime = Time.time + duration;
        public bool IsCompleted => Time.time >= _triggerTime;

        private readonly float _triggerTime;
    }
}
