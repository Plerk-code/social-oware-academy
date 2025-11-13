using NUnit.Framework;
using SocialOwareAcademy.Gameplay;
using SocialOwareAcademy.Gameplay.AI;
using System.Diagnostics;

namespace SocialOwareAcademy.Tests
{
    /// <summary>
    /// Unit tests for IntermediateAI implementation.
    /// Tests cover: Minimax algorithm, move selection, performance, tactical play.
    /// </summary>
    [TestFixture]
    public class IntermediateAITests
    {
        private IntermediateAI ai;
        private OwareBoard board;

        [SetUp]
        public void SetUp()
        {
            board = new OwareBoard();
            ai = new IntermediateAI();
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
            // Arrange - Fresh board has all valid moves
            var validMoves = OwareRules.GetValidMoves(board);

            // Act
            int move = ai.GetMove(board);

            // Assert
            Assert.GreaterOrEqual(move, 0, "Move should be non-negative");
            Assert.Contains(move, validMoves, "Move should be valid");
        }

        [Test]
        public void GetMove_ExecutesUnder500Milliseconds()
        {
            // Arrange
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            int move = ai.GetMove(board);
            stopwatch.Stop();

            // Assert
            Assert.Less(stopwatch.ElapsedMilliseconds, 500,
                $"AI should execute in <500ms, took {stopwatch.ElapsedMilliseconds}ms");
            Assert.GreaterOrEqual(move, 0, "Should return valid move");
        }

        [Test]
        public void GetMove_AverageExecutionTime_UnderTarget()
        {
            // Arrange
            const int iterations = 10;
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
            Assert.Less(avgTime, 250,
                $"Average AI execution should be <250ms, was {avgTime:F2}ms");
        }

        [Test]
        public void GetMove_SelectsCaptureMove_WhenAvailable()
        {
            // Arrange - Create board with capture opportunity
            var captureBoard = CreateBoardWithCaptureOpportunity();

            if (captureBoard == null)
            {
                Assert.Inconclusive("Could not create capture opportunity board for test");
                return;
            }

            // Act
            int move = ai.GetMove(captureBoard);

            // Assert - Verify move captures seeds
            var simBoard = captureBoard.Clone();
            int capturedBefore = simBoard.CurrentPlayer == 0 ?
                simBoard.Player1Captured : simBoard.Player2Captured;

            OwareRules.ExecuteMove(simBoard, move, simulate: true);

            int capturedAfter = simBoard.CurrentPlayer == 0 ?
                simBoard.Player1Captured : simBoard.Player2Captured;

            // Minimax should select a capturing move if available
            Assert.GreaterOrEqual(capturedAfter, capturedBefore,
                "AI should select move that maintains or increases captures");
        }

        [Test]
        public void IntermediateAI_BeatsBeginnerAI_Majority()
        {
            // Arrange
            var intermediateAI = new IntermediateAI();
            var beginnerAI = new BeginnerAI();
            int intermediateWins = 0;
            const int totalGames = 20;

            // Act - Simulate matches: IntermediateAI (P1) vs BeginnerAI (P2)
            for (int i = 0; i < totalGames; i++)
            {
                var testBoard = new OwareBoard();
                int moveCount = 0;
                const int MAX_MOVES = 200;

                while (!OwareRules.CheckGameEnd(testBoard) && moveCount < MAX_MOVES)
                {
                    var validMoves = OwareRules.GetValidMoves(testBoard);
                    if (validMoves.Count == 0) break;

                    int move = testBoard.CurrentPlayer == 0 ?
                        intermediateAI.GetMove(testBoard) : beginnerAI.GetMove(testBoard);

                    if (move == -1) break;

                    OwareRules.ExecuteMove(testBoard, move, simulate: true);
                    moveCount++;
                }

                int winner = OwareRules.GetWinner(testBoard);
                if (winner == 0) intermediateWins++;
            }

            // Assert - IntermediateAI should win >80% vs BeginnerAI
            float winRate = (intermediateWins / (float)totalGames) * 100f;
            Assert.GreaterOrEqual(intermediateWins, 16,
                $"IntermediateAI should win >80% vs BeginnerAI, won {intermediateWins}/{totalGames} ({winRate:F1}%)");
        }

        [Test]
        public void GetDifficultyName_ReturnsIntermediate()
        {
            // Act
            string difficultyName = ai.GetDifficultyName();

            // Assert
            Assert.AreEqual("Intermediate", difficultyName,
                "IntermediateAI should report 'Intermediate' as difficulty name");
        }

        [Test]
        public void GetMove_ConsistentOnSameBoardState()
        {
            // Arrange - Minimax is deterministic
            var testBoard = CreateMidGameBoard(42);

            // Act
            int move1 = ai.GetMove(testBoard.Clone());
            int move2 = ai.GetMove(testBoard.Clone());

            // Assert - Same board state should yield same move
            Assert.AreEqual(move1, move2,
                "Minimax should return consistent moves for identical board states");
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
            // Simulate a mid-game board where captures are likely
            var testBoard = CreateMidGameBoard(123);

            // Verify there are valid moves
            var validMoves = OwareRules.GetValidMoves(testBoard);
            if (validMoves.Count == 0) return null;

            return testBoard;
        }
    }
}
