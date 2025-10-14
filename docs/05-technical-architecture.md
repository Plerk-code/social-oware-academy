# Technical Architecture - Social Oware Academy

Complete technical stack and architecture for cross-platform Unity development.

---

## 🏗️ Architecture Overview

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
│ PHOTON FUSION  │  │    PLAYFAB     │  │  UNITY IAP   │
│  (Multiplayer) │  │   (Backend)    │  │ (Payments)   │
└───────┬────────┘  └───────┬────────┘  └──────┬───────┘
        │                   │                   │
        │                   │                   │
┌───────▼───────────────────▼───────────────────▼───────┐
│              EXTERNAL SERVICES LAYER                   │
│  • RevenueCat (Subscriptions)                          │
│  • Unity Ads (Advertising)                             │
│  • Branch.io (Deep Links)                              │
│  • Unity Analytics (Metrics)                           │
└────────────────────────────────────────────────────────┘
```

---

## 🎮 Unity Configuration

### Engine Version
**Unity 2022 LTS (Long Term Support)**

**Rationale:**
- Stable, production-ready
- Long-term bug fixes and updates
- Photon Fusion + PlayFab official support
- WebGL export improvements
- Mobile optimization

**Build Targets:**
- iOS (minimum iOS 13+)
- Android (minimum API 24 / Android 7.0)
- WebGL (modern browsers)

---

### Project Structure

```
Assets/
├── _Project/                    # All game-specific code
│   ├── Art/
│   │   ├── Materials/           # Board, pieces (primitives for MVP)
│   │   ├── UI/                  # Sprites, icons, fonts
│   │   └── Shaders/             # Custom shaders (post-MVP)
│   ├── Audio/
│   │   ├── Music/               # Background music
│   │   └── SFX/                 # Sound effects (moves, captures)
│   ├── Prefabs/
│   │   ├── UI/                  # UI prefabs (menus, popups)
│   │   ├── Gameplay/            # Board, pieces, effects
│   │   └── Network/             # Networked objects
│   ├── Scenes/
│   │   ├── Bootstrap.unity      # Entry point, loads managers
│   │   ├── MainMenu.unity       # Home screen
│   │   ├── Game.unity           # Match scene
│   │   ├── Lesson.unity         # Training mode
│   │   └── Lobby.unity          # Matchmaking/lobby
│   ├── Scripts/
│   │   ├── Core/
│   │   │   ├── GameManager.cs   # Central game state
│   │   │   ├── OwareBoard.cs    # Game logic
│   │   │   ├── OwareAI.cs       # AI opponent
│   │   │   └── OwareRules.cs    # Rule validation
│   │   ├── Managers/
│   │   │   ├── NetworkManager.cs    # Photon wrapper
│   │   │   ├── PlayFabManager.cs    # Backend calls
│   │   │   ├── AudioManager.cs      # Sound system
│   │   │   ├── UIManager.cs         # UI state machine
│   │   │   └── AnalyticsManager.cs  # Event tracking
│   │   ├── Progression/
│   │   │   ├── LevelSystem.cs       # XP/Level logic
│   │   │   ├── ELOSystem.cs         # Rating calculations
│   │   │   ├── LessonSystem.cs      # Lesson progress
│   │   │   └── StreakSystem.cs      # Daily streaks
│   │   ├── UI/
│   │   │   ├── Screens/             # Screen controllers
│   │   │   ├── Widgets/             # Reusable UI elements
│   │   │   └── Popups/              # Modal dialogs
│   │   ├── Networking/
│   │   │   ├── MatchmakingController.cs
│   │   │   ├── NetworkPlayer.cs
│   │   │   └── SyncedGameState.cs
│   │   ├── Monetization/
│   │   │   ├── PremiumManager.cs    # Subscription checks
│   │   │   ├── PaywallController.cs # Paywall UI
│   │   │   └── AdManager.cs         # Ad integration
│   │   └── Utilities/
│   │       ├── SaveSystem.cs        # Local + cloud save
│   │       ├── Singleton.cs         # Manager base class
│   │       └── Extensions.cs        # Helper methods
│   └── Data/
│       ├── LessonData.json          # Lesson content
│       ├── PuzzleData.json          # Daily puzzles
│       └── ConfigData.json          # Game balance values
├── Plugins/
│   ├── PhotonFusion/                # Photon SDK
│   ├── PlayFabSDK/                  # PlayFab SDK
│   ├── RevenueCat/                  # RevenueCat SDK
│   └── NativeShare/                 # Sharing plugin
├── Settings/
│   ├── Quality Settings/            # Performance tiers
│   └── Input System/                # Touch + keyboard
└── ThirdParty/                      # External assets
    ├── TextMeshPro/                 # Text rendering
    └── DOTween/                     # Animation tweening
