using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using SocialOwareAcademy.Gameplay;
using SocialOwareAcademy.Managers;

namespace SocialOwareAcademy.UI
{
    /// <summary>
    /// Enhanced Match Screen UI Controller with beautiful, polished interface.
    /// Integrates with existing GameManager and provides stunning visual feedback.
    /// </summary>
    public class MatchScreenUI : MonoBehaviour
    {
        [Header("Player Panels")]
        [SerializeField] private Image player1Panel;
        [SerializeField] private Image player2Panel;
        [SerializeField] private TextMeshProUGUI player1NameText;
        [SerializeField] private TextMeshProUGUI player2NameText;
        [SerializeField] private TextMeshProUGUI player1ScoreText;
        [SerializeField] private TextMeshProUGUI player2ScoreText;

        [Header("Turn Indicator")]
        [SerializeField] private GameObject turnIndicator;
        [SerializeField] private TextMeshProUGUI turnText;

        [Header("Game Over")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI gameOverTitleText;
        [SerializeField] private TextMeshProUGUI gameOverScoreText;
        [SerializeField] private Button playAgainButton;
        [SerializeField] private Button mainMenuButton;

        [Header("Settings")]
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button forfeitButton;

        [Header("Colors")]
        [SerializeField] private ColorPalette colorPalette;

        // Animation sequences
        private Sequence turnIndicatorAnim;
        private Sequence gameOverAnim;

        private void Awake()
        {
            // Load color palette if not assigned
            if (colorPalette == null)
            {
                colorPalette = ColorPalette.Instance;
            }

            // Setup button listeners
            if (playAgainButton != null)
                playAgainButton.onClick.AddListener(OnPlayAgainClicked);

            if (mainMenuButton != null)
                mainMenuButton.onClick.AddListener(OnMainMenuClicked);

            if (settingsButton != null)
                settingsButton.onClick.AddListener(OnSettingsClicked);

            if (forfeitButton != null)
                forfeitButton.onClick.AddListener(OnForfeitClicked);

            // Hide game over panel initially
            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);
        }

        private void Start()
        {
            // Subscribe to GameManager events
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStarted += OnGameStarted;
                GameManager.Instance.OnMoveMade += OnMoveMade;
                GameManager.Instance.OnGameEnded += OnGameEnded;
            }

            // Initialize UI
            InitializeUI();
        }

        /// <summary>
        /// Initialize UI with color palette and styling
        /// </summary>
        private void InitializeUI()
        {
            if (colorPalette == null) return;

            // Style player panels
            if (player1Panel != null)
            {
                player1Panel.color = colorPalette.WithAlpha(colorPalette.deepBrown, 0.9f);
            }

            if (player2Panel != null)
            {
                player2Panel.color = colorPalette.WithAlpha(colorPalette.deepBrown, 0.9f);
            }

            // Style player names
            if (player1NameText != null)
            {
                player1NameText.text = "YOU";
                player1NameText.color = colorPalette.offWhite;
            }

            if (player2NameText != null)
            {
                player2NameText.text = "AI";
                player2NameText.color = colorPalette.offWhite;
            }

            // Style score texts
            if (player1ScoreText != null)
            {
                player1ScoreText.text = "0";
                player1ScoreText.color = colorPalette.player1;
            }

            if (player2ScoreText != null)
            {
                player2ScoreText.text = "0";
                player2ScoreText.color = colorPalette.player2;
            }

            // Style turn indicator
            if (turnText != null)
            {
                turnText.color = colorPalette.gold;
            }
        }

        /// <summary>
        /// Handle game started event
        /// </summary>
        private void OnGameStarted(OwareBoard board)
        {
            Debug.Log("[MatchScreenUI] Game started");

            // Hide game over panel
            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);

            // Update UI
            UpdateUI(board);

