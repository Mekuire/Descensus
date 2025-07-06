using UnityEngine;
using UnityEngine.UI;

namespace Descensus
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private Button _closeButton;
        
        private UserInput _userInput;
        private MainMenu _mainMenu;

        public void Initialize(MainMenu mainMenu, UserInput userInput)
        {
            _mainMenu = mainMenu;
            _userInput = userInput;
            _volumeSlider.value = _mainMenu.VolumeHandler.CurrentMainVolume;
            
            Hide();
        }

        private void OnEnable()
        {
            _volumeSlider.onValueChanged.AddListener(ChangeVolume);
            _closeButton.onClick.AddListener(Close);
            _userInput.PauseWasPressed += UserInputOnPauseWasPressed;
        }

        private void OnDisable()
        {
            _volumeSlider.onValueChanged.RemoveListener(ChangeVolume);
            _closeButton.onClick.RemoveListener(Close);
            _userInput.PauseWasPressed -= UserInputOnPauseWasPressed;
        }

        private void ChangeVolume(float volume)
        {
            _mainMenu.PlayButtonClick();
            _mainMenu.VolumeHandler.ChangeMainVolume(volume);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _volumeSlider.Select();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private void UserInputOnPauseWasPressed()
        {
            Close();
        }
        
        private void Close()
        {
            Hide();
            _mainMenu.PlayButtonClick();
            _mainMenu.ShowMenu();
        }
    }
}
