# Epic Overview - Social Oware Academy

**Document Version:** 1.0
**Last Updated:** 2025-10-21
**Status:** Planning
**Sprint Timeframe:** 16 weeks (Sprints 1-16)

---

## Epic Structure

This document provides an overview of all epics for the Social Oware Academy MVP (Minimum Viable Fun). Each epic maps to specific sprints in the 16-week development roadmap.

---

## Epic List

### Epic 1: Foundation & Setup (Weeks 1-2)
**Goal:** Unity project configured with core managers and infrastructure

**Sprint:** Sprint 1-2
**Priority:** P0 (Critical)
**Status:** ✅ Partially Complete (UGS setup exists)

**Key Deliverables:**
- Unity 2022 LTS project with URP
- Third-party SDKs integrated (Photon, UGS, DOTween, TMP)
- Core manager singletons (GameManager, NetworkManager, AudioManager, UIManager, AnalyticsManager)
- Bootstrap scene with manager initialization
- Git repository configured with proper .gitignore

**Epic File:** [epic-1-foundation-and-setup.md](epic-1-foundation-and-setup.md)

---

### Epic 2: Core Gameplay (Weeks 3-4)
**Goal:** Fully playable Oware match (local, vs AI)

**Sprint:** Sprint 3-4
**Priority:** P0 (Critical)
**Status:** ✅ Partially Complete (Core logic exists, AI missing)

**Key Deliverables:**
- OwareBoard, OwareRules, GameManager (fully functional)
- AI opponents (Beginner, Intermediate, Advanced)
- Move validation and execution
- Capture detection with Grand Slam rule
- Win/loss conditions
- Visual feedback with DOTween animations
- Unit tests for all rule edge cases

**Epic File:** [epic-2-core-gameplay.md](epic-2-core-gameplay.md)

---

### Epic 3: Multiplayer Infrastructure (Weeks 5-6)
**Goal:** Networked multiplayer functional (real-time PvP)

**Sprint:** Sprint 5-6
**Priority:** P0 (Critical)
**Status:** ❌ Not Started

**Key Deliverables:**
- Photon Fusion integration
- Networked game state synchronization
- RPC calls (MakeMove, SendEmote, RequestRematch, Forfeit)
- Matchmaking system (ELO-based, ±100 rating)
- Private lobby creation (8-char room codes)
- Network error handling (disconnect, timeout)
- Match result reporting to UGS

**Epic File:** [epic-3-multiplayer-infrastructure.md](epic-3-multiplayer-infrastructure.md)

---

### Epic 4: Progression Systems (Weeks 7-8)
**Goal:** Player progression functional (Level, ELO, Lessons, Streaks)

**Sprint:** Sprint 7-8
**Priority:** P0 (Critical)
**Status:** ❌ Not Started

**Key Deliverables:**
- LevelSystem (XP calculation, level-up logic, exponential curve)
- ELOSystem (rating calculation, rank tiers, placement matches)
- LessonSystem (5 free lessons, progress tracking, star rating)
- StreakSystem (daily tracking, streak freeze, milestones)
- Push notification system (Unity Mobile Notifications)
- UGS Cloud Save integration (all progression syncs)

**Epic File:** [epic-4-progression-systems.md](epic-4-progression-systems.md)

---

### Epic 5: UI/UX Implementation (Weeks 9-10)
**Goal:** All core screens implemented with polished UX

**Sprint:** Sprint 9-10
**Priority:** P0 (Critical)
**Status:** ❌ Not Started

**Key Deliverables:**
- Home screen (header, daily challenge banner, play buttons, footer nav)
- Play mode selection modal (AI, Friend, Online options)
- Match screen (in-game board UI, HUD, emote button, menu)
- Lesson list screen (grid with lock/star indicators)
- Lesson flow screens (intro, puzzle, feedback, summary)
- Profile screen (avatar, stats, achievements placeholder)
- Leaderboard screen (tabs: Global, Friends, Country)
- Settings screen (sound, music, notifications toggles)
- Onboarding (splash, rule selection, guided tutorial)
- Responsive layout testing (iPhone SE to iPhone 13)

