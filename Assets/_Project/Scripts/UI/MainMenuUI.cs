using System;
using UnityEngine;
using UnityEngine.UI;
using Project.Services;

namespace Project.UI
{
    /// <summary>
    /// Main menu UI controller that handles Quick Play functionality.
    /// Attach this to a GameObject in the main menu scene and hook up UI elements in the inspector.
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Button quickPlayButton;
        [SerializeField] private Text statusText;

        private void Start()
        {

            // Hook up button event
            if (quickPlayButton != null)
            {
                quickPlayButton.onClick.AddListener(OnQuickPlayButtonClicked);
            }

            UpdateStatus("Ready to play");
        }

        /// <summary>
        /// Called when the Quick Play button is clicked.
        /// Tries to quick join a lobby, falls back to creating and hosting on failure.
        /// </summary>
        public async void OnQuickPlayButtonClicked()
        {
            Debug.Log("[MainMenuUI] Quick Play button clicked");
            
            if (SessionsService.I == null)
            {
                UpdateStatus("Services not initialized");
                Debug.LogError("[MainMenuUI] SessionsService not available");
                return;
            }

            // Disable button during operation
            if (quickPlayButton != null)
            {
                quickPlayButton.interactable = false;
            }

            UpdateStatus("Searching for match...");

            try
            {
                // Step 1: Try to quick join an existing session
                Debug.Log("[MainMenuUI] Attempting to quick join session...");
                await SessionsService.I.QuickJoinAsync();
                
                UpdateStatus("Joined match! Starting game...");
                Debug.Log("[MainMenuUI] Successfully joined match");
            }
            catch (Exception e)
            {
                // Step 2: If quick join fails, create and host a new session
                Debug.Log($"[MainMenuUI] Quick join failed: {e.Message}. Creating new session...");
                UpdateStatus("Creating new match...");

                try
                {
                    await SessionsService.I.HostSessionAsync();
                    UpdateStatus("Waiting for opponent...");
                    Debug.Log("[MainMenuUI] Successfully created and hosting match");
                }
                catch (Exception hostException)
                {
                    UpdateStatus($"Failed to create match: {hostException.Message}");
                    Debug.LogError($"[MainMenuUI] Failed to create and host: {hostException}");
                    
                    // Re-enable button on failure
                    if (quickPlayButton != null)
                    {
                        quickPlayButton.interactable = true;
                    }
                    return;
                }
            }

            // Note: Button will be re-enabled when returning to menu or through game flow
        }

        private void UpdateStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
            Debug.Log($"[MainMenuUI] Status: {message}");
        }

        private void OnDestroy()
        {
            // Clean up button listener
            if (quickPlayButton != null)
            {
                quickPlayButton.onClick.RemoveListener(OnQuickPlayButtonClicked);
            }
        }
    }
}
