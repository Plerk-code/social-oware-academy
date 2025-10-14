using UnityEngine;
using System.Collections.Generic;

namespace SocialOwareAcademy.Gameplay
{
    /// <summary>
    /// Implements standard Ghanaian Oware rules:
    /// - Counter-clockwise sowing
    /// - Capture on 2-3 seeds in opponent's pit
    /// - Grand Slam rule (can't capture ALL opponent's seeds)
    /// - Forced feeding (must give opponent seeds if they have none)
    /// </summary>
    public static class OwareRules
    {
        /// <summary>
        /// Check if a move is valid for the current player.
        /// </summary>
        public static bool IsValidMove(OwareBoard board, int pitIndex)
        {
            // Must be player's own pit
            if (!board.IsPitOwnedByCurrentPlayer(pitIndex))
            {
                Debug.LogWarning($"[OwareRules] Invalid move: Pit {pitIndex} not owned by Player {board.CurrentPlayer + 1}");
                return false;
            }

            // Pit must have seeds
            if (board.GetSeeds(pitIndex) == 0)
            {
                Debug.LogWarning($"[OwareRules] Invalid move: Pit {pitIndex} is empty");
                return false;
            }

            // Forced feeding rule: If opponent has no seeds, must make a move that gives them seeds
            int opponent = 1 - board.CurrentPlayer;
            if (!board.PlayerHasSeeds(opponent))
            {
                // Simulate move to check if it feeds opponent
                OwareBoard tempBoard = board.Clone();
                ExecuteMove(tempBoard, pitIndex, simulate: true);

                if (!tempBoard.PlayerHasSeeds(opponent))
                {
                    Debug.LogWarning($"[OwareRules] Invalid move: Must feed opponent (they have no seeds)");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Get list of all valid moves for current player.
        /// </summary>
        public static List<int> GetValidMoves(OwareBoard board)
        {
            List<int> validMoves = new List<int>();
            var (start, end) = board.GetPlayerPitRange(board.CurrentPlayer);

            for (int i = start; i <= end; i++)
            {
                if (IsValidMove(board, i))
                {
                    validMoves.Add(i);
                }
            }

            return validMoves;
        }

        /// <summary>
        /// Execute a move and return if it was successful.
        /// </summary>
        public static bool ExecuteMove(OwareBoard board, int pitIndex, bool simulate = false)
        {
            if (!IsValidMove(board, pitIndex))
            {
                return false;
            }

            // Pick up seeds from selected pit
            int seedsInHand = board.GetSeeds(pitIndex);
            board.SetSeeds(pitIndex, 0);

            if (!simulate)
                Debug.Log($"[OwareRules] Player {board.CurrentPlayer + 1} picks up {seedsInHand} seeds from pit {pitIndex}");

            // Sow seeds counter-clockwise
            int currentPit = pitIndex;
            while (seedsInHand > 0)
            {
                currentPit = (currentPit + 1) % OwareBoard.TOTAL_PITS;

                // Skip the starting pit (Oware rule)
                if (currentPit == pitIndex)
                    continue;

                board.SetSeeds(currentPit, board.GetSeeds(currentPit) + 1);
                seedsInHand--;
            }

            int lastPit = currentPit;
            if (!simulate)
                Debug.Log($"[OwareRules] Last seed dropped in pit {lastPit}");

            // Check for captures (if last seed landed in opponent's pit)
            if (board.IsPitOwnedByOpponent(lastPit))
            {
                int totalCaptured = ProcessCaptures(board, lastPit, simulate);

                if (totalCaptured > 0 && !simulate)
                {
                    Debug.Log($"[OwareRules] Player {board.CurrentPlayer + 1} captured {totalCaptured} seeds");
                }
            }

            // Switch turn
            if (!simulate)
            {
                board.NextTurn();
            }

            return true;
        }

        /// <summary>
        /// Process captures starting from the last pit (work backwards).
        /// Returns total seeds captured.
        /// </summary>
        private static int ProcessCaptures(OwareBoard board, int lastPit, bool simulate)
        {
            int totalCaptured = 0;
            List<int> pitsToCapture = new List<int>();

            // Work backwards from last pit (counter-clockwise direction)
            int currentPit = lastPit;

            while (board.IsPitOwnedByOpponent(currentPit))
            {
                int seeds = board.GetSeeds(currentPit);

                // Capture if 2 or 3 seeds in opponent's pit
                if (seeds == 2 || seeds == 3)
                {
                    pitsToCapture.Add(currentPit);
                    totalCaptured += seeds;

                    // Move to previous pit (counter-clockwise, so subtract 1)
                    currentPit = (currentPit - 1 + OwareBoard.TOTAL_PITS) % OwareBoard.TOTAL_PITS;
                }
                else
                {
                    // Stop if pit doesn't have 2 or 3 seeds
                    break;
                }
            }

            // Grand Slam rule: Don't capture if it would leave opponent with no seeds
            if (WouldBeGrandSlam(board, pitsToCapture))
            {
                if (!simulate)
                    Debug.Log("[OwareRules] Grand Slam prevented - opponent must have seeds");
                return 0; // No capture
            }

            // Execute captures
            foreach (int pit in pitsToCapture)
            {
                int seeds = board.GetSeeds(pit);
                board.SetSeeds(pit, 0);
                board.AddCapturedSeeds(board.CurrentPlayer, seeds);
            }

            return totalCaptured;
        }

        /// <summary>
        /// Check if capturing would result in a Grand Slam (opponent left with no seeds).
        /// </summary>
        private static bool WouldBeGrandSlam(OwareBoard board, List<int> pitsToCapture)
        {
            // Count opponent's seeds after proposed captures
            int opponent = 1 - board.CurrentPlayer;
            var (start, end) = board.GetPlayerPitRange(opponent);

            int opponentSeedsAfterCapture = 0;
            for (int i = start; i <= end; i++)
            {
                if (!pitsToCapture.Contains(i))
                {
                    opponentSeedsAfterCapture += board.GetSeeds(i);
                }
            }

            return opponentSeedsAfterCapture == 0;
        }

        /// <summary>
        /// Check if game should end (one player has 25+ seeds or stalemate).
        /// </summary>
        public static bool CheckGameEnd(OwareBoard board)
        {
            // Win condition: 25+ seeds captured
            if (board.Player1Captured >= OwareBoard.SEEDS_TO_WIN ||
                board.Player2Captured >= OwareBoard.SEEDS_TO_WIN)
            {
                return true;
            }

            // Stalemate: No valid moves available
            if (GetValidMoves(board).Count == 0)
            {
                Debug.Log("[OwareRules] No valid moves - game ends");

                // Award remaining seeds to player who can move
                int opponent = 1 - board.CurrentPlayer;
                var (start, end) = board.GetPlayerPitRange(opponent);
                int remainingSeeds = 0;

                for (int i = start; i <= end; i++)
                {
                    remainingSeeds += board.GetSeeds(i);
                }

                if (remainingSeeds > 0)
                {
                    board.AddCapturedSeeds(opponent, remainingSeeds);
                    Debug.Log($"[OwareRules] {remainingSeeds} remaining seeds awarded to Player {opponent + 1}");
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Get the winner (0 or 1), or -1 if draw.
        /// </summary>
        public static int GetWinner(OwareBoard board)
        {
            if (board.Player1Captured > board.Player2Captured)
                return 0;
            else if (board.Player2Captured > board.Player1Captured)
                return 1;
            else
                return -1; // Draw
        }
    }
}
