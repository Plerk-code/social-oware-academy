# Product Requirements Document - Social Oware Academy

**Document Version:** 2.0 (BMAD-Refactored)
**Last Updated:** 2025-10-21
**Status:** Active Development
**Product Owner:** Development Team
**Target Platform:** iOS, Android, WebGL (Unity 2022 LTS)

---

## Executive Summary

**Social Oware Academy** is the world's first gamified Oware learning platform that transforms the ancient West African strategy game into an addictive social experience for the modern mobile era—making cultural heritage accessible, competitive, and community-driven for ages 7 to 77.

### Product Vision

**"Learn. Compete. Connect."**

We combine:
- **Duolingo's gamification** (bite-sized lessons, streaks, progression)
- **Chess.com's competitive depth** (ratings, tournaments, analysis)
- **Social-first virality** (friend challenges, clubs, sharing)

### Key Differentiation

| Current Oware Apps | Social Oware Academy |
|-------------------|---------------------|
| Basic AI opponents | Interactive tutorial academy |
| Minimal social features | Robust social & competitive features |
| Confusing rule variations | Standardized rules (authentic Ghanaian Oware) |
| Outdated UI/UX | Stunning, modern design |
| No learning systems | Daily engagement mechanics |
| Single platform | Cross-platform (mobile + web) |

---

## Product Pillars

### 1. LEARN (Duolingo Model)
- Interactive lessons teach strategy from beginner to advanced
- Daily puzzles create habit loops
- Cultural context educates about West African gaming heritage
- Progress tracking shows skill improvement

### 2. COMPETE (Chess.com Model)
- ELO rating system measures skill
- Rank tiers (Bronze → Master) provide visible progression
- Global leaderboards showcase top players
- Tournaments create esports potential

### 3. CONNECT (Social Features)
- Friend system enables direct challenges
- Private lobbies support family play (ages 7-77)
- Clubs/guilds build communities (post-launch)
- Viral sharing drives organic growth

---

## Target Audience

### Primary Demographics
- **Age Range:** 7-77 (emphasis on 13-35 mobile-first players)
- **Geographic Focus:**
  - **Primary:** USA, UK, Canada (English-first)
  - **Secondary:** Ghana, Nigeria, African diaspora (cultural connection)
  - **Tertiary:** Global (strategic game fans, board game enthusiasts)

### Player Type Distribution (Bartle's Taxonomy)
1. **Achievers** (40%) - Progress-driven, love unlocking content, climbing ranks
2. **Socializers** (30%) - Community-focused, play with friends/family
3. **Explorers** (20%) - Learning-motivated, cultural curiosity
4. **Killers** (10%) - Competitive dominance seekers

---

## Core Features (MVF - Minimum Viable Fun)

### ✅ Gameplay Features
1. **Core Oware Gameplay**
   - Authentic Ghanaian Oware rules implementation
   - 12 pits, 4 seeds per pit, counter-clockwise sowing
   - Grand Slam rule, forced feeding rule
   - Win condition: First to capture 25+ seeds

2. **Game Modes** (5 modes)
   - Casual (vs AI - 3 difficulties)
   - Ranked (ELO-based matchmaking)
   - Training (interactive lessons)
   - Daily Puzzle (one per day)
   - Private Lobbies (friend challenges with room codes)

3. **AI Opponents** (3 difficulties)
   - Beginner: Random moves with 30% strategic heuristic
   - Intermediate: Minimax depth 3
   - Advanced: Minimax with alpha-beta pruning depth 5-7

### ✅ Progression Systems
1. **Player Level System**
   - XP awarded for matches, lessons, puzzles
   - Exponential leveling curve
   - Level-up rewards and animations
   - Feature unlocks gated by level

2. **ELO Rating System**
   - Starting rating: 1200 (after 5 placement matches)
   - Standard ELO formula with K-factor 32
   - Rating tiers: Bronze → Silver → Gold → Platinum → Diamond → Master
   - Tier badges and rank-up animations

