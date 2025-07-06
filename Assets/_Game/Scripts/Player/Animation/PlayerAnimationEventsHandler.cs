using UnityEngine;
using UnityEngine.Audio;

namespace Descensus
{
    public class PlayerAnimationEventsHandler : MonoBehaviour
    {
        private Player _player;
        public void Initialize(Player player) => _player = player;
        
        // This method is called during the animation of the move (the animation has keyframes when this method should be called)
        public void PlayStepSound() => _player.AudioHandler.PlayStepSound();
    }
}
