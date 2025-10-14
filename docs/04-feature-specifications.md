# Feature Specifications - Social Oware Academy

Detailed specifications for all features organized by release phase (MVF → V1.x → V2.0).

---

## 📦 Release Phases

```
MVF (Minimum Viable Fun)
└── Ship-critical features for complete experience

V1.1 (Content Expansion)
└── Achievements, puzzle archive, analytics

V1.2 (Community Growth)
└── Tournaments, clubs/guilds, enhanced social

V1.3 (Competitive Depth)
└── Spectator mode, replay system, AI analysis

V2.0 (Monetization Expansion)
└── Battle Pass, seasonal content, variants
```

---

## 🎯 MVF (Minimum Viable Fun) Features

### 1. Core Gameplay

#### 1.1 Oware Game Logic
**Priority:** P0 (Blocker)

**Specification:**
- Standard Ghanaian Oware rules implementation
- 12-pit board (6 per player)
- Counter-clockwise sowing
- Capture on 2-3 seeds in opponent's pit
- Grand Slam rule enforcement
- Win condition: 25+ seeds captured

**Technical Requirements:**
- Unity C# game logic class
- Board state management
- Move validation system
- AI opponent integration (3 difficulty levels)
- Networked synchronization (Photon)

**Acceptance Criteria:**
- [ ] All Oware rules correctly implemented
- [ ] Board state updates in real-time
- [ ] Move validation prevents illegal moves
- [ ] Win conditions correctly detected
- [ ] Grand Slam rule prevents full capture
- [ ] Unit tests cover all edge cases

---

#### 1.2 AI Opponents (3 Difficulty Levels)
**Priority:** P0 (Blocker)

**Specification:**

**Beginner AI:**
- Random move selection with 30% "good move" probability
- No lookahead
- Target: Beatable by new players within 3-5 games

**Intermediate AI:**
- Minimax algorithm (depth 3)
- Evaluates captures and position
- Target: Challenging for casual players

**Advanced AI (Premium):**
- Minimax with alpha-beta pruning (depth 5-7)
- Advanced position evaluation
- Opening book (5 common openings)
- Target: Challenging for experienced players

**Technical Requirements:**
- AI decision-making in separate thread (no UI blocking)
- Move execution delay (0.5-1.5 sec) for human-like feel
- Difficulty stored in player profile

**Acceptance Criteria:**
- [ ] Beginner AI beatable by tutorial-completed players
- [ ] Intermediate AI wins ~60% vs Beginner AI
- [ ] Advanced AI wins ~80% vs Intermediate AI
- [ ] AI responds within 2 seconds on mobile devices
- [ ] No UI freezing during AI computation

---

### 2. Progression Systems

#### 2.1 Player Level System
**Priority:** P0 (Blocker)

**Specification:**
- XP earned from all activities (see [03-core-loop-mechanics.md](03-core-loop-mechanics.md))
- Exponential level curve (Level 2 = 100 XP, Level 50 = 50,000 XP)
- Unlocks tied to levels (see progression doc)
- Level displayed on player profile and leaderboards

**Technical Requirements:**
- PlayFab player data storage
- XP calculation system
- Level-up animation/notification
- Unlock check system

**Acceptance Criteria:**
- [ ] XP correctly awarded for all activities
- [ ] Level progression follows exponential curve
- [ ] Level-up triggers unlock checks
- [ ] Level-up celebration animation plays
- [ ] Progress syncs across devices (PlayFab cloud save)

---

#### 2.2 ELO Rating & Rank Tiers
**Priority:** P0 (Blocker)

**Specification:**
- Standard ELO formula (K-factor: 32 new players, 16 established)
- Starting rating: 1200 (after 5 placement matches)
- 6 rank tiers (Bronze → Master)
- Promotion/demotion mechanics (see core loop doc)

**Technical Requirements:**
- ELO calculation server-side (prevent cheating)
- PlayFab leaderboards integration
- Rank badge assets (6 tiers)
- Rank-up/down animation

