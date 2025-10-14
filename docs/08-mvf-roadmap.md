# MVF Development Roadmap - Social Oware Academy

Sprint-by-sprint development plan to ship Minimum Viable Fun in 16 weeks.

---

## 🎯 MVF Definition

**Minimum Viable Fun (MVF) = Smallest shippable version that delivers core emotional experience**

### MVF Must-Haves:
✅ Core Oware gameplay (local + networked)
✅ AI opponents (3 difficulties)
✅ Progression systems (Level, ELO, Lessons, Streaks)
✅ 5 game modes (Casual, Ranked, Training, Daily Puzzle, Private Lobbies)
✅ Social features (Friends, Leaderboards, Sharing, Emotes)
✅ Daily engagement (Login bonus, daily challenge, push notifications)
✅ Onboarding (Rule selection, guided tutorial)
✅ Premium subscription + paywall
✅ Cross-platform (iOS, Android, WebGL)
✅ Backend (PlayFab authentication, cloud save, analytics)

### Post-MVF (Deferred to V1.1+):
❌ Achievements (V1.1)
❌ Tournaments (V1.2)
❌ Clubs/Guilds (V1.2)
❌ Spectator mode (V1.3)
❌ Replay system (V1.3)
❌ AI analysis (V1.3)
❌ Battle Pass (V2.0)

---

## 📅 Timeline Overview (16 Weeks)

```
Week 1-2:   Foundation & Setup
Week 3-4:   Core Gameplay (Oware Logic + AI)
Week 5-6:   Multiplayer Infrastructure
Week 7-8:   Progression Systems
Week 9-10:  UI/UX Implementation
Week 11-12: Social Features & Daily Engagement
Week 13-14: Monetization & Polish
Week 15-16: Testing, Bug Fixes & Launch Prep
```

**Target Launch Date:** Week 17 (Soft launch) → Week 18 (Global launch)

---

## 🛠️ Detailed Sprint Plan

### **SPRINT 1-2: Foundation & Setup** (Weeks 1-2)

#### Goals:
- Unity project configured
- Third-party SDKs integrated
- Basic project structure in place
- First prototype playable (primitive visuals)

#### Tasks:

**Week 1: Project Setup**
- [ ] Install Unity 2022 LTS
- [ ] Create new Unity project (Universal Render Pipeline for mobile)
- [ ] Configure build settings (iOS, Android, WebGL)
- [ ] Set up Git repository + .gitignore
- [ ] Import Photon Fusion SDK
- [ ] Import PlayFab SDK
- [ ] Import TextMeshPro
- [ ] Import DOTween
- [ ] Create folder structure (see [05-technical-architecture.md](05-technical-architecture.md))
- [ ] Set up Bootstrap scene (manager initialization)

**Week 2: Core Managers**
- [ ] GameManager singleton (game state machine)
- [ ] NetworkManager (Photon wrapper)
- [ ] PlayFabManager (authentication skeleton)
- [ ] AudioManager (sound/music system)
- [ ] UIManager (screen state machine)
- [ ] AnalyticsManager (event tracking wrapper)
- [ ] Scene loading system (async scene management)
- [ ] First prototype: Empty board with 12 pits (Unity primitives - cubes/spheres)

**Deliverable:** Unity project with managers, can load game scene with empty board

---

### **SPRINT 3-4: Core Gameplay** (Weeks 3-4)

#### Goals:
- Oware game logic fully implemented
- AI opponents working (all 3 difficulties)
- Local 1v1 playable (no network)
- Win/loss conditions functioning

#### Tasks:

**Week 3: Game Logic**
- [ ] OwareBoard class (board state, pit data structure)
- [ ] OwareRules class (move validation, capture logic, grand slam rule)
- [ ] Move execution (sowing animation using DOTween)
- [ ] Capture detection and execution
- [ ] Win condition detection (25+ seeds captured)
- [ ] Unit tests for all rule edge cases
- [ ] Visual feedback (primitive seed objects, capture VFX placeholder)

