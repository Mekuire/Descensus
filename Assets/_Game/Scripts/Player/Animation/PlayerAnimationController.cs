using UnityEngine;

namespace Descensus
{
    public class PlayerAnimationController
    {
        private readonly Animator _playerAnimator;
        private readonly Animator _dashRewindAnimator;
        
        private const string REWIND_DASH_EFFECT_KEY = "Rewind";

        public PlayerAnimationController(Animator playerAnimator, Animator dashRewindAnimator)
        {
            _playerAnimator = playerAnimator;
            _dashRewindAnimator = dashRewindAnimator;
        }
        
        public void Play(string stateName, int layer = 0)
        {
            _playerAnimator.Play(stateName, layer, 0f);
        }

        public void PlayDashRewindEffect()
        {
            _dashRewindAnimator.Play(REWIND_DASH_EFFECT_KEY, 0, 0f);
        }
    }
}
