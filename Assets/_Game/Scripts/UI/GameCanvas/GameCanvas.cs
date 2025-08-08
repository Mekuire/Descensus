using AnnulusGames.LucidTools.Audio;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Descensus
{
    public class GameCanvas : MonoBehaviour
    {
        [SerializeField] private AudioClip _clickSound;
        [SerializeField] private AudioMixerGroup _sfxGroup;
        [SerializeField] private Image _fadeCurtain;
        [SerializeField] private GameObject _gameOverTitle;
        [SerializeField] private PausePanel _pausePanel;
        [SerializeField] private Ease _fadeInEase;
        [SerializeField] private Ease _fadeOutEase;
        
        public PausePanel PausePanel => _pausePanel;
        
        private UserInput _userInput;
        private Tween _fadeTween;
        
        public void Initialize(UserInput userInput)
        {
            _userInput = userInput;
            _pausePanel.Initialize(_userInput);
            
            _gameOverTitle.SetActive(false);
            _fadeCurtain.gameObject.SetActive(false);
        }

        public void FadeInCurtain(float duration)
        {
            if (!_fadeCurtain) return;

            _fadeTween?.Kill();
            _fadeTween = _fadeCurtain.DOFade(1f, duration)
                .From(0f)
                .OnStart(() => _fadeCurtain.gameObject.SetActive(true))
                .SetEase(_fadeInEase)
                .SetLink(gameObject);
        }

        public void FadeOutCurtain(float duration)
        {
            if (!_fadeCurtain) return;

            _fadeTween?.Kill();
            _fadeTween = _fadeCurtain.DOFade(0f, duration)
                .From(1f)
                .SetEase(_fadeOutEase)
                .SetLink(gameObject)
                .OnComplete(() => _fadeCurtain.gameObject.SetActive(false));
        }

        public void ShowGameOverTitle()
        {
            _gameOverTitle.SetActive(true);
        }

        public void HideGameOverTitle()
        {
            _gameOverTitle.SetActive(false);
        }

        public void PlayButtonClick()
        {
            LucidAudio.PlaySE(_clickSound).SetAudioMixerGroup(_sfxGroup);
        }
    }
}