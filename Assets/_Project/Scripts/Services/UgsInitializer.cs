using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;

namespace Project.Services
{
    /// <summary>
    /// Initializes Unity Gaming Services and all dependent services.
    /// Attach this to a Bootstrap GameObject in the Boot scene.
    /// </summary>
    public class UgsInitializer : MonoBehaviour
    {
        [SerializeField] private string environment = "production";
        
        private AuthService authService;
        private AnalyticsService analyticsService;
        private CloudCodeClient cloudCodeClient;
        private EconomyService economyService;
        private LeaderboardsService leaderboardsService;

        private async void Start()
        {
            await InitializeUnityGamingServices();
        }

        private async Task InitializeUnityGamingServices()
        {
            try
            {
                Debug.Log("[UGS] Initializing Unity Gaming Services...");

                // Initialize Unity Services with environment
                var options = new InitializationOptions()
                    .SetOption("com.unity.services.core.environment-name", environment);

                await UnityServices.InitializeAsync(options);
                
                Debug.Log($"[UGS] Unity Gaming Services initialized successfully (Environment: {environment})");

                // Initialize individual services
                await InitializeServices();
                
                Debug.Log("[UGS] All services initialized successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"[UGS] Failed to initialize Unity Gaming Services: {e.Message}");
            }
        }

        private async Task InitializeServices()
        {
            // Initialize Auth Service
            authService = new AuthService();
            await authService.Initialize();

            // Initialize Analytics Service
            analyticsService = new AnalyticsService();
            await analyticsService.Initialize();

            // Initialize Economy Service
            economyService = new EconomyService();
            await economyService.Initialize();

            // Initialize Leaderboards Service
            leaderboardsService = new LeaderboardsService();
            await leaderboardsService.Initialize();

            // Initialize Cloud Code Client
            cloudCodeClient = new CloudCodeClient();
            await cloudCodeClient.Initialize();

            // Note: SessionsService is a MonoBehaviour singleton and initializes itself
            // Access it via SessionsService.I

            // Auto-login anonymously on app start
            await LoginAnonymouslyOnStartup();

            // Log app start event
            LogAppStartEvent();
        }

        private async Task LoginAnonymouslyOnStartup()
        {
            try
            {
                Debug.Log("[UGS] Logging in anonymously on app start...");
                await authService.SignInAnonymouslyAsync();
                Debug.Log($"[UGS] Anonymous login successful. Player ID: {authService.PlayerId}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[UGS] Failed to login anonymously on startup: {e.Message}");
            }
        }

        private void LogAppStartEvent()
        {
            try
            {
                Debug.Log("[UGS] Logging app start event...");
                analyticsService.LogAppStart();
                Debug.Log("[UGS] App start event logged successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"[UGS] Failed to log app start event: {e.Message}");
            }
        }

        public AuthService GetAuthService() => authService;
        public AnalyticsService GetAnalyticsService() => analyticsService;
        public CloudCodeClient GetCloudCodeClient() => cloudCodeClient;
        public EconomyService GetEconomyService() => economyService;
        public LeaderboardsService GetLeaderboardsService() => leaderboardsService;
    }
}
