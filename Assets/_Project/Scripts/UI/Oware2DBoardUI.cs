using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using SocialOwareAcademy.Gameplay;

namespace SocialOwareAcademy.UI
{
    /// <summary>
    /// 2D Canvas-based Oware board UI - CrazyGames Mancala style
    /// Beautiful illustrated board with clear pits, colorful marbles, and smooth animations
    /// </summary>
    public class Oware2DBoardUI : MonoBehaviour
    {
        [Header("Board Background")]
        [SerializeField] private Image boardBackgroundImage;
        [SerializeField] private Sprite boardBackgroundSprite; // Wooden board texture
        [SerializeField] private Color boardTintColor = new Color(0.82f, 0.70f, 0.55f); // Light brown

        [Header("Pit Prefab")]
        [SerializeField] private GameObject pitUIPrefab; // Prefab for each pit
        [SerializeField] private Transform pitsContainer; // Parent for all pits

        [Header("Seed/Marble Sprites")]
        [SerializeField] private Sprite[] marbleSprites; // 3 colored marble sprites
        [SerializeField] private Sprite defaultMarbleSprite; // Fallback if no sprites assigned
        [SerializeField] private float marbleSize = 40f; // Size of marble sprites in pixels

        [Header("Layout Settings")]
        [SerializeField] private Vector2 pitSize = new Vector2(120f, 120f);
        [SerializeField] private float pitSpacing = 140f;
        [SerializeField] private Vector2 player1PitsStartPos = new Vector2(200f, -150f); // Bottom row
        [SerializeField] private Vector2 player2PitsStartPos = new Vector2(200f, 150f); // Top row
        [SerializeField] private Vector2 storeSize = new Vector2(120f, 300f);

        [Header("Colors")]
        [SerializeField] private Color[] marbleColors = new Color[]
        {
            new Color(0.89f, 0.45f, 0.35f), // Red/Terracotta
            new Color(0.95f, 0.88f, 0.77f), // Beige/Tan
            new Color(0.26f, 0.15f, 0.09f)  // Dark Brown
        };

        [Header("Animation Settings")]
        [SerializeField] private float seedDropDuration = 0.3f;
        [SerializeField] private float seedMoveDuration = 0.2f;

        [Header("References")]
        [SerializeField] private Canvas mainCanvas;

        // Runtime data
        private PitUI[] pitUIs;
        private PitUI[] storeUIs;
        private bool isInitialized = false;

        void Start()
        {
            InitializeBoard();
            SubscribeToEvents();
        }

        /// <summary>
        /// Initialize the 2D board UI
        /// </summary>
        private void InitializeBoard()
        {
            Debug.Log("[2D Board UI] Initializing board...");

            // Ensure we have a canvas
            if (mainCanvas == null)
            {
                mainCanvas = GetComponentInParent<Canvas>();
                if (mainCanvas == null)
                {
                    Debug.LogError("[2D Board UI] No Canvas found! Please add this to a Canvas.");
                    return;
                }
            }

            // Setup board background
            if (boardBackgroundImage != null)
            {
                if (boardBackgroundSprite != null)
                {
                    boardBackgroundImage.sprite = boardBackgroundSprite;
                }
                boardBackgroundImage.color = boardTintColor;
            }

            // Create pits container if needed
            if (pitsContainer == null)
            {
                GameObject containerObj = new GameObject("Pits Container");
                containerObj.transform.SetParent(transform);
                pitsContainer = containerObj.GetComponent<RectTransform>();
                if (pitsContainer == null)
                {
                    pitsContainer = containerObj.AddComponent<RectTransform>();
                }
                ((RectTransform)pitsContainer).anchoredPosition = Vector2.zero;
            }

            // Create pit UIs
            CreatePitUIs();

            // Create store UIs
            CreateStoreUIs();

            isInitialized = true;
            Debug.Log("[2D Board UI] Board initialized successfully!");
        }

        /// <summary>
        /// Create UI for all 12 pits
        /// </summary>
        private void CreatePitUIs()
        {
            pitUIs = new PitUI[OwareBoard.TOTAL_PITS];

            // Create Player 1 pits (bottom row, left to right: 0-5)
            for (int i = 0; i < OwareBoard.PITS_PER_PLAYER; i++)
            {
                Vector2 position = player1PitsStartPos + new Vector2(i * pitSpacing, 0);
                pitUIs[i] = CreatePitUI(i, position, true);
            }

            // Create Player 2 pits (top row, right to left: 11-6)
            for (int i = 0; i < OwareBoard.PITS_PER_PLAYER; i++)
            {
                int pitIndex = OwareBoard.TOTAL_PITS - 1 - i;
                Vector2 position = player2PitsStartPos + new Vector2(i * pitSpacing, 0);
                pitUIs[pitIndex] = CreatePitUI(pitIndex, position, false);
            }
        }