            // Animate turn indicator
            AnimateTurnIndicator(board.CurrentPlayer);
        }

        /// <summary>
        /// Handle move made event
        /// </summary>
        private void OnMoveMade(int pitIndex, int player)
        {
            Debug.Log($"[MatchScreenUI] Move made: Pit {pitIndex} by Player {player}");

            if (GameManager.Instance?.Board != null)
            {
                UpdateUI(GameManager.Instance.Board);
                AnimateTurnIndicator(GameManager.Instance.Board.CurrentPlayer);
            }
        }

        /// <summary>
        /// Handle game ended event
        /// </summary>
        private void OnGameEnded(int winner)
        {
            Debug.Log($"[MatchScreenUI] Game ended. Winner: {winner}");

            if (GameManager.Instance?.Board != null)
            {
                UpdateUI(GameManager.Instance.Board);
                ShowGameOver(winner);
            }
        }

        /// <summary>
        /// Update UI based on board state
        /// </summary>
        private void UpdateUI(OwareBoard board)
        {
            // Update scores with animation
            if (player1ScoreText != null)
            {
                AnimateScoreChange(player1ScoreText, board.Player1Captured);
            }

            if (player2ScoreText != null)
            {
                AnimateScoreChange(player2ScoreText, board.Player2Captured);
            }

            // Highlight active player panel
            HighlightActivePlayer(board.CurrentPlayer);
        }

        /// <summary>
        /// Animate score text change
        /// </summary>
        private void AnimateScoreChange(TextMeshProUGUI scoreText, int newScore)
        {
            int currentScore = int.TryParse(scoreText.text, out int score) ? score : 0;

            if (currentScore != newScore)
            {
                // Punch scale animation
                scoreText.transform.DOPunchScale(Vector3.one * 0.2f, LayoutConstants.ANIM_MEDIUM, 1)
                    .SetEase(Ease.OutElastic);

                // Count up animation
                DOTween.To(() => currentScore, x => {
                    currentScore = x;
                    scoreText.text = x.ToString();
                }, newScore, LayoutConstants.ANIM_MEDIUM);
            }
        }

        /// <summary>
        /// Highlight the active player's panel
        /// </summary>
        private void HighlightActivePlayer(int activePlayer)
        {
            if (colorPalette == null) return;

            // Animate player 1 panel
            if (player1Panel != null)
            {
                Color targetColor = activePlayer == 0
                    ? colorPalette.WithAlpha(colorPalette.terracotta, 0.3f)
                    : colorPalette.WithAlpha(colorPalette.deepBrown, 0.6f);

                DOTween.To(() => player1Panel.color, x => player1Panel.color = x, targetColor, LayoutConstants.ANIM_MEDIUM);
            }

            // Animate player 2 panel
            if (player2Panel != null)
            {
                Color targetColor = activePlayer == 1
                    ? colorPalette.WithAlpha(colorPalette.danger, 0.3f)
                    : colorPalette.WithAlpha(colorPalette.deepBrown, 0.6f);

                DOTween.To(() => player2Panel.color, x => player2Panel.color = x, targetColor, LayoutConstants.ANIM_MEDIUM);
            }
        }

        /// <summary>
        /// Animate turn indicator
        /// </summary>
        private void AnimateTurnIndicator(int currentPlayer)
        {
            if (turnIndicator == null || turnText == null) return;

            // Kill existing animation
            turnIndicatorAnim?.Kill();

            // Update text
            turnText.text = currentPlayer == 0 ? "YOUR TURN" : "AI THINKING...";

            // Fade in + scale animation
            turnIndicator.transform.localScale = Vector3.zero;
            turnIndicatorAnim = DOTween.Sequence()
                .Append(turnIndicator.transform.DOScale(Vector3.one, LayoutConstants.ANIM_MEDIUM)
                    .SetEase(Ease.OutBack))
                .AppendInterval(1f)
                .Append(turnIndicator.transform.DOScale(Vector3.zero, LayoutConstants.ANIM_FAST)
                    .SetEase(Ease.InBack));
        }

        /// <summary>
        /// Show game over panel with animation
        /// </summary>
        private void ShowGameOver(int winner)
        {
            if (gameOverPanel == null || colorPalette == null) return;

            // Set winner text
            if (gameOverTitleText != null)
            {
                string winnerText = winner == -1 ? "DRAW!" :
                                   (winner == 0 ? "YOU WIN!" : "AI WINS!");
                gameOverTitleText.text = winnerText;

                Color titleColor = winner == 0 ? colorPalette.success :
                                  winner == 1 ? colorPalette.danger :
                                  colorPalette.warning;
                gameOverTitleText.color = titleColor;
            }

            // Set score text
            if (gameOverScoreText != null && GameManager.Instance?.Board != null)
            {
                gameOverScoreText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(colorPalette.player1)}>{GameManager.Instance.Board.Player1Captured}</color> - " +
                                        $"<color=#{ColorUtility.ToHtmlStringRGB(colorPalette.player2)}>{GameManager.Instance.Board.Player2Captured}</color>";
            }

            // Animate panel entrance
            gameOverPanel.SetActive(true);
            CanvasGroup canvasGroup = gameOverPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = gameOverPanel.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;
            gameOverPanel.transform.localScale = Vector3.one * 0.8f;

            gameOverAnim?.Kill();
            gameOverAnim = DOTween.Sequence()
                .Append(canvasGroup.DOFade(1f, LayoutConstants.ANIM_MEDIUM))
                .Join(gameOverPanel.transform.DOScale(Vector3.one, LayoutConstants.ANIM_MEDIUM)
                    .SetEase(Ease.OutBack));
        }

        // Button handlers
        private void OnPlayAgainClicked()
        {
            Debug.Log("[MatchScreenUI] Play Again clicked");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartNewGame();
            }
        }

        private void OnMainMenuClicked()
        {
            Debug.Log("[MatchScreenUI] Main Menu clicked");
            // TODO: Navigate to main menu using UIManager
            if (UIManager.Instance != null)
            {
                UIManager.Instance.GoToScreen(Screen.MainMenu);
            }
        }

        private void OnSettingsClicked()
        {
            Debug.Log("[MatchScreenUI] Settings clicked");
            // TODO: Show settings modal
        }

        private void OnForfeitClicked()
        {
            Debug.Log("[MatchScreenUI] Forfeit clicked");
            // TODO: Implement forfeit logic
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStarted -= OnGameStarted;
                GameManager.Instance.OnMoveMade -= OnMoveMade;
                GameManager.Instance.OnGameEnded -= OnGameEnded;
            }

            // Kill animations
            turnIndicatorAnim?.Kill();
            gameOverAnim?.Kill();

            // Remove button listeners
            if (playAgainButton != null)
                playAgainButton.onClick.RemoveListener(OnPlayAgainClicked);

            if (mainMenuButton != null)
                mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);

            if (settingsButton != null)
                settingsButton.onClick.RemoveListener(OnSettingsClicked);

            if (forfeitButton != null)
                forfeitButton.onClick.RemoveListener(OnForfeitClicked);
        }
    }
}
