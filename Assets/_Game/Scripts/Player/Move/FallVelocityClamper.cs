using UnityEngine;

namespace Descensus
{
    public class FallVelocityClamper
    {
        private readonly Rigidbody2D _rigidbody2D;
        private float _maxFallVelocity;

        public FallVelocityClamper(Rigidbody2D rigidbody2D, float maxFallVelocity)
        {
            _rigidbody2D = rigidbody2D;
            _maxFallVelocity = maxFallVelocity;
        }

        public void SetMaxFallVelocity(float maxFallVelocity)
        {
            _maxFallVelocity = maxFallVelocity;
        }

        public void ClampVelocity()
        {
            // If player falls faster than it should, changing y fall speed to max allowed
            if (_rigidbody2D.linearVelocity.y < -_maxFallVelocity)
            {
                _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, -_maxFallVelocity);
            }
        }
    }
}
