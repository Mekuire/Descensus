using AnnulusGames.LucidTools.Audio;
using UnityEngine;

namespace Descensus
{
    public class DashingState : MovementState
    {
        private float _elapsedTime;
        private float _dashDirection;

        public DashingState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _elapsedTime = 0f;
            _player.StatesReusableData.CanDashTimer =
                MinimalTimer.Start(_player.Config.DashTime + _player.Config.DashRewindTime);
            _dashDirection = _player.SpriteRenderer.flipX ? -1f : 1f;

            _player.Rigidbody.gravityScale = 0f;
            _player.Rigidbody.linearVelocity = Vector2.zero;
            _player.AnimationController.Play("Dash");

            _player.AudioHandler.PlayDashSound();
        }

        public override void Update()
        {
            //base.Update();
        }

        public override void FixedUpdate()
        {
            _elapsedTime += Time.fixedDeltaTime;
            
            // elapsed dash time (from 0 to 1)
            float t = Mathf.Clamp01(_elapsedTime / _player.Config.DashTime);
            
            // Calculating distance that should be moved and multiplying it by dash curve (for the smooth end)
            float dashVelocity = (_player.Config.DashDistance / _player.Config.DashTime) * _player.Config.DashCurve.Evaluate(t);

            // Here I use not smooth movement for controlling dash
            _player.Mover.Move(_dashDirection * dashVelocity, Time.fixedDeltaTime);

            if (_elapsedTime >= _player.Config.DashTime)
            {
                EndDash();
            }
        }

        private void EndDash()
        {
            _player.AnimationController.PlayDashRewindEffect();
            _player.Rigidbody.gravityScale = _player.Config.DefaultGravityScale;

            if (IsPlayerTouchingGround())
            {
                _playerStateMachine.SwitchState<IdlingState>();
                return;
            }

            if (ShouldStartWallSlide())
            {
                _playerStateMachine.SwitchState<WallSlideState>();
                return;
            }

            _playerStateMachine.SwitchState<FallState>();
        }

        public override void Exit()
        {
            base.Exit();
            _player.Rigidbody.gravityScale = _player.Config.DefaultGravityScale;
        }
    }
}