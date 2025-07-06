using AnnulusGames.LucidTools.Audio;
using AnnulusGames.SceneSystem;
using LightDI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Descensus
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioClip _clickSound;
        [SerializeField] private AudioClip _menuTheme;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioMixerGroup _musicGroup;
        [SerializeField] private AudioMixerGroup _sfxGroup;
        [SerializeField, Min(0f)] private float _fadeOutDuration = 0.3f;
        [Header("UI References")]
        [SerializeField] private Transform _buttonsContainer;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _authorsButton;
        [SerializeField] private SettingsPanel _settingsPanel;
        [Header("Load Scene References")]
        [SerializeField] private LoadingScreen _loadingScreenPrefab;
        
        [Inject] private UserInput _userInput;
        
        private VolumeHandler _volumeHandler;
        private const string GAME_SCENE_NAME = "Game";
        private const string CREATORS_SCENE_NAME = "Creators";
        
        public VolumeHandler VolumeHandler => _volumeHandler;
        
        private void Awake()
        {
            // Its special container that holds some services that needed in all game scenes,
            // such as input manager, audio manager, settings manager, events manager etc.
            // This operation will try to initialize all fields that marked with Inject attribute if it has such service
            // It is not mine script, its an asset
            InjectionManager.InjectTo(this);
            
            // If game was paused in Game scene and player quit to main menu, it will UnPause game
            Time.timeScale = 1f;
            
            // If menu music is not playing, it will start it
            // Its needed for not restarting main menu music after quiting Creators scene
            if (LucidAudio.GetPlayersByID("MainMenuClip").Length < 1)
            {
                LucidAudio.PlayBGM(_menuTheme).SetAudioMixerGroup(_musicGroup).SetLoop().SetID("MainMenuClip");
            }

            _volumeHandler = new VolumeHandler(_audioMixer);
            _settingsPanel.Initialize(this, _userInput);
        }

        private void OnEnable()
        {
            _startButton.onClick.AddListener(OnStartPressed);
            _settingsButton.onClick.AddListener(OnSettingsPressed);
            _authorsButton.onClick.AddListener(OnCreatorsPressed);
        }
        
        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(OnStartPressed);
            _settingsButton.onClick.RemoveListener(OnSettingsPressed);
            _authorsButton.onClick.RemoveListener(OnCreatorsPressed);
        }

        public void ShowMenu()
        {
            SetButtonsActive(true);
            _buttonsContainer.gameObject.SetActive(true);
        }

        public void PlayButtonClick()
        {
            LucidAudio.PlaySE(_clickSound).SetAudioMixerGroup(_sfxGroup);
        }

        private void OnStartPressed()
        {
            PlayButtonClick();
            
            SetButtonsActive(false);
            
            var loadingScreen = Instantiate(_loadingScreenPrefab);
            DontDestroyOnLoad(loadingScreen);
                    
            Scenes.LoadSceneAsync(GAME_SCENE_NAME).WithLoadingScreen(loadingScreen);
            
            LucidAudio.StopAllBGM(_menuTheme, _fadeOutDuration);
        }
        
        private void OnSettingsPressed()
        {
            PlayButtonClick();
            
            SetButtonsActive(false);
            _buttonsContainer.gameObject.SetActive(false);
            _settingsPanel.Show();
        }

        private void OnCreatorsPressed()
        {
            PlayButtonClick();
            SetButtonsActive(false);
            
            Scenes.LoadScene(CREATORS_SCENE_NAME);
        }

        private void SetButtonsActive(bool active)
        {
            _startButton.interactable = active;
            _settingsButton.interactable = active;
            _authorsButton.interactable = active;
        }
    }
}