**Epic File:** [epic-5-ui-ux-implementation.md](epic-5-ui-ux-implementation.md)

---

### Epic 6: Social Features & Daily Engagement (Weeks 11-12)
**Goal:** Social features and daily engagement mechanics functional

**Sprint:** Sprint 11-12
**Priority:** P1 (High)
**Status:** ❌ Not Started

**Key Deliverables:**
- Friend system (UGS Friends API, unique friend codes)
- Add friend flow (enter code, send request, accept/decline)
- Friend list UI (online status, challenge button)
- Direct friend challenge (creates private room, invites friend)
- Leaderboards (Global, Friends, Country with UGS Leaderboards API)
- Safe emotes (preset emote menu, 6 emotes, network sync)
- Login bonus system (10 XP on first open each day)
- Daily challenge system (4 types, progress tracking, rewards)
- Daily puzzle system (one puzzle per day, rotates midnight UTC)
- Sharing system (Native Share plugin, achievement screenshots)

**Epic File:** [epic-6-social-and-daily-engagement.md](epic-6-social-and-daily-engagement.md)

---

### Epic 7: Monetization & Polish (Weeks 13-14)
**Goal:** Premium subscription, ads, and visual/audio polish

**Sprint:** Sprint 13-14
**Priority:** P1 (High)
**Status:** ❌ Not Started

**Key Deliverables:**
- Unity IAP integration (Premium subscription SKUs)
- RevenueCat SDK integration (subscription status management)
- 7-day free trial configuration (App Store + Google Play)
- PremiumManager (entitlement checks)
- Premium benefits implementation (ad-free, 2x XP, streak freezes, etc.)
- Paywall UI (modal design with benefits list + CTA)
- Paywall triggers (6 contextual triggers, frequency cap)
- Unity Ads integration (interstitial ads, frequency cap)
- Visual upgrade (board, seeds, UI polish)
- Animation polish (sowing, captures, level-up, rank-up)
- Sound effects (pit selection, seed sowing, captures, UI clicks)
- Background music (menu music, match music - looping)

**Epic File:** [epic-7-monetization-and-polish.md](epic-7-monetization-and-polish.md)

---

### Epic 8: Testing, Bug Fixes & Launch Prep (Weeks 15-16)
**Goal:** Production-ready build, all P0 bugs fixed, launched to beta

**Sprint:** Sprint 15-16
**Priority:** P0 (Critical)
**Status:** ❌ Not Started

**Key Deliverables:**
- Comprehensive manual testing (all features, all flows)
- Bug triage (P0 = blockers, P1 = high, P2 = nice-to-fix)
- Fix all P0 bugs (crashes, game-breaking issues)
- Performance profiling (Unity Profiler on target devices)
- Optimization (60 FPS on iPhone 8, Android mid-range)
- Analytics verification (all events firing correctly)
- Cloud save testing (device switching, sync conflicts)
- iOS build testing (iPhone 8, iPhone 13, iPad)
- Android build testing (low-end, high-end devices)
- WebGL build testing (Chrome, Firefox, Safari)
- Privacy policy creation (GDPR, hosted URL)
- App Store & Google Play listings (screenshots, description, keywords)
- Beta testing (100-200 testers, collect feedback)
- Critical bug fixes from beta feedback

**Epic File:** [epic-8-testing-and-launch-prep.md](epic-8-testing-and-launch-prep.md)

---

## Epic Dependencies

```
Epic 1 (Foundation) → Epic 2 (Core Gameplay)
                    ↓
                Epic 3 (Multiplayer)
                    ↓
                Epic 4 (Progression)
                    ↓
                Epic 5 (UI/UX)
                    ↓
        ┌───────────┴────────────┐
        ↓                        ↓
Epic 6 (Social)          Epic 7 (Monetization)
        └───────────┬────────────┘
                    ↓
        Epic 8 (Testing & Launch)
```