**Week 4: AI Implementation**
- [ ] OwareAI base class (AI interface)
- [ ] Beginner AI (random moves with 30% "good move" heuristic)
- [ ] Intermediate AI (minimax depth 3)
- [ ] Advanced AI (minimax with alpha-beta pruning depth 5-7)
- [ ] AI move execution with delay (0.5-1.5s for human feel)
- [ ] AI difficulty testing and tuning
- [ ] Local PvAI match flow (player vs AI, complete game loop)

**Deliverable:** Fully playable Oware match (Player vs AI, any difficulty, local only)

---

### **SPRINT 5-6: Multiplayer Infrastructure** (Weeks 5-6)

#### Goals:
- Photon Fusion integrated
- Networked multiplayer functional (real-time PvP)
- Matchmaking system working
- Private lobbies with room codes

#### Tasks:

**Week 5: Photon Integration**
- [ ] Photon Fusion network scripts (NetworkPlayer, SyncedGameState)
- [ ] NetworkManager connects to Photon servers
- [ ] Room creation (casual, ranked, private)
- [ ] Player synchronization (board state syncs across network)
- [ ] RPC calls (MakeMove, SendEmote, RequestRematch, Forfeit)
- [ ] Turn-based state machine (host authoritative)
- [ ] Network error handling (disconnect, timeout)

**Week 6: Matchmaking & Lobbies**
- [ ] Matchmaking system (ELO-based, ±100 rating)
- [ ] Matchmaking queue UI (searching animation, "Opponent found!")
- [ ] Private lobby creation (8-char room code generation)
- [ ] Private lobby joining (code entry UI)
- [ ] Lobby settings (time limit per move, rematch option)
- [ ] Match result reporting to PlayFab (XP, rating changes)

**Deliverable:** Can play networked match with friend via room code, matchmaking finds opponents

---

### **SPRINT 7-8: Progression Systems** (Weeks 7-8)

#### Goals:
- Player Level system (XP, leveling, unlocks)
- ELO Rating system (ranked matches update rating)
- Lesson system (5 free lessons playable)
- Streak system (daily tracking)

#### Tasks:

**Week 7: Level & ELO Systems**
- [ ] LevelSystem class (XP calculation, level-up logic, exponential curve)
- [ ] XP grant system (match wins/losses, lessons, puzzles)
- [ ] Level-up animation/notification
- [ ] Unlock checks (features gated by level)
- [ ] ELOSystem class (rating calculation using standard ELO formula)
- [ ] Placement matches (5 matches to determine starting ELO)
- [ ] Rank tier badges (Bronze, Silver, Gold, Platinum, Diamond, Master)
- [ ] Rank-up/down animations
- [ ] PlayFab integration (save Level, XP, ELO to cloud)

**Week 8: Lessons & Streaks**
- [ ] LessonSystem class (lesson progress tracking, star rating)
- [ ] Lesson content (5 beginner lessons as JSON data)
- [ ] Lesson UI flow (intro → puzzle → feedback → summary)
- [ ] Interactive puzzle board (player solves puzzle, validates solution)
- [ ] Star calculation (3 stars = perfect, 2 = good, 1 = pass)
- [ ] StreakSystem class (daily streak tracking, freeze mechanic)
- [ ] Streak milestone rewards (7/30/100 day badges)
- [ ] Push notification system (Unity Mobile Notifications setup)
- [ ] Streak reminder notification (8 PM local time)

**Deliverable:** Player can level up, earn ELO rating in ranked matches, complete 5 lessons, maintain daily streak

---

### **SPRINT 9-10: UI/UX Implementation** (Weeks 9-10)

#### Goals:
- All core screens implemented
- Navigation flows working
- Onboarding complete (rule selection + guided tutorial)
- Responsive design (mobile portrait)

#### Tasks:

**Week 9: Core Screens**
- [ ] Home screen (header, daily challenge banner, play buttons, footer nav)
- [ ] Play mode selection modal (AI, Friend, Online options)
- [ ] Match screen (in-game board UI, HUD, emote button, menu)
- [ ] Lesson list screen (grid of lessons with lock/star indicators)
- [ ] Lesson flow screens (intro, puzzle, feedback, summary)
- [ ] Profile screen (avatar, stats, achievements placeholder)
- [ ] Leaderboard screen (tabs: Global, Friends, Country)
- [ ] Settings screen (sound, music, notifications toggles)

**Week 10: Onboarding & Polish**
- [ ] Splash screen (app launch animation)
- [ ] Rule selection screen (New/I Know Rules/Expert buttons)
- [ ] Guided tutorial match (AI walks player through first game)
- [ ] Tutorial tooltips (step-by-step instructions with arrows/pointers)
- [ ] First match completion celebration (XP reward, achievement unlock)
- [ ] Navigation state machine (screen transitions, back button handling)
- [ ] Responsive layout testing (iPhone SE, iPhone 13, Android devices)
- [ ] Loading screens (async scene loading with progress bar)

**Deliverable:** Complete UI flow from app launch → onboarding → home → play match → return home

---

### **SPRINT 11-12: Social Features & Daily Engagement** (Weeks 11-12)

#### Goals:
- Friend system functional (add friends, friend list, challenges)
- Leaderboards displaying correctly (Global, Friends)
- Sharing system (achievements to social media)
- Daily challenge system (refreshes daily)
- Login bonus (grants XP + maintains streak)

#### Tasks:

**Week 11: Social Features**
- [ ] Friend system (PlayFab friends API integration)
- [ ] Friend code generation (unique "PLAYER#1234" codes)
- [ ] Add friend flow (enter code, send request, accept/decline)
- [ ] Friend list UI (online status, challenge button)
- [ ] Direct friend challenge (creates private room, invites friend)
- [ ] Leaderboards (PlayFab leaderboards API integration)
- [ ] Global leaderboard (top 100 by ELO)
- [ ] Friends leaderboard (all friends ranked)
- [ ] Player rank display (shows own position even if outside top 100)
- [ ] Safe emotes (preset emote menu, 6 emotes, network sync)

**Week 12: Daily Engagement**
- [ ] Login bonus system (grants 10 XP on first open each day)
- [ ] Daily challenge system (server-managed challenge rotation)
- [ ] Daily challenge types (Win 2 matches, Complete lesson, Solve puzzle, Play with friend)
- [ ] Daily challenge progress tracking (UI updates in real-time)
- [ ] Daily challenge completion rewards (bonus XP, 3x multiplier)
- [ ] Daily puzzle system (one puzzle per day, rotates midnight UTC)
- [ ] Puzzle content library (create 100+ puzzles as JSON data)
- [ ] Puzzle leaderboard (fastest solve times)
- [ ] Sharing system (Unity Native Share plugin integration)
- [ ] Share image generation (achievement screenshots with branding + deep link)

**Deliverable:** Can add friends, see leaderboards, share achievements, complete daily challenges, maintain streak

---

### **SPRINT 13-14: Monetization & Polish** (Weeks 13-14)

#### Goals:
- Premium subscription implemented (Unity IAP + RevenueCat)
- Paywall triggers functional (6 contextual paywalls)
- Ad integration (Unity Ads, interstitial between casual matches)
- Visual polish (replace primitives with polished assets)
- Audio implementation (SFX + music)

#### Tasks:

