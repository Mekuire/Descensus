using UnityEngine;
using UnityEngine.Audio;

namespace Descensus
{
    public class VolumeHandler
    {
        public float CurrentMainVolume { get; private set; } = 10;
        public float CurrentMusicVolume { get; private set; } = 10;
        public float CurrentSFXVolume { get; private set; } = 10;
        public float CurrentAmbientVolume { get; private set; } = 10;
        
        private const string MAIN_VOLUME_KEY = "MainVolume";
        private const string MUSIC_VOLUME_KEY = "MusicVolume";
        private const string SFX_VOLUME_KEY = "SoundFXVolume";
        private const string AMBIENT_VOLUME_KEY = "AmbientVolume";
        
        private readonly AudioMixer _audioMixer;

        public VolumeHandler(AudioMixer audioMixer)
        {
            _audioMixer = audioMixer;
        }

        public void ChangeMusicVolume(float value)
        {
            CurrentMusicVolume = value;
            ChangeVolume(MUSIC_VOLUME_KEY, value);
        }

        public void ChangeSFXVolume(float value)
        {
            CurrentSFXVolume = value;
            ChangeVolume(SFX_VOLUME_KEY, value);
        }
        
        public void ChangeAmbientVolume(float value)
        {
            CurrentAmbientVolume = value;
            ChangeVolume(AMBIENT_VOLUME_KEY, value);
        }

        public void ChangeMainVolume(float value)
        {
            CurrentMainVolume = value;
            ChangeVolume(MAIN_VOLUME_KEY, value);
        }
        
        private void ChangeVolume(string key, float value)
        {
            // setting very small volume for preventing 0 volume
            value = value == 0 ? 0.0001f : value;
            
             /*
             second parameter is the logarithmic formula for setting mixer volume 
             (Its needed because mixer volume value is in range of -80 to 20 and InGame volume slider value is in range 0 to 10)
             Additionaly I added -20f because mixer value of 20f causes sound boost, so it should be 0 as max value
             */
            _audioMixer.SetFloat(key, (Mathf.Log10(value) * 20f) - 20f);
        }
    }
}