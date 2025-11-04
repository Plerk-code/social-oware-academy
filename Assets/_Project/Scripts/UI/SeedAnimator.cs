using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;
using System;

namespace SocialOwareAcademy.UI
{
    /// <summary>
    /// Handles beautiful seed animations - hopping, sowing, and capturing.
    /// Uses object pooling for performance.
    /// </summary>
    public class SeedAnimator : MonoBehaviour
    {
        [Header("Seed Prefab")]
        [SerializeField] private GameObject seedPrefab;

        [Header("Animation Settings")]
        [SerializeField] private float hopHeight = 50f;
        [SerializeField] private float hopDuration = 0.15f;
        [SerializeField] private Ease hopEase = Ease.OutQuad;

        [Header("Capture Settings")]
        [SerializeField] private float captureDuration = 0.5f;
        [SerializeField] private float captureStagger = 0.05f;
        [SerializeField] private GameObject captureParticlePrefab;

        [Header("Audio")]
        [SerializeField] private bool playSound = true;

        // Object pool for seeds
        private ObjectPool<GameObject> seedPool;

        private void Awake()
        {
            // Initialize seed pool
            if (seedPrefab != null)
            {
                seedPool = new ObjectPool<GameObject>(
                    createFunc: () => Instantiate(seedPrefab, transform),
                    actionOnGet: (obj) => obj.SetActive(true),
                    actionOnRelease: (obj) => obj.SetActive(false),
                    actionOnDestroy: (obj) => Destroy(obj),
                    collectionCheck: false,
                    defaultCapacity: 48, // Max seeds in game
                    maxSize: 60
                );
            }
            else
            {
                Debug.LogWarning("[SeedAnimator] Seed prefab not assigned!");
            }
        }

        /// <summary>
        /// Animate sowing seeds from one pit to others
        /// </summary>
        /// <param name="pitPositions">Array of pit world positions</param>
        /// <param name="pitIndices">Indices of pits to sow into</param>
        /// <param name="onComplete">Callback when animation completes</param>
        public void AnimateSowing(Vector3[] pitPositions, int[] pitIndices, Action onComplete = null)
        {
            if (seedPool == null || pitPositions == null || pitIndices == null)
            {
                onComplete?.Invoke();
                return;
            }

            Sequence sowSequence = DOTween.Sequence();

            for (int i = 0; i < pitIndices.Length; i++)
            {
                int pitIndex = pitIndices[i];
                if (pitIndex < 0 || pitIndex >= pitPositions.Length)
                    continue;

                Vector3 targetPosition = pitPositions[pitIndex];

                // Get seed from pool
                GameObject seed = seedPool.Get();

                // Start position (from previous pit or starting pit)
                Vector3 startPosition = i == 0
                    ? pitPositions[pitIndices[0]]
                    : pitPositions[pitIndices[i - 1]];

                seed.transform.position = startPosition;

                // Create arc path for hop animation
                Vector3[] path = CreateHopPath(startPosition, targetPosition, hopHeight);

                // Animate along path
                sowSequence.Append(seed.transform.DOPath(path, hopDuration, PathType.CatmullRom)
                    .SetEase(hopEase)
                    .OnComplete(() =>
                    {
                        // Play landing sound
                        PlaySeedSound("seed_drop");

                        // Return seed to pool
                        seedPool.Release(seed);
                    }));
            }

            // Call completion callback
            sowSequence.OnComplete(() => onComplete?.Invoke());
        }

        /// <summary>
        /// Animate capturing seeds from pit to player store
        /// </summary>
        public void AnimateCapture(Vector3 fromPosition, Vector3 toPosition, int seedCount, Action onComplete = null)
        {
            if (seedPool == null)
            {
                onComplete?.Invoke();
                return;
            }

            Sequence captureSequence = DOTween.Sequence();

            for (int i = 0; i < seedCount; i++)
            {
                // Get seed from pool
                GameObject seed = seedPool.Get();
                seed.transform.position = fromPosition;

                // Stagger animation for visual appeal
                float delay = i * captureStagger;

                captureSequence.Insert(delay,
                    seed.transform.DOMove(toPosition, captureDuration)
                        .SetEase(Ease.InOutQuad)
                        .OnComplete(() =>
                        {
                            // Particle effect on last seed
                            if (i == seedCount - 1)
                            {
                                PlayCaptureEffect(toPosition);
                                PlaySeedSound("capture");
                            }

                            // Return to pool
                            seedPool.Release(seed);
                        }));
            }

            // Call completion callback
            captureSequence.OnComplete(() => onComplete?.Invoke());
        }

        /// <summary>
        /// Create arc path for seed hop animation
        /// </summary>
        private Vector3[] CreateHopPath(Vector3 start, Vector3 end, float height)
        {
            Vector3[] path = new Vector3[3];
            path[0] = start;
            path[1] = (start + end) / 2f + Vector3.up * height; // Arc peak
            path[2] = end;
            return path;
        }

        /// <summary>
        /// Play capture particle effect
        /// </summary>
        private void PlayCaptureEffect(Vector3 position)
        {
            if (captureParticlePrefab != null)
            {
                GameObject particle = Instantiate(captureParticlePrefab, position, Quaternion.identity);
                Destroy(particle, 2f); // Cleanup after 2 seconds
            }
        }

        /// <summary>
        /// Play seed-related sound effect
        /// </summary>
        private void PlaySeedSound(string soundName)
        {
            if (playSound && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(soundName);
            }
        }

        /// <summary>
        /// Clear all active animations and return seeds to pool
        /// </summary>
        public void ClearAllAnimations()
        {
            DOTween.KillAll();

            // Return all active seeds to pool
            foreach (Transform child in transform)
            {
                if (child.gameObject.activeSelf)
                {
                    seedPool.Release(child.gameObject);
                }
            }
        }

        private void OnDestroy()
        {
            ClearAllAnimations();
            seedPool?.Clear();
        }
    }
}