```

---

## 🌐 Multiplayer Architecture (Photon Fusion)

### Why Photon Fusion?

**Pros:**
- ✅ Unity-native integration
- ✅ Turn-based + real-time support
- ✅ Cross-platform (mobile + web)
- ✅ CCU-based pricing (scales with growth)
- ✅ Matchmaking built-in
- ✅ Room system for private lobbies
- ✅ Proven in mobile games (Among Us, Fall Guys)

**Cons:**
- ❌ Monthly cost (free tier: 20 CCU)
- ❌ Vendor lock-in (migration complexity)

**Pricing:**
- Free: 20 CCU
- Indie: $95/mo (100 CCU)
- Pro: $295/mo (500 CCU)
- Scale as you grow (target <100 CCU at launch)

---

### Network Architecture

**Connection Flow:**
```
Player opens app
    ↓
Authenticate with PlayFab (get PlayerID)
    ↓
Connect to Photon (using PlayFab token)
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
          PlayFab)        PlayFab)
```

---

### Room Types

#### 1. Casual Room (vs AI)
- **Type:** Single-player (local AI)
- **Network:** None (offline-capable)
- **State:** Local only, no sync needed

#### 2. Casual Room (vs Friend)
- **Type:** Private room
- **Network:** Photon room with 2 max players
- **State:** Turn-based sync (board state after each move)

#### 3. Ranked Room
- **Type:** Matchmaking room
- **Network:** Photon matchmaking system
- **Criteria:** ±100 ELO rating
- **State:** Turn-based sync + result reporting to PlayFab

#### 4. Private Lobby
- **Type:** Private room with code
- **Network:** Photon room with 8-char code
- **State:** Turn-based sync, no rating changes

---

### State Synchronization

**Turn-Based Model (Not Real-Time Physics):**
- Only sync on player actions (move selection)
- Server-authoritative (host validates moves)
- No interpolation needed (discrete turns)

**Synced Data:**
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
- `RPC_MakeMove(int pitIndex)` - Player selects pit
- `RPC_SendEmote(int emoteID)` - Player sends emote
- `RPC_RequestRematch()` - Player wants rematch
- `RPC_Forfeit()` - Player abandons match

---

### Matchmaking Logic

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

## 🗄️ Backend Architecture (PlayFab)

### Why PlayFab?

**Pros:**
- ✅ Free tier (up to 100K MAU)
- ✅ Built for games (not generic backend)
- ✅ Unity SDK official support
- ✅ All features needed: Auth, Leaderboards, Cloud Save, Analytics, Push
- ✅ Azure scalability
- ✅ Economy system for future IAP

**Cons:**
- ❌ Microsoft ecosystem (vendor lock-in)
- ❌ Dashboard UI learning curve

**Pricing:**
- Free: 100K MAU
- Standard: Pay-as-you-go after 100K MAU (~$0.01 per MAU)

---

### PlayFab Services Used

#### 1. Player Authentication
**Methods:**
- Email + Password (PlayFabClientAPI.LoginWithEmailAddress)
- Google Sign-In (PlayFabClientAPI.LoginWithGoogleAccount)
- Apple Sign-In (PlayFabClientAPI.LoginWithApple)
- Facebook Login (PlayFabClientAPI.LoginWithFacebook)
- Guest (PlayFabClientAPI.LoginWithCustomID, device ID)

**Account Linking:**
- Guest accounts can link to email/social later
- One account across all devices (cloud save)

---

#### 2. Player Data (Cloud Save)
**Data Storage:**
- **Player Data:** User-specific, private (PlayFabClientAPI.UpdateUserData)
- **Player Read-Only Data:** Set by server, readable by client (XP, Rating)
- **Internal Data:** Server-only (anti-cheat, admin flags)

**Saved Data Structure:**
```json
{
  "PlayerLevel": 12,
  "TotalXP": 5430,
  "ELORating": 1450,
  "RankTier": "Gold",
  "StreakCount": 15,
  "StreakLastPlayed": "2025-10-14",
  "LessonsCompleted": [1, 2, 3, 4, 5],
  "LessonStars": [3, 2, 3, 3, 1],
  "Achievements": ["tutorial_complete", "7_day_streak", "first_win"],
  "Settings": {
    "SoundEnabled": true,
    "MusicEnabled": true,
    "NotificationsEnabled": true
  }
}
```

**Sync Strategy:**
- Save after every activity (match, lesson, etc.)
- Load on app launch
- Conflict resolution: Server timestamp wins

---

#### 3. Leaderboards
**Leaderboard Types:**
- **Global ELO:** All players ranked by ELO rating
- **Friends ELO:** Friends-only leaderboard
- **Country ELO:** Players in same country (IP geolocation)

**Update Frequency:**
- Real-time (after each ranked match)
- Client calls: `PlayFabClientAPI.GetLeaderboard()`

**Leaderboard Schema:**
```
StatisticName: "ELORating"
Value: 1450 (player's current ELO)
DisplayName: Player username
Position: Player's rank (e.g., #47)
```

---

#### 4. CloudScript (Server-Side Logic)
**Use Cases:**
- **ELO Calculation:** Server calculates new rating (prevent cheating)
- **Match Result Validation:** Server verifies match outcome
- **Streak Check:** Server determines if streak maintained
- **XP Grant:** Server awards XP (prevent client manipulation)

**Example CloudScript Function:**
```javascript
// CloudScript function: AwardMatchXP
handlers.AwardMatchXP = function(args, context) {
    var isWin = args.isWin;
    var isRanked = args.isRanked;
    var xp = isRanked ? (isWin ? 75 : 30) : (isWin ? 50 : 20);

    // Get player's current XP
    var playerData = server.GetUserData({
        PlayFabId: currentPlayerId,
        Keys: ["TotalXP"]
    });

    var currentXP = parseInt(playerData.Data["TotalXP"].Value) || 0;
    var newXP = currentXP + xp;

    // Update XP
    server.UpdateUserData({
        PlayFabId: currentPlayerId,
        Data: { "TotalXP": newXP.toString() }
    });

    return { xp: xp, newTotal: newXP };
};
```

---

#### 5. Analytics & Events
**Tracked Events:**
- `account_created`
- `tutorial_completed`
- `first_match_played`
- `level_up`
- `rank_up`
- `lesson_completed`
- `daily_challenge_completed`
- `streak_milestone` (7/30/100 days)
- `premium_purchased`
- `match_abandoned`
- `app_crash`

**Custom Event Example:**
```csharp
var eventData = new Dictionary<string, object>
{
    { "level", 12 },
    { "xp", 5430 }
};
PlayFabClientAPI.WritePlayerEvent(new WriteClientPlayerEventRequest
{
    EventName = "level_up",
    Body = eventData
}, OnEventSuccess, OnEventFailure);
```

**Analytics Dashboard:**
- PlayFab Game Manager: Real-time event stream
- Custom reports: Funnels, cohorts, retention curves

---

#### 6. Push Notifications
**Scheduled Tasks:**
- Daily 8 PM local time: Check if player opened app today
- If not → Send push notification: "Don't lose your X-day streak!"

**Implementation:**
- PlayFab Scheduled Tasks (CloudScript runs on schedule)
- Unity Mobile Notifications (local notifications)
- Player timezone stored in PlayFab (detect on first login)

**Push Notification Flow:**
```
8 PM Local Time (per player timezone)
    ↓
PlayFab CloudScript runs: CheckStreakStatus()
    ↓
If player hasn't opened app today:
    ↓
Queue push notification → Unity Notifications Service
    ↓
Notification delivered to device
    ↓
Player taps notification → App opens
```

---

## 💰 Monetization Tech Stack

### Unity IAP (In-App Purchases)
**Purpose:** Handle App Store / Google Play subscriptions

**Products:**
- `premium_monthly`: $6.99/month subscription
- `premium_yearly`: $59.99/year subscription (30% savings)

**Features:**
- 7-day free trial (first-time subscribers)
- Auto-renewal
- Cancellation support
- Receipt validation (prevent piracy)

---

### RevenueCat (Subscription Management)
**Purpose:** Cross-platform subscription status, analytics, paywall optimization

**Why RevenueCat:**
- ✅ Free tier (up to $2.5M tracked revenue)
- ✅ Handles subscription edge cases (grace periods, billing retries)
- ✅ Cross-platform (iOS subscription works on Android after login)
- ✅ Analytics (MRR, churn, LTV)
- ✅ A/B testing paywalls

**Integration:**
- RevenueCat SDK wraps Unity IAP
- Subscription status synced to PlayFab (server-side entitlement checks)

**Entitlement Flow:**
```
Player purchases Premium
    ↓
Unity IAP completes transaction
    ↓
RevenueCat receives receipt
    ↓
RevenueCat validates with App Store/Google Play
    ↓
RevenueCat updates subscription status
    ↓
Unity client checks: Purchases.GetCustomerInfo()
    ↓
If "premium" entitlement active:
    ↓
Unlock Premium features
    ↓
Sync to PlayFab (server validates on each login)
```

---

### Unity Ads (Advertising)
**Purpose:** Monetize free users with interstitial ads

**Ad Placement:**
- After casual match (not ranked)
- Frequency cap: Max 1 ad per 5 minutes
- Premium users: No ads

**Ad Types:**
- Interstitial (full-screen, skippable after 5 sec)
- Rewarded video (optional, watch for bonus XP) - Future feature

**Implementation:**
- Unity Ads SDK
- Ad Manager script checks Premium status before showing

---

## 📱 Platform-Specific Considerations

### iOS
**Requirements:**
- Xcode 14+ (for Unity builds)
- Apple Developer Account ($99/year)
- App Store Connect setup
- Privacy Policy URL (required for App Store submission)
- Age Rating: 4+ (ESRB: Everyone)

**iOS-Specific Features:**
- Sign in with Apple (required if offering other social logins)
- Push Notifications (APNS integration)
- TestFlight for beta testing

**Performance Targets:**
- 60 FPS on iPhone 8+ (A11 chip)
- <150 MB app size
- <5 sec cold start time

---

### Android
**Requirements:**
- Android Studio (for build troubleshooting)
- Google Play Console account ($25 one-time)
- Privacy Policy URL
- Age Rating: Everyone (PEGI: 3)

**Android-Specific Features:**
- Google Sign-In
- Push Notifications (FCM integration)
- Google Play Beta Track for testing

**Performance Targets:**
- 60 FPS on mid-range devices (Snapdragon 660+)
- <200 MB app size (APK + OBB)
- <5 sec cold start time

**Fragmentation Mitigation:**
- Test on min spec device (Android 7.0, 2GB RAM)
- Quality settings auto-detect (low/medium/high)
- Fallback shaders for older GPUs

---

### WebGL
**Browser Support:**
- Chrome 90+
- Firefox 88+
- Safari 14+ (iOS Safari)
- Edge 90+

**Limitations:**
- No threading (Unity's WebGL constraint)
- Larger build size (~50-100 MB compressed)
- No push notifications
- No local file access (cloud save only)

**Optimizations:**
- Brotli compression
- Asset bundles for lazy loading
- Simple graphics (primitives for MVF help here)

**Use Cases:**
- Viral sharing (play instantly via link)
- Classroom use (no install needed)
- Desktop players without mobile devices

---

## 🧪 Testing & QA

### Automated Testing
**Unit Tests:**
- Oware game logic (NUnit)
- ELO calculation
- XP/Level progression
- Rule validation

**Integration Tests:**
- PlayFab authentication flow
- Cloud save/load
- Leaderboard updates
- Purchase validation

---

### Manual Testing
**Test Devices:**
- iOS: iPhone 8, iPhone 13 (min + modern)
- Android: Low-end (2GB RAM) + High-end (8GB RAM)
- WebGL: Chrome desktop

**Test Scenarios:**
- Complete onboarding flow
- Play all game modes
- Test all progression systems
- Verify Premium benefits
- Network interruption handling (airplane mode)
- Device switching (cloud save sync)

---

### Beta Testing
**Phases:**
- **Alpha:** Internal team (10-20 testers)
- **Closed Beta:** Friends & family (100-200 testers)
- **Open Beta:** Public (1000+ testers)

**Feedback Collection:**
- In-game feedback form
- Discord community
- Analytics (crash logs, event tracking)

---

## 🔒 Security Considerations

### Anti-Cheat
- **Server-Authoritative:** All progression updates happen server-side (PlayFab CloudScript)
- **Move Validation:** Server validates Oware moves (prevent illegal moves via network manipulation)
- **Receipt Validation:** RevenueCat validates IAP receipts (prevent piracy)
- **Rate Limiting:** PlayFab API calls rate-limited (prevent spam)

### Privacy
- **GDPR Compliance:** Privacy policy, data deletion requests supported
- **COPPA Compliance:** No free text chat (ages 7+ safe), parental consent flow (future)
- **Data Encryption:** PlayFab uses HTTPS, player data encrypted at rest

---

## 📊 Performance Targets

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

---

## 🚀 Deployment Pipeline

### CI/CD (Future Enhancement)
**Tools:**
- Unity Cloud Build (or GitHub Actions)
- Automated builds on Git push
- TestFlight/Google Play auto-upload

**Pipeline:**
```
Git Push to main branch
    ↓
CI triggers Unity build (iOS + Android + WebGL)
    ↓
Run automated tests
    ↓
If tests pass:
    ↓
Upload iOS build to TestFlight
Upload Android build to Google Play Beta Track
Deploy WebGL to hosting (Firebase Hosting or AWS S3)
    ↓
Slack notification: "Build X.Y.Z deployed to beta"
```

---

## 🔧 Development Tools

### Required SDKs/Plugins
- [x] Unity 2022 LTS
- [x] Photon Fusion SDK
- [x] PlayFab SDK
- [x] Unity IAP
- [x] RevenueCat SDK
- [x] Unity Ads SDK
- [x] TextMeshPro (UI text)
- [x] DOTween (animation)
- [x] Unity Mobile Notifications
- [x] Native Share Plugin (iOS/Android sharing)

### Optional Tools
- [ ] Unity Profiler (performance debugging)
- [ ] Unity Test Framework (unit tests)
- [ ] Odin Inspector (editor enhancements) - Premium, optional
- [ ] Visual Studio Code or Rider (IDE)

---

**Document Version:** 1.0
**Last Updated:** 2025-10-14
**Status:** Tech stack finalized

*"Build it right, scale it fast"*
