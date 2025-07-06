using UnityEngine;

namespace Descensus
{
    public class Mover
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly PlayerSO _config;

        private float _currentVelocity;

        public Mover(Rigidbody2D rigidbody, PlayerSO config)
        {
            _rigidbody = rigidbody;
            _config = config;
        }

        // Move without smoothness (For Dash)
        public void Move(float direction, float deltaTime)
        {
            _currentVelocity = direction * deltaTime;
            _rigidbody.linearVelocity = new Vector2(_currentVelocity,  _rigidbody.linearVelocity.y);
        }

        // For default movement with smoothness
        public void MoveSmooth(float direction, float deltaTime)
        {
            float desiredVelocity = direction * _config.MaxSpeed;

            float accel = (direction != 0f) ? _config.Acceleration : _config.Deceleration;

            _currentVelocity = Mathf.MoveTowards(_currentVelocity, desiredVelocity, accel * deltaTime);

            _rigidbody.linearVelocity = new Vector2(_currentVelocity,  _rigidbody.linearVelocity.y);
        }
    }
}