**Week 13: Monetization**
- [ ] Unity IAP integration (Premium subscription product setup)
- [ ] RevenueCat SDK integration (subscription status management)
- [ ] Premium subscription SKUs (monthly $6.99, yearly $59.99)
- [ ] 7-day free trial configuration (App Store + Google Play)
- [ ] PremiumManager class (entitlement checks)
- [ ] Premium benefits implementation (ad-free, 2x XP, streak freezes, etc.)
- [ ] Paywall UI (modal design with benefits list + CTA)
- [ ] Paywall triggers (6 contextual triggers: after ads, lesson completion, streak break, level 10, losing streak, 4th lobby)
- [ ] Paywall frequency cap (max 1 per session)
- [ ] Unity Ads integration (interstitial ads between casual matches, frequency cap)
- [ ] Ad-free logic (Premium users skip ads)

**Week 14: Visual & Audio Polish**
- [ ] Board visual upgrade (replace primitives with polished models/textures)
- [ ] Seed visual upgrade (3D models or high-quality sprites)
- [ ] UI polish (icon design, color palette finalization, typography refinement)
- [ ] Animation polish (seed sowing, captures, level-up, rank-up)
- [ ] Particle effects (captures, level-up confetti, win celebration)
- [ ] Sound effects (pit selection, seed sowing, captures, UI clicks, level-up)
- [ ] Background music (menu music, match music - looping tracks)
- [ ] AudioManager volume controls (settings sliders work correctly)
- [ ] Loading screen polish (branded animation, tips/facts)

**Deliverable:** Premium subscription purchasable, ads display (Free users only), game looks/sounds polished

---

### **SPRINT 15-16: Testing, Bug Fixes & Launch Prep** (Weeks 15-16)

#### Goals:
- All P0 bugs fixed
- Performance optimized (60 FPS on target devices)
- Analytics verified (events tracking correctly)
- Builds tested on iOS, Android, WebGL
- App Store / Google Play submission prep

#### Tasks:

**Week 15: Testing & Optimization**
- [ ] Comprehensive manual testing (all features, all flows)
- [ ] Bug triage (P0 = blockers, P1 = high, P2 = nice-to-fix)
- [ ] Fix all P0 bugs (crashes, game-breaking issues)
- [ ] Fix high-priority P1 bugs (UX issues, confusing flows)
- [ ] Performance profiling (Unity Profiler on target devices)
- [ ] Optimize frame rate (60 FPS on iPhone 8, Android mid-range)
- [ ] Optimize memory usage (<300 MB RAM)
- [ ] Optimize network usage (<1 MB per match)
- [ ] Analytics verification (all events firing correctly, appearing in PlayFab dashboard)
- [ ] Cloud save testing (device switching, sync conflicts)

**Week 16: Launch Preparation**
- [ ] iOS build testing (iPhone 8, iPhone 13, iPad)
- [ ] Android build testing (low-end device, high-end device)
- [ ] WebGL build testing (Chrome, Firefox, Safari)
- [ ] Privacy policy creation (GDPR compliance, hosted URL)
- [ ] App Store listing (screenshots, description, keywords)
- [ ] Google Play listing (screenshots, description, keywords)
- [ ] App Store submission (TestFlight beta first)
- [ ] Google Play submission (closed beta first)
- [ ] WebGL deployment (Firebase Hosting or AWS S3 + CloudFront)
- [ ] Beta testing (invite 100-200 testers, collect feedback)
- [ ] Critical bug fixes from beta feedback

**Deliverable:** App submitted to App Store & Google Play, WebGL live, beta testing complete

---

## 🚀 Launch Strategy (Weeks 17-18)

### Week 17: Soft Launch (Limited Release)
- **Regions:** USA only (or 1-2 test markets)
- **Goal:** Monitor metrics, catch critical bugs, iterate quickly
- **Metrics to Watch:**
  - D1 Retention (target: 40%+)
  - Tutorial completion rate (target: 70%+)
  - Crash rate (target: <1%)
  - Premium conversion (target: 2%+)

### Week 18: Global Launch
- **Regions:** Worldwide (100+ countries)
- **Marketing:**
  - Social media (TikTok, Instagram, Twitter)
  - Gaming press outreach (TouchArcade, Pocket Gamer)
  - Cultural media (African diaspora publications)
  - Product Hunt launch
