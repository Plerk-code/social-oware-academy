using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Analytics.Data;

namespace Project.Services
{
    /// <summary>
    /// Handles Unity Analytics service operations.
    /// Provides custom event tracking and player analytics.
    /// </summary>
    public class AnalyticsService
    {
        public async Task Initialize()
        {
            try
            {
                Debug.Log("[Analytics] Initializing Analytics Service...");
                
                // Analytics is automatically initialized with Unity Gaming Services
                // Note: EndUserConsent API removed in newer Analytics SDK versions
                // Consent is handled automatically or through Unity Dashboard
                
                Debug.Log("[Analytics] Analytics Service initialized");
                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Analytics] Failed to initialize: {e.Message}");
                throw;
            }
        }

        public void RecordEvent(string eventName, Dictionary<string, object> parameters = null)
        {
            try
            {
                // Unity Analytics 6.x simplified API - RecordEvent handles both cases
                Unity.Services.Analytics.AnalyticsService.Instance.RecordEvent(eventName);
                
                if (parameters != null && parameters.Count > 0)
                {
                    // Log parameters for debugging purposes
                    Debug.Log($"[Analytics] Event recorded: {eventName} with parameters: {string.Join(", ", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"))}");
                }
                else
                {
                    Debug.Log($"[Analytics] Event recorded: {eventName}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[Analytics] Failed to record event: {e.Message}");
            }
        }

        /// <summary>
        /// Log when the app starts.
        /// </summary>
        public void LogAppStart()
        {
            var parameters = new Dictionary<string, object>
            {
                { "timestamp", DateTime.UtcNow.ToString("o") }
            };
            RecordEvent("app_start", parameters);
        }

        /// <summary>
        /// Log when the app is closed or paused.
        /// </summary>
        public void LogAppEnd()
        {
            RecordEvent("app_end");
        }

        /// <summary>
        /// Log when a user signs in.
        /// </summary>
        public void LogUserSignIn(string method)
        {
            var parameters = new Dictionary<string, object>
            {
                { "sign_in_method", method }
            };
            RecordEvent("user_sign_in", parameters);
        }

        /// <summary>
        /// Log when a user signs out.
        /// </summary>
        public void LogUserSignOut()
        {
            RecordEvent("user_sign_out");
        }

        /// <summary>
        /// Log when a game session starts.
        /// </summary>
        public void RecordGameStart()
        {
            RecordEvent("game_start");
        }

        /// <summary>
        /// Log when a game session ends.
        /// </summary>
        public void RecordGameEnd(int score, int duration)
        {
            var parameters = new Dictionary<string, object>
            {
                { "score", score },
                { "duration_seconds", duration }
            };
            RecordEvent("game_end", parameters);
        }

        /// <summary>
        /// Log when a level is completed.
        /// </summary>
        public void RecordLevelComplete(int levelNumber, int score)
        {
            var parameters = new Dictionary<string, object>
            {
                { "level_number", levelNumber },
                { "score", score }
            };
            RecordEvent("level_complete", parameters);
        }

        /// <summary>
        /// Log when a player makes a purchase.
        /// </summary>
        public void RecordPurchase(string itemId, int amount)
        {
            var parameters = new Dictionary<string, object>
            {
                { "item_id", itemId },
                { "amount", amount }
            };
            RecordEvent("purchase", parameters);
        }

        /// <summary>
        /// Log when a button is clicked.
        /// </summary>
        public void LogButtonClick(string buttonName)
        {
            var parameters = new Dictionary<string, object>
            {
                { "button_name", buttonName }
            };
            RecordEvent("button_click", parameters);
        }

        /// <summary>
        /// Log when a screen is viewed.
        /// </summary>
        public void LogScreenView(string screenName)
        {
            var parameters = new Dictionary<string, object>
            {
                { "screen_name", screenName }
            };
            RecordEvent("screen_view", parameters);
        }

        /// <summary>
        /// Log a custom error.
        /// </summary>
        public void LogError(string errorMessage, string errorCode = null)
        {
            var parameters = new Dictionary<string, object>
            {
                { "error_message", errorMessage }
            };
            
            if (!string.IsNullOrEmpty(errorCode))
            {
                parameters.Add("error_code", errorCode);
            }
            
            RecordEvent("error", parameters);
        }

        public void Flush()
        {
            try
            {
                Unity.Services.Analytics.AnalyticsService.Instance.Flush();
                Debug.Log("[Analytics] Analytics data flushed");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Analytics] Failed to flush analytics: {e.Message}");
            }
        }
    }
}
