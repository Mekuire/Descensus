using AnnulusGames.LucidTools.Audio;
using UnityEngine;

namespace Descensus
{
    /// <summary>
    /// Class containing logic for playing player sounds
    /// </summary>
    public class PlayerAudioHandler
    {
        private const string FALL_SOUND_KEY = "Fall";
        private const string WALL_SLIDE_KEY = "WallSlide";
        
        private readonly PlayerSO Config;
        private readonly Transform _playerTransform;

        public PlayerAudioHandler(PlayerSO config, Transform playerTransform)
        {
            Config = config;
            _playerTransform = playerTransform;
        }
        
        public void PlayStepSound()
        {
            PlaySoundEffect(Config.StepSounds[Random.Range(0, Config.StepSounds.Length)], Config.SfxSpatialBlend);
        }

        public void PlayLandSound()
        {
            PlaySoundEffect(Config.LandSound, Config.SfxSpatialBlend);
        }

        public void PlayDashSound()
        {
            PlaySoundEffect(Config.DashSound, Config.SfxSpatialBlend);
        }

        public void PlayDeathSound()
        {
            int chance = Random.Range(0, 10);
            
            // this chance should be in config, and volume parameters too. Im just lazy some times
            if (chance < 2)
            {
                PlaySoundEffect(Config.DeathSound, 0f);
            }
            else
            {
                PlaySoundEffect(Config.GameOverSound, 0f);
            }
        }

        public void StartFallSound()
        {
            StartSoundEffect(Config.FallSound, 0f, FALL_SOUND_KEY, 0.3f);
        }

        public void StopFallSound()
        {
            StopSoundEffect(FALL_SOUND_KEY, 0.2f);
        }

        public void StartSlideSound()
        {
            StartSoundEffect(Config.SlideSound, Config.SfxSpatialBlend, WALL_SLIDE_KEY, 0.5f);
        }

        public void StopSlideSound()
        {
            StopSoundEffect(WALL_SLIDE_KEY, 0.1f);
        }

        private void PlaySoundEffect(AudioClip clip, float blend)
        {
            LucidAudio.PlaySE(clip)
                .SetPosition(_playerTransform.position)
                .SetAudioMixerGroup(Config.SfxGroup)
                .SetSpatialBlend(blend);
        }

        private void StartSoundEffect(AudioClip clip, float blend, string id, float volume = 1.0f)
        {
            LucidAudio.PlaySE(clip)
                .SetID(id)
                .SetPosition(_playerTransform.position)
                .SetAudioMixerGroup(Config.SfxGroup)
                .SetSpatialBlend(blend)
                .SetVolume(volume)
                .SetLoop();
        }

        private void StopSoundEffect(string id, float duration = 0f)
        {
            LucidAudio.StopAllSE(id, duration);
        }
    }
}
