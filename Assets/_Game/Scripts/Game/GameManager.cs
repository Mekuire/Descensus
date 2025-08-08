using System.Collections;
using AnnulusGames.LucidTools.Audio;
using AnnulusGames.SceneSystem;
using DG.Tweening;
using LightDI;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Audio;

namespace Descensus
{
    public class GameManager : SystemBase
    {
        [Header("Start Level")]
        [SerializeField] private Transform _startSavePoint;
        [SerializeField] private Level _startLevel;
        [Header("Player")]
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private ParticleSystem _transitionEffect;
        [Header("Camera")]
        [SerializeField] private CinemachineCamera _camera;
        [SerializeField] private CinemachinePositionComposer _positionComposer;
        [Header("Audio")]
        [SerializeField, Min(0f)] private float _fadeInDuration = 0.3f;
        [SerializeField, Min(0f)] private float _fadeOutDuration = 0.3f;
        [Header("UI")]
        [SerializeField] private GameCanvas _gameCanvasPrefab;
        [SerializeField] private LoadingScreen _loadingScreenPrefab;
        [Header("Configs")]
        [SerializeField] private PlayerSO _playerConfig;
        [SerializeField] private GameSO _gameConfig;
        
        [Inject] private UserInput  _userInput;
        
        private const string MENU_SCENE_NAME = "Menu";

        private GameCanvas _gameCanvas;
        private Vector2 _currentSavePoint;
        private Player _player;
        private Level _currentLevel;
        private bool _isMovingToNewLocation;
        
        private void Awake()
        {
            InjectionManager.InjectTo(this);
            
            // First level is already on scene
            _currentLevel = _startLevel;
            SetNewLevel(colorLerpDuration: 0f);
            PlayMusic(_currentLevel.MusicClip);

            // Creating Canvas
            _gameCanvas = Instantiate(_gameCanvasPrefab);
            _gameCanvas.Initialize(_userInput);
            _gameCanvas.PausePanel.OnPauseClosed += ResumeGame;
            _gameCanvas.PausePanel.OnPauseOpened += PauseGame;
            _gameCanvas.PausePanel.OnExitPressed += ExitToMainMenu;
            
            // Creating player on start to have more control of the game
            _player = Instantiate(_playerPrefab, _startSavePoint.position, Quaternion.identity);
            _player.OnPlayerDeath += HandleGameOver;
            _player.OnSavePointChanged += SetSavePoint;
            
            // Set camera to follow object that follows player only on Y axis
            _camera.Follow = _player.FollowPlayer.transform;
        }

        private void SetNewLevel(float colorLerpDuration)
        {
            _currentSavePoint = _startSavePoint.position;
            _currentLevel.LevelTransitionTrigger.Initialize(this);
            LerpCameraColor(_currentLevel.ColorTheme, colorLerpDuration);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            // Check if object still exists
            if (_player)
            {
                _player.OnPlayerDeath -= HandleGameOver;
                _player.OnSavePointChanged -= SetSavePoint;
            }

            if (_gameCanvas)
            {
                _gameCanvas.PausePanel.OnPauseClosed += ResumeGame;
                _gameCanvas.PausePanel.OnPauseOpened += PauseGame;
                _gameCanvas.PausePanel.OnExitPressed += ExitToMainMenu;
            }
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                HandleGameOver();
            }
#endif
        }

        public void SetSavePoint(Vector2 savePoint)
        {
            _currentSavePoint = savePoint;
        }
        
        public void StopMusic()
        {
            LucidAudio.StopAllBGM(_fadeOutDuration);
        }

        private void PlayMusic(AudioClip clip)
        {
            LucidAudio.PlayBGM(clip, _fadeInDuration)
                .SetLoop(true)
                .SetAudioMixerGroup(_gameConfig.MusicGroup);
        }
        
        public void PauseGame()
        {
            Time.timeScale = 0f;
            DisablePlayerInput();
            LucidAudio.PauseAll();
        }

        public void ResumeGame()
        {
            Time.timeScale = 1f;
            LucidAudio.UnPauseAll();
            _gameCanvas.PlayButtonClick();
            EnablePlayerInput();
        }

        public void EnablePlayerInput()
        {
            _userInput.EnableAction(UserActions.Move);
            _userInput.EnableAction(UserActions.Dash);
        }

        public void DisablePlayerInput()
        {
            _userInput.DisableAction(UserActions.Move);
            _userInput.DisableAction(UserActions.Dash);
        }
        
        public void HandleGameOver()
        {
            StartCoroutine(nameof(GameOver));
        }

        public void LerpCameraColor(Color color, float duration)
        {
            Camera camera = Camera.main;
            
            if (camera == null) return;
            
            camera.DOColor(color, duration).SetLink(camera.gameObject);
        }

