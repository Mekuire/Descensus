using UnityEngine;

namespace Descensus
{
    public class MovingState : MovementState
    {
        private const string MOVE_KEY = "Move";

        public MovingState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
        { }

        public override void Enter()
        {
            base.Enter();
            _player.AnimationController.Play(MOVE_KEY);
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

            if (_player.Input.MoveInput == 0f)
            {
                _player.StateMachine.SwitchState<IdlingState>();
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}