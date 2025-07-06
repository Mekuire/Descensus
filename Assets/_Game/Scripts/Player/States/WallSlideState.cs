using UnityEngine;

namespace Descensus
{
    public class WallSlideState : MovementState
    {
        private const string WALL_SLIDE_KEY = "WallSlide";

        public WallSlideState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
        {
            
        }
        
        public override void Enter()
        {
            base.Enter();
            _player.StatesReusableData.PlayerLandedPreviously = true;
            _player.Rigidbody.linearVelocity = Vector2.zero;
            _player.Rigidbody.gravityScale = _player.Config.WalledGravityScale;
            _player.AnimationController.Play(WALL_SLIDE_KEY);
            _player.AudioHandler.StartSlideSound();
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void Update()
        {
            //base.Update();

            if (IsTouchingWall() == false)
            {
                _player.StateMachine.SwitchState<FallState>();
                return;
            }

            if (IsPlayerTouchingGround())
            {
                _player.StateMachine.SwitchState<IdlingState>();
                return;
            }
            
            if (_player.Input.MoveInput != 0f)
            {
                float possibleMoveSide = _player.SpriteRenderer.flipX ? 1f : -1f;

                if (Mathf.Approximately(_player.Input.MoveInput, possibleMoveSide))
                {
                    _player.StateMachine.SwitchState<FallState>();
                    return;
                }
            }
            
            if (ShouldDash())
            {
                _player.StateMachine.SwitchState<DashingState>();
            }
        }

        public override void FixedUpdate()
        {
            //base.FixedUpdate();
        }

        public override void Exit()
        {
            base.Exit();
            _player.AudioHandler.StopSlideSound();
            _player.SpriteRenderer.flipX = !_player.SpriteRenderer.flipX;
            _player.Rigidbody.gravityScale = _player.Config.DefaultGravityScale;
        }
    }
}
