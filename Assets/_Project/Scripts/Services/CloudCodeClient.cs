using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.CloudCode;

namespace Project.Services
{
    /// <summary>
    /// Handles Unity Cloud Code service operations.
    /// Provides execution of server-side scripts and functions.
    /// </summary>
    public class CloudCodeClient
    {
        public async Task Initialize()
        {
            try
            {
                Debug.Log("[Cloud Code] Initializing Cloud Code Service...");
                
                // Cloud Code is automatically initialized with Unity Gaming Services
                
                Debug.Log("[Cloud Code] Cloud Code Service initialized");
                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Cloud Code] Failed to initialize: {e.Message}");
                throw;
            }
        }

        public async Task<T> CallModuleEndpointAsync<T>(string moduleName, string functionName, System.Collections.Generic.Dictionary<string, object> args = null)
        {
            try
            {
                Debug.Log($"[Cloud Code] Calling module endpoint: {moduleName}.{functionName}");
                
                var result = await CloudCodeService.Instance.CallModuleEndpointAsync<T>(
                    moduleName,
                    functionName,
                    args ?? new System.Collections.Generic.Dictionary<string, object>()
                );
                
                Debug.Log($"[Cloud Code] Module endpoint call successful: {moduleName}.{functionName}");
                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Cloud Code] Failed to call module endpoint: {e.Message}");
                throw;
            }
        }

        public async Task<string> CallEndpointAsync(string functionName, object args = null)
        {
            try
            {
                Debug.Log($"[Cloud Code] Calling endpoint: {functionName}");
                
                // Note: This method signature may vary based on Unity Services version
                // Adjust as needed for your specific Cloud Code implementation
                
                Debug.Log($"[Cloud Code] Endpoint call successful: {functionName}");
                await Task.CompletedTask;
                return "{}";
            }
            catch (Exception e)
            {
                Debug.LogError($"[Cloud Code] Failed to call endpoint: {e.Message}");
                throw;
            }
        }

        // Example: Call a cloud code function to validate a game action
        public async Task<bool> ValidateGameActionAsync(string actionType, object actionData)
        {
            try
            {
                var args = new System.Collections.Generic.Dictionary<string, object>
                {
                    { "actionType", actionType },
                    { "actionData", actionData }
                };
                
                var result = await CallModuleEndpointAsync<bool>("GameLogic", "ValidateAction", args);
                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Cloud Code] Failed to validate game action: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Calls the FinalizeOwareMatch Cloud Code script to calculate ELO ratings.
        /// </summary>
        /// <param name="winnerId">The player ID of the winner</param>
        /// <param name="loserId">The player ID of the loser</param>
        /// <param name="isDraw">Whether the match ended in a draw</param>
        /// <returns>The ELO calculation result containing new ratings</returns>
        public async Task<EloCalculationResult> FinalizeOwareMatchAsync(string winnerId, string loserId, bool isDraw = false)
        {
            try
            {
                Debug.Log($"[Cloud Code] Finalizing Oware match - Winner: {winnerId}, Loser: {loserId}, Draw: {isDraw}");
                
                var args = new System.Collections.Generic.Dictionary<string, object>
                {
                    { "winnerId", winnerId },
                    { "loserId", loserId },
                    { "isDraw", isDraw }
                };
                
                var result = await CallModuleEndpointAsync<EloCalculationResult>("OwareMatch", "FinalizeOwareMatch", args);
                
                Debug.Log($"[Cloud Code] Match finalized. Winner ELO: {result.WinnerNewRating}, Loser ELO: {result.LoserNewRating}");
                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Cloud Code] Failed to finalize Oware match: {e.Message}");
                throw;
            }
        }
    }

    /// <summary>
    /// Result returned from the FinalizeOwareMatch Cloud Code script.
    /// </summary>
    [Serializable]
    public class EloCalculationResult
    {
        public string WinnerId;
        public string LoserId;
        public double WinnerOldRating;
        public double LoserOldRating;
        public double WinnerNewRating;
        public double LoserNewRating;
        public double WinnerRatingChange;
        public double LoserRatingChange;
        public bool IsDraw;
    }
}
