# Multiplayer Services Migration Guide

## Overview

This document outlines the migration from deprecated Unity Lobby/Relay SDKs to the new Multiplayer Services (MPS) Sessions API.

## Changes Summary

### 1. Package Updates

**Removed Packages:**
- `com.unity.services.lobby` (v2.1.2) - DEPRECATED
- `com.unity.services.relay` (v1.3.2) - DEPRECATED

**Retained Package:**
- `com.unity.services.multiplayer` (v1.1.8) - Using new Sessions API

### 2. Code Changes

#### LobbyAndRelayService.cs → SessionsService

The service class has been completely rewritten:

**Old Pattern (Class-based Service):**
```csharp
public class LobbyAndRelayService
{
    public async Task Initialize() { ... }
    public async Task<string> CreateAndHostAsync() { ... }
    public async Task QuickJoinAsClientAsync() { ... }
    public async Task LeaveLobbyAsync() { ... }
}
```

**New Pattern (MonoBehaviour Singleton):**
```csharp
public class SessionsService : MonoBehaviour
{
    public static SessionsService I;
    
    void Awake() { I = this; DontDestroyOnLoad(gameObject); }
    
    public async Task HostSessionAsync() { ... }
    public async Task JoinSessionAsync(string joinCode) { ... }
    public async Task QuickJoinAsync() { ... }
}
```

**Key Differences:**
- Now a MonoBehaviour with singleton pattern (accessed via `SessionsService.I`)
- Handles service initialization internally via `EnsureServicesAsync()`
- No separate `Initialize()` method needed
- Simplified API: `QuickJoinAsync()` instead of `QuickJoinAsClientAsync()`
- Uses `SessionOptions` with `.WithRelayNetwork()` for relay integration

### 3. Using Statements Changes

**Old (Deprecated):**
```csharp
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
```

**New:**
```csharp
using Unity.Services.Multiplayer;
```

### 4. Integration Changes

#### UgsInitializer.cs

**Removed:**
- `LobbyAndRelayService` field
- Service instantiation and initialization
- `GetLobbyAndRelayService()` method

**Added Comment:**
```csharp
// Note: SessionsService is a MonoBehaviour singleton and initializes itself
// Access it via SessionsService.I
```

#### MainMenuUI.cs

**Before:**
```csharp
[SerializeField] private UgsInitializer ugsInitializer;
private LobbyAndRelayService lobbyAndRelayService;

private void Start()
{
    lobbyAndRelayService = ugsInitializer.GetLobbyAndRelayService();
}

await lobbyAndRelayService.QuickJoinAsClientAsync();
await lobbyAndRelayService.CreateAndHostAsync();
```

**After:**
```csharp
// No need for UgsInitializer reference for SessionsService
// No need for lobbyAndRelayService field

private void Start()
{
    // SessionsService accessed directly
}

await SessionsService.I.QuickJoinAsync();
await SessionsService.I.HostSessionAsync();
```

## Migration Steps

### Step 1: Update Packages ✅
Packages have been updated in `Packages/manifest.json`:
- Removed `com.unity.services.lobby`
- Removed `com.unity.services.relay`
- Kept `com.unity.services.multiplayer`

### Step 2: Update SessionsService ✅
The file `Assets/_Project/Scripts/Services/LobbyAndRelayService.cs` has been replaced with the new SessionsService implementation.

### Step 3: Update Code References ✅
All code references have been updated:
- `UgsInitializer.cs` - Removed LobbyAndRelayService instantiation
- `MainMenuUI.cs` - Updated to use SessionsService.I singleton
- Documentation files updated with new API references

### Step 4: Unity Setup Required ⚠️

**IMPORTANT: You must complete these steps in Unity:**

1. **Add SessionsService GameObject:**
   - In the Boot scene, create a new GameObject
   - Name it "SessionsService"
   - Add the `SessionsService` component
   - The component will automatically call `DontDestroyOnLoad` in Awake

2. **Remove UgsInitializer Reference from MainMenuUI:**
   - Open the Main Menu scene
   - Select the GameObject with MainMenuUI component
   - The `UgsInitializer` field is no longer needed (remove reference)

3. **Verify Compilation:**
   - Unity will automatically detect the package changes
   - Wait for Unity to recompile
   - Check for any compilation errors in the Console

## API Comparison

### Quick Join

**Old API:**
```csharp
await lobbyAndRelayService.QuickJoinAsClientAsync();
```

**New API:**
```csharp
await SessionsService.I.QuickJoinAsync();
```

### Host Session

**Old API:**
```csharp
string joinCode = await lobbyAndRelayService.CreateAndHostAsync();
```

**New API:**
```csharp
await SessionsService.I.HostSessionAsync();
// Join code is logged, access via session.Code if needed
```

### Join by Code

**Old API:**
```csharp
// Not previously implemented
```

**New API:**
```csharp
await SessionsService.I.JoinSessionAsync(joinCode);
```

## Benefits of New API

1. **Simplified Architecture:**
   - No need to manually instantiate and manage service lifecycle
   - MonoBehaviour singleton pattern is more Unity-friendly
   - Automatic service initialization

2. **Cleaner Code:**
   - Single namespace import (`Unity.Services.Multiplayer`)
   - More intuitive method names
   - Built-in authentication handling

3. **Better Integration:**
   - Relay network integration via `.WithRelayNetwork()`
   - Automatic session management
   - Simplified error handling

4. **Future-Proof:**
   - Uses officially supported Sessions API
   - No deprecated package dependencies
   - Better Unity support and documentation

## Testing Checklist

After migration, verify the following:

- [ ] Unity compiles without errors
- [ ] SessionsService GameObject exists in Boot scene
- [ ] SessionsService.I is accessible from other scripts
- [ ] Quick Play button successfully creates or joins sessions
- [ ] Session join codes are logged correctly
- [ ] Network connection establishes properly
- [ ] Both host and client can connect and play

## Troubleshooting

### "SessionsService.I is null"
**Solution:** Ensure you've added the SessionsService component to a GameObject in the Boot scene.

### "Unity.Services.Multiplayer namespace not found"
**Solution:** 
1. Check that `com.unity.services.multiplayer` is in manifest.json
2. Force Unity to reimport packages (Assets > Reimport All)
3. Restart Unity Editor

### Quick Join/Host fails
**Solution:**
1. Check Unity Dashboard - ensure Multiplayer Services are enabled
2. Verify authentication is working (check logs for player ID)
3. Ensure NetworkManager is properly configured

### Cannot find deprecated Lobby/Relay types
**Solution:** This is expected - those packages have been removed. Use the new Sessions API instead.

## Additional Resources

- [Unity Multiplayer Services Documentation](https://docs.unity.com/ugs/manual/multiplayer/manual/welcome)
- [Sessions API Guide](https://docs.unity.com/ugs/manual/multiplayer/manual/sessions)
- [Migration Guide](https://docs.unity.com/ugs/manual/multiplayer/manual/migrating-to-sessions)

## Support

If you encounter issues:
1. Check the Unity Console for detailed error messages
2. Enable Debug logging in SessionsService
3. Verify all Unity Gaming Services are properly configured
4. Consult Unity's official migration documentation

## Status

✅ **Migration Complete - Unity Setup Required**

All code changes have been completed. You must now:
1. Open Unity
2. Wait for package resolution and compilation
3. Add SessionsService GameObject to Boot scene
4. Test the Quick Play functionality