- **Goals:**
  - 10K downloads in first week
  - 40%+ D7 retention
  - 3%+ Premium conversion

---

## 📊 Success Criteria (Post-Launch)

### Week 20 (1 Month Post-Launch):
- ✅ 10K MAU
- ✅ 40%+ D1 retention
- ✅ 20%+ D7 retention
- ✅ 3%+ Premium conversion
- ✅ <1% crash rate

### Week 32 (3 Months Post-Launch):
- ✅ 50K MAU
- ✅ $5K MRR
- ✅ Preparing V1.1 (Achievements, Puzzle Archive)

### Week 52 (6 Months Post-Launch):
- ✅ 150K MAU
- ✅ $15K MRR
- ✅ V1.2 launched (Tournaments, Clubs)
- ✅ Preparing V2.0 (Battle Pass)

---

## ⚠️ Risks & Mitigation

### Risk 1: Development Takes Longer Than 16 Weeks
**Mitigation:**
- Cut scope: Remove daily puzzle (defer to V1.1)
- Cut scope: Remove private lobbies (defer to V1.1)
- Focus on core loop: Match + AI + Progression
- **Contingency:** 20-week timeline with buffer

### Risk 2: Photon Multiplayer More Complex Than Expected
**Mitigation:**
- Start Photon integration early (Sprint 5)
- Use Photon sample projects as reference
- Fallback: Launch with local + AI only, add multiplayer in V1.1
- **Contingency:** Hire Photon contractor if stuck >1 week

### Risk 3: Premium Conversion <2% (Below Target)
**Mitigation:**
- A/B test paywalls (messaging, timing, pricing)
- Improve value proposition (add more Premium benefits)
- Extend free trial (3 days → 7 days → 14 days)
- **Contingency:** Add ad revenue focus (rewarded video ads)

### Risk 4: Low D7 Retention (<20%)
**Mitigation:**
- Analyze drop-off points (where users churn)
- Improve onboarding (reduce friction)
- Enhance daily engagement (better challenges, streaks)
- **Contingency:** Pivot core loop based on user feedback

### Risk 5: App Store Rejection
**Mitigation:**
- Follow iOS Human Interface Guidelines strictly
- Privacy policy compliant (GDPR, COPPA)
- Age-appropriate content (no free text chat for ages 7+)
- **Contingency:** Re-submit with requested changes (typical 2-5 days)

---

## 📦 Deliverables Checklist

### By End of Sprint 16:
- [ ] iOS app (TestFlight beta)
- [ ] Android app (Google Play beta)
- [ ] WebGL build (live URL)
- [ ] PlayFab backend configured
- [ ] Photon multiplayer working
- [ ] Premium subscription functional
- [ ] All MVF features implemented
- [ ] Performance optimized (60 FPS)
- [ ] Analytics tracking correctly
- [ ] Privacy policy live
- [ ] App Store listings complete
- [ ] Beta feedback collected

---

## 🎯 Post-MVF Roadmap (V1.1 → V2.0)

### V1.1 (Month 2-3): Content Expansion
- Achievement system (30-40 achievements)
- Puzzle archive (Premium)
- Enhanced analytics dashboard

### V1.2 (Month 4-5): Community Growth
- Tournaments (weekly events)
- Clubs/Guilds (max 50 members)
- Enhanced friend features

### V1.3 (Month 6-7): Competitive Depth
- Spectator mode (watch live matches)
- Replay system (save + share)
- AI game analysis (Premium)

### V2.0 (Month 8+): Monetization Expansion
- Battle Pass (seasonal content)
- Oware variants (Kalah, Bao, Omweso)
- Cosmetic shop (board skins, avatars)

---

**Document Version:** 1.0
**Last Updated:** 2025-10-14
**Status:** 16-week MVF roadmap finalized

*"Ship fast, iterate faster"*
