# Unity Gaming Services (UGS) Project

This Unity project is configured with Unity Gaming Services (UGS) scaffolding and bootstrap infrastructure.

## Project Structure

```
Assets/
├── _Project/
│   ├── Scripts/
│   │   ├── Services/         # UGS service wrappers
│   │   │   ├── UgsInitializer.cs
│   │   │   ├── AuthService.cs
│   │   │   ├── LobbyAndRelayService.cs
│   │   │   ├── AnalyticsService.cs
│   │   │   ├── CloudCodeClient.cs
│   │   │   ├── EconomyService.cs
│   │   │   └── LeaderboardsService.cs
│   │   ├── Netcode/          # Netcode-related scripts
│   │   ├── Gameplay/         # Game logic scripts
│   │   └── UI/               # UI controller scripts
│   ├── Scenes/
│   │   └── Boot.unity        # Bootstrap scene (must be first in build)
│   ├── UI/
│   │   ├── UXML/             # UI Toolkit UXML files
│   │   ├── USS/              # UI Toolkit stylesheets
│   │   └── PanelSettings.asset
│   └── Tests/                # Test scripts
└── Scenes/                   # Additional game scenes
```

## Setup Instructions

### 1. Unity Gaming Services Configuration

Before running the project, you need to configure Unity Gaming Services:

1. **Link Project to Unity Dashboard**
   - Open Unity Editor
   - Go to `Edit > Project Settings > Services`
   - Link your project to a Unity Cloud Project ID
   - If you don't have one, create a new organization and project

