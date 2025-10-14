using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication;
using Project.Services;

namespace Project.UI
{
    /// <summary>
    /// Results screen UI controller that handles match finalization.
    /// Attach this to a GameObject in the results scene and hook up UI elements in the inspector.
    /// </summary>
    public class ResultsUI : MonoBehaviour
    {
        [Header("Service References")]
        [SerializeField] private UgsInitializer ugsInitializer;

        [Header("UI Elements")]
        [SerializeField] private Text resultText;
        [SerializeField] private Text eloText;
        [SerializeField] private Text statusText;
        [SerializeField] private Button continueButton;

        private CloudCodeClient cloudCodeClient;
        private LeaderboardsService leaderboardsService;
        
        // Leaderboard configuration
        private const string ELO_LEADERBOARD_ID = "oware-elo-leaderboard";

        private void Start()
        {
            // Get service references from UgsInitializer
            if (ugsInitializer != null)
            {
                cloudCodeClient = ugsInitializer.GetCloudCodeClient();
                leaderboardsService = ugsInitializer.GetLeaderboardsService();
            }
            else
            {
                Debug.LogError("[ResultsUI] UgsInitializer not assigned!");
            }
        }

        /// <summary>
        /// Displays the match results and finalizes the match with backend services.
        /// </summary>
        /// <param name="opponentId">The player ID of the opponent</param>
        /// <param name="didWin">True if local player won, false if lost</param>
        /// <param name="isDraw">True if the match ended in a draw</param>
        public async void ShowResults(string opponentId, bool didWin, bool isDraw = false)
        {
            Debug.Log($"[ResultsUI] Showing results - Opponent: {opponentId}, Win: {didWin}, Draw: {isDraw}");
            
            if (cloudCodeClient == null || leaderboardsService == null)
            {
                UpdateStatus("Services not initialized");
                Debug.LogError("[ResultsUI] Services not available");
                return;
            }

            // Get local player ID
            string localPlayerId = AuthenticationService.Instance.PlayerId;
            
            // Display result to player
            string resultMessage = isDraw ? "Draw!" : (didWin ? "Victory!" : "Defeat");
            UpdateResultText(resultMessage);
            UpdateStatus("Calculating ratings...");

            try
            {
                // Determine winner and loser IDs
                string winnerId = isDraw ? null : (didWin ? localPlayerId : opponentId);
                string loserId = isDraw ? null : (didWin ? opponentId : localPlayerId);

                // Call Cloud Code to finalize match and calculate ELO
                Debug.Log($"[ResultsUI] Finalizing match with Cloud Code...");
                EloCalculationResult eloResult;
                
                if (isDraw)
                {
                    // For draws, we still need to pass player IDs (order doesn't matter for draws)
                    eloResult = await cloudCodeClient.FinalizeOwareMatchAsync(localPlayerId, opponentId, true);
                }
                else
                {
                    eloResult = await cloudCodeClient.FinalizeOwareMatchAsync(winnerId, loserId, false);
                }

                Debug.Log($"[ResultsUI] ELO calculation complete - Old: {(didWin ? eloResult.WinnerOldRating : eloResult.LoserOldRating)}, " +
                          $"New: {(didWin ? eloResult.WinnerNewRating : eloResult.LoserNewRating)}, " +
                          $"Change: {(didWin ? eloResult.WinnerRatingChange : eloResult.LoserRatingChange)}");

                // Get the local player's new ELO rating
                double newElo = didWin ? eloResult.WinnerNewRating : eloResult.LoserNewRating;
                double eloChange = didWin ? eloResult.WinnerRatingChange : eloResult.LoserRatingChange;

                // Submit new ELO to leaderboard
                UpdateStatus("Updating leaderboard...");
                Debug.Log($"[ResultsUI] Submitting score to leaderboard: {newElo}");
                await leaderboardsService.SubmitScoreAsync(ELO_LEADERBOARD_ID, newElo);

                // Display ELO information
                string eloChangePrefix = eloChange >= 0 ? "+" : "";
                UpdateEloText($"ELO: {newElo:F0} ({eloChangePrefix}{eloChange:F0})");
                UpdateStatus("Match complete!");
                
                Debug.Log($"[ResultsUI] Results finalized successfully");
            }
            catch (Exception e)
            {
                UpdateStatus($"Error finalizing match: {e.Message}");
                Debug.LogError($"[ResultsUI] Failed to finalize match: {e}");
            }
        }

        /// <summary>
        /// Alternative method that takes a MatchResult enum for cleaner API.
        /// </summary>
        /// <param name="opponentId">The player ID of the opponent</param>
        /// <param name="result">The match result (Win, Loss, or Draw)</param>
        public void ShowResults(string opponentId, MatchResult result)
        {
            bool didWin = result == MatchResult.Win;
            bool isDraw = result == MatchResult.Draw;
            ShowResults(opponentId, didWin, isDraw);
        }

        private void UpdateResultText(string message)
        {
            if (resultText != null)
            {
                resultText.text = message;
            }
            Debug.Log($"[ResultsUI] Result: {message}");
        }

        private void UpdateEloText(string message)
        {
            if (eloText != null)
            {
                eloText.text = message;
            }
            Debug.Log($"[ResultsUI] ELO: {message}");
        }

        private void UpdateStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
            Debug.Log($"[ResultsUI] Status: {message}");
        }
    }

    /// <summary>
    /// Enum representing possible match results.
    /// </summary>
    public enum MatchResult
    {
        Win,
        Loss,
        Draw
    }
}