        /// <summary>
        /// Create a single pit UI
        /// </summary>
        private PitUI CreatePitUI(int pitIndex, Vector2 position, bool isPlayer1)
        {
            GameObject pitObj;

            if (pitUIPrefab != null)
            {
                // Use prefab if assigned
                pitObj = Instantiate(pitUIPrefab, pitsContainer);
            }
            else
            {
                // Create basic pit UI if no prefab
                pitObj = CreateBasicPitUI();
                pitObj.transform.SetParent(pitsContainer);
            }

            pitObj.name = $"Pit_{pitIndex}_{(isPlayer1 ? "P1" : "P2")}";

            // Position
            RectTransform rectTransform = pitObj.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = position;
            rectTransform.sizeDelta = pitSize;

            // Setup PitUI component
            PitUI pitUI = pitObj.GetComponent<PitUI>();
            if (pitUI == null)
            {
                pitUI = pitObj.AddComponent<PitUI>();
            }

            pitUI.Initialize(pitIndex, isPlayer1, marbleSprites, marbleColors, marbleSize);

            // Add click handler for player 1 pits
            if (isPlayer1)
            {
                Button button = pitObj.GetComponent<Button>();
                if (button == null)
                {
                    button = pitObj.AddComponent<Button>();
                }
                int capturedIndex = pitIndex; // Capture for closure
                button.onClick.AddListener(() => OnPitClicked(capturedIndex));
            }

            return pitUI;
        }

        /// <summary>
        /// Create basic pit UI if no prefab assigned
        /// </summary>
        private GameObject CreateBasicPitUI()
        {
            GameObject pitObj = new GameObject("Pit");

            // Add RectTransform
            RectTransform rectTransform = pitObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = pitSize;

            // Add background image (pit hole)
            Image bgImage = pitObj.AddComponent<Image>();
            bgImage.color = new Color(0.25f, 0.18f, 0.12f, 0.9f); // Dark brown pit

            // Add counter text background
            GameObject counterBG = new GameObject("Counter BG");
            counterBG.transform.SetParent(pitObj.transform);
            RectTransform counterBGRect = counterBG.AddComponent<RectTransform>();
            counterBGRect.anchoredPosition = new Vector2(0, -70);
            counterBGRect.sizeDelta = new Vector2(60, 60);

            Image counterBGImage = counterBG.AddComponent<Image>();
            counterBGImage.color = new Color(0.15f, 0.1f, 0.07f, 0.8f);

            // Make it circular
            counterBG.AddComponent<Mask>();
            counterBGImage.sprite = CreateCircleSprite();

            // Add counter text
            GameObject counterText = new GameObject("Counter Text");
            counterText.transform.SetParent(counterBG.transform);
            RectTransform counterTextRect = counterText.AddComponent<RectTransform>();
            counterTextRect.anchorMin = Vector2.zero;
            counterTextRect.anchorMax = Vector2.one;
            counterTextRect.offsetMin = Vector2.zero;
            counterTextRect.offsetMax = Vector2.zero;

            TextMeshProUGUI text = counterText.AddComponent<TextMeshProUGUI>();
            text.text = "0";
            text.fontSize = 28;
            text.fontStyle = FontStyles.Bold;
            text.alignment = TextAlignmentOptions.Center;
            text.color = Color.white;

            // Add seeds container
            GameObject seedsContainer = new GameObject("Seeds Container");
            seedsContainer.transform.SetParent(pitObj.transform);
            RectTransform seedsRect = seedsContainer.AddComponent<RectTransform>();
            seedsRect.anchorMin = Vector2.zero;
            seedsRect.anchorMax = Vector2.one;
            seedsRect.offsetMin = Vector2.zero;
            seedsRect.offsetMax = Vector2.zero;

            return pitObj;
        }