3. **Lesson System**
   - 5 free beginner lessons (Premium unlocks 10+ advanced)
   - Interactive puzzle-based learning
   - Star rating system (1-3 stars per lesson)
   - Progress tracking and completion rewards

4. **Streak System**
   - Daily login streak tracking
   - Milestone rewards (7/30/100 day badges)
   - Streak freeze mechanic (2 per month for Premium)
   - Push notification reminders (8 PM local time)

### ✅ Social Features
1. **Friend System**
   - Unique friend codes (PLAYER#1234 format)
   - Add friend via code entry
   - Friend list with online status
   - Direct challenge functionality

2. **Leaderboards**
   - Global leaderboard (top 100 by ELO)
   - Friends leaderboard (all friends ranked)
   - Country leaderboard (IP geolocation)
   - Player rank display (shows position even outside top 100)

3. **Safe Communication**
   - Preset emote system (6 emotes)
   - No free text chat (ages 7+ safety)
   - Network-synced emote animations

4. **Viral Sharing**
   - Achievement screenshots with branding
   - Deep links (Branch.io integration)
   - Social media sharing (Native Share plugin)

### ✅ Daily Engagement
1. **Login Bonus**
   - 10 XP on first daily open
   - Maintains streak status

2. **Daily Challenges** (rotates daily)
   - Types: Win 2 matches, Complete lesson, Solve puzzle, Play with friend
   - Progress tracking UI
   - Completion rewards (bonus XP, 3x multiplier)

3. **Daily Puzzle**
   - One puzzle per day (rotates midnight UTC)
   - Leaderboard for fastest solve times
   - Content library: 100+ puzzles

### ✅ Onboarding
1. **Rule Selection Screen**
   - Options: New Player / I Know Rules / Expert
   - Tailored tutorial based on selection

2. **Guided Tutorial Match**
   - AI walks player through first game
   - Step-by-step tooltips with pointers
   - First match completion celebration

### ✅ Monetization
1. **Freemium Model (Chess.com approach)**

   **FREE TIER** (generous - drives viral growth):
   - Unlimited casual & ranked matches (with ads)
   - Basic AI opponents
   - First 5 tutorial lessons
   - Daily puzzles
   - Friend system & private lobbies
   - Streaks & progression

   **PREMIUM TIER** ($6.99/mo or $59.99/yr):
   - Ad-free experience
   - All 10+ tutorial lessons unlocked
   - Advanced AI opponent
   - 2x XP multiplier
   - 2 streak freezes per month
   - Puzzle archive access
   - Priority matchmaking
   - Exclusive badge

2. **Advertising** (Free users only)
   - Interstitial ads (after casual matches, not ranked)
   - Frequency cap: Max 1 ad per 5 minutes
   - Unity Ads integration

3. **Contextual Paywalls** (6 triggers)
   - After viewing ads (upgrade to ad-free)
   - After lesson completion (unlock all lessons)
   - Before streak break (use streak freeze)
   - At level 10 (XP multiplier value prop)
   - After 3-match losing streak (advanced AI offer)
   - 4th private lobby creation (Premium gets unlimited)
   - Frequency cap: Max 1 per session

---

## Success Metrics

### Primary KPIs (First 6 Months)
1. **User Growth (Acquisition)**
   - Total signups
   - Viral coefficient (K-factor > 1.2)
   - Organic acquisition %

2. **Premium Conversion (Engagement)**
   - Free-to-premium conversion % (target: 3%+)
   - Subscription retention rate
   - 7-day free trial conversion

3. **Social Engagement (Retention)**
   - Friend invites per user
   - Share events per week
   - Club participation % (post-launch)

### Supporting Metrics
- **D7 Retention:** 7-day retention rate (target: 40%+)
- **DAU/MAU Ratio:** Daily habit formation (target: 25%+)
- **Session Length:** Average playtime per session (target: 12+ min)
- **Tutorial Completion:** % completing guided tutorial (target: 70%+)
- **Crash Rate:** App stability (target: <1%)

### Month-by-Month Targets
| Metric | Month 1 | Month 3 | Month 6 |
|--------|---------|---------|---------|
| MAU | 10K | 50K | 150K |
| D1 Retention | 40% | 42% | 45% |
| D7 Retention | 20% | 25% | 30% |
| Premium Conversion | 2% | 3% | 4% |
| MRR | $1K | $5K | $15K |

---

## Platform Requirements

### Supported Platforms
1. **iOS**
   - Minimum: iOS 13+
   - Target devices: iPhone 8+ (A11 chip)
   - Performance: 60 FPS constant
   - App size: <150 MB

2. **Android**
   - Minimum: API 24 (Android 7.0)
   - Target devices: Mid-range (Snapdragon 660+)
   - Performance: 60 FPS on mid-range
   - App size: <200 MB (APK + OBB)

3. **WebGL**
   - Browsers: Chrome 90+, Firefox 88+, Safari 14+, Edge 90+
   - Performance: 30-60 FPS (browser-dependent)
   - Load time: <10 seconds initial load
   - Build size: <50 MB compressed (Brotli)

### Cross-Platform Features
- **Cloud Save:** Account syncs across all devices (PlayFab)
- **Cross-Platform Multiplayer:** Play iOS vs Android vs Web
- **Unified Progression:** Level, ELO, lessons sync everywhere

---

## Technical Stack Summary

### Core Technologies
- **Engine:** Unity 2022 LTS
- **Multiplayer:** Photon Fusion (turn-based)
- **Backend:** PlayFab (authentication, cloud save, leaderboards, analytics)
- **IAP:** Unity IAP + RevenueCat (subscription management)
- **Ads:** Unity Ads
- **Analytics:** PlayFab Analytics + Unity Analytics
- **Deep Links:** Branch.io
- **Sharing:** Unity Native Share plugin

*Detailed technical architecture in `docs/architecture.md`*

---

## Non-Functional Requirements

### Performance
- Frame rate: 60 FPS (mobile), 30-60 FPS (WebGL)
- Memory usage: <300 MB RAM (mobile), <500 MB (WebGL)
- Network usage: <1 MB per match
- Cold start time: <5 seconds
- Battery drain: <5% per 10-min session (mobile)

### Security
- Server-authoritative game logic (anti-cheat)
- Move validation server-side
- Receipt validation (IAP)
- PlayFab API rate limiting
- HTTPS encryption for all network traffic

### Privacy & Compliance
- GDPR compliance (Privacy policy, data deletion requests)
- COPPA compliance (ages 7+ safe, no free text chat)
- Parental consent flow (future enhancement)
- Data encryption at rest (PlayFab)

### Accessibility
- Clear UI/UX for ages 7-77
- Colorblind-friendly palette (future enhancement)
- Tutorial supports non-readers (visual guidance)
- Sound/music toggles in settings

---

## User Stories Overview

*Note: Detailed user stories are generated per epic in `docs/stories/` directory*

### Epic 1: Core Gameplay
- As a player, I can play a full Oware match following authentic Ghanaian rules
- As a player, I can choose AI difficulty (Beginner/Intermediate/Advanced)
- As a player, I can see visual feedback for moves, captures, and game end

### Epic 2: Multiplayer
- As a player, I can play ranked matches with ELO-based matchmaking
- As a player, I can create/join private lobbies with friends via room codes
- As a player, I can send emotes during matches

### Epic 3: Progression
- As a player, I can earn XP and level up through gameplay
- As a player, I can see my ELO rating and rank tier
- As a player, I can complete lessons and earn stars
- As a player, I can maintain a daily streak with reminders

### Epic 4: Social Features
- As a player, I can add friends using unique friend codes
- As a player, I can challenge friends to direct matches
- As a player, I can view leaderboards (Global/Friends/Country)
- As a player, I can share achievements to social media

### Epic 5: Daily Engagement
- As a player, I receive login bonuses daily
- As a player, I can complete daily challenges for bonus XP
- As a player, I can solve the daily puzzle
- As a player, I receive push notifications for streak reminders

### Epic 6: Onboarding
- As a new player, I can choose my rule familiarity level
- As a new player, I complete a guided tutorial match
- As a new player, I understand how to progress and earn rewards

### Epic 7: Monetization
- As a free player, I see ads between casual matches
- As a player, I can subscribe to Premium for benefits
- As a Premium subscriber, I have an ad-free experience with enhanced features
- As a player, I see contextual paywalls at strategic moments

---

## Out of Scope (Post-MVF)

### V1.1 (Month 2-3)
- Achievement system (30-40 achievements)
- Puzzle archive (Premium)
- Enhanced analytics dashboard

### V1.2 (Month 4-5)
- Tournaments (weekly events)
- Clubs/Guilds (max 50 members)
- Enhanced friend features (profiles, stats)

### V1.3 (Month 6-7)
- Spectator mode (watch live matches)
- Replay system (save + share matches)
- AI game analysis (Premium)

### V2.0 (Month 8+)
- Battle Pass (seasonal content)
- Oware variants (Kalah, Bao, Omweso)
- Cosmetic shop (board skins, avatars, emotes)

---

## Risks & Mitigation

### Risk: Oware unknown in Western markets
**Mitigation:** Position as "ancient strategy game" like chess, emphasize gentle learning curve, cultural storytelling creates intrigue

### Risk: Multiplayer complexity delays launch
**Mitigation:** Use Photon (proven middleware), focus on turn-based (simpler than real-time), defer tournaments to post-launch

### Risk: Premium conversion rate too low (<2%)
**Mitigation:** Generous free tier drives volume, 7-day free trial reduces friction, paywall at high-value moments, A/B test messaging

### Risk: Low D7 retention (<20%)
**Mitigation:** Analyze drop-off points, improve onboarding, enhance daily engagement (better challenges, streaks), iterate based on feedback

### Risk: App Store/Google Play rejection
**Mitigation:** Follow platform guidelines strictly, privacy policy compliant (GDPR, COPPA), age-appropriate content (no free text chat)

---

## Glossary

- **MVF:** Minimum Viable Fun - smallest shippable version that delivers core emotional experience
- **ELO:** Rating system named after Arpad Elo, measures player skill
- **CCU:** Concurrent users (players online simultaneously)
- **MAU:** Monthly active users
- **DAU:** Daily active users
- **MRR:** Monthly recurring revenue
- **IAP:** In-app purchase
- **Freemium:** Free-to-play with premium subscription option
- **K-factor:** Viral coefficient (avg. invites sent per user)
- **Grand Slam:** Oware rule preventing capture of all opponent's seeds
- **Forced Feeding:** Oware rule requiring player to give opponent seeds if they have none

---

## Approval & Sign-Off

| Role | Name | Date | Status |
|------|------|------|--------|
| Product Owner | [Name] | 2025-10-21 | ✅ Approved |
| Tech Lead | [Name] | 2025-10-21 | ✅ Approved |
| Scrum Master | [Name] | 2025-10-21 | ✅ Approved |

---

## Document Change Log

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-10-14 | Team | Initial vision and feature specifications |
| 2.0 | 2025-10-21 | BMAD Analyst | Refactored into BMAD PRD format, consolidated all requirements |

---

**Next Steps:**
1. ✅ PRD finalized and approved
2. ⏭️ Generate technical architecture document
3. ⏭️ Create epics from feature requirements
4. ⏭️ Generate user stories from epics
5. ⏭️ Create development tasks and commit to Git

---

*"Making the world's oldest strategy game accessible to the TikTok generation"*
