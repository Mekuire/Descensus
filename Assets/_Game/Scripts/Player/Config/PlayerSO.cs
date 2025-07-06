using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "SO/PlayerSO")]
    public class PlayerSO : ScriptableObject
    {
        [field: SerializeField] public float MaxSpeed { get; private set; } = 4f;
        [field: SerializeField] public float Acceleration { get; private set; } = 1f;
        [field: SerializeField] public float Deceleration { get; private set; } = 1f;
        [field: SerializeField] public float MaxFallVelocity { get; private set; } = 7f;
        [field: SerializeField] public float DashTime { get; private set; } = 0.5f;
        [field: SerializeField] public float DashDistance { get; private set; } = 3f;
        [field: SerializeField] public float DashRewindTime { get; private set; } = 0.3f;
        [field: SerializeField] public float DefaultGravityScale { get; private set; } = 3f;
        [field: SerializeField] public float WalledGravityScale { get; private set; } = 0.4f;
        [field: SerializeField] public float GroundCheckRadius { get; private set; } = 0.3f;
        [field: SerializeField] public float WallCheckDistance { get; private set; } = 0.3f;
        [field: SerializeField] public float YFallDistanceToGameOver { get; private set; } = 8f;
        [field: SerializeField] public LayerMask ExcludeWhenFall { get; private set; } 
        [field: SerializeField] public LayerMask GroundLayer { get; private set; } 
        [field: SerializeField] public AnimationCurve DashCurve { get; private set; }
        [field: SerializeField] public AudioMixerGroup SfxGroup { get; private set; }
        [field: SerializeField, Range(0f, 1f)] public float SfxSpatialBlend { get; private set; }
        [field: SerializeField] public AudioClip[] StepSounds { get; private set; }
        [field: SerializeField] public AudioClip DashSound { get; private set; }
        [field: SerializeField] public AudioClip LandSound { get; private set; }
        [field: SerializeField] public AudioClip FallSound { get; private set; }
        [field: SerializeField] public AudioClip DeathSound { get; private set; }
        [field: SerializeField] public AudioClip GameOverSound { get; private set; }
        [field: SerializeField] public AudioClip SlideSound { get; private set; }
        [field: SerializeField] public AudioClip TransitionSound { get; private set; }
    }
