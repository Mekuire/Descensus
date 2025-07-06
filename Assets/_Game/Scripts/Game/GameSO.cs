using UnityEngine;
using UnityEngine.Audio;

namespace Descensus
{
    [CreateAssetMenu(fileName = "GameSO", menuName = "SO/GameSO")]
    public class GameSO : ScriptableObject
    {
        [field: SerializeField] public Vector2 PlayerRewindPosition { get; private set; }
        [field: SerializeField] public float ColorLerpDuration { get; private set; } = 0.5f;
        [field: SerializeField] public float GameOverTitleDuration { get; private set; } = 0.8f;
        [field: SerializeField] public float FadeDuration { get; private set; } = 0.4f;
        [field: SerializeField] public float CameraReturnDuration { get; private set; } = 0.4f;
        [field: SerializeField] public AudioMixerGroup MusicGroup { get; private set; } 
        
    }
}
