using System;
using LightDI;
using UnityEngine;

namespace Descensus
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerSO _config;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private Transform _wallCheck;
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private Animator _dashRewindAnimator;
        [SerializeField] private FallIndicator _fallIndicator;
        [SerializeField] private SpriteRenderer _playerSpriteRenderer;
        [SerializeField] private bool _drawGizmos;

        [Inject] private UserInput _input;

        private Rigidbody2D _rb;
        private PlayerStateMachine _stateMachine;
        private Mover _mover;
        private FallVelocityClamper _fallVelocityClamper;
        private PlayerAnimationController _animationController;
        private PlayerAnimationEventsHandler _animationEventsHandler;
        private RigidbodyLayerExcluder _layerExcluder;
        private FollowPlayer _followPlayer;
        private PlayerAudioHandler _audioHandler;
        
        public PlayerAudioHandler AudioHandler => _audioHandler;
        public FollowPlayer FollowPlayer => _followPlayer;
        public FallIndicator FallIndicator => _fallIndicator;
        public RigidbodyLayerExcluder LayerExcluder => _layerExcluder;
        public PlayerAnimationController AnimationController => _animationController;
        public Transform GroundCheck => _groundCheck;
        public Transform WallCheck => _wallCheck;
        public UserInput Input => _input;
        public PlayerStateMachine StateMachine => _stateMachine;
        public SpriteRenderer SpriteRenderer => _playerSpriteRenderer;
        public Rigidbody2D Rigidbody => _rb;
        public PlayerSO Config => _config;
        public Mover Mover => _mover;
        public FallVelocityClamper FallVelocityClamper => _fallVelocityClamper;
        public StatesReusableData StatesReusableData { get; private set; }
        
        public event Action OnPlayerDeath;
        public event Action<Vector2> OnSavePointChanged;

        private void Awake()
        {
            InjectionManager.InjectTo(this);

            _rb = GetComponent<Rigidbody2D>();
            _animationEventsHandler = GetComponentInChildren<PlayerAnimationEventsHandler>();
            _animationEventsHandler.Initialize(this);

            _fallVelocityClamper = new FallVelocityClamper(_rb, Config.MaxFallVelocity);
            _mover = new Mover(_rb, Config);
            _layerExcluder = new(_rb);
            _animationController = new PlayerAnimationController(_playerAnimator, _dashRewindAnimator);
            _audioHandler = new PlayerAudioHandler(Config, this.transform);    
            
            _followPlayer = new GameObject("FollowPlayer").AddComponent<FollowPlayer>();
            _followPlayer.SetFollow(transform);

            StatesReusableData = new StatesReusableData();
            _stateMachine = new(this);
        }

        private void Update()
        {
            _stateMachine.HandleInput();
            _stateMachine.Update();
        }

        private void LateUpdate()
        {
            _followPlayer.UpdatePosition();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
            _fallVelocityClamper.ClampVelocity();
        }

        private void OnDrawGizmos()
        {
            if (_drawGizmos && Config)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_groundCheck.position, Config.GroundCheckRadius);

                Gizmos.DrawRay(_wallCheck.position, Vector3.right * Config.WallCheckDistance);
            }
        }

        public void RewindPlayerState()
        {
            _stateMachine.SwitchState<IdlingState>();
        }

        public void InvokeDeath()
        {
            OnPlayerDeath?.Invoke();
        }

        public void InvokeSavePointChange()
        {
            OnSavePointChanged?.Invoke(Rigidbody.position);
        }
    }
}