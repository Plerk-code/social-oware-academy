using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using SocialOwareAcademy.Gameplay;
using SocialOwareAcademy.Managers;

namespace SocialOwareAcademy.UI
{
    /// <summary>
    /// Optional UI overlay for Enhanced Board - adds score panels, player info, etc.
    /// Can be used alongside the existing MatchScreenUI or standalone.
    /// </summary>
    public class EnhancedBoardUIOverlay : MonoBehaviour
    {
        [Header("Background")]
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Color backgroundColor = new Color(0.35f, 0.25f, 0.2f, 1f); // Dark reddish-brown

        [Header("Player Info Panels")]
        [SerializeField] private GameObject player1Panel;
        [SerializeField] private GameObject player2Panel;
        [SerializeField] private TextMeshProUGUI player1NameText;
        [SerializeField] private TextMeshProUGUI player2NameText;
        [SerializeField] private TextMeshProUGUI player1ScoreText;
        [SerializeField] private TextMeshProUGUI player2ScoreText;

        [Header("Game Info")]
        [SerializeField] private TextMeshProUGUI turnIndicatorText;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI gameOverText;

        [Header("Buttons")]
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button settingsButton;

        [Header("Colors")]
        [SerializeField] private Color player1Color = new Color(0.3f, 0.8f, 0.4f); // Green
        [SerializeField] private Color player2Color = new Color(0.89f, 0.45f, 0.35f); // Red/Terracotta
        [SerializeField] private Color textColor = Color.white;

        [Header("Settings")]
        [SerializeField] private bool autoCreate = true; // Auto-create UI if not assigned
        [SerializeField] private bool minimalistMode = false; // Show only essential info

        private Canvas canvas;
        private int lastPlayer1Score = 0;
        private int lastPlayer2Score = 0;

        void Start()
        {
            // Create UI if needed
            if (autoCreate && backgroundImage == null)
            {
                CreateUIElements();
            }

            // Setup
            InitializeUI();
            SubscribeToEvents();
        }

        /// <summary>
        /// Auto-create UI elements if not manually assigned
        /// </summary>
        private void CreateUIElements()
        {
            Debug.Log("[BoardUIOverlay] Auto-creating UI elements...");

            // Get or create canvas
            canvas = GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = gameObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 10; // Above other UI

                var scaler = gameObject.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);

                gameObject.AddComponent<GraphicRaycaster>();
            }

            // Create background
            GameObject bgObj = new GameObject("Background");
            bgObj.transform.SetParent(transform);
            backgroundImage = bgObj.AddComponent<Image>();
            backgroundImage.color = backgroundColor;
            
            RectTransform bgRect = backgroundImage.rectTransform;
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;

            // Create player panels (simplified for auto-create)
            CreatePlayerInfoPanel(true);  // Player 1
            CreatePlayerInfoPanel(false); // Player 2

            // Create turn indicator
            CreateTurnIndicator();

            // Create buttons
            CreateButtons();