**Critical Path:**
1. Foundation (Epic 1)
2. Core Gameplay (Epic 2)
3. Multiplayer (Epic 3)
4. Progression (Epic 4)
5. UI/UX (Epic 5)
6. Social + Monetization (Epic 6 + Epic 7 in parallel)
7. Testing & Launch (Epic 8)

---

## Epic Priority Matrix

| Epic | Priority | Risk | Complexity | Current Status |
|------|----------|------|------------|----------------|
| Epic 1 | P0 | Low | Low | ✅ Partial (75%) |
| Epic 2 | P0 | Medium | Medium | ✅ Partial (60%) |
| Epic 3 | P0 | High | High | ❌ Not Started |
| Epic 4 | P0 | Medium | Medium | ❌ Not Started |
| Epic 5 | P0 | Low | Medium | ❌ Not Started |
| Epic 6 | P1 | Medium | Medium | ❌ Not Started |
| Epic 7 | P1 | Medium | High | ❌ Not Started |
| Epic 8 | P0 | High | Medium | ❌ Not Started |

**Risk Assessment:**
- **High Risk:** Epic 3 (Multiplayer) - New technology (Photon Fusion)
- **High Risk:** Epic 7 (Monetization) - IAP/subscription complexity
- **High Risk:** Epic 8 (Launch) - App Store approval process
- **Medium Risk:** Epic 2, 4, 6 - Standard game features
- **Low Risk:** Epic 1, 5 - Well-understood Unity development

---

## Epic to Sprint Mapping

| Sprint | Weeks | Epic(s) | Focus |
|--------|-------|---------|-------|
| Sprint 1-2 | 1-2 | Epic 1 | Foundation & Setup |
| Sprint 3-4 | 3-4 | Epic 2 | Core Gameplay |
| Sprint 5-6 | 5-6 | Epic 3 | Multiplayer Infrastructure |
| Sprint 7-8 | 7-8 | Epic 4 | Progression Systems |
| Sprint 9-10 | 9-10 | Epic 5 | UI/UX Implementation |
| Sprint 11-12 | 11-12 | Epic 6 | Social Features & Daily Engagement |
| Sprint 13-14 | 13-14 | Epic 7 | Monetization & Polish |
| Sprint 15-16 | 15-16 | Epic 8 | Testing, Bug Fixes & Launch Prep |

---

## Success Criteria (MVF Completion)

By end of Sprint 16, all epics must be complete:

**Functional Requirements:**
- [ ] Core Oware gameplay fully functional (local + networked)
- [ ] AI opponents (3 difficulties) working
- [ ] Multiplayer matchmaking and private lobbies functional
- [ ] Player progression (Level, ELO, Lessons, Streaks) syncing to cloud
- [ ] All core UI screens implemented and responsive
- [ ] Friend system, leaderboards, and daily engagement working
- [ ] Premium subscription purchasable, ads displaying (Free users)
- [ ] Visual and audio polish applied

**Non-Functional Requirements:**
- [ ] 60 FPS on target devices (iPhone 8+, Android mid-range)
- [ ] <5 second cold start time
- [ ] <1% crash rate
- [ ] All analytics events tracking correctly

**Deployment:**
- [ ] iOS build on TestFlight
- [ ] Android build on Google Play Beta
- [ ] WebGL build deployed to hosting
- [ ] Privacy policy live
- [ ] App Store & Google Play listings complete

---

## Next Steps

1. ✅ Epic overview created
2. ⏭️ Create detailed epic documents (epic-1 through epic-8)
3. ⏭️ Generate user stories from each epic
4. ⏭️ Create development tasks with acceptance criteria
5. ⏭️ Commit all documentation to Git

---

## Document Change Log

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-10-21 | BMAD Analyst | Created epic overview based on MVF roadmap |

---

*"Ship First - Playcentric Design - MVF over MVP"*