**Acceptance Criteria:**
- [ ] ELO correctly calculates after each ranked match
- [ ] Placement matches determine initial rating
- [ ] Rank badges display correctly
- [ ] Promotion match triggers at tier threshold
- [ ] Demotion protection (3-game grace period) works
- [ ] Rating syncs to PlayFab leaderboards

---

#### 2.3 Lesson Progress System
**Priority:** P0 (Blocker)

**Specification:**
- 15 lessons total (5 free, 5 Level 10+, 5 Premium)
- Each lesson: Explanation + 3-5 interactive puzzles
- Star rating (1-3 stars per lesson)
- Replay anytime to improve stars
- Mastery % tracking

**Technical Requirements:**
- Lesson content data (JSON format)
- Interactive puzzle board
- Star evaluation logic
- Progress tracking (PlayFab)

**Acceptance Criteria:**
- [ ] All 5 free lessons playable
- [ ] Interactive puzzles correctly evaluate moves
- [ ] Stars awarded based on performance
- [ ] Lessons 6-10 locked until Level 10
- [ ] Lessons 11-15 locked behind Premium paywall
- [ ] Mastery % calculates correctly

---

#### 2.4 Streak System
**Priority:** P0 (Blocker)

**Specification:**
- Daily streak counter
- Any activity maintains streak (login bonus counts)
- Streak milestones (7, 30, 100, 365 days)
- Streak freeze mechanic (Premium: 2/month, Free: earn 1/week)
- Push notification reminder (8 PM local time)

**Technical Requirements:**
- Server-side date tracking (prevent clock manipulation)
- PlayFab scheduled tasks (check streak status daily)
- Push notification system (Unity Notifications)
- Streak freeze inventory

**Acceptance Criteria:**
- [ ] Streak increments correctly each day
- [ ] Login bonus maintains streak
- [ ] Midnight UTC = day boundary
- [ ] Streak freeze prevents break (if available)
- [ ] Push notification sends at 8 PM local if not played
- [ ] Milestone rewards granted at 7/30/100/365 days

---

### 3. Game Modes

#### 3.1 Casual Play
**Priority:** P0 (Blocker)

**Specification:**
- Play vs AI (3 difficulties) or friends
- No rating changes
- XP earned: 50 (win), 20 (loss)
- Ads between matches (Free) or ad-free (Premium)

**Technical Requirements:**
- Match type selection UI
- AI opponent selection
- Friend selection (if friend system enabled)
- Ad integration (Unity Ads)

**Acceptance Criteria:**
- [ ] Can start match vs Beginner/Intermediate/Advanced AI
- [ ] Can start match vs friend (if online)
- [ ] No rating changes recorded
- [ ] XP awarded correctly
- [ ] Ads display between matches (Free users only)

---

#### 3.2 Ranked Play
**Priority:** P0 (Blocker)

**Specification:**
- Unlocked at Level 5
- Matchmaking: ±100 rating points
- ELO rating changes
- XP earned: 75 (win), 30 (loss)
- Leave penalty (-50 rating, 3 abandons = 1-hour ban)

**Technical Requirements:**
- Matchmaking service (Photon matchmaking or custom)
- Rating change calculation
- Abandon detection
- Ban system (time-based)

**Acceptance Criteria:**
- [ ] Ranked mode locked until Level 5
- [ ] Matchmaking finds opponents within ±100 rating
- [ ] If no match in 90 sec, expands search
- [ ] Rating updates after match completion
- [ ] Abandoning match applies penalty
- [ ] 3 abandons in 24 hours = 1-hour ban

---

#### 3.3 Training Mode (Lessons)
**Priority:** P0 (Blocker)

**Specification:**
- 15 interactive lessons
- Each lesson: Text explanation + Interactive puzzles + Summary
- Stars awarded (1-3) based on performance
- XP reward: 40 XP per lesson completion

**Technical Requirements:**
- Lesson UI flow (intro → puzzles → summary)
- Puzzle validation logic
- Star calculation
- Lesson unlock checks

