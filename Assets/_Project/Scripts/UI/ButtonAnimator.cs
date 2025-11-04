using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using SocialOwareAcademy.Managers;

namespace SocialOwareAcademy.UI
{
    /// <summary>
    /// Adds smooth press/release animations to any UI button.
    /// Attach to any button for instant polish.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class ButtonAnimator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [Header("Animation Settings")]
        [SerializeField] private float pressScale = 0.95f;
        [SerializeField] private float pressDuration = 0.1f;
        [SerializeField] private Ease pressEase = Ease.OutQuad;
        [SerializeField] private Ease releaseEase = Ease.OutBack;

        [Header("Audio")]
        [SerializeField] private bool playSound = true;
        [SerializeField] private AudioClip buttonPressSound;

        [Header("Haptics")]
        [SerializeField] private bool useHaptics = true;

        private Vector3 originalScale;
        private Sequence animSequence;
        private RectTransform rectTransform;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            originalScale = rectTransform.localScale;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Kill any existing animation
            animSequence?.Kill();

            // Scale down animation
            animSequence = DOTween.Sequence()
                .Append(rectTransform.DOScale(originalScale * pressScale, pressDuration)
                    .SetEase(pressEase));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Kill any existing animation
            animSequence?.Kill();

            // Scale back up with bounce
            animSequence = DOTween.Sequence()
                .Append(rectTransform.DOScale(originalScale, pressDuration)
                    .SetEase(releaseEase));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // Play sound
            if (playSound && buttonPressSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(buttonPressSound);
            }

            // Haptic feedback
            if (useHaptics)
            {
                TriggerHaptic();
            }
        }

        /// <summary>
        /// Trigger haptic feedback (mobile only)
        /// </summary>
        private void TriggerHaptic()
        {
#if UNITY_IOS || UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }

        /// <summary>
        /// Public method to trigger press animation programmatically
        /// </summary>
        public void AnimatePress()
        {
            animSequence?.Kill();
            animSequence = DOTween.Sequence()
                .Append(rectTransform.DOScale(originalScale * pressScale, pressDuration).SetEase(pressEase))
                .Append(rectTransform.DOScale(originalScale, pressDuration).SetEase(releaseEase));

            if (playSound && buttonPressSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(buttonPressSound);
            }

            if (useHaptics)
            {
                TriggerHaptic();
            }
        }

        void OnDestroy()
        {
            animSequence?.Kill();
        }
    }
}
