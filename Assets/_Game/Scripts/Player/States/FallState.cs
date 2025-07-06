using Unity.Mathematics.Geometry;
using UnityEngine;

namespace Descensus
{
    public class FallState : MovementState
    {
        private const string FALL_KEY = "Fall";

        public FallState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
        {
            
        }
        
        public override void Enter()
        {
            base.Enter();
            
            if (_player.StatesReusableData.PlayerLandedPreviously)
            {
                _player.StatesReusableData.YAxisWhenFallStarted = _player.Rigidbody.position.y;
            }
            
            _player.AnimationController.Play(FALL_KEY);
            _player.FallIndicator.Show();
            _player.AudioHandler.StartFallSound();
        }
        
        public override void Update()
        {
            base.Update();

            if (_player.StatesReusableData.IsFallingToNewLevel == false)
            {
                float fallDistance =
                    Mathf.Abs(_player.Rigidbody.position.y - _player.StatesReusableData.YAxisWhenFallStarted);

                _player.FallIndicator.SetAmount(fallDistance / _player.Config.YFallDistanceToGameOver);

                // distance from fall start is big and hero is not falling to new level
                if (fallDistance >= _player.Config.YFallDistanceToGameOver &&
                    _player.StatesReusableData.IsFallingToNewLevel == false)
                {
                    _player.StateMachine.SwitchState<DeathState>();
                    return;
                }
            }
            else
            {
                if (_player.FallIndicator.gameObject.activeSelf)
                {
                    _player.FallIndicator.Hide();
                }
            }
            
            if (ShouldStartWallSlide())
            {
                _player.StateMachine.SwitchState<WallSlideState>();
                return;
            }
            
            if (ShouldDash())
            {
                _player.StateMachine.SwitchState<DashingState>();
                return;
            }
            
            if (IsPlayerTouchingGround())
            {
                _player.StateMachine.SwitchState<IdlingState>();
                _player.AudioHandler.PlayLandSound();
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            _player.FallIndicator.Hide();
            _player.AudioHandler.StopFallSound();
            _player.StatesReusableData.PlayerLandedPreviously = false;
        }
    }
}