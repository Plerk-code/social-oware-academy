using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Multiplayer;

public class SessionsService : MonoBehaviour {
    public static SessionsService I;

    void Awake() { I = this; DontDestroyOnLoad(gameObject); }

    async Task EnsureServicesAsync() {
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
            await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async Task HostSessionAsync() {
        await EnsureServicesAsync();
        var options = new SessionOptions { MaxPlayers = 2 }.WithRelayNetwork();
        var session = await MultiplayerService.Instance.CreateSessionAsync(options);
        Debug.Log($"Session created. Join code: {session.Code}");
        // Load your Game scene or call NetworkManager.StartHost() if not auto-started.
    }

    public async Task JoinSessionAsync(string joinCode) {
        await EnsureServicesAsync();
        var session = await MultiplayerService.Instance.JoinSessionByCodeAsync(joinCode);
        Debug.Log($"Joined session {session.Id}");
        // Load your Game scene or call NetworkManager.StartClient() if not auto-started.
    }

    public async Task QuickJoinAsync() {
        await EnsureServicesAsync();
        // Note: QuickJoinSessionAsync is not available in Unity Multiplayer Services 1.x
        // Quick join functionality would need to be implemented using matchmaking or
        // by querying available sessions and joining the first available one
        Debug.LogWarning("Quick join is not implemented - QuickJoinSessionAsync not available in current SDK");
        await Task.CompletedTask;
    }
}