**Acceptance Criteria:**
- [ ] Lesson list displays with lock/unlock status
- [ ] Lesson intro text displays with visuals
- [ ] Puzzles evaluate correctly (correct/incorrect feedback)
- [ ] Stars awarded based on puzzle performance
- [ ] Completion grants 40 XP
- [ ] Can replay lessons to improve stars

---

#### 3.4 Daily Puzzle
**Priority:** P0 (Blocker)

**Specification:**
- One puzzle per day (rotates midnight UTC)
- Tactical puzzle format: "Capture X seeds in Y moves"
- Timer tracks solve speed
- Leaderboards: Global top 100, Friends
- XP reward: 60 XP base + bonuses

**Technical Requirements:**
- Puzzle rotation system (server-managed)
- Puzzle content library (100+ puzzles pre-made)
- Timer integration
- Leaderboard submission (PlayFab)

**Acceptance Criteria:**
- [ ] New puzzle appears at midnight UTC
- [ ] Puzzle displays with objective ("Capture 12 seeds in 3 moves")
- [ ] Timer starts when puzzle begins
- [ ] Solution validated correctly
- [ ] Solve time submitted to leaderboard
- [ ] 60 XP granted on completion
- [ ] Bonus XP if top 100 global or #1 friends

---

#### 3.5 Private Lobbies
**Priority:** P0 (Blocker)

**Specification:**
- Create lobby → Generate 8-character room code (e.g., "ABCD-1234")
- Share code via external means (WhatsApp, text, etc.)
- Lobby settings: Time limit per move (optional), Rematch (yes/no)
- Max 2 players per lobby
- Free: 3 active lobbies, Premium: Unlimited

**Technical Requirements:**
- Room code generation (alphanumeric, collision-resistant)
- Photon private room creation
- Lobby settings UI
- Active lobby tracking

**Acceptance Criteria:**
- [ ] Can create private lobby with generated code
- [ ] Code displays clearly for sharing
- [ ] Entering code joins correct lobby
- [ ] Lobby settings apply to match
- [ ] Free users limited to 3 active lobbies
- [ ] Premium users have unlimited lobbies

---

### 4. Social Features

#### 4.1 Friend System
**Priority:** P0 (Blocker)

**Specification:**
- Friend codes (format: "PLAYER#1234")
- Add friend via code
- Friend list displays:
  - Username
  - Online status (green dot)
  - Level + Rank badge
  - Last online (if offline)
- Direct challenge button (if online)

**Technical Requirements:**
- PlayFab friend system integration
- Unique player ID generation
- Online presence tracking
- Friend invite/accept flow

**Acceptance Criteria:**
- [ ] Each player has unique friend code
- [ ] Can add friend via code entry
- [ ] Friend requests require acceptance
- [ ] Friend list displays correctly
- [ ] Online status updates in real-time
- [ ] Challenge button enabled if friend online

---

#### 4.2 Leaderboards
**Priority:** P0 (Blocker)

**Specification:**
- **Global Leaderboard:** Top 100 players by ELO rating
- **Friends Leaderboard:** All friends ranked by ELO
- **Country Leaderboard:** Top 100 in player's country (optional)
- Refresh frequency: Real-time (after each match)

**Technical Requirements:**
- PlayFab leaderboards (3 separate leaderboards)
- Player country detection (IP geolocation)
- Leaderboard UI (scrollable list)

**Acceptance Criteria:**
- [ ] Global leaderboard displays top 100
- [ ] Friends leaderboard shows all friends
- [ ] Player's rank shown even if outside top 100
- [ ] Leaderboards update after ranked matches
- [ ] Can tap player to view profile (future feature)

---

#### 4.3 Sharing System
**Priority:** P0 (Blocker)

**Specification:**
- Share buttons for:
  - Match result: "I just won against a 1500-rated player! 🎉"
  - Streak milestone: "20-day Oware streak! 🔥"
  - Rank-up: "Just hit Gold rank in Social Oware Academy! 💪"
- Auto-generated image with:
  - Player username
  - Achievement details
  - Game branding
  - Deep link to app download
