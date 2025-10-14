# Cloud Code Implementation Guide

## Overview

This directory contains Cloud Code scripts that run server-side for secure game logic processing. The scripts integrate with Unity Gaming Services (Cloud Save, Leaderboards) to manage player data.

## FinalizeOwareMatch.js

### Purpose
Calculates and updates ELO ratings for players after an Oware match ends.

### Features
- **Server-side ELO calculation** - Prevents client-side manipulation
- **Default rating**: 1200 for new players
- **K-factor**: 32 (standard for competitive games)
- **Cloud Save integration** - Stores player ELO ratings persistently
- **Leaderboard integration** - Automatically updates the leaderboard
- **Draw support** - Handles both win/loss and draw scenarios

### Parameters
```javascript
{
  winnerId: string,  // Unity Player ID of the winner
  loserId: string,   // Unity Player ID of the loser
  isDraw: boolean    // Optional, defaults to false
}
```

### Return Value
```javascript
{
  WinnerId: string,
  LoserId: string,
  WinnerOldRating: number,
  LoserOldRating: number,
  WinnerNewRating: number,
  LoserNewRating: number,
  WinnerRatingChange: number,
  LoserRatingChange: number,
  IsDraw: boolean
}
```

### ELO Calculation Formula
The script uses the standard ELO rating system:

1. **Expected Score**: `E = 1 / (1 + 10^((opponentRating - playerRating) / 400))`
2. **New Rating**: `NewRating = OldRating + K * (ActualScore - ExpectedScore)`
   - ActualScore = 1.0 for win, 0.5 for draw, 0.0 for loss
   - K-factor = 32

### Dependencies
- `@unity-services/cloud-save-1.3`
- `@unity-services/leaderboards-1.0`

### Setup Instructions

#### 1. Upload to Unity Dashboard
1. Go to [Unity Cloud Dashboard](https://cloud.unity.com/)
2. Select your project
3. Navigate to **Cloud Code** → **Scripts**
4. Click **New Script** → **Module**
5. Name the module: `OwareMatch`
6. Name the function: `FinalizeOwareMatch`
7. Copy and paste the content from `FinalizeOwareMatch.js`
8. Click **Deploy**

#### 2. Configure Leaderboard
1. In Unity Dashboard, go to **Leaderboards**
2. Create a new leaderboard with ID: `oware-elo-ratings`
3. Set the sort order to **Descending** (higher ELO = better rank)
4. Configure update type as needed (e.g., "Keep Best")

#### 3. Test the Integration
Use the CloudCodeClient in Unity:
```csharp
var cloudCodeClient = new CloudCodeClient();
var result = await cloudCodeClient.FinalizeOwareMatchAsync(
    winnerId: "player1-id",
    loserId: "player2-id",
    isDraw: false
);

Debug.Log($"Winner's new rating: {result.WinnerNewRating}");
Debug.Log($"Loser's new rating: {result.LoserNewRating}");
```

## Usage Example in Game Code

### After Match Completion
```csharp
using Project.Services;

public class MatchManager : MonoBehaviour
{
    private CloudCodeClient cloudCodeClient;
    private LeaderboardsService leaderboardsService;

    private async void OnMatchEnd(string winnerId, string loserId, bool isDraw)
    {
        try
        {
            // Step 1: Calculate ELO on server
            var eloResult = await cloudCodeClient.FinalizeOwareMatchAsync(
                winnerId, 
                loserId, 
                isDraw
            );

            // Step 2: Display results to players
            Debug.Log($"Match finalized!");
            Debug.Log($"Winner: {eloResult.WinnerNewRating} ({eloResult.WinnerRatingChange:+#;-#;0})");
            Debug.Log($"Loser: {eloResult.LoserNewRating} ({eloResult.LoserRatingChange:+#;-#;0})");

            // Step 3: Optionally fetch updated leaderboard standings
            var myScore = await leaderboardsService.GetMyScoreAsync("oware-elo-ratings");
            Debug.Log($"Your current rank: {myScore.Rank}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to finalize match: {e.Message}");
        }
    }
}
```

## Security Considerations

- **Server-side validation**: All ELO calculations happen server-side
- **Input validation**: Script validates winnerId ≠ loserId
- **Error handling**: Graceful fallbacks for missing data
- **Leaderboard resilience**: Continues even if leaderboard submission fails

## Customization

### Adjusting K-Factor
Modify the `K_FACTOR` constant in the script:
- **16**: For established players (less rating volatility)
- **32**: Standard for most games (balanced)
- **64**: For new players (more rating volatility)

### Changing Default Rating
Modify the `DEFAULT_RATING` constant:
```javascript
const DEFAULT_RATING = 1500;  // Change from 1200 to 1500
```

### Using Different Leaderboard
Modify the `LEADERBOARD_ID` constant:
```javascript
const LEADERBOARD_ID = "your-custom-leaderboard-id";
```

## Troubleshooting

### Common Issues

1. **"Module not found" error**
   - Ensure the script is deployed in Unity Dashboard
   - Check that the module name is `OwareMatch`

2. **"Leaderboard not found" error**
   - Create the leaderboard in Unity Dashboard with ID `oware-elo-ratings`
   - Verify the leaderboard ID matches the constant in the script

3. **Cloud Save data not persisting**
   - Ensure Cloud Save is enabled in your Unity project
   - Check that the player is authenticated before calling the function

4. **Invalid player IDs**
   - Verify both players are authenticated with Unity Authentication
   - Use `AuthenticationService.Instance.PlayerId` to get valid player IDs

## Next Steps

- Implement UI to display ELO ratings and rating changes
- Add match history tracking
- Implement seasonal rankings/resets
- Add anti-cheat measures for match reporting
- Create analytics for rating distribution