            Debug.Log("[BoardUIOverlay] UI auto-creation complete!");
        }

        /// <summary>
        /// Create player info panel
        /// </summary>
        private void CreatePlayerInfoPanel(bool isPlayer1)
        {
            // Create panel
            GameObject panelObj = new GameObject(isPlayer1 ? "Player1Panel" : "Player2Panel");
            panelObj.transform.SetParent(transform);

            Image panelImage = panelObj.AddComponent<Image>();
            panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.7f);

            RectTransform panelRect = panelImage.rectTransform;
            panelRect.sizeDelta = new Vector2(300, 150);

            // Position (left or right side)
            if (isPlayer1)
            {
                panelRect.anchorMin = new Vector2(0, 0.5f);
                panelRect.anchorMax = new Vector2(0, 0.5f);
                panelRect.anchoredPosition = new Vector2(170, -300);
                player1Panel = panelObj;
            }
            else
            {
                panelRect.anchorMin = new Vector2(1, 0.5f);
                panelRect.anchorMax = new Vector2(1, 0.5f);
                panelRect.anchoredPosition = new Vector2(-170, -300);
                player2Panel = panelObj;
            }

            // Create name text
            GameObject nameObj = new GameObject("NameText");
            nameObj.transform.SetParent(panelObj.transform);
            TextMeshProUGUI nameText = nameObj.AddComponent<TextMeshProUGUI>();
            nameText.text = isPlayer1 ? "YOU" : "AI";
            nameText.fontSize = 24;
            nameText.alignment = TextAlignmentOptions.Center;
            nameText.color = textColor;

            RectTransform nameRect = nameText.rectTransform;
            nameRect.anchorMin = new Vector2(0, 0.6f);
            nameRect.anchorMax = new Vector2(1, 1);
            nameRect.offsetMin = Vector2.zero;
            nameRect.offsetMax = Vector2.zero;

            if (isPlayer1)
                player1NameText = nameText;
            else
                player2NameText = nameText;

            // Create score text
            GameObject scoreObj = new GameObject("ScoreText");
            scoreObj.transform.SetParent(panelObj.transform);
            TextMeshProUGUI scoreText = scoreObj.AddComponent<TextMeshProUGUI>();
            scoreText.text = "0";
            scoreText.fontSize = 48;
            scoreText.fontStyle = FontStyles.Bold;
            scoreText.alignment = TextAlignmentOptions.Center;
            scoreText.color = isPlayer1 ? player1Color : player2Color;

            RectTransform scoreRect = scoreText.rectTransform;
            scoreRect.anchorMin = new Vector2(0, 0);
            scoreRect.anchorMax = new Vector2(1, 0.6f);
            scoreRect.offsetMin = Vector2.zero;
            scoreRect.offsetMax = Vector2.zero;

            if (isPlayer1)
                player1ScoreText = scoreText;
            else
                player2ScoreText = scoreText;
        }

        /// <summary>
        /// Create turn indicator
        /// </summary>
        private void CreateTurnIndicator()
        {
            GameObject indicatorObj = new GameObject("TurnIndicator");
            indicatorObj.transform.SetParent(transform);

            turnIndicatorText = indicatorObj.AddComponent<TextMeshProUGUI>();
            turnIndicatorText.text = "YOUR TURN";
            turnIndicatorText.fontSize = 36;
            turnIndicatorText.fontStyle = FontStyles.Bold;
            turnIndicatorText.alignment = TextAlignmentOptions.Center;
            turnIndicatorText.color = new Color(1f, 0.84f, 0f); // Gold

            RectTransform indicatorRect = turnIndicatorText.rectTransform;
            indicatorRect.anchorMin = new Vector2(0.5f, 1f);
            indicatorRect.anchorMax = new Vector2(0.5f, 1f);
            indicatorRect.sizeDelta = new Vector2(400, 80);
            indicatorRect.anchoredPosition = new Vector2(0, -60);

            // Add background
            GameObject bgObj = new GameObject("Background");
            bgObj.transform.SetParent(indicatorObj.transform);
            bgObj.transform.SetAsFirstSibling();

            Image bgImage = bgObj.AddComponent<Image>();
            bgImage.color = new Color(0, 0, 0, 0.8f);

            RectTransform bgRect = bgImage.rectTransform;
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = new Vector2(-20, -10);
            bgRect.offsetMax = new Vector2(20, 10);
        }

        /// <summary>
        /// Create control buttons
        /// </summary>
        private void CreateButtons()
        {
            // New Game button
            newGameButton = CreateButton("New Game", new Vector2(-120, 40), OnNewGameClicked);
            
            // Menu button  
            menuButton = CreateButton("Menu", new Vector2(0, 40), OnMenuClicked);
            
            // Settings button
            settingsButton = CreateButton("Settings", new Vector2(120, 40), OnSettingsClicked);
        }

        /// <summary>
        /// Create a button
        /// </summary>
        private Button CreateButton(string label, Vector2 position, UnityEngine.Events.UnityAction onClick)
        {
            GameObject buttonObj = new GameObject(label + "Button");
            buttonObj.transform.SetParent(transform);

            Image buttonImage = buttonObj.AddComponent<Image>();
            buttonImage.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);

            Button button = buttonObj.AddComponent<Button>();
            button.targetGraphic = buttonImage;
            button.onClick.AddListener(onClick);

            RectTransform buttonRect = buttonImage.rectTransform;
            buttonRect.anchorMin = new Vector2(0.5f, 0);
            buttonRect.anchorMax = new Vector2(0.5f, 0);
            buttonRect.sizeDelta = new Vector2(100, 50);
            buttonRect.anchoredPosition = position;

            // Add text
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform);

            TextMeshProUGUI buttonText = textObj.AddComponent<TextMeshProUGUI>();
            buttonText.text = label;
            buttonText.fontSize = 18;
            buttonText.alignment = TextAlignmentOptions.Center;
            buttonText.color = textColor;

            RectTransform textRect = buttonText.rectTransform;
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            return button;
        }

        /// <summary>
        /// Initialize UI colors and styles
        /// </summary>
        private void InitializeUI()
        {
            // Set background
            if (backgroundImage != null)
            {
                backgroundImage.color = backgroundColor;
            }

            // Set player colors
            if (player1ScoreText != null)
                player1ScoreText.color = player1Color;

            if (player2ScoreText != null)
                player2ScoreText.color = player2Color;

            // Hide game over panel initially
            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);

            // Set initial scores
            UpdateScores(0, 0);
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
                Debug.Log("[BoardUIOverlay] Subscribed to GameManager events");
            }
        }

        /// <summary>
        /// Update score displays
        /// </summary>
        private void UpdateScores(int player1Score, int player2Score)
        {
            // Animate if scores changed
            if (player1Score != lastPlayer1Score && player1ScoreText != null)
            {
                AnimateScoreChange(player1ScoreText, lastPlayer1Score, player1Score);
                lastPlayer1Score = player1Score;
            }

            if (player2Score != lastPlayer2Score && player2ScoreText != null)
            {
                AnimateScoreChange(player2ScoreText, lastPlayer2Score, player2Score);
                lastPlayer2Score = player2Score;
            }
        }

        /// <summary>
        /// Animate score number change
        /// </summary>
        private void AnimateScoreChange(TextMeshProUGUI scoreText, int from, int to)
        {
            // Punch scale for emphasis
            scoreText.transform.DOPunchScale(Vector3.one * 0.15f, 0.4f, 5, 1f)
                .SetEase(Ease.OutElastic);

            // Count up animation
            DOTween.To(
                () => from,
                x => scoreText.text = x.ToString(),
                to,
                0.5f
            ).SetEase(Ease.OutQuad);
        }

        /// <summary>
        /// Update turn indicator
        /// </summary>
        private void UpdateTurnIndicator(int currentPlayer)
        {
            if (turnIndicatorText == null) return;

            string turnText = currentPlayer == 0 ? "YOUR TURN" : "AI THINKING...";
            turnIndicatorText.text = turnText;

            // Fade in/out animation using color
            Color textColor = turnIndicatorText.color;
            textColor.a = 0;
            turnIndicatorText.color = textColor;
            
            DOTween.ToAlpha(() => turnIndicatorText.color, x => turnIndicatorText.color = x, 1f, 0.3f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    DOVirtual.DelayedCall(1.5f, () =>
                    {
                        DOTween.ToAlpha(() => turnIndicatorText.color, x => turnIndicatorText.color = x, 0f, 0.3f);
                    });
                });

            // Scale animation
            turnIndicatorText.transform.localScale = Vector3.zero;
            turnIndicatorText.transform.DOScale(1f, 0.4f)
                .SetEase(Ease.OutBack);
        }

        // Event Handlers
        private void OnGameStarted(OwareBoard board)
        {
            Debug.Log("[BoardUIOverlay] Game started");
            
            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);

            UpdateScores(board.Player1Captured, board.Player2Captured);
            UpdateTurnIndicator(board.CurrentPlayer);
        }

        private void OnMoveMade(int pitIndex, int player)
        {
            if (GameManager.Instance?.Board != null)
            {
                OwareBoard board = GameManager.Instance.Board;
                UpdateScores(board.Player1Captured, board.Player2Captured);
                UpdateTurnIndicator(board.CurrentPlayer);
            }
        }

        private void OnGameEnded(int winner)
        {
            Debug.Log($"[BoardUIOverlay] Game ended. Winner: {winner}");

            // Show game over
            if (gameOverPanel != null && gameOverText != null)
            {
                string winnerText = winner == 0 ? "YOU WIN!" :
                                   winner == 1 ? "AI WINS!" : "DRAW!";
                
                gameOverText.text = winnerText;
                gameOverPanel.SetActive(true);

                // Animate entrance
                gameOverPanel.transform.localScale = Vector3.zero;
                gameOverPanel.transform.DOScale(1f, 0.5f)
                    .SetEase(Ease.OutBack);
            }
        }

        // Button Handlers
        private void OnNewGameClicked()
        {
            Debug.Log("[BoardUIOverlay] New Game clicked");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartNewGame();
            }
        }

        private void OnMenuClicked()
        {
            Debug.Log("[BoardUIOverlay] Menu clicked");
            // Navigate to menu
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowScreen(Screen.MainMenu);
            }
        }

        private void OnSettingsClicked()
        {
            Debug.Log("[BoardUIOverlay] Settings clicked");
            // Show settings modal
        }

        void OnDestroy()
        {
            // Unsubscribe
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStarted -= OnGameStarted;
                GameManager.Instance.OnMoveMade -= OnMoveMade;
                GameManager.Instance.OnGameEnded -= OnGameEnded;
            }
        }
    }
}

