using UnityEngine;

namespace Descensus
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private string _nextLevelPath;
        [SerializeField] private Color _colorTheme;
        [SerializeField] private AudioClip _musicClip;
        [SerializeField] private LevelTransitionTrigger _levelTransitionTrigger;
        
        public string NextLevelPath => _nextLevelPath;
        public LevelTransitionTrigger LevelTransitionTrigger => _levelTransitionTrigger;
        public Color ColorTheme => _colorTheme;
        public AudioClip MusicClip => _musicClip;
    }
}