- Share to: Instagram, TikTok, WhatsApp, Twitter, Facebook, Copy Link

**Technical Requirements:**
- Unity Native Share plugin
- Image generation (render texture)
- Deep link integration (Branch.io or Unity Deep Linking)
- Platform-specific share sheets

**Acceptance Criteria:**
- [ ] Share button appears after achievements
- [ ] Image generated with correct branding
- [ ] Deep link redirects to app download/profile
- [ ] Share sheet works on iOS and Android
- [ ] Shared content tracks attribution (analytics)

---

#### 4.4 Safe Emotes (No Text Chat)
**Priority:** P0 (Blocker)

**Specification:**
- Preset emotes only (no free text)
- Emote list:
  - "Good game! 👍"
  - "Nice move! 😊"
  - "Thanks! 🙏"
  - "Rematch? 🔄"
  - "Thinking... 🤔"
  - "Wow! 😮"
- Emote cooldown: 3 seconds (prevent spam)
- Report player option (for abuse)

**Technical Requirements:**
- Emote UI (chat bubble)
- Network sync (Photon RPC)
- Cooldown timer
- Report system (logs to PlayFab)

**Acceptance Criteria:**
- [ ] Emote menu accessible during match
- [ ] Emotes display in opponent's UI
- [ ] Cooldown prevents spam (3 sec)
- [ ] No way to enter custom text
- [ ] Report button logs player ID for review

---

### 5. Daily Engagement

#### 5.1 Login Bonus
**Priority:** P0 (Blocker)

**Specification:**
- Trigger: Open app
- Reward: +10 XP, streak maintained
- UI: Small popup on home screen ("Welcome back! +10 XP, 12-day streak 🔥")

**Technical Requirements:**
- Server timestamp check (prevent clock manipulation)
- PlayFab scheduled task (mark day as "played")
- XP grant via PlayFab CloudScript

**Acceptance Criteria:**
- [ ] Login bonus grants 10 XP on first open each day
- [ ] Streak increments on first open
- [ ] UI notification displays briefly
- [ ] No bonus on subsequent opens same day

---

#### 5.2 Daily Challenge
**Priority:** P0 (Blocker)

**Specification:**
- Refreshes midnight UTC
- Challenge types (rotate daily):
  - "Win 2 matches" → 50 bonus XP
  - "Complete 1 lesson" → 40 bonus XP
  - "Solve daily puzzle in <2 min" → 80 bonus XP
  - "Play 1 match with friend" → 60 bonus XP
- 3x XP multiplier on challenge-related activities
- UI: Prominent banner on home screen

**Technical Requirements:**
- Challenge rotation system (server-managed)
- Progress tracking per challenge type
- XP multiplier logic
- Completion detection

**Acceptance Criteria:**
- [ ] New challenge appears at midnight UTC
- [ ] Challenge progress updates in real-time
- [ ] Completion grants bonus XP
- [ ] 3x multiplier applies to relevant activities
- [ ] Banner displays challenge + progress

---

#### 5.3 Push Notifications (Streak Reminder)
**Priority:** P0 (Blocker)

**Specification:**
- Trigger: 8 PM local time IF player hasn't opened app today
- Message: "Don't lose your [X]-day streak! Play a quick match 🔥"
- Frequency: Max 1 per day
- User can disable in settings

**Technical Requirements:**
- Unity Mobile Notifications (iOS/Android)
- PlayFab scheduled task (check streak status at 8 PM each timezone)
- Player timezone detection
- Opt-out preference storage

**Acceptance Criteria:**
- [ ] Notification sends at 8 PM local time
- [ ] Only sends if player hasn't played today
- [ ] Displays streak count in message
- [ ] Tapping notification opens app
- [ ] Can disable in settings
- [ ] Respects OS notification permissions

---

### 6. Onboarding

#### 6.1 Rule Selection Screen
**Priority:** P0 (Blocker)

**Specification:**
- First-time user experience
- 3 buttons:
  - "New to Oware" → Guided first match
  - "I know the rules" → Skip to match selection
  - "Expert player" → Skip to main menu
