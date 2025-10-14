using UnityEngine;
using System;
using SocialOwareAcademy.Gameplay;

/// <summary>
/// Central game manager - Singleton that orchestrates Oware matches.
/// Manages game flow, player turns, AI opponents, and game state.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    [SerializeField] private bool isLocalGame = true;
    [SerializeField] private bool isAIOpponent = true;
    [SerializeField] private int humanPlayerIndex = 0; // 0 or 1

    // Core game state
    private OwareBoard board;
    private bool isGameActive = false;

    // Events
    public event Action<OwareBoard> OnGameStarted;
    public event Action<int> OnGameEnded; // winner index
    public event Action<int, int> OnMoveMade; // (pitIndex, player)

    // Properties
    public OwareBoard Board => board;
    public bool IsGameActive => isGameActive;
    public bool IsHumanTurn => board != null && board.CurrentPlayer == humanPlayerIndex;

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

        Debug.Log("[GameManager] Initialized");
    }

    void Start()
    {
        // Auto-start a game for testing
        StartNewGame();
    }

    /// <summary>
    /// Start a new Oware game.
    /// </summary>
    public void StartNewGame()
    {
        Debug.Log("[GameManager] Starting new game...");

        // Create new board
        board = new OwareBoard();

        // Subscribe to board events
        board.OnGameOver += HandleGameOver;
        board.OnTurnChanged += HandleTurnChanged;
        board.OnCaptureOccurred += HandleCapture;

        isGameActive = true;

        // Print initial board state
        board.DebugPrintBoard();

        // Notify listeners
        OnGameStarted?.Invoke(board);

        // If AI starts, trigger AI move
        if (isAIOpponent && !IsHumanTurn)
        {
            Debug.Log("[GameManager] AI starts first");
            Invoke(nameof(TriggerAIMove), 1f); // Delay for readability
        }
    }

    /// <summary>
    /// Player makes a move (human player only).
    /// </summary>
    public bool MakeMove(int pitIndex)
    {
        if (!isGameActive)
        {
            Debug.LogWarning("[GameManager] Game not active");
            return false;
        }

        if (!IsHumanTurn && isAIOpponent)
        {
            Debug.LogWarning("[GameManager] Not human's turn");
            return false;
        }

        // Validate and execute move
        if (OwareRules.ExecuteMove(board, pitIndex))
        {
            Debug.Log($"[GameManager] Move executed: Pit {pitIndex}");
            board.DebugPrintBoard();

            OnMoveMade?.Invoke(pitIndex, board.CurrentPlayer);

            // Check for game end
            if (OwareRules.CheckGameEnd(board))
            {
                EndGame();
                return true;
            }

            // If AI opponent and now AI's turn, trigger AI move
            if (isAIOpponent && !IsHumanTurn)
            {
                Invoke(nameof(TriggerAIMove), 1.5f); // Delay for visual feedback
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// Trigger AI to make a move.
    /// </summary>
    private void TriggerAIMove()
    {
        if (!isGameActive || IsHumanTurn)
            return;

        Debug.Log("[GameManager] AI is thinking...");

        // Simple AI: Pick first valid move (for now - will improve later)
        var validMoves = OwareRules.GetValidMoves(board);

        if (validMoves.Count > 0)
        {
            int aiMove = validMoves[UnityEngine.Random.Range(0, validMoves.Count)];
            Debug.Log($"[GameManager] AI chooses pit {aiMove}");

            OwareRules.ExecuteMove(board, aiMove);
            board.DebugPrintBoard();

            OnMoveMade?.Invoke(aiMove, board.CurrentPlayer);

            // Check for game end
            if (OwareRules.CheckGameEnd(board))
            {
                EndGame();
            }
        }
        else
        {
            Debug.LogWarning("[GameManager] AI has no valid moves");
            EndGame();
        }
    }

    /// <summary>
    /// End the current game.
    /// </summary>
    private void EndGame()
    {
        isGameActive = false;

        int winner = OwareRules.GetWinner(board);
        string winnerName = winner == 0 ? "Player 1" : (winner == 1 ? "Player 2" : "Draw");

        Debug.Log($"[GameManager] Game Over! Winner: {winnerName}");
        Debug.Log($"Final Score - P1: {board.Player1Captured} | P2: {board.Player2Captured}");

        OnGameEnded?.Invoke(winner);
    }

    /// <summary>
    /// Handle board game over event.
    /// </summary>
    private void HandleGameOver(int winner)
    {
        Debug.Log($"[GameManager] Board reported game over: Winner {winner}");
    }

    /// <summary>
    /// Handle turn change event.
    /// </summary>
    private void HandleTurnChanged(int newPlayer)
    {
        Debug.Log($"[GameManager] Turn changed to Player {newPlayer + 1}");
    }

    /// <summary>
    /// Handle capture event.
    /// </summary>
    private void HandleCapture(int player, int seedsCaptured)
    {
        Debug.Log($"[GameManager] Player {player + 1} captured {seedsCaptured} seeds");
    }

    /// <summary>
    /// Get current game state summary for UI.
    /// </summary>
    public string GetGameStateSummary()
    {
        if (board == null)
            return "No game active";

        return $"Turn: Player {board.CurrentPlayer + 1}\n" +
               $"P1 Captured: {board.Player1Captured}\n" +
               $"P2 Captured: {board.Player2Captured}\n" +
               $"Active: {isGameActive}";
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        if (board != null)
        {
            board.OnGameOver -= HandleGameOver;
            board.OnTurnChanged -= HandleTurnChanged;
            board.OnCaptureOccurred -= HandleCapture;
        }
    }
}
