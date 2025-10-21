using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace SocialOwareAcademy.Managers
{
    /// <summary>
    /// Centralized audio management for sound effects and background music.
    /// Singleton pattern, persists across scenes.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private int maxSimultaneousSFX = 10;

        [Header("Default Volumes")]
        [SerializeField] private float defaultMusicVolume = 0.5f;
        [SerializeField] private float defaultSFXVolume = 1.0f;

        // SFX object pool
        private Queue<AudioSource> sfxPool = new Queue<AudioSource>();
        private List<AudioSource> activeSFX = new List<AudioSource>();

        // Volume settings
        private float currentMusicVolume;
        private float currentSFXVolume;
        private bool isMusicMuted;
        private bool isSFXMuted;

        // PlayerPrefs keys
        private const string MUSIC_VOLUME_KEY = "Audio_MusicVolume";
        private const string SFX_VOLUME_KEY = "Audio_SFXVolume";
        private const string MUSIC_MUTED_KEY = "Audio_MusicMuted";
        private const string SFX_MUTED_KEY = "Audio_SFXMuted";

        void Awake()
        {
            // Singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize audio sources
            InitializeAudioSources();

            // Load saved settings
            LoadAudioSettings();

            Debug.Log("[AudioManager] Initialized");
        }

        private void InitializeAudioSources()
        {
            // Create music source if not assigned
            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.loop = true;
                musicSource.playOnAwake = false;
            }

            // Pre-create SFX AudioSource pool
            for (int i = 0; i < maxSimultaneousSFX; i++)
            {
                AudioSource sfx = gameObject.AddComponent<AudioSource>();
                sfx.playOnAwake = false;
                sfx.loop = false;
                sfxPool.Enqueue(sfx);
            }
        }

        private void LoadAudioSettings()
        {
            currentMusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, defaultMusicVolume);
            currentSFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, defaultSFXVolume);
            isMusicMuted = PlayerPrefs.GetInt(MUSIC_MUTED_KEY, 0) == 1;
            isSFXMuted = PlayerPrefs.GetInt(SFX_MUTED_KEY, 0) == 1;

            ApplyMusicVolume();
        }

        private void SaveAudioSettings()
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, currentMusicVolume);
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, currentSFXVolume);
            PlayerPrefs.SetInt(MUSIC_MUTED_KEY, isMusicMuted ? 1 : 0);
            PlayerPrefs.SetInt(SFX_MUTED_KEY, isSFXMuted ? 1 : 0);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Play a sound effect.
        /// </summary>
        public void PlaySFX(AudioClip clip, float volume = 1.0f)
        {
            if (clip == null || isSFXMuted)
                return;

            AudioSource source = GetAvailableSFXSource();
            if (source != null)
            {
                source.clip = clip;
                source.volume = volume * currentSFXVolume;
                source.Play();
            }
            else
            {
                Debug.LogWarning($"[AudioManager] Max simultaneous SFX reached ({maxSimultaneousSFX})");
            }
        }

        /// <summary>
        /// Play background music with optional loop.
        /// </summary>
        public void PlayMusic(AudioClip clip, bool loop = true, float volume = 0.5f)
        {
            if (clip == null)
                return;

            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = isMusicMuted ? 0f : volume * currentMusicVolume;
            musicSource.Play();
        }

        /// <summary>
        /// Stop background music with fade out.
        /// </summary>
        public void StopMusic(float fadeOutDuration = 0.5f)
        {
            if (musicSource.isPlaying)
            {
                StartCoroutine(FadeMusicOut(fadeOutDuration));
            }
        }

        private IEnumerator FadeMusicOut(float duration)
        {
            float startVolume = musicSource.volume;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
                yield return null;
            }

            musicSource.volume = 0f;
            musicSource.Stop();
            musicSource.volume = isMusicMuted ? 0f : currentMusicVolume; // Reset for next play
        }

        /// <summary>
        /// Set music volume (0.0 to 1.0).
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            currentMusicVolume = Mathf.Clamp01(volume);
            ApplyMusicVolume();
            SaveAudioSettings();
        }

        /// <summary>
        /// Set SFX volume (0.0 to 1.0).
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            currentSFXVolume = Mathf.Clamp01(volume);
            SaveAudioSettings();
        }

        /// <summary>
        /// Mute/unmute music.
        /// </summary>
        public void SetMusicMuted(bool muted)
        {
            isMusicMuted = muted;
            ApplyMusicVolume();
            SaveAudioSettings();
        }

        /// <summary>
        /// Mute/unmute sound effects.
        /// </summary>
        public void SetSFXMuted(bool muted)
        {
            isSFXMuted = muted;
            SaveAudioSettings();
        }

        private void ApplyMusicVolume()
        {
            musicSource.volume = isMusicMuted ? 0f : currentMusicVolume;
        }

        private AudioSource GetAvailableSFXSource()
        {
            // Recycle finished SFX sources
            for (int i = activeSFX.Count - 1; i >= 0; i--)
            {
                if (!activeSFX[i].isPlaying)
                {
                    sfxPool.Enqueue(activeSFX[i]);
                    activeSFX.RemoveAt(i);
                }
            }

            // Get source from pool
            if (sfxPool.Count > 0)
            {
                AudioSource source = sfxPool.Dequeue();
                activeSFX.Add(source);
                return source;
            }

            return null; // Pool exhausted
        }

        // Public getters for UI
        public float MusicVolume => currentMusicVolume;
        public float SFXVolume => currentSFXVolume;
        public bool IsMusicMuted => isMusicMuted;
        public bool IsSFXMuted => isSFXMuted;
    }
}
