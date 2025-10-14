using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Authentication;
using Project.Services;

namespace Project.Examples
{
    /// <summary>
    /// Example integration showing how to finalize an Oware match
    /// with ELO calculation and leaderboard updates.
    /// 
    /// Call FinalizeMatchAsync() when a match ends to:
    /// 1. Calculate ELO ratings server-side
    /// 2. Update player ratings in Cloud Save
    /// 3. Submit ratings to leaderboard
    /// 4. Retrieve updated leaderboard standings
    /// </summary>
    public class MatchIntegrationExample : MonoBehaviour
    {
        [Header("Services")]
        private CloudCodeClient cloudCodeClient;
        private LeaderboardsService leaderboardsService;

        [Header("Configuration")]
        private const string ELO_LEADERBOARD_ID = "oware-elo-ratings";

        private void Awake()
        {
            // Initialize services (these would typically be injected via dependency injection)
            cloudCodeClient = new CloudCodeClient();
            leaderboardsService = new LeaderboardsService();
        }

        /// <summary>
        /// Call this method when an Oware match ends.
        /// </summary>
        /// <param name="winnerId">Player ID of the winner</param>
        /// <param name="loserId">Player ID of the loser</param>
        /// <param name="isDraw">True if the match was a draw</param>
        public async Task FinalizeMatchAsync(string winnerId, string loserId, bool isDraw = false)
        {
            try
            {
                Debug.Log($"[Match] Finalizing match - Winner: {winnerId}, Loser: {loserId}, Draw: {isDraw}");

                // Step 1: Call Cloud Code to calculate ELO ratings server-side
                var eloResult = await cloudCodeClient.FinalizeOwareMatchAsync(
                    winnerId,
                    loserId,
                    isDraw
                );

                // Step 2: Log the results
                LogEloResults(eloResult);

                // Step 3: If this is the local player, fetch their updated leaderboard standing
                var localPlayerId = AuthenticationService.Instance.PlayerId;
                if (localPlayerId == winnerId || localPlayerId == loserId)
                {
                    await UpdateLocalPlayerLeaderboardInfo();
                }

                Debug.Log("[Match] Match finalization complete");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Match] Failed to finalize match: {e.Message}");
                throw;
            }
        }

        /// <summary>
        /// Example: Finalize a match where the local player won.
        /// </summary>
        public async Task FinalizeMatchAsWinner(string opponentId)
        {
            var localPlayerId = AuthenticationService.Instance.PlayerId;
            await FinalizeMatchAsync(localPlayerId, opponentId, isDraw: false);
        }

        /// <summary>
        /// Example: Finalize a match where the local player lost.
        /// </summary>
        public async Task FinalizeMatchAsLoser(string opponentId)
        {
            var localPlayerId = AuthenticationService.Instance.PlayerId;
            await FinalizeMatchAsync(opponentId, localPlayerId, isDraw: false);
        }

        /// <summary>
        /// Example: Finalize a match that ended in a draw.
        /// </summary>
        public async Task FinalizeMatchAsDraw(string opponentId)
        {
            var localPlayerId = AuthenticationService.Instance.PlayerId;
            // In a draw, order doesn't matter, but we still pass winner/loser for consistency
            await FinalizeMatchAsync(localPlayerId, opponentId, isDraw: true);
        }

        /// <summary>
        /// Fetches and displays the local player's current leaderboard standing.
        /// </summary>
        private async Task UpdateLocalPlayerLeaderboardInfo()
        {
            try
            {
                var myScore = await leaderboardsService.GetMyScoreAsync(ELO_LEADERBOARD_ID);
                
                Debug.Log($"[Leaderboard] Your current standing:");
                Debug.Log($"  - Rank: {myScore.Rank}");
                Debug.Log($"  - ELO Rating: {myScore.Score}");
                Debug.Log($"  - Player ID: {myScore.PlayerId}");

                // You can also fetch the top players
                var topScores = await leaderboardsService.GetScoresAsync(ELO_LEADERBOARD_ID, limit: 10);
                Debug.Log($"[Leaderboard] Top 10 players retrieved: {topScores.Results.Count} entries");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Leaderboard] Could not fetch leaderboard info: {e.Message}");
            }
        }

        /// <summary>
        /// Logs detailed ELO calculation results.
        /// </summary>
        private void LogEloResults(EloCalculationResult result)
        {
            Debug.Log("=== Match Results ===");
            Debug.Log($"Match Type: {(result.IsDraw ? "Draw" : "Win/Loss")}");
            Debug.Log("");
            Debug.Log($"Winner: {result.WinnerId}");
            Debug.Log($"  Old Rating: {result.WinnerOldRating}");
            Debug.Log($"  New Rating: {result.WinnerNewRating}");
            Debug.Log($"  Change: {result.WinnerRatingChange:+#;-#;0}");
            Debug.Log("");
            Debug.Log($"Loser: {result.LoserId}");
            Debug.Log($"  Old Rating: {result.LoserOldRating}");
            Debug.Log($"  New Rating: {result.LoserNewRating}");
            Debug.Log($"  Change: {result.LoserRatingChange:+#;-#;0}");
            Debug.Log("==================");
        }

        #region Example Usage in Game Flow

        // Example: Call this from your game manager when the match ends
        private async void OnGameStateChanged(GameState newState)
        {
            if (newState == GameState.MatchEnded)
            {
                // Determine the winner and loser based on your game logic
                var winnerId = DetermineWinner();
                var loserId = DetermineLoser();
                var isDraw = CheckIfDraw();

                // Finalize the match
                await FinalizeMatchAsync(winnerId, loserId, isDraw);
            }
        }

        // Placeholder methods - replace with your actual game logic
        private string DetermineWinner() => "player1-id";
        private string DetermineLoser() => "player2-id";
        private bool CheckIfDraw() => false;

        #endregion

        #region Editor Testing

#if UNITY_EDITOR
        [Header("Testing")]
        [SerializeField] private string testOpponentId = "test-opponent-id";

        [ContextMenu("Test - Finalize Match (Win)")]
        private async void TestFinalizeMatchWin()
        {
            await FinalizeMatchAsWinner(testOpponentId);
        }

        [ContextMenu("Test - Finalize Match (Loss)")]
        private async void TestFinalizeMatchLoss()
        {
            await FinalizeMatchAsLoser(testOpponentId);
        }

        [ContextMenu("Test - Finalize Match (Draw)")]
        private async void TestFinalizeMatchDraw()
        {
            await FinalizeMatchAsDraw(testOpponentId);
        }

        [ContextMenu("Test - Get My Leaderboard Score")]
        private async void TestGetMyScore()
        {
            try
            {
                var myScore = await leaderboardsService.GetMyScoreAsync(ELO_LEADERBOARD_ID);
                Debug.Log($"My ELO: {myScore.Score}, Rank: {myScore.Rank}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to get score: {e.Message}");
            }
        }

        [ContextMenu("Test - Get Top 10 Leaderboard")]
        private async void TestGetTopScores()
        {
            try
            {
                var scores = await leaderboardsService.GetScoresAsync(ELO_LEADERBOARD_ID, 10);
                Debug.Log($"Top {scores.Results.Count} players:");
                foreach (var entry in scores.Results)
                {
                    Debug.Log($"  #{entry.Rank}: {entry.PlayerName ?? entry.PlayerId} - {entry.Score} ELO");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to get scores: {e.Message}");
            }
        }
#endif

        #endregion
    }

    // Example enum - replace with your actual GameState enum
    public enum GameState
    {
        Waiting,
        InProgress,
        MatchEnded
    }
}