        /// <summary>
        /// Create store UIs (captured seeds areas)
        /// </summary>
        private void CreateStoreUIs()
        {
            storeUIs = new PitUI[2];

            // Player 1 store (right side)
            Vector2 store1Pos = new Vector2(-100f, 0);
            storeUIs[0] = CreateStoreUI(0, store1Pos);

            // Player 2 store (left side)
            Vector2 store2Pos = new Vector2(player1PitsStartPos.x + (pitSpacing * 6), 0);
            storeUIs[1] = CreateStoreUI(1, store2Pos);
        }

        /// <summary>
        /// Create a store UI
        /// </summary>
        private PitUI CreateStoreUI(int storeIndex, Vector2 position)
        {
            GameObject storeObj = CreateBasicPitUI();
            storeObj.name = $"Store_{storeIndex}";
            storeObj.transform.SetParent(pitsContainer);

            RectTransform rectTransform = storeObj.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = position;
            rectTransform.sizeDelta = storeSize;

            PitUI storeUI = storeObj.GetComponent<PitUI>();
            if (storeUI == null)
            {
                storeUI = storeObj.AddComponent<PitUI>();
            }

            storeUI.Initialize(-1, storeIndex == 0, marbleSprites, marbleColors, marbleSize);

            return storeUI;
        }

        /// <summary>
        /// Create a simple circle sprite
        /// </summary>
        private Sprite CreateCircleSprite()
        {
            // Create a simple white circle texture
            int size = 64;
            Texture2D texture = new Texture2D(size, size);
            Color[] pixels = new Color[size * size];

            Vector2 center = new Vector2(size / 2f, size / 2f);
            float radius = size / 2f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), center);
                    pixels[y * size + x] = distance <= radius ? Color.white : Color.clear;
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();

            return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        }

        /// <summary>
        /// Subscribe to game events
        /// </summary>
        private void SubscribeToEvents()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStarted += OnGameStarted;
                GameManager.Instance.OnMoveMade += OnMoveMade;
                GameManager.Instance.OnGameEnded += OnGameEnded;
                Debug.Log("[2D Board UI] Subscribed to GameManager events");
            }
        }

        /// <summary>
        /// Update all pit displays based on board state
        /// </summary>
        private void UpdateBoardDisplay()
        {
            if (!isInitialized || GameManager.Instance?.Board == null)
                return;

            OwareBoard board = GameManager.Instance.Board;

            // Update pits
            for (int i = 0; i < OwareBoard.TOTAL_PITS; i++)
            {
                int seedCount = board.GetSeeds(i);
                pitUIs[i].UpdateSeedCount(seedCount);
            }

            // Update stores
            if (storeUIs[0] != null)
                storeUIs[0].UpdateSeedCount(board.Player1Captured);

            if (storeUIs[1] != null)
                storeUIs[1].UpdateSeedCount(board.Player2Captured);
        }

        /// <summary>
        /// Handle pit click
        /// </summary>
        private void OnPitClicked(int pitIndex)
        {
            if (GameManager.Instance == null || !GameManager.Instance.IsGameActive)
                return;

            if (!GameManager.Instance.IsHumanTurn)
            {
                Debug.Log("[2D Board UI] Not your turn!");
                return;
            }

            // Attempt move
            bool success = GameManager.Instance.MakeMove(pitIndex);

            if (success)
            {
                Debug.Log($"[2D Board UI] Move successful: Pit {pitIndex}");
                // Animate the pit
                pitUIs[pitIndex].AnimatePress();
            }
            else
            {
                Debug.LogWarning($"[2D Board UI] Invalid move: Pit {pitIndex}");
                // Shake to indicate invalid
                pitUIs[pitIndex].AnimateShake();
            }
        }

        // Event handlers
        private void OnGameStarted(OwareBoard board)
        {
            Debug.Log("[2D Board UI] Game started");
            UpdateBoardDisplay();
        }

        private void OnMoveMade(int pitIndex, int player)
        {
            Debug.Log($"[2D Board UI] Move made: Pit {pitIndex} by Player {player + 1}");
            UpdateBoardDisplay();
        }

        private void OnGameEnded(int winner)
        {
            Debug.Log($"[2D Board UI] Game ended. Winner: {winner}");
            UpdateBoardDisplay();
        }

        void OnDestroy()
        {
            // Unsubscribe from events
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStarted -= OnGameStarted;
                GameManager.Instance.OnMoveMade -= OnMoveMade;
                GameManager.Instance.OnGameEnded -= OnGameEnded;
            }
        }
    }
}

