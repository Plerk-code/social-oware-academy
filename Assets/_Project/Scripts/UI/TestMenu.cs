using UnityEngine;
using UnityEngine.UI;
using Project.Services;

namespace Project.UI
{
    /// <summary>
    /// Simple test menu for demonstrating AuthService and AnalyticsService functionality.
    /// Attach this to a GameObject and hook up the buttons in the inspector.
    /// </summary>
    public class TestMenu : MonoBehaviour
    {
        [Header("Service References")]
        [SerializeField] private UgsInitializer ugsInitializer;

        [Header("UI Elements (Optional - for visual feedback)")]
        [SerializeField] private Text statusText;

        private AuthService authService;
        private AnalyticsService analyticsService;

        private void Start()
        {
            // Get service references from UgsInitializer if available
            if (ugsInitializer != null)
            {
                // Get already-initialized services from UgsInitializer
                authService = ugsInitializer.GetAuthService();
                analyticsService = ugsInitializer.GetAnalyticsService();
            }
            else
            {
                Debug.LogWarning("[TestMenu] UgsInitializer not assigned. Services may not be initialized.");
            }

            UpdateStatus("Test Menu Ready");
        }

        #region Auth Service Test Methods

        /// <summary>
        /// Call this from a UI button to test anonymous login.
        /// </summary>
        public async void OnLoginAnonymouslyButton()
        {
            Debug.Log("[TestMenu] Login Anonymously button clicked");
            UpdateStatus("Logging in anonymously...");

            try
            {
                if (authService == null)
                {
                    UpdateStatus("Auth service not available. Assign UgsInitializer.");
                    return;
                }

                await authService.SignInAnonymouslyAsync();
                
                UpdateStatus($"Signed in! Player ID: {authService.PlayerId}");
                
                // Log analytics event
                if (analyticsService != null)
                {
                    analyticsService.LogUserSignIn("anonymous");
                }
            }
            catch (System.Exception e)
            {
                UpdateStatus($"Login failed: {e.Message}");
                Debug.LogError($"[TestMenu] Login failed: {e}");
            }
        }

        /// <summary>
        /// Call this from a UI button to test email/password login stub.
        /// </summary>
        public async void OnLoginEmailPasswordButton()
        {
            Debug.Log("[TestMenu] Login with Email/Password button clicked");
            UpdateStatus("Logging in with email/password...");

            try
            {
                if (authService == null)
                {
                    UpdateStatus("Auth service not available. Assign UgsInitializer.");
                    return;
                }

                // Using dummy credentials for testing the stub
                await authService.SignInWithEmailPasswordAsync("test@example.com", "password123");
                
                UpdateStatus($"Signed in! Player ID: {authService.PlayerId}");
                
                // Log analytics event
                if (analyticsService != null)
                {
                    analyticsService.LogUserSignIn("email");
                }
            }
            catch (System.Exception e)
            {
                UpdateStatus($"Login failed: {e.Message}");
                Debug.LogError($"[TestMenu] Login failed: {e}");
            }
        }

        /// <summary>
        /// Call this from a UI button to sign out.
        /// </summary>
        public void OnSignOutButton()
        {
            Debug.Log("[TestMenu] Sign Out button clicked");
            UpdateStatus("Signing out...");

            try
            {
                if (authService != null)
                {
                    authService.SignOut();
                    UpdateStatus("Signed out successfully");
                    
                    // Log analytics event
                    if (analyticsService != null)
                    {
                        analyticsService.LogUserSignOut();
                    }
                }
                else
                {
                    UpdateStatus("Auth service not initialized");
                }
            }
            catch (System.Exception e)
            {
                UpdateStatus($"Sign out failed: {e.Message}");
                Debug.LogError($"[TestMenu] Sign out failed: {e}");
            }
        }

        #endregion

        #region Analytics Service Test Methods

        /// <summary>
        /// Call this from a UI button to log app start event.
        /// </summary>
        public void OnLogAppStartButton()
        {
            Debug.Log("[TestMenu] Log App Start button clicked");
            UpdateStatus("Logging app start event...");

            try
            {
                if (analyticsService == null)
                {
                    UpdateStatus("Analytics service not available. Assign UgsInitializer.");
                    return;
                }

                analyticsService.LogAppStart();
                UpdateStatus("App start event logged");
            }
            catch (System.Exception e)
            {
                UpdateStatus($"Failed to log event: {e.Message}");
                Debug.LogError($"[TestMenu] Failed to log app start: {e}");
            }
        }

        /// <summary>
        /// Call this from a UI button to log a game start event.
        /// </summary>
        public void OnLogGameStartButton()
        {
            Debug.Log("[TestMenu] Log Game Start button clicked");
            UpdateStatus("Logging game start event...");

            try
            {
                if (analyticsService == null)
                {
                    UpdateStatus("Analytics service not available. Assign UgsInitializer.");
                    return;
                }

                analyticsService.RecordGameStart();
                UpdateStatus("Game start event logged");
            }
            catch (System.Exception e)
            {
                UpdateStatus($"Failed to log event: {e.Message}");
                Debug.LogError($"[TestMenu] Failed to log game start: {e}");
            }
        }

        /// <summary>
        /// Call this from a UI button to log a button click event.
        /// </summary>
        public void OnLogButtonClickButton()
        {
            Debug.Log("[TestMenu] Log Button Click button clicked");
            UpdateStatus("Logging button click event...");

            try
            {
                if (analyticsService == null)
                {
                    UpdateStatus("Analytics service not available. Assign UgsInitializer.");
                    return;
                }

                analyticsService.LogButtonClick("test_button");
                UpdateStatus("Button click event logged");
            }
            catch (System.Exception e)
            {
                UpdateStatus($"Failed to log event: {e.Message}");
                Debug.LogError($"[TestMenu] Failed to log button click: {e}");
            }
        }

        /// <summary>
        /// Call this from a UI button to log a screen view event.
        /// </summary>
        public void OnLogScreenViewButton()
        {
            Debug.Log("[TestMenu] Log Screen View button clicked");
            UpdateStatus("Logging screen view event...");

            try
            {
                if (analyticsService == null)
                {
                    UpdateStatus("Analytics service not available. Assign UgsInitializer.");
                    return;
                }

                analyticsService.LogScreenView("test_menu");
                UpdateStatus("Screen view event logged");
            }
            catch (System.Exception e)
            {
                UpdateStatus($"Failed to log event: {e.Message}");
                Debug.LogError($"[TestMenu] Failed to log screen view: {e}");
            }
        }

        #endregion

        #region Helper Methods

        private void UpdateStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
            Debug.Log($"[TestMenu] {message}");
        }

        /// <summary>
        /// Check current auth status - can be called from a button.
        /// </summary>
        public void OnCheckAuthStatusButton()
        {
            if (authService != null && authService.IsSignedIn)
            {
                UpdateStatus($"Signed in as: {authService.PlayerId}");
            }
            else
            {
                UpdateStatus("Not signed in");
            }
        }

        #endregion
    }
}
