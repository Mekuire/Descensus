using System;
using UnityEngine;
using UnityEngine.UI;

namespace Descensus
{
    public class PausePanel : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _exitButton;

        private UserInput _userInput;
        
        public event Action OnPauseOpened;
        public event Action OnPauseClosed;
        public event Action OnExitPressed;
        
        public void Initialize(UserInput userInput)
        {
            _userInput = userInput;
            _userInput.PauseWasPressed += UserInputOnPauseWasPressed;
            
            Hide();
        }

        private void OnDestroy()
        {
            _userInput.PauseWasPressed -= UserInputOnPauseWasPressed;
        }

        private void OnEnable()
        {
            _continueButton.onClick.AddListener(OnClosePause);
            _exitButton.onClick.AddListener(OnExit);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private void UserInputOnPauseWasPressed()
        {
            if (gameObject.activeSelf)
            {
                OnClosePause();
            }
            else
            {
                OnPauseOpened?.Invoke();
                Show();
            }
        }

        private void OnClosePause()
        {
            OnPauseClosed?.Invoke();
            Hide();
        }

        private void OnExit()
        {
            SetButtonsActive(false);
            OnExitPressed?.Invoke();
        }

        private void SetButtonsActive(bool active)
        {
            _continueButton.interactable = active;
            _exitButton.interactable = active;
        }
        
        private void OnDisable()
        {
            _continueButton.onClick.RemoveListener(OnClosePause);
            _exitButton.onClick.RemoveListener(OnExit);
        }
    }
}
