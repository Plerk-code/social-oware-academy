using UnityEngine;
using SocialOwareAcademy.Gameplay;
using SocialOwareAcademy.Gameplay.AI;
using System.Collections.Generic;

namespace SocialOwareAcademy.Testing
{
    /// <summary>
    /// Test script to verify BeginnerAI performance and win rate.
    /// Attach to any GameObject and use context menu buttons.
    /// DELETE THIS SCRIPT after verification is complete.
    /// </summary>
    public class BeginnerAITest : MonoBehaviour
    {
        [Header("Test Configuration")]
        [SerializeField] private int numberOfGames = 100;
        [SerializeField] private bool showDetailedLogs = false;

        private BeginnerAI aiOpponent;
        private List<long> aiDecisionTimes = new List<long>();
        private int aiWins = 0;
        private int playerWins = 0;
        private int draws = 0;

        [ContextMenu("1. Run Performance Test (AI Decision Time)")]
        public void TestAIPerformance()
        {
            Debug.Log("=== BeginnerAI Performance Test ===");
            aiOpponent = new BeginnerAI();
            aiDecisionTimes.Clear();

            // Test AI decision time across different board states
            for (int i = 0; i < 50; i++)
            {
                OwareBoard board = CreateRandomBoardState();
                
                var startTime = System.Diagnostics.Stopwatch.StartNew();
                int move = aiOpponent.GetMove(board);
                startTime.Stop();

                aiDecisionTimes.Add(startTime.ElapsedMilliseconds);

                if (showDetailedLogs)
                    Debug.Log($"Test {i + 1}: AI decision time = {startTime.ElapsedMilliseconds}ms, Move = {move}");
            }

            // Calculate statistics
            long minTime = long.MaxValue;
            long maxTime = 0;
            long totalTime = 0;

            foreach (long time in aiDecisionTimes)
            {
                if (time < minTime) minTime = time;
                if (time > maxTime) maxTime = time;
                totalTime += time;
            }

            float avgTime = totalTime / (float)aiDecisionTimes.Count;

            Debug.Log($"--- Performance Results ---");
            Debug.Log($"Average Decision Time: {avgTime:F2}ms");
            Debug.Log($"Min Decision Time: {minTime}ms");
            Debug.Log($"Max Decision Time: {maxTime}ms");
            Debug.Log($"Target: <100ms ✅ {(avgTime < 100 ? "PASS" : "FAIL")}");
        }

        [ContextMenu("2. Run Win Rate Test (100 Games)")]
        public void TestWinRate()
        {
            Debug.Log($"=== BeginnerAI Win Rate Test ({numberOfGames} games) ===");
            aiOpponent = new BeginnerAI();
            aiWins = 0;
            playerWins = 0;
            draws = 0;

            for (int i = 0; i < numberOfGames; i++)
            {
                SimulateGame();
                
                if ((i + 1) % 10 == 0)
                {
                    Debug.Log($"Progress: {i + 1}/{numberOfGames} games completed");
                }
            }

            // Calculate percentages
            float aiWinRate = (aiWins / (float)numberOfGames) * 100f;
            float playerWinRate = (playerWins / (float)numberOfGames) * 100f;
            float drawRate = (draws / (float)numberOfGames) * 100f;

            Debug.Log($"--- Win Rate Results ---");
            Debug.Log($"Player Wins: {playerWins} ({playerWinRate:F1}%)");
            Debug.Log($"AI Wins: {aiWins} ({aiWinRate:F1}%)");
            Debug.Log($"Draws: {draws} ({drawRate:F1}%)");
            Debug.Log($"Target: 60-70% player win rate ✅ {(playerWinRate >= 60f && playerWinRate <= 70f ? "PASS" : "NEEDS TUNING")}");
        }

        [ContextMenu("3. Run Strategic Move Analysis")]
        public void TestStrategicBehavior()
        {
            Debug.Log("=== BeginnerAI Strategic Behavior Test ===");
            aiOpponent = new BeginnerAI();

            // Test 1: Does AI recognize capture opportunities?
            OwareBoard captureTestBoard = CreateCaptureOpportunityBoard();
            Debug.Log("Test Board (Capture Opportunity):");
            captureTestBoard.DebugPrintBoard();
            
            int captureMove = aiOpponent.GetMove(captureTestBoard);
            Debug.Log($"AI selected move: Pit {captureMove}");
            
            // Test 2: Does AI avoid obvious blunders?
            OwareBoard defensiveBoard = CreateDefensiveBoard();
            Debug.Log("Test Board (Defensive Situation):");
            defensiveBoard.DebugPrintBoard();
            
            int defensiveMove = aiOpponent.GetMove(defensiveBoard);
            Debug.Log($"AI selected move: Pit {defensiveMove}");
        }

        /// <summary>
        /// Simulate a full game between AI (Player 1) and random player (Player 2).
        /// </summary>
        private void SimulateGame()
        {
            OwareBoard board = new OwareBoard();
            int moveCount = 0;
            const int MAX_MOVES = 200; // Prevent infinite games

            while (!OwareRules.CheckGameEnd(board) && moveCount < MAX_MOVES)
            {
                var validMoves = OwareRules.GetValidMoves(board);
                
                if (validMoves.Count == 0)
                    break;

                int move;
                if (board.CurrentPlayer == 0)
                {
                    // Player 1 (AI)
                    move = aiOpponent.GetMove(board);
                }
                else
                {
                    // Player 2 (Random - simulates beginner human player)
                    move = validMoves[Random.Range(0, validMoves.Count)];
                }

                OwareRules.ExecuteMove(board, move, simulate: true);
                moveCount++;
            }

            // Determine winner
            int winner = OwareRules.GetWinner(board);
            if (winner == 0)
                aiWins++;
            else if (winner == 1)
                playerWins++;
            else
                draws++;

            if (showDetailedLogs)
            {
                string winnerName = winner == 0 ? "AI" : (winner == 1 ? "Player" : "Draw");
                Debug.Log($"Game ended: {winnerName} wins (P1: {board.Player1Captured}, P2: {board.Player2Captured})");
            }
        }

        /// <summary>
        /// Create a random mid-game board state for testing.
        /// </summary>
        private OwareBoard CreateRandomBoardState()
        {
            OwareBoard board = new OwareBoard();
            
            // Simulate random moves to get a mid-game state
            int moves = Random.Range(3, 10);
            for (int i = 0; i < moves; i++)
            {
                var validMoves = OwareRules.GetValidMoves(board);
                if (validMoves.Count == 0) break;
                
                int randomMove = validMoves[Random.Range(0, validMoves.Count)];
                OwareRules.ExecuteMove(board, randomMove, simulate: true);
            }

            return board;
        }

        /// <summary>
        /// Create a board state with an obvious capture opportunity.
        /// </summary>
        private OwareBoard CreateCaptureOpportunityBoard()
        {
            OwareBoard board = new OwareBoard();
            // This would require manually setting pit values
            // For now, return a standard board
            return board;
        }

        /// <summary>
        /// Create a board state requiring defensive play.
        /// </summary>
        private OwareBoard CreateDefensiveBoard()
        {
            OwareBoard board = new OwareBoard();
            // This would require manually setting pit values
            // For now, return a standard board
            return board;
        }
    }
}

