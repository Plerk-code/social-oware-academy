# Services Implementation

## Overview
This document describes the AuthService and AnalyticsService implementations for Unity Gaming Services integration.

## AuthService

### Features Implemented
- **Anonymous Authentication**: Fully functional sign-in using Unity Authentication
- **Email/Password Authentication**: Stub implementation (ready for future enhancement)
  - `SignUpWithEmailPasswordAsync(email, password)` - Currently falls back to anonymous sign-in
  - `SignInWithEmailPasswordAsync(email, password)` - Currently falls back to anonymous sign-in
- **Sign Out**: Full functionality to sign out current user
- **Event Handlers**: Automatic logging of sign-in, sign-out, failures, and session expiration

### Methods
- `Initialize()` - Sets up authentication service and event handlers
- `SignInAnonymouslyAsync()` - Signs in anonymously
- `SignUpWithEmailPasswordAsync(email, password)` - Stub for email/password sign-up
- `SignInWithEmailPasswordAsync(email, password)` - Stub for email/password sign-in
- `SignOut()` - Signs out the current user
- `DeleteAccountAsync()` - Deletes the current account

### Properties
- `PlayerId` - Returns the current player's ID
- `IsSignedIn` - Returns whether a user is currently signed in

### Usage Example
```csharp
var authService = ugsInitializer.GetAuthService();

// Anonymous login
await authService.SignInAnonymouslyAsync();

// Email/Password login (stub - currently falls back to anonymous)
await authService.SignInWithEmailPasswordAsync("test@example.com", "password");

// Sign out
authService.SignOut();

// Check status
if (authService.IsSignedIn)
{
    Debug.Log($"Player ID: {authService.PlayerId}");
}
```

## AnalyticsService

### Features Implemented
- **Core Event Tracking**: Generic event recording with custom parameters
- **App Lifecycle Events**: LogAppStart(), LogAppEnd()
- **Authentication Events**: LogUserSignIn(), LogUserSignOut()
- **Game Session Events**: RecordGameStart(), RecordGameEnd()
- **Level Progress**: RecordLevelComplete()
- **Economy Events**: RecordPurchase()
- **UI Tracking**: LogButtonClick(), LogScreenView()
- **Error Logging**: LogError()

### Methods

#### Lifecycle Methods
- `Initialize()` - Initializes analytics and starts data collection
- `LogAppStart()` - Logs when the application starts
- `LogAppEnd()` - Logs when the application ends or is paused

#### User Events
- `LogUserSignIn(method)` - Logs user sign-in with authentication method
- `LogUserSignOut()` - Logs user sign-out

#### Game Events
- `RecordGameStart()` - Logs game session start
- `RecordGameEnd(score, duration)` - Logs game session end with metrics
- `RecordLevelComplete(levelNumber, score)` - Logs level completion

#### Economy Events
- `RecordPurchase(itemId, amount)` - Logs in-game purchases

#### UI Events
- `LogButtonClick(buttonName)` - Logs button interactions
- `LogScreenView(screenName)` - Logs screen/menu views

#### Error Tracking
- `LogError(errorMessage, errorCode)` - Logs custom errors

#### Utility Methods
- `RecordEvent(eventName, parameters)` - Generic event recording
- `Flush()` - Forces analytics data to be sent immediately

### Usage Example
```csharp
var analyticsService = ugsInitializer.GetAnalyticsService();

// Log app start
analyticsService.LogAppStart();

// Log user actions
analyticsService.LogUserSignIn("anonymous");
analyticsService.LogButtonClick("start_game");
analyticsService.LogScreenView("main_menu");

// Log game events
analyticsService.RecordGameStart();
analyticsService.RecordGameEnd(score: 1500, duration: 180);
analyticsService.RecordLevelComplete(levelNumber: 1, score: 500);

// Log purchases
analyticsService.RecordPurchase("gold_pack_100", amount: 100);

// Log errors
analyticsService.LogError("Connection timeout", "NET001");

// Custom events
var customParams = new Dictionary<string, object>
{
    { "player_level", 5 },
    { "zone", "forest" }
};
analyticsService.RecordEvent("custom_event", customParams);
```

## TestMenu Component

A UI component is provided for testing the services: `Assets/_Project/Scripts/UI/TestMenu.cs`

### Setup Instructions

1. **Add TestMenu to Scene**:
   - Create a Canvas in your scene if one doesn't exist
   - Add an empty GameObject under the Canvas
   - Attach the `TestMenu` component to it

2. **Create UI Buttons**:
   Create buttons in the Canvas and assign these methods to their onClick events:
   - **Authentication Buttons**:
     - `OnLoginAnonymouslyButton()` - Test anonymous login
     - `OnLoginEmailPasswordButton()` - Test email/password login stub
     - `OnSignOutButton()` - Test sign out
     - `OnCheckAuthStatusButton()` - Check current auth status
   
   - **Analytics Buttons**:
     - `OnLogAppStartButton()` - Log app start event
     - `OnLogGameStartButton()` - Log game start event
     - `OnLogButtonClickButton()` - Log button click event
     - `OnLogScreenViewButton()` - Log screen view event

3. **Assign References**:
   - In the Inspector, assign the `UgsInitializer` reference
   - Optionally assign a Text component for status feedback

### Example Button Setup
```
Button: "Login Anonymously"
  OnClick() -> TestMenu.OnLoginAnonymouslyButton()

Button: "Log App Start"
  OnClick() -> TestMenu.OnLogAppStartButton()
```

## Integration with UgsInitializer

Both services are automatically initialized by the `UgsInitializer` component:

```csharp
// Services are initialized on Start()
// Access them through the initializer
var authService = ugsInitializer.GetAuthService();
var analyticsService = ugsInitializer.GetAnalyticsService();
```

## Next Steps

### Email/Password Authentication
To implement full email/password authentication:

1. Enable Username/Password authentication in Unity Dashboard
2. Update `SignUpWithEmailPasswordAsync()`:
   ```csharp
   await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(email, password);
   ```
3. Update `SignInWithEmailPasswordAsync()`:
   ```csharp
   await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(email, password);
   ```
4. Remove the fallback to anonymous sign-in
5. Add proper error handling and validation

### Analytics Enhancement
- Add more game-specific events
- Implement custom conversion funnels
- Add A/B testing support
- Create analytics dashboard visualization

## Notes
- All services require Unity Gaming Services to be properly configured in the Unity Dashboard
- Analytics events are queued and sent in batches
- Use `Flush()` when you need to ensure events are sent immediately (e.g., before app closes)
- Authentication is required before most other services can be used
