using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;

namespace Project.Services
{
    /// <summary>
    /// Handles Unity Authentication service operations.
    /// Provides sign-in, sign-out, and player ID management.
    /// </summary>
    public class AuthService
    {
        public string PlayerId => AuthenticationService.Instance.PlayerId;
        public bool IsSignedIn => AuthenticationService.Instance.IsSignedIn;

        public async Task Initialize()
        {
            try
            {
                Debug.Log("[Auth] Initializing Authentication Service...");
                
                // Set up authentication event handlers
                AuthenticationService.Instance.SignedIn += OnSignedIn;
                AuthenticationService.Instance.SignedOut += OnSignedOut;
                AuthenticationService.Instance.SignInFailed += OnSignInFailed;
                AuthenticationService.Instance.Expired += OnExpired;

                // Automatically sign in anonymously
                await SignInAnonymouslyAsync();
                
                Debug.Log("[Auth] Authentication Service initialized");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Auth] Failed to initialize Authentication Service: {e.Message}");
                throw;
            }
        }

        public async Task SignInAnonymouslyAsync()
        {
            try
            {
                if (AuthenticationService.Instance.IsSignedIn)
                {
                    Debug.Log($"[Auth] Already signed in as {AuthenticationService.Instance.PlayerId}");
                    return;
                }

                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"[Auth] Signed in anonymously as {AuthenticationService.Instance.PlayerId}");
            }
            catch (AuthenticationException ex)
            {
                Debug.LogError($"[Auth] Sign in failed: {ex.Message}");
                throw;
            }
            catch (RequestFailedException ex)
            {
                Debug.LogError($"[Auth] Sign in request failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Sign up with email and password (stub implementation).
        /// </summary>
        public async Task SignUpWithEmailPasswordAsync(string email, string password)
        {
            try
            {
                if (AuthenticationService.Instance.IsSignedIn)
                {
                    Debug.Log($"[Auth] Already signed in as {AuthenticationService.Instance.PlayerId}");
                    return;
                }

                Debug.Log($"[Auth] Attempting to sign up with email: {email}");
                // TODO: Implement actual email/password sign up
                // await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(email, password);
                
                Debug.LogWarning("[Auth] Email/Password sign up is not yet implemented. Use anonymous sign-in instead.");
                await SignInAnonymouslyAsync();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Auth] Email/Password sign up failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Sign in with email and password (stub implementation).
        /// </summary>
        public async Task SignInWithEmailPasswordAsync(string email, string password)
        {
            try
            {
                if (AuthenticationService.Instance.IsSignedIn)
                {
                    Debug.Log($"[Auth] Already signed in as {AuthenticationService.Instance.PlayerId}");
                    return;
                }

                Debug.Log($"[Auth] Attempting to sign in with email: {email}");
                // TODO: Implement actual email/password sign in
                // await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(email, password);
                
                Debug.LogWarning("[Auth] Email/Password sign in is not yet implemented. Use anonymous sign-in instead.");
                await SignInAnonymouslyAsync();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Auth] Email/Password sign in failed: {ex.Message}");
                throw;
            }
        }

        public void SignOut()
        {
            try
            {
                AuthenticationService.Instance.SignOut();
                Debug.Log("[Auth] Signed out successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Auth] Sign out failed: {e.Message}");
            }
        }

        public async Task DeleteAccountAsync()
        {
            try
            {
                await AuthenticationService.Instance.DeleteAccountAsync();
                Debug.Log("[Auth] Account deleted successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Auth] Failed to delete account: {e.Message}");
                throw;
            }
        }

        private void OnSignedIn()
        {
            Debug.Log($"[Auth] Player signed in: {AuthenticationService.Instance.PlayerId}");
        }

        private void OnSignedOut()
        {
            Debug.Log("[Auth] Player signed out");
        }

        private void OnSignInFailed(RequestFailedException exception)
        {
            Debug.LogError($"[Auth] Sign in failed: {exception.Message}");
        }

        private void OnExpired()
        {
            Debug.LogWarning("[Auth] Session expired. Please sign in again.");
        }
    }
}
