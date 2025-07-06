using UnityEngine;

namespace Descensus
{
    public class DeathState : MovementState
    {
        private const string UNCONTROLLED_FALL_KEY = "UncontrolledFall";

        public DeathState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            _player.Rigidbody.linearVelocity = new Vector2(0f, _player.Rigidbody.linearVelocity.y);
            _player.AnimationController.Play(UNCONTROLLED_FALL_KEY);
            _player.LayerExcluder.ExcludeLayers(_player.Config.ExcludeWhenFall);
            _player.AudioHandler.PlayDeathSound();
            _player.InvokeDeath();
        }
        
        public override void Update()
        {
            //base.Update();
        }

        public override void FixedUpdate()
        {
            //base.FixedUpdate();
        }

        public override void Exit()
        {
            base.Exit();
            _player.LayerExcluder.ClearExcludedLayers();
        }
    }
}