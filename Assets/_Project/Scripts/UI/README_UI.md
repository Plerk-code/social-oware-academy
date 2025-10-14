# UI Components Documentation

This directory contains UI components for the Oware game.

## Overview

The UI system is divided into several components that handle different aspects of the game flow:

1. **MainMenuUI** - Handles Quick Play matchmaking
2. **ResultsUI** - Handles match result display and ELO updates
3. **TestMenu** - Testing and debugging interface
4. **StoreUI** - In-game store interface

## MainMenuUI

### Purpose
Handles the main menu Quick Play functionality with automatic matchmaking.

### Features
- **Quick Play Button**: Attempts to join an existing lobby, falls back to creating a new one if none available
- **Status Display**: Shows current matchmaking status to the player

### Setup
1. Attach `MainMenuUI` to a GameObject in your main menu scene
2. Assign the following in the Inspector:
   - `UgsInitializer`: Reference to the UgsInitializer component (usually in Boot scene, persists via DontDestroyOnLoad)
   - `quickPlayButton`: The UI Button for Quick Play
   - `statusText`: A Text component for displaying status messages

### Usage Flow
1. Player clicks Quick Play button
2. System attempts to quick join an existing session using `SessionsService.I.QuickJoinAsync()`
3. If quick join fails (no available sessions), automatically falls back to `SessionsService.I.HostSessionAsync()`
4. Player either joins a match as client or waits as host for an opponent

### Example Setup
```csharp
// The button is automatically connected in Start()
// To manually trigger Quick Play:
mainMenuUI.OnQuickPlayButtonClicked();
```

## ResultsUI

### Purpose
Displays match results and handles backend integration for ELO calculation and leaderboard updates.

### Features
- **Result Display**: Shows win/loss/draw outcome
- **ELO Calculation**: Calls Cloud Code to calculate new ELO ratings
- **Leaderboard Update**: Submits new ELO to leaderboards
- **Rating Change Display**: Shows ELO change (+/- value)

### Setup
1. Attach `ResultsUI` to a GameObject in your results/end game scene
2. Assign the following in the Inspector:
   - `UgsInitializer`: Reference to the UgsInitializer component
   - `resultText`: Text component for "Victory!", "Defeat", or "Draw!"
   - `eloText`: Text component for displaying ELO rating and change
   - `statusText`: Text component for status messages
   - `continueButton`: Button for continuing to next screen (optional)

### Usage Flow
When a match ends, call `ShowResults()` with the match outcome:

```csharp
// Method 1: Using boolean parameters
resultsUI.ShowResults(opponentId, didWin: true, isDraw: false);

// Method 2: Using MatchResult enum (recommended)
resultsUI.ShowResults(opponentId, MatchResult.Win);
```

### MatchResult Enum
```csharp
public enum MatchResult
{
    Win,    // Local player won
    Loss,   // Local player lost
    Draw    // Match ended in a draw
}
```

### Backend Integration
The component automatically:
1. Determines winner and loser based on match result
2. Calls `CloudCodeClient.FinalizeOwareMatchAsync()` to calculate ELO changes
3. Submits new ELO to leaderboard using `LeaderboardsService.SubmitScoreAsync()`

### Leaderboard Configuration
- Default leaderboard ID: `"oware-elo-leaderboard"`
- To change: Modify the `ELO_LEADERBOARD_ID` constant in ResultsUI.cs

## Integration with UgsInitializer

### App Startup Flow
The `UgsInitializer` component now automatically:
1. Initializes all Unity Gaming Services
2. Signs in anonymously via `AuthService.SignInAnonymouslyAsync()`
3. Logs an app start event via `AnalyticsService.LogAppStart()`

This happens automatically when the Boot scene loads, so no additional code is needed.

### Accessing Services
All UI components get service references through the UgsInitializer:

```csharp
// In your UI component
[SerializeField] private UgsInitializer ugsInitializer;

private void Start()
{
    var authService = ugsInitializer.GetAuthService();
    // SessionsService is now a MonoBehaviour singleton
    // Access it directly via SessionsService.I
    var analyticsService = ugsInitializer.GetAnalyticsService();
    var cloudCodeClient = ugsInitializer.GetCloudCodeClient();
    var leaderboardsService = ugsInitializer.GetLeaderboardsService();
}
```

## Complete Usage Example

### Scene Setup

**Boot Scene:**
- GameObject with `UgsInitializer` component
- Set to DontDestroyOnLoad

**Main Menu Scene:**
- GameObject with `MainMenuUI` component
- UI Canvas with Quick Play Button
- Status Text for feedback

**Game Scene:**
- Your game logic that determines match outcome

**Results Scene:**
- GameObject with `ResultsUI` component
- UI Canvas with result displays

### Game Flow Integration

```csharp
// In your game manager when match ends:
public class GameManager : MonoBehaviour
{
    [SerializeField] private ResultsUI resultsUI;
    
    private string opponentPlayerId; // Set during matchmaking
    
    private void OnMatchEnd()
    {
        // Determine the result
        MatchResult result;
        
        if (localPlayerScore > opponentScore)
            result = MatchResult.Win;
        else if (localPlayerScore < opponentScore)
            result = MatchResult.Loss;
        else
            result = MatchResult.Draw;
        
        // Show results and handle backend
        resultsUI.ShowResults(opponentPlayerId, result);
    }
}
```

## Error Handling

All UI components include comprehensive error handling:
- Service availability checks
- Try-catch blocks for async operations
- Debug logging for troubleshooting
- User-friendly status messages

### Debugging
Enable Debug logs in Unity Console to see detailed operation flows:
- `[MainMenuUI]` - Quick Play operations
- `[ResultsUI]` - Match finalization and ELO updates
- `[UGS]` - Service initialization and startup

## Requirements

### Unity Packages
- Unity Gaming Services Core
- Unity Authentication
- Unity Lobbies
- Unity Relay
- Unity Netcode for GameObjects
- Unity Cloud Code
- Unity Leaderboards
- Unity Analytics

### Backend Configuration
1. Cloud Code module "OwareMatch" with "FinalizeOwareMatch" endpoint
2. Leaderboard with ID "oware-elo-leaderboard"
3. Cloud Save for storing player ELO ratings

## Notes

- The ELO system uses Cloud Code for server-authoritative calculation
- Anonymous authentication happens automatically on app start
- All async operations are properly awaited
- UI components handle both success and failure scenarios
- NetworkManager singleton must exist for lobby/relay functionality
