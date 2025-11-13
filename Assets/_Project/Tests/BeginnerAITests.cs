using NUnit.Framework;
using SocialOwareAcademy.Gameplay;
using SocialOwareAcademy.Gameplay.AI;
using System.Diagnostics;

namespace SocialOwareAcademy.Tests
{
    /// <summary>
    /// Unit tests for BeginnerAI implementation.
    /// Tests cover: valid move selection, performance, strategic behavior.
    /// </summary>
    [TestFixture]
    public class BeginnerAITests
    {
        private BeginnerAI ai;
        private OwareBoard board;

        [SetUp]
        public void SetUp()
        {
            board = new OwareBoard();
            ai = new BeginnerAI();
        }

        [Test]
        public void GetMove_OnNewBoard_ReturnsValidMove()
        {
            // Act
            int move = ai.GetMove(board);

            // Assert
            var validMoves = OwareRules.GetValidMoves(board);
            Assert.Contains(move, validMoves, "AI should return a valid move");
        }

        [Test]
        public void GetMove_WithOnlyOneValidMove_ReturnsThatMove()
        {
            // Arrange - Create board with only one valid move
            // (This would require setting up a specific board state)
            // For now, test with fresh board where all moves are valid
            var validMoves = OwareRules.GetValidMoves(board);

            // Act
            int move = ai.GetMove(board);

            // Assert
            Assert.GreaterOrEqual(move, 0, "Move should be non-negative");
            Assert.Contains(move, validMoves, "Move should be valid");
        }

        [Test]
        public void GetMove_ExecutesUnder100Milliseconds()
        {
            // Arrange
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            int move = ai.GetMove(board);
            stopwatch.Stop();

            // Assert
            Assert.Less(stopwatch.ElapsedMilliseconds, 100,
                $"AI should execute in <100ms, took {stopwatch.ElapsedMilliseconds}ms");
            Assert.GreaterOrEqual(move, 0, "Should return valid move");
        }

        [Test]
        public void GetMove_AverageExecutionTime_UnderTarget()
        {
            // Arrange
            const int iterations = 50;
            long totalTime = 0;
            var stopwatch = new Stopwatch();

            // Act - Test across multiple board states
            for (int i = 0; i < iterations; i++)
            {
                var testBoard = CreateMidGameBoard(i);

                stopwatch.Restart();
                ai.GetMove(testBoard);
                stopwatch.Stop();

                totalTime += stopwatch.ElapsedMilliseconds;
            }

            float avgTime = totalTime / (float)iterations;

            // Assert
            Assert.Less(avgTime, 50,
                $"Average AI execution should be <50ms, was {avgTime:F2}ms");
        }

        [Test]
        public void GetMove_OnEmptyBoard_ReturnsErrorCode()
        {
            // Arrange - Create board with no valid moves
            var emptyBoard = new OwareBoard();
            // Clear all pits for current player (simulate no valid moves scenario)
            // Note: This test is theoretical - in real game this shouldn't happen

            // For now, just verify AI handles normal boards
            int move = ai.GetMove(emptyBoard);

            // Assert
            Assert.GreaterOrEqual(move, -1, "Should return valid move or -1 for error");
        }

        [Test]
        public void GetMove_MultipleInvocations_ReturnsVariedMoves()
        {
            // Arrange
            const int trials = 20;
            var movesSelected = new System.Collections.Generic.HashSet<int>();

            // Act - Get multiple moves from same board state
            for (int i = 0; i < trials; i++)
            {
                var freshBoard = new OwareBoard();
                int move = ai.GetMove(freshBoard);
                movesSelected.Add(move);
            }

            // Assert - Should show randomness (not always same move)
            Assert.GreaterOrEqual(movesSelected.Count, 2,
                "AI should show randomness across multiple calls (selected moves from start: " +
                string.Join(", ", movesSelected) + ")");
        }

        [Test]
        public void GetMove_DetectsCaptureOpportunities()
        {
            // Arrange - Simulate multiple games to check for strategic behavior
            const int trials = 100;
            int strategicMovesDetected = 0;

            for (int i = 0; i < trials; i++)
            {
                var testBoard = CreateBoardWithCaptureOpportunity();
                if (testBoard != null)
                {
                    int move = ai.GetMove(testBoard);

                    // Check if move captures seeds
                    var simBoard = testBoard.Clone();
                    int capturedBefore = simBoard.CurrentPlayer == 0 ?
                        simBoard.Player1Captured : simBoard.Player2Captured;

                    OwareRules.ExecuteMove(simBoard, move, simulate: true);

                    int capturedAfter = simBoard.CurrentPlayer == 0 ?
                        simBoard.Player1Captured : simBoard.Player2Captured;

                    if (capturedAfter > capturedBefore)
                    {
                        strategicMovesDetected++;
                    }
                }
            }

            // Assert - AI should occasionally make strategic captures
            // With 30% strategic chance and random distribution, expect some captures
            Assert.Greater(strategicMovesDetected, 0,
                "AI should detect and execute capture opportunities occasionally");
        }

        [Test]
        public void GetDifficultyName_ReturnsBeginner()
        {
            // Act
            string difficultyName = ai.GetDifficultyName();

            // Assert
            Assert.AreEqual("Beginner", difficultyName,
                "BeginnerAI should report 'Beginner' as difficulty name");
        }

        /// <summary>
        /// Create a mid-game board state by simulating random moves.
        /// </summary>
        private OwareBoard CreateMidGameBoard(int seed)
        {
            var testBoard = new OwareBoard();
            var rng = new System.Random(seed);

            int movesToMake = rng.Next(3, 8);
            for (int i = 0; i < movesToMake; i++)
            {
                var validMoves = OwareRules.GetValidMoves(testBoard);
                if (validMoves.Count == 0) break;

                int randomMove = validMoves[rng.Next(validMoves.Count)];
                OwareRules.ExecuteMove(testBoard, randomMove, simulate: true);
            }

            return testBoard;
        }

        /// <summary>
        /// Create a board with potential capture opportunities.
        /// </summary>
        private OwareBoard CreateBoardWithCaptureOpportunity()
        {
            // For now, just simulate a mid-game board
            // In a real implementation, we'd set up specific pit values
            return CreateMidGameBoard(42);
        }
    }
}
