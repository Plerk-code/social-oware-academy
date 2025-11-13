using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

namespace SocialOwareAcademy.UI
{
    /// <summary>
    /// Individual pit UI component - handles displaying seeds/marbles and animations
    /// </summary>
    public class PitUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI counterText;
        [SerializeField] private Transform seedsContainer;
        [SerializeField] private Image backgroundImage;

        // Runtime data
        private int pitIndex;
        private bool isPlayer1;
        private Sprite[] marbleSprites;
        private Color[] marbleColors;
        private float marbleSize;
        private List<GameObject> seedObjects = new List<GameObject>();
        private int currentSeedCount = 0;

        /// <summary>
        /// Initialize this pit UI
        /// </summary>
        public void Initialize(int index, bool player1, Sprite[] sprites, Color[] colors, float size)
        {
            pitIndex = index;
            isPlayer1 = player1;
            marbleSprites = sprites;
            marbleColors = colors;
            marbleSize = size;

            // Find or create references
            if (counterText == null)
            {
                counterText = GetComponentInChildren<TextMeshProUGUI>();
            }

            if (seedsContainer == null)
            {
                Transform found = transform.Find("Seeds Container");
                if (found != null)
                {
                    seedsContainer = found;
                }
                else
                {
                    GameObject container = new GameObject("Seeds Container");
                    container.transform.SetParent(transform);
                    RectTransform rect = container.AddComponent<RectTransform>();
                    rect.anchorMin = Vector2.zero;
                    rect.anchorMax = Vector2.one;
                    rect.offsetMin = Vector2.zero;
                    rect.offsetMax = Vector2.zero;
                    seedsContainer = container.transform;
                }
            }

            if (backgroundImage == null)
            {
                backgroundImage = GetComponent<Image>();
            }
        }

        /// <summary>
        /// Update the seed count display
        /// </summary>
        public void UpdateSeedCount(int newCount)
        {
            if (newCount == currentSeedCount)
                return;

            int oldCount = currentSeedCount;
            currentSeedCount = newCount;

            // Update counter text
            if (counterText != null)
            {
                counterText.text = newCount.ToString();
                
                // Animate counter
                counterText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 1f);
            }

            // Update seed visuals
            UpdateSeedVisuals(oldCount, newCount);
        }

        /// <summary>
        /// Update the visual seed/marble display
        /// </summary>
        private void UpdateSeedVisuals(int oldCount, int newCount)
        {
            if (seedsContainer == null)
                return;

            // Add seeds if count increased
            if (newCount > oldCount)
            {
                for (int i = oldCount; i < newCount; i++)
                {
                    CreateSeed(i);
                }
            }
            // Remove seeds if count decreased
            else if (newCount < oldCount)
            {
                for (int i = oldCount - 1; i >= newCount; i--)
                {
                    if (i < seedObjects.Count)
                    {
                        RemoveSeed(i);
                    }
                }
            }

            // Rearrange all seeds
            ArrangeSeeds();
        }

        /// <summary>
        /// Create a seed/marble UI element
        /// </summary>
        private void CreateSeed(int index)
        {
            GameObject seedObj = new GameObject($"Seed_{index}");
            seedObj.transform.SetParent(seedsContainer);

            // Add image
            Image seedImage = seedObj.AddComponent<Image>();
            
            // Use sprite if available, otherwise use colored circle
            if (marbleSprites != null && marbleSprites.Length > 0)
            {
                int spriteIndex = Random.Range(0, marbleSprites.Length);
                if (marbleSprites[spriteIndex] != null)
                {
                    seedImage.sprite = marbleSprites[spriteIndex];
                }
            }
            else if (marbleColors != null && marbleColors.Length > 0)
            {
                // Use random color
                int colorIndex = Random.Range(0, marbleColors.Length);
                seedImage.color = marbleColors[colorIndex];
            }
            else
            {
                // Default color
                seedImage.color = new Color(0.89f, 0.45f, 0.35f); // Red
            }

            // Set size
            RectTransform rectTransform = seedObj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(marbleSize, marbleSize);

            // Animate in
            seedObj.transform.localScale = Vector3.zero;
            seedObj.transform.DOScale(1f, 0.3f)
                .SetEase(Ease.OutBack);

            seedObjects.Add(seedObj);
        }

        /// <summary>
        /// Remove a seed with animation
        /// </summary>
        private void RemoveSeed(int index)
        {
            if (index >= seedObjects.Count)
                return;

            GameObject seedObj = seedObjects[index];
            seedObjects.RemoveAt(index);

            if (seedObj != null)
            {
                seedObj.transform.DOScale(0f, 0.2f)
                    .SetEase(Ease.InBack)
                    .OnComplete(() => Destroy(seedObj));
            }
        }

        /// <summary>
        /// Arrange seeds in a natural cluster pattern
        /// </summary>
        private void ArrangeSeeds()
        {
            if (seedObjects.Count == 0)
                return;

            RectTransform pitRect = GetComponent<RectTransform>();
            float pitWidth = pitRect.sizeDelta.x;
            float pitHeight = pitRect.sizeDelta.y;
            float arrangeRadius = Mathf.Min(pitWidth, pitHeight) * 0.3f;

            for (int i = 0; i < seedObjects.Count; i++)
            {
                if (seedObjects[i] == null)
                    continue;

                RectTransform seedRect = seedObjects[i].GetComponent<RectTransform>();

                // Arrange in circular pattern with randomness
                float angle = (360f / Mathf.Max(seedObjects.Count, 1)) * i + Random.Range(-15f, 15f);
                float radius = arrangeRadius * Random.Range(0.3f, 0.9f);

                Vector2 position = new Vector2(
                    Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                    Mathf.Sin(angle * Mathf.Deg2Rad) * radius
                );

                // Animate to position
                seedRect.DOAnchorPos(position, 0.3f)
                    .SetEase(Ease.OutQuad);

                // Slight rotation for variation
                float rotation = Random.Range(-15f, 15f);
                seedRect.DORotate(new Vector3(0, 0, rotation), 0.3f);
            }
        }

        /// <summary>
        /// Animate pit press (when clicked)
        /// </summary>
        public void AnimatePress()
        {
            transform.DOPunchScale(Vector3.one * -0.1f, 0.2f, 5, 0.5f);
        }

        /// <summary>
        /// Animate shake (for invalid move)
        /// </summary>
        public void AnimateShake()
        {
            RectTransform rect = GetComponent<RectTransform>();
            Vector2 originalPos = rect.anchoredPosition;

            rect.DOShakeAnchorPos(0.3f, 10f, 20, 90f)
                .OnComplete(() => rect.anchoredPosition = originalPos);
        }

        /// <summary>
        /// Highlight this pit (for selection/hover)
        /// </summary>
        public void SetHighlighted(bool highlighted)
        {
            if (backgroundImage == null)
                return;

            Color targetColor = highlighted 
                ? new Color(1f, 0.84f, 0f, 0.5f) // Gold highlight
                : new Color(0.25f, 0.18f, 0.12f, 0.9f); // Normal dark brown

            backgroundImage.DOColor(targetColor, 0.2f);
        }

        void OnDestroy()
        {
            // Clean up tweens
            transform.DOKill();
            if (backgroundImage != null)
                backgroundImage.DOKill();
        }
    }
}