- Can replay tutorial anytime from settings

**Technical Requirements:**
- First-launch detection (PlayerPrefs)
- Tutorial state storage
- Settings menu option to replay

**Acceptance Criteria:**
- [ ] Rule selection appears on first launch
- [ ] "New to Oware" starts guided tutorial
- [ ] "I know rules" skips to quick rule reminder
- [ ] "Expert" skips directly to menu
- [ ] Tutorial replayable from settings

---

#### 6.2 Guided First Match
**Priority:** P0 (Blocker)

**Specification:**
- For "New to Oware" players
- AI walks player through one complete game
- Tooltips explain each step:
  - "Tap a pit on your side to select"
  - "Seeds sow counter-clockwise"
  - "You captured 3 seeds! 🎉"
- Guaranteed win (AI makes suboptimal moves)
- Duration: 2-3 minutes

**Technical Requirements:**
- Tutorial AI (intentionally weak)
- Tooltip system (pointer arrows + text)
- Step-by-step flow (advance on player action)
- Completion reward: 50 XP + "Tutorial Complete" achievement

**Acceptance Criteria:**
- [ ] Tutorial plays smoothly without bugs
- [ ] Tooltips appear at correct moments
- [ ] Player always wins tutorial match
- [ ] Completion grants 50 XP
- [ ] After tutorial, rule selection doesn't reappear

---

### 7. UI/UX

#### 7.1 Home Screen
**Priority:** P0 (Blocker)

**Specification:**
- **Header:**
  - Player profile (avatar, username, level, XP bar)
  - Settings button (top-right)
  - Friend icon (notifications badge if requests)
- **Main Content:**
  - Daily challenge banner (progress bar)
  - Play buttons: [Casual] [Ranked] [Training] [Daily Puzzle]
  - Streak display: "12-day streak 🔥"
- **Footer:**
  - Leaderboard button
  - Profile button
  - Shop button (Premium upsell)

**Design Principles:**
- Clean, minimalist (stunningly beautiful goal)
- Primary actions above the fold
- Quick access to all game modes

**Acceptance Criteria:**
- [ ] All elements display correctly
- [ ] Buttons navigate to correct screens
- [ ] XP bar updates in real-time
- [ ] Daily challenge banner updates

---

#### 7.2 Match UI (In-Game)
**Priority:** P0 (Blocker)

**Specification:**
- **Board:**
  - 12 pits (6 per player, clear visual separation)
  - Seed count visible in each pit
  - Captured seeds counter (both players)
- **HUD:**
  - Player names + ratings (ranked) or levels (casual)
  - Turn indicator (glowing player name)
  - Timer (if time limit enabled)
- **Controls:**
  - Tap pit to select (highlights valid selections)
  - Confirm/Cancel buttons (optional, for clarity)
  - Emote button (bottom-right)
  - Menu button (pause/forfeit options)

**Design Principles:**
- Board is focal point (center screen)
- Clear turn indication (no confusion)
- Accessible emotes (social engagement)

**Acceptance Criteria:**
- [ ] Board renders correctly on all screen sizes
- [ ] Pit selection highlights clearly
- [ ] Seed sowing animates smoothly
- [ ] Captures display with visual flair
- [ ] Turn indicator obvious
- [ ] Emotes display without blocking board

---

#### 7.3 Lesson UI
**Priority:** P0 (Blocker)

**Specification:**
- **Lesson List Screen:**
  - 15 lessons in grid/list
  - Lock icons on unavailable lessons
  - Star rating (0-3 stars) on completed lessons
  - Mastery % at top
- **Lesson Flow:**
  1. Intro screen (text + visuals)
  2. Interactive puzzle (Oware board)
  3. Feedback screen (correct/incorrect + explanation)
  4. Summary screen (stars earned, XP reward)

**Design Principles:**
- Clear progression path
- Immediate feedback on puzzles
- Encouraging tone (not punishing)

**Acceptance Criteria:**
- [ ] Lesson list shows lock/unlock status
- [ ] Tapping locked lesson shows unlock requirement
- [ ] Lesson flow progresses smoothly
- [ ] Puzzles evaluate correctly
- [ ] Stars display on completion

