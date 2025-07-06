using UnityEngine;

namespace Descensus
{
    /// <summary>
    /// Base state class that holds some useful methods for all states
    /// </summary>
    public abstract class MovementState : IState
    {
        protected readonly PlayerStateMachine _playerStateMachine;
        protected readonly Player _player;

        public MovementState(PlayerStateMachine playerStateMachine, Player player)
        {
            _playerStateMachine = playerStateMachine;
            _player = player;
            _player.StatesReusableData.CanDashTimer = MinimalTimer.Start(0f);
        }

        public virtual void Enter()
        {
            //Debug.Log(GetType().Name + " entered");
        }

        // This method not used in project, but here should perform some input logic
        public virtual void HandleInput()
        {
        }

        public virtual void Update()
        {
            SetFaceDirection();
        }

        public virtual void FixedUpdate()
        {
            _player.Mover.MoveSmooth(_player.Input.MoveInput,  Time.fixedDeltaTime);
        }

        public virtual void Exit()
        {
        }

        private void SetFaceDirection()
        {
            if (_player.Input.MoveInput == 0f) return;

            _player.SpriteRenderer.flipX = _player.Input.MoveInput < 0f;
        }

        protected bool IsPlayerTouchingGround()
        {
            return Physics2D.OverlapCircle(
                _player.GroundCheck.position,
                _player.Config.GroundCheckRadius,
                _player.Config.GroundLayer
            );
        }
        
        protected bool IsTouchingWall()
        {
            Vector2 direction = _player.SpriteRenderer.flipX ? Vector2.left : Vector2.right;

            RaycastHit2D hit = Physics2D.Raycast(
                _player.WallCheck.position,
                direction,
                _player.Config.WallCheckDistance,
                _player.Config.GroundLayer
            );

            Debug.DrawRay(_player.WallCheck.position, direction * _player.Config.WallCheckDistance, Color.yellow);
            
            return hit.collider != null;
        }
        
        protected bool ShouldStartWallSlide()
        {
            bool isTouchingWall = IsTouchingWall();
            bool isGrounded = IsPlayerTouchingGround();
            // I decided to not use this check for direct movement towards wall
            bool isMovingTowardWall = (_player.Input.MoveInput > 0f && _player.SpriteRenderer.flipX == false) ||
                                      (_player.Input.MoveInput < 0f && _player.SpriteRenderer.flipX == true);

            return isTouchingWall && !isGrounded /* && isMovingTowardWall */;
        }

        protected bool ShouldDash()
        {
            return CanDash() && _player.Input.DashWasPressed;
        }

        protected bool CanDash()
        {
            return _player.StatesReusableData.CanDashTimer.IsCompleted;
        }
    }
}