2. **Enable Required Services**
   - Go to the [Unity Dashboard](https://dashboard.unity3d.com/)
   - Select your project
   - Enable the following services:
     - **Authentication** (for player identity)
     - **Analytics** (for event tracking)
     - **Cloud Code** (for server-side logic)
     - **Economy** (for currencies and inventory)
     - **Leaderboards** (for competitive features)
     - **Lobby** (optional, for matchmaking)
     - **Relay** (optional, for P2P networking)

3. **Install Unity Packages** (if not already installed)
   - Open `Window > Package Manager`
   - Ensure the following packages are installed:
     - Unity Gaming Services Core
     - Authentication
     - Analytics
     - Cloud Code
     - Economy
     - Leaderboards
     - Netcode for GameObjects
     - (Optional) Lobby
     - (Optional) Relay

### 2. Build Settings Configuration

1. **Add Boot Scene to Build Settings**
   - Go to `File > Build Settings`
   - Click `Add Open Scenes` or drag `Assets/_Project/Scenes/Boot.unity` to the list
   - **Ensure Boot scene is at index 0** (first scene in the build)
   - Add any other game scenes below it

2. **Platform-Specific Settings**
   - For Android builds, ensure Android platform is selected
   - Configure Player Settings as needed (`Edit > Project Settings > Player`)

## Running the Project

### Running in Unity Editor

1. Open the project in Unity Editor
2. Open the Boot scene: `Assets/_Project/Scenes/Boot.unity`
3. Click the Play button in the Unity Editor
4. Check the Console for UGS initialization logs:
   ```
   [UGS] Initializing Unity Gaming Services...
   [UGS] Unity Gaming Services initialized successfully
   [Auth] Initializing Authentication Service...
   [Auth] Signed in anonymously as [PLAYER_ID]
   [Analytics] Analytics Service initialized...
   [Economy] Economy Service initialized...
   [Leaderboards] Leaderboards Service initialized...
   [Cloud Code] Cloud Code Service initialized...
   [Lobby & Relay] Lobby and Relay Service initialized...
   [UGS] All services initialized successfully
   ```

### Building for Android

1. **Prerequisites**
   - Android SDK and NDK installed
   - JDK configured in Unity
   - Android Build Support module installed

2. **Build Steps**
   - Go to `File > Build Settings`
   - Select `Android` platform
   - Click `Switch Platform` (if not already on Android)
   - Configure Player Settings:
     - Set Package Name (e.g., `com.yourcompany.yourproject`)
     - Set Minimum API Level (recommended: API 24 or higher)
     - Configure signing keys for release builds
   - Click `Build` or `Build And Run`
   - Choose output location for APK

3. **Testing on Device**
   - Enable Developer Options and USB Debugging on your Android device
   - Connect device via USB
   - Click `Build And Run` to install and launch on device
   - Use `adb logcat` to view logs on device:
     ```bash
     adb logcat -s Unity
     ```

## UGS Services Overview

### Authentication Service
- Automatically signs in players anonymously on startup
- Provides player ID for backend services
- Can be extended for platform-specific authentication

### Analytics Service
- Tracks custom game events
- Provides built-in events (game start, level complete, purchases)
- Data visible in Unity Analytics Dashboard

### Economy Service
- Manages virtual currencies
- Handles player inventory
- Supports in-game purchases

### Leaderboards Service
- Submit and retrieve player scores
- Support for multiple leaderboards
- Player ranking system

### Cloud Code
- Execute server-side logic
- Validate game actions
- Secure sensitive operations

### Lobby & Relay (Optional)
- Create and join multiplayer lobbies
- Allocate relay servers for P2P connections
- Requires additional packages and configuration

## Environment Configuration

The Bootstrap initializer uses the `production` environment by default. To change environments:

1. Select the Bootstrap GameObject in the Boot scene
2. In the Inspector, modify the `Environment` field in the UgsInitializer component
3. Available options: `production`, `development`, `staging` (configure in Unity Dashboard)

## Troubleshooting

### Common Issues

1. **"Project not linked to Unity Services"**
   - Solution: Link project in `Edit > Project Settings > Services`

2. **"Authentication failed"**
   - Solution: Ensure Authentication is enabled in Unity Dashboard
   - Check internet connection

3. **"Assembly reference errors"**
   - Solution: Reimport assembly definitions or restart Unity Editor
   - Verify all required UGS packages are installed

4. **Scene not found in build**
   - Solution: Add Boot scene to Build Settings at index 0

5. **Android build fails**
   - Solution: Check Android SDK path in `Edit > Preferences > External Tools`
   - Verify minimum API level is set correctly

### Debug Logs

All UGS services use tagged debug logs:
- `[UGS]` - General initialization
- `[Auth]` - Authentication operations
- `[Analytics]` - Analytics events
- `[Economy]` - Economy operations
- `[Leaderboards]` - Leaderboard operations
- `[Cloud Code]` - Cloud Code calls
- `[Lobby & Relay]` - Lobby and Relay operations

## Next Steps

1. **Configure Cloud Resources**
   - Set up currencies, items, and leaderboards in Unity Dashboard
   - Create Cloud Code scripts for server logic

2. **Implement Game Logic**
   - Add game scripts to `Assets/_Project/Scripts/Gameplay/`
   - Use service wrappers to interact with UGS

3. **Design UI**
   - Create UXML files in `Assets/_Project/UI/UXML/`
   - Style with USS in `Assets/_Project/UI/USS/`
   - Reference `PanelSettings.asset` in UIDocument components

4. **Add Netcode Features**
   - Implement multiplayer scripts in `Assets/_Project/Scripts/Netcode/`
   - Configure Netcode for GameObjects

5. **Testing**
   - Add test scripts to `Assets/_Project/Tests/`
   - Run tests via Unity Test Runner

## Resources

- [Unity Gaming Services Documentation](https://docs.unity.com/ugs/en-us/manual/overview/manual/overview)
- [Authentication Documentation](https://docs.unity.com/authentication/)
- [Analytics Documentation](https://docs.unity.com/analytics/)
- [Economy Documentation](https://docs.unity.com/economy/)
- [Leaderboards Documentation](https://docs.unity.com/leaderboards/)
- [Cloud Code Documentation](https://docs.unity.com/cloud-code/)
- [Netcode for GameObjects](https://docs-multiplayer.unity3d.com/)

## License

[Add your license information here]