---

### 8. Backend Integration

#### 8.1 PlayFab Authentication
**Priority:** P0 (Blocker)

**Specification:**
- Login methods:
  - Email + password
  - Google Sign-In (Android)
  - Apple Sign-In (iOS, required)
  - Facebook Login
  - Play as Guest (local account, prompt to link later)
- Account linking (upgrade guest to full account)

**Technical Requirements:**
- PlayFab SDK integration
- OAuth flows for social logins
- Guest account upgrade flow

**Acceptance Criteria:**
- [ ] Can create account with email/password
- [ ] Can login with Google (Android)
- [ ] Can login with Apple (iOS)
- [ ] Can login with Facebook
- [ ] Guest accounts work offline
- [ ] Guest can upgrade to full account without losing progress

---

#### 8.2 Cloud Save System
**Priority:** P0 (Blocker)

**Specification:**
- Auto-save after every activity (match, lesson, etc.)
- Sync on app launch (load latest data)
- Conflict resolution: Server wins (prevent rollbacks)
- Offline mode: Queue actions, sync when online

**Technical Requirements:**
- PlayFab Player Data API
- Save data structure (JSON)
- Delta sync (only changed data)

**Data to Sync:**
- Player Level + XP
- ELO Rating + Rank
- Lesson progress + Stars
- Streak count
- Achievement progress
- Settings preferences

**Acceptance Criteria:**
- [ ] Progress saves after each activity
- [ ] Logging in on new device loads latest progress
- [ ] No progress loss on device switch
- [ ] Offline play queues actions
- [ ] Online sync resolves conflicts (server wins)

---

#### 8.3 Analytics Integration
**Priority:** P0 (Blocker)

**Specification:**
- Track key events:
  - Account creation
  - Tutorial completion
  - First match played
  - Level up
  - Rank up
  - Lesson completed
  - Daily challenge completed
  - Streak milestone (7/30/100 days)
  - Premium purchase
  - Match abandoned
  - App crash

**Technical Requirements:**
- PlayFab Analytics
- Unity Analytics (supplemental)
- Custom event system

**Metrics to Track:**
- **Acquisition:** Installs, attribution source
- **Engagement:** DAU, MAU, session length, sessions per day
- **Retention:** D1, D7, D30 retention cohorts
- **Monetization:** Premium conversions, ARPU, LTV

**Acceptance Criteria:**
- [ ] All key events tracked correctly
- [ ] Events appear in PlayFab dashboard
- [ ] Funnels configured (tutorial → first match → D7 retention)
- [ ] Crash logs captured and uploaded

---

### 9. Monetization (MVF)

#### 9.1 Premium Subscription
**Priority:** P0 (Blocker)

**Specification:**
- Pricing: $6.99/month or $59.99/year (30% savings)
- 7-day free trial (first-time subscribers)
- Benefits:
  - Ad-free experience
  - 2x XP multiplier
  - 2 streak freezes per month
  - Advanced AI opponent
  - Advanced lesson tier (lessons 11-15)
  - Puzzle archive access
  - Priority matchmaking
  - Premium badge on profile

**Technical Requirements:**
- Unity IAP
- RevenueCat (subscription management)
- Paywall UI (strategic placement)
- Entitlement checks (server-side)

**Acceptance Criteria:**
- [ ] Can subscribe on iOS and Android
- [ ] 7-day free trial applies correctly
- [ ] Subscription status syncs across devices
- [ ] All Premium benefits unlock immediately
- [ ] Subscription can be canceled (retains benefits until end of period)
- [ ] Failed payment shows renewal prompt

---

#### 9.2 Paywall Placement
**Priority:** P0 (Blocker)

**Specification:**
- **Soft paywall locations:**
  1. After completing free lessons: "Unlock 5 more advanced lessons!"
  2. After 3rd ad view: "Go ad-free with Premium!"
  3. When streak breaks: "Premium includes 2 streak freezes per month!"
  4. After reaching Level 10: "Speed up your progress with 2x XP!"
  5. After losing 3 ranked matches: "Premium includes rating protection!"

