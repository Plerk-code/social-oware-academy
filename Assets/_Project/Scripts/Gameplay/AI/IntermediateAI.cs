using UnityEngine;
using System.Collections.Generic;

namespace SocialOwareAcademy.Gameplay.AI
{
    /// <summary>
    /// Intermediate-level AI opponent for Oware.
    /// Uses Minimax algorithm with depth 3 to evaluate moves.
    /// Suitable for players with basic strategy knowledge seeking a challenge.
    /// </summary>
    public class IntermediateAI : IOwareAI
    {
        private const int MAX_DEPTH = 3;
        private const int MAX_SCORE = 10000;
        private const int MIN_SCORE = -10000;

        /// <summary>
        /// Select the best move using Minimax algorithm with depth 3.
        /// </summary>
        public int GetMove(OwareBoard board)
        {
            var validMoves = OwareRules.GetValidMoves(board);

            if (validMoves.Count == 0)
            {
                Debug.LogWarning("[IntermediateAI] No valid moves available");
                return -1;
            }

            if (validMoves.Count == 1)
            {
                return validMoves[0];
            }

            // Evaluate each root move using Minimax
            int bestMove = validMoves[0];
            int bestScore = MIN_SCORE;

            foreach (int move in validMoves)
            {
                OwareBoard simBoard = board.Clone();
                OwareRules.ExecuteMove(simBoard, move, simulate: true);

                // Minimax from opponent's perspective (minimize)
                int score = Minimax(simBoard, MAX_DEPTH - 1, false, board.CurrentPlayer);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
            }

            Debug.Log($"[IntermediateAI] Selected move {bestMove} with score {bestScore}");
            return bestMove;
        }

        /// <summary>
        /// Recursive Minimax algorithm.
        /// </summary>
        /// <param name="board">Current board state</param>
        /// <param name="depth">Remaining search depth</param>
        /// <param name="isMaximizing">True if maximizing player's turn</param>
        /// <param name="aiPlayer">AI's player index (0 or 1)</param>
        /// <returns>Best score for current player</returns>
        private int Minimax(OwareBoard board, int depth, bool isMaximizing, int aiPlayer)
        {
            // Terminal conditions
            if (depth == 0 || OwareRules.CheckGameEnd(board))
            {
                return EvaluateBoard(board, aiPlayer);
            }

            var validMoves = OwareRules.GetValidMoves(board);

            if (validMoves.Count == 0)
            {
                return EvaluateBoard(board, aiPlayer);
            }

            if (isMaximizing)
            {
                int maxScore = MIN_SCORE;

                foreach (int move in validMoves)
                {
                    OwareBoard simBoard = board.Clone();
                    OwareRules.ExecuteMove(simBoard, move, simulate: true);

                    int score = Minimax(simBoard, depth - 1, false, aiPlayer);
                    maxScore = Mathf.Max(maxScore, score);
                }

                return maxScore;
            }
            else
            {
                int minScore = MAX_SCORE;

                foreach (int move in validMoves)
                {
                    OwareBoard simBoard = board.Clone();
                    OwareRules.ExecuteMove(simBoard, move, simulate: true);

                    int score = Minimax(simBoard, depth - 1, true, aiPlayer);
                    minScore = Mathf.Min(minScore, score);
                }

                return minScore;
            }
        }

        /// <summary>
        /// Evaluate board state from AI's perspective.
        /// Higher score = better for AI.
        /// </summary>
        /// <param name="board">Board state to evaluate</param>
        /// <param name="aiPlayer">AI's player index</param>
        /// <returns>Board evaluation score</returns>
        private int EvaluateBoard(OwareBoard board, int aiPlayer)
        {
            int aiCaptured = aiPlayer == 0 ? board.Player1Captured : board.Player2Captured;
            int opponentCaptured = aiPlayer == 0 ? board.Player2Captured : board.Player1Captured;

            // Primary heuristic: captured seed difference (winning condition)
            int score = (aiCaptured - opponentCaptured) * 10;

            // Secondary heuristic: seed count on AI's side (board control)
            var (aiStart, aiEnd) = board.GetPlayerPitRange(aiPlayer);
            int aiSeeds = 0;
            for (int i = aiStart; i <= aiEnd; i++)
            {
                aiSeeds += board.GetSeeds(i);
            }

            // Opponent's seed count
            int opponentPlayer = aiPlayer == 0 ? 1 : 0;
            var (oppStart, oppEnd) = board.GetPlayerPitRange(opponentPlayer);
            int opponentSeeds = 0;
            for (int i = oppStart; i <= oppEnd; i++)
            {
                opponentSeeds += board.GetSeeds(i);
            }

            // Board control bonus (more seeds on our side)
            score += (aiSeeds - opponentSeeds);

            return score;
        }

        /// <summary>
        /// Get the display name of this AI difficulty level.
        /// </summary>
        public string GetDifficultyName()
        {
            return "Intermediate";
        }
    }
}
