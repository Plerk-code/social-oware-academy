using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;

namespace Project.Services
{
    /// <summary>
    /// Handles Unity Leaderboards service operations.
    /// Provides leaderboard score submission and retrieval.
    /// </summary>
    public class LeaderboardsService
    {
        public async Task Initialize()
        {
            try
            {
                Debug.Log("[Leaderboards] Initializing Leaderboards Service...");
                
                // Leaderboards is automatically initialized with Unity Gaming Services
                
                Debug.Log("[Leaderboards] Leaderboards Service initialized");
                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Leaderboards] Failed to initialize: {e.Message}");
                throw;
            }
        }

        public async Task AddPlayerScoreAsync(string leaderboardId, double score)
        {
            try
            {
                Debug.Log($"[Leaderboards] Adding player score to leaderboard: {leaderboardId}, Score: {score}");
                
                var result = await Unity.Services.Leaderboards.LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score);
                Debug.Log($"[Leaderboards] Score added successfully. Rank: {result.Rank}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Leaderboards] Failed to add player score: {e.Message}");
                throw;
            }
        }

        public async Task<LeaderboardScoresPage> GetScoresAsync(string leaderboardId, int limit = 10)
        {
            try
            {
                Debug.Log($"[Leaderboards] Fetching scores for leaderboard: {leaderboardId}");
                
                var options = new GetScoresOptions
                {
                    Offset = 0,
                    Limit = limit
                };
                
                var scores = await Unity.Services.Leaderboards.LeaderboardsService.Instance.GetScoresAsync(leaderboardId, options);
                Debug.Log($"[Leaderboards] Retrieved {scores.Results.Count} scores");
                
                return scores;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Leaderboards] Failed to get scores: {e.Message}");
                throw;
            }
        }

        public async Task<LeaderboardEntry> GetPlayerScoreAsync(string leaderboardId)
        {
            try
            {
                Debug.Log($"[Leaderboards] Fetching player score from leaderboard: {leaderboardId}");
                
                var entry = await Unity.Services.Leaderboards.LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboardId);
                Debug.Log($"[Leaderboards] Player rank: {entry.Rank}, Score: {entry.Score}");
                
                return entry;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Leaderboards] Failed to get player score: {e.Message}");
                throw;
            }
        }

        public async Task<LeaderboardScores> GetPlayerRangeAsync(string leaderboardId, int rangeLimit = 10)
        {
            try
            {
                Debug.Log($"[Leaderboards] Fetching player range for leaderboard: {leaderboardId}");
                
                var options = new GetPlayerRangeOptions
                {
                    RangeLimit = rangeLimit
                };
                
                var scores = await Unity.Services.Leaderboards.LeaderboardsService.Instance.GetPlayerRangeAsync(leaderboardId, options);
                Debug.Log($"[Leaderboards] Retrieved {scores.Results.Count} scores in player range");
                
                return scores;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Leaderboards] Failed to get player range: {e.Message}");
                throw;
            }
        }

        public async Task<List<string>> GetLeaderboardIdsAsync()
        {
            try
            {
                Debug.Log("[Leaderboards] Fetching leaderboard IDs...");
                
                // Note: This may require additional configuration depending on your setup
                // You might need to maintain a local list of leaderboard IDs
                
                await Task.CompletedTask;
                return new List<string>();
            }
            catch (Exception e)
            {
                Debug.LogError($"[Leaderboards] Failed to get leaderboard IDs: {e.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets the current player's score from the specified leaderboard.
        /// </summary>
        /// <param name="leaderboardId">The ID of the leaderboard to query</param>
        /// <returns>The player's leaderboard entry with score and rank</returns>
        public async Task<LeaderboardEntry> GetMyScoreAsync(string leaderboardId)
        {
            try
            {
                Debug.Log($"[Leaderboards] Fetching my score from leaderboard: {leaderboardId}");
                
                var entry = await Unity.Services.Leaderboards.LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboardId);
                Debug.Log($"[Leaderboards] My rank: {entry.Rank}, Score: {entry.Score}");
                
                return entry;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Leaderboards] Failed to get my score: {e.Message}");
                throw;
            }
        }

        /// <summary>
        /// Submits a score to the specified leaderboard.
        /// </summary>
        /// <param name="leaderboardId">The ID of the leaderboard to submit to</param>
        /// <param name="score">The score to submit</param>
        /// <returns>The updated leaderboard entry with new rank</returns>
        public async Task<LeaderboardEntry> SubmitScoreAsync(string leaderboardId, double score)
        {
            try
            {
                Debug.Log($"[Leaderboards] Submitting score to leaderboard: {leaderboardId}, Score: {score}");
                
                var result = await Unity.Services.Leaderboards.LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score);
                Debug.Log($"[Leaderboards] Score submitted successfully. New rank: {result.Rank}");
                
                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Leaderboards] Failed to submit score: {e.Message}");
                throw;
            }
        }
    }
}