**Design Principles:**
- Value-driven (show benefits, not just "pay")
- Contextual (offer at moments of pain/desire)
- Dismissible (not blocking)
- No dark patterns (clear cancellation, honest trial terms)

**Acceptance Criteria:**
- [ ] Paywall triggers at correct moments
- [ ] Displays Premium benefits clearly
- [ ] "Start Free Trial" button prominent
- [ ] Can dismiss without subscribing
- [ ] No deceptive language or hidden fees

---

## 🚀 Post-MVF Features (Roadmap)

### V1.1 - Content Expansion (Month 2-3)

#### Achievement System
- 30-40 achievements
- Badge showcase on profile
- Achievement notifications
- Rare achievements for prestige

#### Puzzle Archive (Premium)
- Access past 365 days of daily puzzles
- Filter by difficulty
- Practice mode (no leaderboard)

#### Enhanced Analytics Dashboard
- Personal stats page:
  - Win/loss record
  - Most played opponents
  - Favorite openings
  - Session history

---

### V1.2 - Community Growth (Month 4-5)

#### Tournaments
- Weekly Swiss-system tournaments
- Prize pool (Premium currency, cosmetics)
- Tournament leaderboards
- Spectator mode during finals

#### Clubs/Guilds
- Create/join clubs (max 50 members)
- Club chat (adults only, moderation tools)
- Club leaderboards
- Team tournaments

#### Enhanced Friend Features
- Activity feed (friend rank-ups, achievements)
- Friend profiles (stats comparison)
- Friend recommendations (based on skill level)

---

### V1.3 - Competitive Depth (Month 6-7)

#### Spectator Mode
- Watch live top-ranked matches
- Replay famous games
- Spectator chat (moderated)

#### Replay System
- Save match replays
- Share replay links
- Analysis mode (review moves)

#### AI Game Analysis (Premium)
- Post-game AI review
- Highlight critical moves
- Suggest improvements
- "You could have captured here" tooltips

---

### V2.0 - Monetization Expansion (Month 8+)

#### Battle Pass System
- 3-month seasons
- Free track + Premium track
- 50 tiers of rewards:
  - Cosmetics (board skins, avatars)
  - XP boosts
  - Exclusive lessons
  - Premium currency
- FOMO-driven engagement

#### Seasonal Content
- Seasonal challenges
- Themed cosmetics (cultural festivals)
- Limited-time game modes

#### Oware Variants
- Kalah (American Mancala)
- Bao (East African variant)
- Omweso (Ugandan variant)
- Separate leaderboards per variant
- Educational content about each variant

---

## 📊 Feature Priority Matrix

### P0 (Blocker - Must Have for MVF):
- Core Oware game logic
- AI opponents (3 levels)
- Player Level system
- ELO Rating & Ranks
- Lesson Progress (5 free lessons)
- Streak System
- Casual + Ranked + Training + Daily Puzzle modes
- Private Lobbies
- Friend System
- Leaderboards
- Sharing System
- Safe Emotes
- Login Bonus + Daily Challenge
- Push Notifications
- Onboarding (Rule Selection + Guided Match)
- Core UI (Home, Match, Lesson screens)
- PlayFab (Auth + Cloud Save + Analytics)
- Premium Subscription + Paywall

### P1 (High Priority - MVF Nice-to-Have):
- Achievement System (deferred to V1.1)
- Puzzle Archive (Premium, deferred to V1.1)
- Enhanced stats dashboard (deferred to V1.1)

### P2 (Post-Launch):
- Tournaments (V1.2)
- Clubs/Guilds (V1.2)
- Spectator Mode (V1.3)
- Replay System (V1.3)
- AI Analysis (V1.3 Premium)
- Battle Pass (V2.0)
- Variants (V2.0)

---

**Document Version:** 1.0
**Last Updated:** 2025-10-14
**Status:** MVF scope locked

*"Ship the fun, iterate the fancy"*
