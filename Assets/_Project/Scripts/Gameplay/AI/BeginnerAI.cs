using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace SocialOwareAcademy.Gameplay.AI
{
    /// <summary>
    /// Beginner-level AI opponent for Oware.
    /// Uses mostly random moves (70%) with occasional strategic moves (30%).
    /// Designed to be beatable by new players while teaching basic strategy.
    /// </summary>
    public class BeginnerAI : IOwareAI
    {
        private const float STRATEGIC_MOVE_CHANCE = 0.3f; // 30% strategic, 70% random
        private System.Random random;

        public BeginnerAI()
        {
            random = new System.Random();
        }

        /// <summary>
        /// Get the AI's chosen move using random or strategic logic.
        /// </summary>
        public int GetMove(OwareBoard board)
        {
            var validMoves = OwareRules.GetValidMoves(board);

            if (validMoves.Count == 0)
            {
                Debug.LogWarning("[BeginnerAI] No valid moves available");
                return -1;
            }

            // 30% chance to use strategic move, 70% chance to use random move
            if (random.NextDouble() < STRATEGIC_MOVE_CHANCE)
            {
                int strategicMove = GetStrategicMove(board, validMoves);
                if (strategicMove != -1)
                {
                    Debug.Log($"[BeginnerAI] Strategic move selected: Pit {strategicMove}");
                    return strategicMove;
                }
            }

            // Fallback to random move
            int randomMove = validMoves[random.Next(validMoves.Count)];
            Debug.Log($"[BeginnerAI] Random move selected: Pit {randomMove}");
            return randomMove;
        }

        /// <summary>
        /// Apply basic strategic heuristic to select a move.
        /// Priority:
        /// 1. Moves that capture seeds (2-3 seeds in opponent pit after sowing)
        /// 2. Moves that prevent opponent from capturing on their next turn
        /// 3. Fallback to random
        /// </summary>
        private int GetStrategicMove(OwareBoard board, List<int> validMoves)
        {
            List<MoveScore> moveScores = new List<MoveScore>();

            foreach (int move in validMoves)
            {
                int score = EvaluateMove(board, move);
                moveScores.Add(new MoveScore { Move = move, Score = score });
            }

            // Sort by score descending
            moveScores = moveScores.OrderByDescending(ms => ms.Score).ToList();

            // Return best move if it has positive score
            if (moveScores[0].Score > 0)
            {
                Debug.Log($"[BeginnerAI] Best strategic move: Pit {moveScores[0].Move} (Score: {moveScores[0].Score})");
                return moveScores[0].Move;
            }

            // No good strategic move found
            return -1;
        }

        /// <summary>
        /// Evaluate a potential move and return a score.
        /// Higher score = better move.
        /// </summary>
        private int EvaluateMove(OwareBoard board, int move)
        {
            int score = 0;

            // Simulate the move
            OwareBoard tempBoard = board.Clone();
            int capturedBefore = board.CurrentPlayer == 0 ? tempBoard.Player1Captured : tempBoard.Player2Captured;
            
            OwareRules.ExecuteMove(tempBoard, move, simulate: true);
            
            int capturedAfter = board.CurrentPlayer == 0 ? tempBoard.Player1Captured : tempBoard.Player2Captured;
            int seedsCaptured = capturedAfter - capturedBefore;

            // Priority 1: Capturing moves (highest weight)
            if (seedsCaptured > 0)
            {
                score += seedsCaptured * 10; // +10 points per seed captured
                Debug.Log($"[BeginnerAI] Move {move} captures {seedsCaptured} seeds (+{seedsCaptured * 10} points)");
            }

            // Priority 2: Prevent opponent captures (medium weight)
            int opponentThreatScore = CalculateOpponentThreatAfterMove(tempBoard);
            score -= opponentThreatScore * 5; // -5 points per seed opponent could capture

            // Priority 3: Avoid leaving vulnerable pits (low weight)
            int vulnerabilityScore = CalculateVulnerability(tempBoard);
            score -= vulnerabilityScore * 2; // -2 points per vulnerable seed

            return score;
        }

        /// <summary>
        /// Calculate how many seeds opponent could capture on their next turn.
        /// </summary>
        private int CalculateOpponentThreatAfterMove(OwareBoard board)
        {
            var opponentMoves = OwareRules.GetValidMoves(board);
            int maxThreat = 0;

            foreach (int opponentMove in opponentMoves)
            {
                OwareBoard testBoard = board.Clone();
                int capturedBefore = board.CurrentPlayer == 0 ? testBoard.Player1Captured : testBoard.Player2Captured;
                
                OwareRules.ExecuteMove(testBoard, opponentMove, simulate: true);
                
                int capturedAfter = board.CurrentPlayer == 0 ? testBoard.Player1Captured : testBoard.Player2Captured;
                int threat = capturedAfter - capturedBefore;

                if (threat > maxThreat)
                {
                    maxThreat = threat;
                }
            }

            return maxThreat;
        }

        /// <summary>
        /// Calculate how many seeds are in vulnerable positions (2-3 seeds in our pits).
        /// </summary>
        private int CalculateVulnerability(OwareBoard board)
        {
            int vulnerableSeeds = 0;
            var (start, end) = board.GetPlayerPitRange(board.CurrentPlayer);

            for (int i = start; i <= end; i++)
            {
                int seeds = board.GetSeeds(i);
                if (seeds == 2 || seeds == 3)
                {
                    vulnerableSeeds += seeds;
                }
            }

            return vulnerableSeeds;
        }

        public string GetDifficultyName()
        {
            return "Beginner";
        }

        /// <summary>
        /// Helper class to pair a move with its evaluation score.
        /// </summary>
        private class MoveScore
        {
            public int Move;
            public int Score;
        }
    }
}

