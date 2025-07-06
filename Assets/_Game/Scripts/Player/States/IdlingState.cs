using UnityEngine;

namespace Descensus
{
    public class IdlingState : MovementState
    {
        private const string IDLE_KEY = "Idle";

        public IdlingState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            _player.AnimationController.Play(IDLE_KEY);
            _player.StatesReusableData.PlayerLandedPreviously = true;
            _player.StatesReusableData.IsFallingToNewLevel = false;
            _player.InvokeSavePointChange();
        }

        public override void Update()
        {
            base.Update();

            if (IsPlayerTouchingGround() == false)
            {
                _player.StateMachine.SwitchState<FallState>();
                return;
            }
            
            if (ShouldDash())
            {
                _player.StateMachine.SwitchState<DashingState>();
                return;
            }
            
            if (_player.Input.MoveInput != 0f)
            {
                _player.StateMachine.SwitchState<MovingState>();
            }
        }
        
        public override void Exit()
        {
            base.Exit();
        }
    }
}