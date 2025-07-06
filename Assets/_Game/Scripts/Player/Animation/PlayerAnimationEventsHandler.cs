using UnityEngine;
using UnityEngine.Audio;

namespace Descensus
{
    public class PlayerAnimationEventsHandler : MonoBehaviour
    {
        private Player _player;
        public void Initialize(Player player) => _player = player;
        
        // This method calling on move animation (Animation has key frames when this method should be called)
        public void PlayStepSound() => _player.AudioHandler.PlayStepSound();
    }
}