        public void MoveToNewLocation()
        {
            if (_isMovingToNewLocation) return;
            
            StartCoroutine(nameof(SetNewLocation), _currentLevel.NextLevelPath);
        }

        public void PlayTransitionEffect()
        {
            _transitionEffect.Play();
            LucidAudio.PlaySE(_player.Config.TransitionSound).SetAudioMixerGroup(_player.Config.SfxGroup);
        }

        public void StopTransitionEffect()
        {
            _transitionEffect.Stop();
        }

        private IEnumerator SetNewLocation(string loadPath)
        {
            _userInput.DisableInput();
            _isMovingToNewLocation = true;
            _player.StatesReusableData.IsFallingToNewLevel = true;
            StopMusic();
            PlayTransitionEffect();
            
            // Wait for player fall down a bit
            yield return new WaitForSeconds(0.75f);
            
            // Stop player fall
            _player.Rigidbody.gravityScale = 0f;
            _player.Rigidbody.linearVelocity = Vector2.zero;
            
            // Stop camera follow
            _camera.Follow = null;
            _positionComposer.enabled = false;
            
            // Loading next level from resources
            var loadAsync = Resources.LoadAsync(loadPath, typeof(Level));
            yield return loadAsync;

            float yDifferenceBetweenPlayerAndCamera = _player.Rigidbody.position.y - _camera.transform.position.y;
            
            // Set Player position above new level
            Vector2 newPlayerPosition = _gameConfig.PlayerRewindPosition;
            newPlayerPosition.x = _player.transform.position.x;
            _player.Rigidbody.position = newPlayerPosition;
            
            // As well as camera position
            Vector3 newCameraPosition = new Vector3(0f, newPlayerPosition.y - yDifferenceBetweenPlayerAndCamera, -10f);
            _camera.ForceCameraPosition(newCameraPosition, Quaternion.identity);
            
            // Cinemachine camera has its own updating algorythm
            // and this small wait needed for camera to change its position
            // otherwise changing its position dont affect at all
            yield return new WaitForSeconds(0.01f);
            
            // Following player again with camera moved up
            _camera.Follow = _player.FollowPlayer.transform;
            _positionComposer.enabled = true;
            
            // Just some wait
            yield return new WaitForSeconds(0.2f);
            
            // Destroying old level and instantiating new
            Destroy(_currentLevel.gameObject);
            _currentLevel = null;
            _currentLevel = Instantiate(loadAsync.asset as Level);
            SetNewLevel(_gameConfig.ColorLerpDuration);
            
            // wait for camera change its background color
            yield return new WaitForSeconds(_gameConfig.ColorLerpDuration / 2f);
            
            // Player now falling and effect stops
            _player.Rigidbody.gravityScale = _playerConfig.DefaultGravityScale;
            StopTransitionEffect();
            
            // some wait for before landing
            yield return new WaitForSeconds(0.7f);
            
            PlayMusic(_currentLevel.MusicClip);
            
            _userInput.EnableInput();
            _isMovingToNewLocation = false;
        }
        
        private IEnumerator GameOver()
        {
            _userInput.DisableInput();
            _camera.Follow = null;
            _gameCanvas.ShowGameOverTitle();
            
            yield return new WaitForSeconds(_gameConfig.GameOverTitleDuration);
            
            // Fade to black
            _gameCanvas.FadeInCurtain(_gameConfig.FadeDuration);
            
            yield return new WaitForSeconds(_gameConfig.FadeDuration);
            
            _gameCanvas.HideGameOverTitle();
            
            // Changing players position
            _player.Rigidbody.gravityScale = 0f;
            _player.Rigidbody.linearVelocity = Vector3.zero;
            _player.Rigidbody.position = _currentSavePoint;
            _player.Rigidbody.gravityScale = _playerConfig.DefaultGravityScale;
            // Resetting player state to Idle
            _player.RewindPlayerState();
            
            _camera.Follow = _player.FollowPlayer.transform;
            
            yield return new WaitForSeconds(_gameConfig.CameraReturnDuration);
            
            _gameCanvas.FadeOutCurtain(_gameConfig.FadeDuration);
            _userInput.EnableInput();
        }
        
        private void ExitToMainMenu()
        {
            LucidAudio.StopAll();
            _gameCanvas.PlayButtonClick();

            EnablePlayerInput();
            var loadingScreen = Instantiate(_loadingScreenPrefab);
            DontDestroyOnLoad(loadingScreen);
                    
            Scenes.LoadSceneAsync(MENU_SCENE_NAME).WithLoadingScreen(loadingScreen);
        }
    }
}
