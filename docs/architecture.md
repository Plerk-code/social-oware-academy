# Technical Architecture - Social Oware Academy

**Document Version:** 2.0 (BMAD-Refactored)
**Last Updated:** 2025-10-21
**Status:** Active Development
**Unity Version:** 2022 LTS
**Target Platforms:** iOS 13+, Android 7.0+, WebGL

---

## Table of Contents

1. [Architecture Overview](#architecture-overview)
2. [Technology Stack](#technology-stack)
3. [Unity Project Structure](#unity-project-structure)
4. [Core Game Systems](#core-game-systems)
5. [Multiplayer Architecture](#multiplayer-architecture)
6. [Backend Architecture](#backend-architecture)
7. [Data Models](#data-models)
8. [Coding Standards](#coding-standards)
9. [Performance Requirements](#performance-requirements)
10. [Security Considerations](#security-considerations)
11. [Deployment Pipeline](#deployment-pipeline)

---

## 1. Architecture Overview

### High-Level System Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    CLIENT LAYER                          │
│  ┌─────────────┐  ┌─────────────┐  ┌────────────────┐ │
│  │   iOS App   │  │ Android App │  │  WebGL Browser │ │
│  └──────┬──────┘  └──────┬──────┘  └────────┬───────┘ │
│         │                 │                   │         │
│         └─────────────────┴───────────────────┘         │
│                           │                             │
│                  ┌────────▼────────┐                    │
│                  │  UNITY ENGINE   │                    │
│                  │   (2022 LTS)    │                    │
│                  └────────┬────────┘                    │
└───────────────────────────┼─────────────────────────────┘
                            │
        ┌───────────────────┼───────────────────┐
        │                   │                   │
┌───────▼────────┐  ┌───────▼────────┐  ┌──────▼───────┐
│ PHOTON FUSION  │  │  UNITY GAMING  │  │  UNITY IAP   │
│  (Multiplayer) │  │    SERVICES    │  │ (Payments)   │
└───────┬────────┘  └───────┬────────┘  └──────┬───────┘
        │                   │                   │
        │                   │                   │
┌───────▼───────────────────▼───────────────────▼───────┐
│              EXTERNAL SERVICES LAYER                   │
│  • Unity Authentication (Player identity)              │
│  • Unity Analytics (Event tracking)                    │
│  • Unity Cloud Save (Cross-device sync)                │
│  • Unity Leaderboards (Rankings)                       │
│  • RevenueCat (Subscription management)                │
│  • Unity Ads (Advertising)                             │
│  • Branch.io (Deep Links)                              │
└────────────────────────────────────────────────────────┘
```

### Architecture Philosophy

**Principles:**
1. **Separation of Concerns:** Game logic, network, UI, and data layers are independent
2. **Server-Authoritative:** Critical game state validated server-side (anti-cheat)
3. **Offline-First:** Core gameplay works offline, syncs when online
4. **Modular Design:** Systems are loosely coupled via events/interfaces
5. **Testability:** Game logic is unit-testable (pure C# classes)

---

## 2. Technology Stack

### Core Engine
- **Unity 2022 LTS**
  - Stable, production-ready
  - Long-term bug fixes and updates
  - Official support for all required SDKs
  - WebGL export improvements
  - Mobile optimization

### Rendering
- **Universal Render Pipeline (URP)**
  - Optimized for mobile and WebGL
  - 2D rendering support
  - Shader Graph compatibility
  - Post-processing effects

### Networking
- **Photon Fusion 2.0**
  - Turn-based multiplayer support
  - Cross-platform (iOS/Android/WebGL)
  - Built-in matchmaking
  - Room system for private lobbies
  - CCU-based pricing (free tier: 20 CCU)

### Backend
- **Unity Gaming Services (UGS)**
  - Authentication (player identity)
  - Analytics (event tracking)
  - Cloud Save (cross-device sync)
  - Leaderboards (global/friends rankings)
  - Free tier: 100K MAU

### Monetization
- **Unity IAP** - In-app purchases
- **RevenueCat** - Subscription management
- **Unity Ads** - Interstitial advertising

### Additional SDKs
- **TextMeshPro** - UI text rendering
- **DOTween** - Animation tweening
- **Unity Mobile Notifications** - Push notifications
- **Native Share Plugin** - Social media sharing

### Development Tools
- **Git** - Version control
- **Visual Studio Code / Rider** - IDE
- **Unity Test Framework** - Unit testing
- **Unity Profiler** - Performance analysis

---

## 3. Unity Project Structure

### Project Folder Organization

```
My project/
├── Assets/
│   ├── _Project/                    # All game-specific code
│   │   ├── Scenes/
│   │   │   ├── Boot.unity            # Bootstrap scene (manager initialization)
│   │   │   ├── MainMenu.unity        # Home screen
│   │   │   ├── Game.unity            # Match scene
│   │   │   ├── Lesson.unity          # Training mode
│   │   │   └── Lobby.unity           # Matchmaking/lobby
│   │   ├── Scripts/
│   │   │   ├── Gameplay/             # Game logic (CURRENT)
│   │   │   │   ├── GameManager.cs     # Central game state
│   │   │   │   ├── OwareBoard.cs      # Board state & operations
│   │   │   │   ├── OwareRules.cs      # Rule validation & execution
│   │   │   │   ├── OwareBoardVisualizer.cs  # Visual representation
│   │   │   │   └── OwareAI.cs         # AI opponents (TBD)
│   │   │   ├── Services/             # UGS service wrappers (CURRENT)
│   │   │   │   ├── UgsInitializer.cs  # UGS bootstrap
│   │   │   │   ├── AuthService.cs     # Authentication
│   │   │   │   ├── AnalyticsService.cs  # Event tracking
│   │   │   │   ├── LeaderboardsService.cs  # Rankings
│   │   │   │   ├── EconomyService.cs  # Virtual economy
│   │   │   │   └── CloudCodeClient.cs  # Server-side logic
│   │   │   ├── Netcode/              # Multiplayer (TBD)
│   │   │   │   ├── NetworkManager.cs
│   │   │   │   ├── MatchmakingController.cs
│   │   │   │   ├── NetworkPlayer.cs
│   │   │   │   └── SyncedGameState.cs
│   │   │   ├── Progression/          # Player progression (TBD)
│   │   │   │   ├── LevelSystem.cs
│   │   │   │   ├── ELOSystem.cs
│   │   │   │   ├── LessonSystem.cs
│   │   │   │   └── StreakSystem.cs
│   │   │   ├── UI/                   # UI controllers (CURRENT - partial)
│   │   │   │   ├── MainMenuUI.cs
│   │   │   │   ├── MainMenuController.cs
│   │   │   │   ├── ResultsUI.cs
│   │   │   │   ├── StoreUI.cs
│   │   │   │   └── TestMenu.cs
│   │   │   ├── Monetization/         # IAP & Ads (TBD)
│   │   │   │   ├── PremiumManager.cs
│   │   │   │   ├── PaywallController.cs
│   │   │   │   └── AdManager.cs
│   │   │   └── Utilities/            # Helper classes
│   │   │       ├── SaveSystem.cs
│   │   │       ├── Singleton.cs
│   │   │       └── Extensions.cs
│   │   ├── Prefabs/
│   │   │   ├── UI/                   # UI prefabs
│   │   │   ├── Gameplay/             # Board, pieces, effects
│   │   │   └── Network/              # Networked objects
│   │   ├── UI/
│   │   │   ├── UXML/                 # UI Toolkit UXML files
│   │   │   ├── USS/                  # UI Toolkit stylesheets
│   │   │   └── PanelSettings.asset
│   │   ├── Editor/                   # Editor tools (CURRENT)
│   │   │   ├── PlayFromBoot.cs
│   │   │   ├── AddScenesToBuild.cs
│   │   │   └── OwareSetupValidator.cs
│   │   ├── Tests/                    # Test scripts
│   │   │   ├── EditMode/
│   │   │   └── PlayMode/
│   │   ├── Data/
│   │   │   ├── LessonData.json       # Lesson content
│   │   │   ├── PuzzleData.json       # Daily puzzles
│   │   │   └── ConfigData.json       # Game balance values
│   │   └── CloudCode/                # Server-side scripts (CURRENT)
│   ├── Settings/
│   │   ├── Quality Settings/         # Performance tiers
│   │   └── Input System/             # Touch + keyboard
│   ├── TextMesh Pro/                 # TMP assets (CURRENT)
│   ├── Resources/                    # Runtime loadable assets
│   └── TutorialInfo/                 # Unity template artifacts (CURRENT)
├── Packages/
│   └── manifest.json                 # Package dependencies (CURRENT)
├── ProjectSettings/                  # Unity project settings (CURRENT)
├── Library/                          # Unity cache (ignored by Git)
├── Logs/                             # Unity logs (ignored by Git)
├── Temp/                             # Temporary build files (ignored by Git)
└── UserSettings/                     # User-specific settings (ignored by Git)
```

### Current Implementation Status
- ✅ **Complete:** Core gameplay (GameManager, OwareBoard, OwareRules, OwareBoardVisualizer)
- ✅ **Complete:** UGS service wrappers (Auth, Analytics, Leaderboards, Economy, CloudCode)
- ✅ **Complete:** Basic UI (MainMenu, Results, Store)
- ✅ **Complete:** Editor tools (PlayFromBoot, AddScenesToBuild, OwareSetupValidator)
- ⏳ **Partial:** BootLoader (scene management skeleton exists)
- ❌ **Not Started:** Multiplayer (Netcode/)
- ❌ **Not Started:** Progression systems
- ❌ **Not Started:** Monetization (IAP, Ads)
- ❌ **Not Started:** AI opponents

---

## 4. Core Game Systems

### 4.1 Game State Management

**GameManager.cs** (Singleton)
- **Responsibilities:**
  - Orchestrates game flow (new game, move execution, game end)
  - Manages player turns (human vs AI)
  - Triggers AI moves with delay
  - Fires game events (OnGameStarted, OnGameEnded, OnMoveMade)
  - Maintains reference to OwareBoard

**Current Implementation:** `Assets/_Project/Scripts/Gameplay/GameManager.cs:1`

```csharp
// Key properties and methods (existing)
public static GameManager Instance { get; private set; }
public OwareBoard Board { get; }
public bool IsGameActive { get; }
public bool IsHumanTurn { get; }

public void StartNewGame()
public bool MakeMove(int pitIndex)
private void TriggerAIMove()
private void EndGame()
```

**State Machine:**
```
Idle → StartNewGame() → GameActive
  ├─ IsHumanTurn == true → Player makes move → Check game end
  │                                           ├─ Game continues → AI turn
  │                                           └─ Game over → EndGame() → Idle
  └─ IsHumanTurn == false → AI makes move → Check game end
                                            ├─ Game continues → Player turn
                                            └─ Game over → EndGame() → Idle
```

---

### 4.2 Oware Board Logic

**OwareBoard.cs** (Data Model)
- **Responsibilities:**
  - Maintains board state (12 pits, captured seeds, current player)
  - Provides query methods (GetSeeds, IsPitOwnedByCurrentPlayer, etc.)
  - Fires board events (OnBoardChanged, OnCaptureOccurred, OnTurnChanged, OnGameOver)
  - Supports board cloning (for AI lookahead)

**Current Implementation:** `Assets/_Project/Scripts/Gameplay/OwareBoard.cs:1`

```csharp
// Key constants
public const int PITS_PER_PLAYER = 6;
public const int TOTAL_PITS = 12;
public const int STARTING_SEEDS_PER_PIT = 4;
public const int TOTAL_SEEDS = 48;
public const int SEEDS_TO_WIN = 25;

// State
private int[] pits;              // 12 pits (0-5 = P1, 6-11 = P2)
private int player1Captured;
private int player2Captured;
private int currentPlayer;       // 0 = P1, 1 = P2

// Events
public event Action<int[]> OnBoardChanged;
public event Action<int, int> OnCaptureOccurred;
public event Action<int> OnTurnChanged;
public event Action<int> OnGameOver;

// Methods
public void ResetBoard()
public int GetSeeds(int pitIndex)
public void SetSeeds(int pitIndex, int seeds)
public void AddCapturedSeeds(int player, int seeds)
public void NextTurn()
public OwareBoard Clone()
```

**Board Layout:**
```
Player 2 Side (pits 11 → 6, displayed right-to-left)
[11] [10] [9] [8] [7] [6]

[0]  [1]  [2] [3] [4] [5]
Player 1 Side (pits 0 → 5, displayed left-to-right)
```

---

### 4.3 Oware Rules Engine

**OwareRules.cs** (Static Class - Pure Logic)
- **Responsibilities:**
  - Validates moves (ownership, empty pit, forced feeding)
  - Executes moves (sowing, capturing)
  - Enforces authentic Ghanaian Oware rules:
    - Counter-clockwise sowing
    - Capture on 2-3 seeds in opponent's pit
    - Grand Slam rule (can't capture ALL opponent's seeds)
    - Forced feeding (must give opponent seeds if they have none)
  - Checks game end conditions
  - Determines winner

**Current Implementation:** `Assets/_Project/Scripts/Gameplay/OwareRules.cs:1`

```csharp
// Key methods
public static bool IsValidMove(OwareBoard board, int pitIndex)
public static List<int> GetValidMoves(OwareBoard board)
public static bool ExecuteMove(OwareBoard board, int pitIndex, bool simulate = false)
private static int ProcessCaptures(OwareBoard board, int lastPit, bool simulate)
private static bool WouldBeGrandSlam(OwareBoard board, List<int> pitsToCapture)
public static bool CheckGameEnd(OwareBoard board)
public static int GetWinner(OwareBoard board)
```

**Move Execution Flow:**
```
1. Validate move (IsValidMove)
   ├─ Check pit ownership
   ├─ Check pit has seeds
   └─ Check forced feeding rule

2. Execute move
   ├─ Pick up seeds from selected pit
   ├─ Sow seeds counter-clockwise (skip starting pit)
   └─ Track last pit where seed dropped

3. Check for captures (if last pit in opponent's side)
   ├─ Work backwards from last pit
   ├─ Capture pits with 2-3 seeds
   ├─ Stop when pit doesn't have 2-3 seeds
   └─ Apply Grand Slam rule (prevent capturing all opponent's seeds)

4. Switch turn

5. Check game end
   ├─ Win condition: 25+ seeds captured
   └─ Stalemate: No valid moves (award remaining seeds to opponent)
```

---

### 4.4 Board Visualization

**OwareBoardVisualizer.cs**
- **Responsibilities:**
  - Visual representation of board state
  - Animates seed movement (sowing)
  - Displays captures with visual feedback
  - Updates UI in response to OwareBoard events

**Current Implementation:** `Assets/_Project/Scripts/Gameplay/OwareBoardVisualizer.cs:1`

---

### 4.5 AI System (TBD - To Be Developed)

**OwareAI.cs** (Base Interface)
```csharp
public interface IOwareAI
{
    int SelectMove(OwareBoard board);
}

// Implementations
public class BeginnerAI : IOwareAI
{
    // Random moves with 30% "good move" heuristic
}

public class IntermediateAI : IOwareAI
{
    // Minimax depth 3
}

public class AdvancedAI : IOwareAI
{
    // Minimax with alpha-beta pruning depth 5-7
}
```

**AI Decision Making:**
```
BeginnerAI:
- 70% random valid move
- 30% heuristic: prioritize captures or block opponent captures

IntermediateAI:
- Minimax algorithm depth 3
- Evaluation function: (my captured seeds) - (opponent captured seeds)

AdvancedAI:
- Minimax with alpha-beta pruning depth 5-7
- Enhanced evaluation: position value + mobility + capture potential
- Opening book (first 3 moves optimized)
```

---

## 5. Multiplayer Architecture

### 5.1 Photon Fusion Integration

**NetworkManager.cs** (TBD)
- **Responsibilities:**
  - Connect to Photon servers
  - Create/join rooms
  - Handle disconnections
  - Sync game state across network

**Connection Flow:**
```
Player opens app
    ↓
Authenticate with UGS (get PlayerId)
    ↓
Connect to Photon (using UGS token)
    ↓
[Casual/Ranked/Private Mode Selected]
    ↓
┌─────────────┬──────────────┬─────────────────┐
│   Casual    │    Ranked    │   Private Lobby │
└──────┬──────┴──────┬───────┴────────┬────────┘
       │             │                │
       ▼             ▼                ▼
  Join Room    Matchmaking      Create Room
  (vs AI)      (find opponent)  (share code)
       │             │                │
       └──────┬──────┴────────┬───────┘
              ▼               ▼
         Game Room       Game Room
         (2 players)     (2 players)
              │               │
              └───────┬───────┘
                      ▼
               Play Match
                      │
              ┌───────┴────────┐
              │                │
              ▼                ▼
         Match End       Match End
         (report to      (report to
          UGS)           UGS)
```

---

### 5.2 State Synchronization

**Turn-Based Model (Not Real-Time Physics)**
- Only sync on player actions (move selection)
- Server-authoritative (host validates moves)
- No interpolation needed (discrete turns)

**Synced Data Structure:**
```csharp
public struct GameState : INetworkStruct
{
    public int[] PitSeeds;              // 12 ints (seeds in each pit)
    public int Player1Captured;         // Total seeds captured by P1
    public int Player2Captured;         // Total seeds captured by P2
    public int CurrentTurn;             // 0 = P1, 1 = P2
    public NetworkBool IsGameOver;      // Match ended
    public int WinnerID;                // PlayerID of winner
}
```

**RPC Calls:**
```csharp
[Rpc(RpcSources.All, RpcTargets.All)]
public void RPC_MakeMove(int pitIndex) { }

[Rpc(RpcSources.All, RpcTargets.All)]
public void RPC_SendEmote(int emoteID) { }

[Rpc(RpcSources.All, RpcTargets.All)]
public void RPC_RequestRematch() { }

[Rpc(RpcSources.All, RpcTargets.All)]
public void RPC_Forfeit() { }
```

---

### 5.3 Matchmaking System

**MatchmakingController.cs** (TBD)

**Ranked Matchmaking Algorithm:**
```
1. Player enters ranked queue with ELO rating (e.g., 1450)
2. Server searches for opponent:
   - Priority 1 (0-30s): ±50 rating (1400-1500)
   - Priority 2 (30-60s): ±100 rating (1350-1550)
   - Priority 3 (60-90s): ±200 rating (1250-1650)
   - Priority 4 (90s+): Any rating (rare, logged for analysis)
3. Match found → Create room, start match
4. No match after 120s → "No opponents found, try again" (retry button)
```

**Implementation:**
- Photon Matchmaking API with custom properties:
  - `ELO`: Player rating
  - `Region`: Server region (for latency)
  - `SearchRange`: Expands every 30s

---

## 6. Backend Architecture

### 6.1 Unity Gaming Services (UGS)

**Services Used:**
1. **Authentication** - Player identity
2. **Analytics** - Event tracking
3. **Cloud Save** - Cross-device sync
4. **Leaderboards** - Global/Friends rankings
5. **Economy** - Virtual currencies (future)
6. **Cloud Code** - Server-side logic (future)

**Current Implementation:**
- ✅ UgsInitializer.cs: `Assets/_Project/Scripts/Services/UgsInitializer.cs:1`
- ✅ AuthService.cs: `Assets/_Project/Scripts/Services/AuthService.cs:1`
- ✅ AnalyticsService.cs: `Assets/_Project/Scripts/Services/AnalyticsService.cs:1`
- ✅ LeaderboardsService.cs: `Assets/_Project/Scripts/Services/LeaderboardsService.cs:1`
- ✅ EconomyService.cs: `Assets/_Project/Scripts/Services/EconomyService.cs:1`
- ✅ CloudCodeClient.cs: `Assets/_Project/Scripts/Services/CloudCodeClient.cs:1`

---

### 6.2 Authentication Flow

```
App Launch
    ↓
UgsInitializer.InitializeAsync()
    ↓
AuthService.SignInAnonymouslyAsync()
    ↓
[Success] Player ID stored
    ↓
Load player data from Cloud Save
    ↓
Proceed to MainMenu
```

**Account Linking (Future):**
- Guest accounts can link to email/social later
- One account across all devices

---

### 6.3 Cloud Save

**SaveSystem.cs** (TBD)

**Data Storage:**
```csharp
public class PlayerData
{
    public int PlayerLevel;
    public int TotalXP;
    public int ELORating;
    public string RankTier;
    public int StreakCount;
    public string StreakLastPlayed;  // ISO 8601 date
    public List<int> LessonsCompleted;
    public List<int> LessonStars;
    public List<string> Achievements;
    public PlayerSettings Settings;
}

public class PlayerSettings
{
    public bool SoundEnabled = true;
    public bool MusicEnabled = true;
    public bool NotificationsEnabled = true;
}
```

**Sync Strategy:**
- Save after every activity (match, lesson, etc.)
- Load on app launch
- Conflict resolution: Server timestamp wins

---

### 6.4 Analytics Events

**Tracked Events:**
```csharp
// Account
"account_created"

// Onboarding
"tutorial_completed"
"first_match_played"

// Progression
"level_up" (level, xp)
"rank_up" (old_rank, new_rank, elo)

// Gameplay
"match_completed" (mode, result, duration)
"lesson_completed" (lesson_id, stars)
"daily_challenge_completed" (challenge_type, xp_earned)

// Engagement
"streak_milestone" (days: 7/30/100)
"daily_login"

// Monetization
"premium_purchased" (sku, price)
"paywall_shown" (trigger_type)
"ad_watched" (placement)

// Errors
"match_abandoned" (reason)
"app_crash" (error_message)
```

**Implementation:**
```csharp
AnalyticsService.RecordEvent("level_up", new Dictionary<string, object>
{
    { "level", 12 },
    { "xp", 5430 }
});
```

---

### 6.5 Leaderboards

**Leaderboard Types:**
- **Global ELO:** All players ranked by ELO rating
- **Friends ELO:** Friends-only leaderboard
- **Country ELO:** Players in same country (IP geolocation)
- **Daily Puzzle:** Fastest solve times (today only)

**Schema:**
```
StatisticName: "ELORating"
Value: 1450 (player's current ELO)
DisplayName: Player username
Position: Player's rank (e.g., #47)
```

**Update Frequency:**
- Real-time (after each ranked match)
- Client calls: `LeaderboardsService.GetScoresAsync()`

---

## 7. Data Models

### 7.1 Player Profile

```csharp
public class PlayerProfile
{
    public string PlayerId;          // UGS Auth ID
    public string Username;
    public string FriendCode;        // "PLAYER#1234"
    public int Level;
    public int TotalXP;
    public int ELORating;
    public RankTier Tier;
    public int StreakCount;
    public DateTime StreakLastPlayed;
    public bool IsPremium;
    public DateTime PremiumExpiresAt;
}

public enum RankTier
{
    Bronze,
    Silver,
    Gold,
    Platinum,
    Diamond,
    Master
}
```

---

### 7.2 Match Data

```csharp
public class MatchData
{
    public string MatchId;
    public MatchMode Mode;
    public string Player1Id;
    public string Player2Id;
    public int WinnerId;             // 0, 1, or -1 (draw)
    public int TurnCount;
    public float DurationSeconds;
    public DateTime CompletedAt;
    public int XPEarned;
    public int ELOChange;            // ±rating
}

public enum MatchMode
{
    CasualAI,
    CasualFriend,
    Ranked,
    PrivateLobby,
    Lesson,
    DailyPuzzle
}
```

---

### 7.3 Lesson Data

```csharp
public class LessonData
{
    public int LessonId;
    public string Title;
    public string Description;
    public DifficultyLevel Difficulty;
    public List<LessonStep> Steps;
    public int XPReward;
    public bool RequiresPremium;
}

public class LessonStep
{
    public string Instruction;
    public OwareBoardSetup BoardSetup;  // Initial board state
    public int CorrectPitIndex;         // Expected move
    public string Hint;
}

public enum DifficultyLevel
{
    Beginner,
    Intermediate,
    Advanced
}
```

---

### 7.4 Daily Challenge Data

```csharp
public class DailyChallenge
{
    public string ChallengeId;
    public ChallengeType Type;
    public int RequiredCount;       // e.g., "Win 2 matches"
    public int CurrentProgress;
    public int XPReward;
    public DateTime ExpiresAt;      // Midnight UTC
}

public enum ChallengeType
{
    WinMatches,
    CompleteLesson,
    SolvePuzzle,
    PlayWithFriend
}
```

---

## 8. Coding Standards

### 8.1 C# Conventions

**Naming:**
```csharp
// Classes: PascalCase
public class GameManager { }

// Interfaces: I + PascalCase
public interface IOwareAI { }

// Public fields/properties: PascalCase
public int TotalSeeds { get; set; }

// Private fields: camelCase
private int currentPlayer;

// Serialized fields: camelCase
[SerializeField] private bool isLocalGame;

// Constants: UPPER_SNAKE_CASE
public const int TOTAL_PITS = 12;

// Methods: PascalCase (verb phrases)
public void StartNewGame() { }

// Events: On + PascalCase
public event Action<int> OnGameEnded;
```

**File Organization:**
```csharp
// 1. Usings
using UnityEngine;
using System;

// 2. Namespace (if applicable)
namespace SocialOwareAcademy.Gameplay
{
    // 3. Class documentation
    /// <summary>
    /// Central game manager - orchestrates Oware matches.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        // 4. Serialized fields
        [Header("Game State")]
        [SerializeField] private bool isLocalGame = true;

        // 5. Private fields
        private OwareBoard board;

        // 6. Public properties
        public OwareBoard Board => board;

        // 7. Events
        public event Action OnGameStarted;

        // 8. Unity lifecycle methods
        void Awake() { }
        void Start() { }

        // 9. Public methods
        public void StartNewGame() { }

        // 10. Private methods
        private void EndGame() { }

        // 11. Event handlers
        private void HandleGameOver(int winner) { }
    }
}
```

---

### 8.2 Unity Best Practices

**MonoBehaviour Usage:**
```csharp
// ✅ Good: Use MonoBehaviour for game objects
public class OwareBoardVisualizer : MonoBehaviour { }

// ✅ Good: Pure logic as plain C# classes (testable)
public class OwareBoard { }
public static class OwareRules { }

// ❌ Bad: Don't use MonoBehaviour for data models
// Use plain C# classes or structs instead
```

**Singleton Pattern:**
```csharp
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
```

**Event-Driven Architecture:**
```csharp
// Publisher
public class OwareBoard
{
    public event Action<int[]> OnBoardChanged;

    public void SetSeeds(int pitIndex, int seeds)
    {
        pits[pitIndex] = seeds;
        OnBoardChanged?.Invoke(pits);  // Notify subscribers
    }
}

// Subscriber
public class OwareBoardVisualizer : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.Board.OnBoardChanged += HandleBoardChanged;
    }

    void OnDestroy()
    {
        // CRITICAL: Always unsubscribe to prevent memory leaks
        if (GameManager.Instance?.Board != null)
            GameManager.Instance.Board.OnBoardChanged -= HandleBoardChanged;
    }

    void HandleBoardChanged(int[] pits)
    {
        UpdateVisuals(pits);
    }
}
```

**SerializeField Best Practices:**
```csharp
// ✅ Good: Private field with [SerializeField] for Inspector editing
[SerializeField] private int startingSeeds = 4;

// ✅ Good: [Header] for organization in Inspector
[Header("Game Settings")]
[SerializeField] private bool isAIOpponent = true;

// ✅ Good: [Tooltip] for documentation
[Tooltip("Delay in seconds before AI makes move")]
[SerializeField] private float aiMoveDelay = 1.5f;

// ❌ Bad: Public fields (use properties instead)
public int startingSeeds = 4;
```

**Coroutine vs Async/Await:**
```csharp
// ✅ Good: Use async/await for network calls
public async Task<bool> AuthenticateAsync()
{
    var result = await AuthService.SignInAnonymouslyAsync();
    return result.Success;
}

// ✅ Good: Use coroutines for time-based animations
IEnumerator SowSeedsAnimation(int pitIndex, int seeds)
{
    for (int i = 0; i < seeds; i++)
    {
        yield return new WaitForSeconds(0.3f);
        // Animate seed movement
    }
}
```

---

### 8.3 Performance Best Practices

**Avoid Allocations in Update/FixedUpdate:**
```csharp
// ❌ Bad: Creates garbage every frame
void Update()
{
    string debugText = "Current turn: " + currentPlayer;  // String concat allocates
}

// ✅ Good: Cache references, use StringBuilder for complex strings
private StringBuilder sb = new StringBuilder();

void Update()
{
    sb.Clear();
    sb.Append("Current turn: ");
    sb.Append(currentPlayer);
    Debug.Log(sb.ToString());
}
```

**Object Pooling:**
```csharp
// For frequently instantiated objects (e.g., seed prefabs, VFX)
public class SeedPool : MonoBehaviour
{
    private Queue<GameObject> pool = new Queue<GameObject>();

    public GameObject GetSeed()
    {
        if (pool.Count > 0)
            return pool.Dequeue();
        else
            return Instantiate(seedPrefab);
    }

    public void ReturnSeed(GameObject seed)
    {
        seed.SetActive(false);
        pool.Enqueue(seed);
    }
}
```

**Cache Component References:**
```csharp
// ❌ Bad: GetComponent every frame
void Update()
{
    GetComponent<Animator>().Play("Idle");
}

// ✅ Good: Cache in Awake/Start
private Animator animator;

void Awake()
{
    animator = GetComponent<Animator>();
}

void Update()
{
    animator.Play("Idle");
}
```

---

## 9. Performance Requirements

### Mobile (iOS/Android)
- **Frame Rate:** 60 FPS constant
- **Memory:** <300 MB RAM usage
- **Battery:** <5% drain per 10-min session
- **Network:** <1 MB data per match
- **Cold Start:** <5 seconds

### WebGL
- **Frame Rate:** 30-60 FPS (browser-dependent)
- **Memory:** <500 MB RAM usage
- **Load Time:** <10 seconds (initial load)
- **Network:** <50 MB initial download (compressed)

### Profiling Targets
- **CPU:** <16ms per frame (60 FPS)
- **GPU:** <16ms per frame (60 FPS)
- **Rendering:** <50 draw calls per frame
- **Physics:** Minimal (turn-based game)

---

## 10. Security Considerations

### 10.1 Anti-Cheat

**Server-Authoritative Logic:**
```csharp
// ❌ Bad: Client validates moves (exploitable)
public bool MakeMove(int pitIndex)
{
    if (OwareRules.IsValidMove(board, pitIndex))
    {
        OwareRules.ExecuteMove(board, pitIndex);
        NetworkSync(board);  // Client sends final state
        return true;
    }
    return false;
}

// ✅ Good: Server validates moves (secure)
[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
public void RPC_RequestMove(int pitIndex)
{
    // Server validates
    if (OwareRules.IsValidMove(board, pitIndex))
    {
        OwareRules.ExecuteMove(board, pitIndex);
        RPC_SyncBoardState(board.Pits, board.Player1Captured, board.Player2Captured);
    }
    else
    {
        Debug.LogWarning($"Invalid move rejected from {PlayerId}");
    }
}

[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
public void RPC_SyncBoardState(int[] pits, int p1Captured, int p2Captured)
{
    // Clients receive validated state from server
}
```

**Progression Validation:**
- XP, Level, ELO updates happen server-side (UGS Cloud Code)
- Client displays UI but doesn't directly modify values
- Receipt validation for IAP (RevenueCat)

---

### 10.2 Privacy & Compliance

**GDPR:**
- Privacy policy URL: [TBD]
- Data deletion requests supported (UGS Delete Player API)
- Player data encrypted at rest (UGS default)

**COPPA (ages 7+):**
- No free text chat (preset emotes only)
- Parental consent flow (future enhancement)
- No personal data collection from children

**App Transport Security (iOS):**
- All network traffic uses HTTPS
- UGS, Photon, RevenueCat use secure connections

---

## 11. Deployment Pipeline

### Build Targets

**iOS:**
```
Unity Build Settings:
- Platform: iOS
- Architecture: ARM64
- Build Type: Release
- Development Build: OFF
- IL2CPP (not Mono)
- Strip Engine Code: ON

Xcode Settings:
- Signing: Automatic (development) / Manual (release)
- Bitcode: NO (Unity doesn't support)
- Deployment Target: iOS 13.0
```

**Android:**
```
Unity Build Settings:
- Platform: Android
- Texture Compression: ASTC
- Build Type: AAB (App Bundle, required for Google Play)
- IL2CPP (not Mono)
- ARM64 + ARMv7 (for backward compatibility)
- Strip Engine Code: ON

Keystore:
- Release keystore (password-protected)
- SHA-256 fingerprint registered in Google Play Console
```

**WebGL:**
```
Unity Build Settings:
- Platform: WebGL
- Compression: Brotli (best compression)
- Code Optimization: Fastest (IL2CPP)
- Data Caching: ON
- Strip Engine Code: ON

Hosting:
- Firebase Hosting or AWS S3 + CloudFront
- HTTPS required
- CORS headers configured
```

---

### CI/CD (Future Enhancement)

**Pipeline:**
```
Git Push to main branch
    ↓
GitHub Actions triggers Unity Cloud Build
    ↓
Build iOS + Android + WebGL in parallel
    ↓
Run automated tests (Unity Test Framework)
    ↓
If tests pass:
    ↓
Upload iOS build to TestFlight (Fastlane)
Upload Android build to Google Play Beta Track
Deploy WebGL to Firebase Hosting
    ↓
Slack notification: "Build X.Y.Z deployed to beta"
```

---

## Appendix: Package Dependencies

**Current Unity Packages** (from `Packages/manifest.json`):

```json
{
  "com.unity.ai.navigation": "2.0.9",
  "com.unity.collab-proxy": "2.10.0",
  "com.unity.ide.rider": "3.0.37",
  "com.unity.ide.visualstudio": "2.0.23",
  "com.unity.inputsystem": "1.14.2",
  "com.unity.multiplayer.center": "1.0.0",
  "com.unity.multiplayer.tools": "2.2.6",
  "com.unity.netcode.gameobjects": "2.5.1",
  "com.unity.render-pipelines.universal": "17.2.0",
  "com.unity.services.analytics": "6.1.1",
  "com.unity.services.authentication": "3.5.2",
  "com.unity.services.cloudcode": "2.10.2",
  "com.unity.services.cloudsave": "3.2.2",
  "com.unity.services.economy": "3.5.3",
  "com.unity.services.leaderboards": "2.3.3",
  "com.unity.services.multiplayer": "1.1.8",
  "com.unity.test-framework": "1.6.0",
  "com.unity.timeline": "1.8.9",
  "com.unity.ugui": "2.0.0",
  "com.unity.visualscripting": "1.9.7"
}
```

**Additional Packages to Install:**
- [ ] Photon Fusion SDK (Asset Store or direct download)
- [ ] RevenueCat SDK (Unity Package Manager or GitHub)
- [ ] DOTween (Asset Store - free)
- [ ] Native Share Plugin (Asset Store or GitHub)
- [ ] Branch.io SDK (Unity Package Manager)

---

## Document Change Log

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-10-14 | Team | Initial technical architecture |
| 2.0 | 2025-10-21 | BMAD Analyst | Refactored into BMAD architecture format, added current implementation status, consolidated all technical details |

---

**Next Steps:**
1. ✅ Architecture documented
2. ⏭️ Create epic-level feature breakdown
3. ⏭️ Generate user stories from epics
4. ⏭️ Create development tasks with Git integration
5. ⏭️ Begin Sprint 1 implementation

---

*"Build it right, scale it fast"*
