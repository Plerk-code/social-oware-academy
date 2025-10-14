using UnityEngine;
using System;

namespace SocialOwareAcademy.Gameplay
{
    /// <summary>
    /// Represents the Oware game board state and provides methods to query/modify it.
    /// Based on standard Ghanaian Oware rules (12 pits, 4 seeds each, no store pits).
    /// </summary>
    public class OwareBoard
    {
        // Constants
        public const int PITS_PER_PLAYER = 6;
        public const int TOTAL_PITS = 12;
        public const int STARTING_SEEDS_PER_PIT = 4;
        public const int TOTAL_SEEDS = 48;
        public const int SEEDS_TO_WIN = 25;

        // Board state
        private int[] pits;                  // 12 pits (indices 0-5 = Player 1, 6-11 = Player 2)
        private int player1Captured;         // Seeds captured by Player 1
        private int player2Captured;         // Seeds captured by Player 2
        private int currentPlayer;           // 0 = Player 1, 1 = Player 2

        // Events
        public event Action<int[]> OnBoardChanged;
        public event Action<int, int> OnCaptureOccurred;  // (player, seedsCaptured)
        public event Action<int> OnTurnChanged;           // (newPlayer)
        public event Action<int> OnGameOver;              // (winnerPlayer, -1 if draw)

        // Properties
        public int[] Pits => pits;
        public int Player1Captured => player1Captured;
        public int Player2Captured => player2Captured;
        public int CurrentPlayer => currentPlayer;
        public bool IsGameOver => player1Captured >= SEEDS_TO_WIN || player2Captured >= SEEDS_TO_WIN;

        /// <summary>
        /// Initialize board with standard Oware starting position.
        /// </summary>
        public OwareBoard()
        {
            ResetBoard();
        }

        /// <summary>
        /// Reset board to starting position (4 seeds per pit).
        /// </summary>
        public void ResetBoard()
        {
            pits = new int[TOTAL_PITS];
            for (int i = 0; i < TOTAL_PITS; i++)
            {
                pits[i] = STARTING_SEEDS_PER_PIT;
            }

            player1Captured = 0;
            player2Captured = 0;
            currentPlayer = 0; // Player 1 starts

            OnBoardChanged?.Invoke(pits);
            OnTurnChanged?.Invoke(currentPlayer);

            Debug.Log("[OwareBoard] Board reset. Player 1 starts.");
        }

        /// <summary>
        /// Get seeds in a specific pit.
        /// </summary>
        public int GetSeeds(int pitIndex)
        {
            if (pitIndex < 0 || pitIndex >= TOTAL_PITS)
            {
                Debug.LogError($"[OwareBoard] Invalid pit index: {pitIndex}");
                return 0;
            }
            return pits[pitIndex];
        }

        /// <summary>
        /// Set seeds in a specific pit (for move execution).
        /// </summary>
        public void SetSeeds(int pitIndex, int seeds)
        {
            if (pitIndex < 0 || pitIndex >= TOTAL_PITS)
            {
                Debug.LogError($"[OwareBoard] Invalid pit index: {pitIndex}");
                return;
            }
            pits[pitIndex] = seeds;
            OnBoardChanged?.Invoke(pits);
        }

        /// <summary>
        /// Add captured seeds to player's total.
        /// </summary>
        public void AddCapturedSeeds(int player, int seeds)
        {
            if (player == 0)
            {
                player1Captured += seeds;
                Debug.Log($"[OwareBoard] Player 1 captured {seeds} seeds (total: {player1Captured})");
            }
            else if (player == 1)
            {
                player2Captured += seeds;
                Debug.Log($"[OwareBoard] Player 2 captured {seeds} seeds (total: {player2Captured})");
            }

            OnCaptureOccurred?.Invoke(player, seeds);

            // Check win condition
            if (player1Captured >= SEEDS_TO_WIN)
            {
                Debug.Log("[OwareBoard] Player 1 wins!");
                OnGameOver?.Invoke(0);
            }
            else if (player2Captured >= SEEDS_TO_WIN)
            {
                Debug.Log("[OwareBoard] Player 2 wins!");
                OnGameOver?.Invoke(1);
            }
        }

        /// <summary>
        /// Switch to next player's turn.
        /// </summary>
        public void NextTurn()
        {
            currentPlayer = 1 - currentPlayer; // Toggle between 0 and 1
            OnTurnChanged?.Invoke(currentPlayer);
            Debug.Log($"[OwareBoard] Turn changed to Player {currentPlayer + 1}");
        }

        /// <summary>
        /// Check if a pit belongs to the current player.
        /// </summary>
        public bool IsPitOwnedByCurrentPlayer(int pitIndex)
        {
            if (currentPlayer == 0)
                return pitIndex >= 0 && pitIndex < PITS_PER_PLAYER;
            else
                return pitIndex >= PITS_PER_PLAYER && pitIndex < TOTAL_PITS;
        }

        /// <summary>
        /// Check if a pit belongs to the opponent.
        /// </summary>
        public bool IsPitOwnedByOpponent(int pitIndex)
        {
            if (currentPlayer == 0)
                return pitIndex >= PITS_PER_PLAYER && pitIndex < TOTAL_PITS;
            else
                return pitIndex >= 0 && pitIndex < PITS_PER_PLAYER;
        }

        /// <summary>
        /// Get player's pit range (start and end indices).
        /// </summary>
        public (int start, int end) GetPlayerPitRange(int player)
        {
            if (player == 0)
                return (0, PITS_PER_PLAYER - 1);
            else
                return (PITS_PER_PLAYER, TOTAL_PITS - 1);
        }

        /// <summary>
        /// Check if player has any seeds on their side (for forced feeding rule).
        /// </summary>
        public bool PlayerHasSeeds(int player)
        {
            var (start, end) = GetPlayerPitRange(player);
            for (int i = start; i <= end; i++)
            {
                if (pits[i] > 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Get total seeds remaining on the board (excluding captured).
        /// </summary>
        public int GetTotalSeedsOnBoard()
        {
            int total = 0;
            for (int i = 0; i < TOTAL_PITS; i++)
            {
                total += pits[i];
            }
            return total;
        }

        /// <summary>
        /// Clone the current board state (for AI lookahead).
        /// </summary>
        public OwareBoard Clone()
        {
            OwareBoard clone = new OwareBoard();
            Array.Copy(this.pits, clone.pits, TOTAL_PITS);
            clone.player1Captured = this.player1Captured;
            clone.player2Captured = this.player2Captured;
            clone.currentPlayer = this.currentPlayer;
            return clone;
        }

        /// <summary>
        /// Debug: Print current board state.
        /// </summary>
        public void DebugPrintBoard()
        {
            Debug.Log("=== OWARE BOARD STATE ===");
            Debug.Log($"Player 2 Pits: [{string.Join(", ", pits[11], pits[10], pits[9], pits[8], pits[7], pits[6])}]");
            Debug.Log($"Player 1 Pits: [{string.Join(", ", pits[0], pits[1], pits[2], pits[3], pits[4], pits[5])}]");
            Debug.Log($"Captured - P1: {player1Captured} | P2: {player2Captured}");
            Debug.Log($"Current Turn: Player {currentPlayer + 1}");
            Debug.Log("========================");
        }
    }
}
