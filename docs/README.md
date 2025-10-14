# Social Oware Academy - Game Design Document

**"Learn. Compete. Connect."**

The world's first gamified Oware learning platform that transforms the ancient West African strategy game into an addictive social experience for the modern mobile era.

---

## 📚 Documentation Index

This Game Design Document (GDD) is organized as a living documentation system. Each document focuses on a specific aspect of the game and can be updated independently.

### Core Documents

| Document | Description | Status |
|----------|-------------|--------|
| [01-vision-document.md](01-vision-document.md) | Product vision, positioning, target audience, success metrics | ✅ Complete |
| [02-player-personas.md](02-player-personas.md) | Detailed player profiles using Bartle's taxonomy | ✅ Complete |
| [03-core-loop-mechanics.md](03-core-loop-mechanics.md) | Core gameplay loop, progression systems, engagement hooks | ✅ Complete |
| [04-feature-specifications.md](04-feature-specifications.md) | Detailed feature specs (MVF → V2.0) | ✅ Complete |
| [05-technical-architecture.md](05-technical-architecture.md) | Tech stack, Unity setup, backend architecture | ✅ Complete |
| [06-monetization-strategy.md](06-monetization-strategy.md) | Revenue model, pricing, conversion optimization | ✅ Complete |
| [07-ux-wireframes.md](07-ux-wireframes.md) | Screen wireframes, user flows, design principles | ✅ Complete |
| [08-mvf-roadmap.md](08-mvf-roadmap.md) | 16-week sprint plan to ship MVF | ✅ Complete |

---

## 🎯 Quick Start Guide

**New to this project?** Start here:

1. **Understand the Vision:** Read [01-vision-document.md](01-vision-document.md) (10 min)
2. **Know Your Players:** Skim [02-player-personas.md](02-player-personas.md) (5 min)
3. **See the Plan:** Review [08-mvf-roadmap.md](08-mvf-roadmap.md) (10 min)
4. **Start Building:** Follow Sprint 1-2 tasks in the roadmap

**Total time to context:** ~25 minutes

---

## 🚀 Project Overview

### What is Social Oware Academy?

**Oware** is a 3,000-year-old West African strategy game (part of the Mancala family). Despite its rich cultural heritage and strategic depth, it lacks a modern, accessible digital experience.

**Social Oware Academy** modernizes Oware using proven models:
- **Duolingo's gamification:** Bite-sized lessons, streaks, progression
- **Chess.com's competitive depth:** ELO ratings, tournaments, analysis
- **Social-first virality:** Friend challenges, clubs, shareable achievements

### Core Pillars

1. **LEARN** - Interactive lessons teach strategy (beginner → advanced)
2. **COMPETE** - ELO ratings, ranks, leaderboards, tournaments
3. **CONNECT** - Friends, clubs, private lobbies, safe social features

---

## 🎮 Game Summary

### Genre
**Turn-Based Strategy • Puzzle • Social • Educational**

### Platform
**Cross-Platform** (iOS, Android, WebGL)

### Target Audience
**Ages 7-77** with emphasis on mobile-first players (13-35 years old)

### Monetization
**Freemium** - Generous free tier + Premium subscription ($6.99/mo or $59.99/yr)

### Player Types (Priority)
1. **Achievers (40%)** - Progress-driven, love unlocking content
2. **Socializers (30%)** - Community-focused, play with friends
3. **Explorers (20%)** - Learning-motivated, cultural curiosity
4. **Killers (10%)** - Competitive dominance seekers

### Core Loop
```
Play Match/Lesson/Puzzle
    ↓
Earn XP + Rating + Rewards
    ↓
Progress on Multiple Tracks (Level, Rank, Lessons, Streak)
    ↓
See Next Goals ("3 wins to Gold rank!")
    ↓
"Just one more!" → REPEAT
```

---

## 📊 Success Metrics (First 6 Months)

### Primary KPIs:
1. **User Growth (A)** - Total signups, viral coefficient, organic acquisition
2. **Premium Conversion (E)** - Free → Premium %, subscription retention
3. **Social Engagement (F)** - Friend invites, shares, club participation

### Supporting Metrics:
- **D7 Retention:** 20%+ (industry: 10%)
- **D30 Retention:** 10%+ (industry: 5%)
- **ARPU:** $0.20/month
- **LTV:** $50 (7-month avg subscription)

---

## 🏗️ Technical Stack

| Component | Technology | Purpose |
|-----------|-----------|---------|
| **Engine** | Unity 2022 LTS | Cross-platform game engine |
| **Multiplayer** | Photon Fusion | Turn-based networking |
| **Backend** | PlayFab | Auth, cloud save, leaderboards, analytics |
| **Payments** | Unity IAP + RevenueCat | Subscription management |
| **Ads** | Unity Ads | Monetize free users |
| **Analytics** | PlayFab + Unity Analytics | Metrics tracking |
| **Sharing** | Unity Native Share | Social media integration |
| **Notifications** | Unity Mobile Notifications | Streak reminders |

---

## 💰 Monetization Overview

### Free Tier (Generous - Drives Viral Growth):
- ✅ Unlimited casual & ranked matches (with ads)
- ✅ Basic AI opponents (Beginner, Intermediate)
- ✅ First 5 tutorial lessons
- ✅ Daily puzzles
- ✅ Friend system & private lobbies (3 max)
- ✅ Streaks & progression

### Premium Tier ($6.99/mo or $59.99/yr):
- ✅ Ad-free experience
- ✅ 2x XP multiplier
- ✅ 2 streak freezes per month
- ✅ Advanced AI opponent
- ✅ Advanced lessons (11-15)
- ✅ Puzzle archive (365 days)
- ✅ Priority matchmaking
- ✅ Premium badge

### Future: Battle Pass (V2.0)
- Seasonal content ($9.99/season or included with Premium)
- Cosmetics, cultural content, exclusive challenges

---

## 📅 Development Timeline

### MVF (Minimum Viable Fun) - 16 Weeks

| Phase | Weeks | Focus |
|-------|-------|-------|
| **Foundation** | 1-2 | Unity setup, managers, SDKs |
| **Core Gameplay** | 3-4 | Oware logic, AI opponents |
| **Multiplayer** | 5-6 | Photon integration, matchmaking |
| **Progression** | 7-8 | Levels, ELO, lessons, streaks |
| **UI/UX** | 9-10 | All screens, onboarding |
| **Social** | 11-12 | Friends, leaderboards, sharing |
| **Monetization** | 13-14 | Premium subscription, polish |
| **Launch Prep** | 15-16 | Testing, bug fixes, submissions |

**Target Launch:** Week 17 (Soft launch) → Week 18 (Global launch)

### Post-MVF Roadmap

- **V1.1 (Month 2-3):** Achievements, puzzle archive
- **V1.2 (Month 4-5):** Tournaments, clubs/guilds
- **V1.3 (Month 6-7):** Spectator mode, replays, AI analysis
- **V2.0 (Month 8+):** Battle Pass, variants, cosmetics

---

## 🎨 Design Philosophy

### Core Principles:
1. **Ship First** - Prototype with primitives, polish after validation
2. **MVF over MVP** - Minimum Viable **Fun** before features
3. **Retention over Acquisition** - Duolingo proved retention has 5x impact
4. **Playcentric Design** - Prototype → Test → Iterate → Repeat
5. **Living Documentation** - This GDD evolves with the project

### Aesthetic Goals (MDA Framework):
1. **Challenge** - Strategic mastery, satisfying problem-solving
2. **Discovery** - "Aha!" moments learning tactics & culture
3. **Fellowship** - Warm social connections, shared victories

### Experience Promise:
**"Super fun & easy, stunningly beautiful"**
- Low friction, joyful flow states
- Elevated aesthetic (minimalist, cultural richness)
- Accessible for ages 7-77

---

## 🌍 Market Opportunity

### Problem:
- Oware apps are outdated (poor UI/UX, no social features, confusing rules)
- No modern learning platform for Oware strategy
- African cultural games underrepresented in gaming market

### Solution:
- Modern F2P model (Chess.com + Duolingo playbook)
- Cross-platform (mobile + web)
- Social-first (viral growth via sharing, friend challenges)
- Cultural authenticity (respectful representation, storytelling)

### Competitive Advantages:
1. **First-Mover** - No modern Oware app with social/learning features
2. **Cultural Authenticity** - Partnering with African gaming communities
3. **Proven Tech Stack** - Unity + Photon + PlayFab = scalable
4. **Proven Models** - Chess.com + Duolingo playbooks validated

---

## 📖 How to Use This GDD

### For Developers:
- Start with [05-technical-architecture.md](05-technical-architecture.md) for tech stack
- Follow [08-mvf-roadmap.md](08-mvf-roadmap.md) sprint-by-sprint
- Reference [04-feature-specifications.md](04-feature-specifications.md) for detailed specs

### For Designers:
- Read [07-ux-wireframes.md](07-ux-wireframes.md) for screen layouts
- Reference [02-player-personas.md](02-player-personas.md) for user empathy
- Check [01-vision-document.md](01-vision-document.md) for design principles

### For Product/Business:
- Start with [01-vision-document.md](01-vision-document.md) for strategy
- Review [06-monetization-strategy.md](06-monetization-strategy.md) for revenue model
- Track metrics from [03-core-loop-mechanics.md](03-core-loop-mechanics.md)

### For QA/Testers:
- Use [04-feature-specifications.md](04-feature-specifications.md) for acceptance criteria
- Follow user flows in [07-ux-wireframes.md](07-ux-wireframes.md)
- Check [03-core-loop-mechanics.md](03-core-loop-mechanics.md) for game rules

---

## 🔄 Document Update Process

This is a **living GDD** - it evolves as the project grows.

### When to Update:
- Major design decisions change
- New features added to roadmap
- User feedback requires pivots
- Post-launch learnings

### How to Update:
1. Edit relevant document in `/docs` folder
2. Update version number at bottom of document
3. Update "Last Updated" date
4. Commit to Git with clear message: "Updated [doc] - [reason]"
5. Notify team in Slack/Discord

### Version History:
- **v1.0 (2025-10-14):** Initial GDD complete - Foundation phase
- (Future updates logged here)

---

## 🤝 Team Roles (Future)

**Current:** Solo developer (Benjamin Hinson) learning game development

**Future Team Structure:**
- **Product Owner:** Benjamin Hinson (you!)
- **Game Designer:** TBD (or external consultant)
- **Unity Developer:** TBD (or you, after Udemy course completion)
- **UI/UX Designer:** TBD (or freelancer for visual polish)
- **Sound Designer:** TBD (or Asset Store audio packs)
- **Community Manager:** TBD (post-launch, for social features)

---

## 🎓 Learning Resources

As you're following the Udemy course "The Ultimate Guide to Game Development with C# in Unity," here are specific modules that map to this project:

### Relevant Course Sections:
1. **Unity Basics** → Sprint 1-2 (Foundation)
2. **C# Programming** → Sprint 3-4 (Game Logic)
3. **UI Systems** → Sprint 9-10 (UI Implementation)
4. **Multiplayer (if covered)** → Sprint 5-6 (Photon)
5. **Mobile Deployment** → Sprint 15-16 (Launch Prep)

### Additional Resources:
- **Photon Fusion Docs:** https://doc.photonengine.com/fusion/current/
- **PlayFab Docs:** https://docs.microsoft.com/gaming/playfab/
- **Unity Learn:** https://learn.unity.com (free courses)
- **Game Design Theory:** "The Art of Game Design" by Jesse Schell
- **F2P Monetization:** Deconstructor of Fun blog

---

## 🚨 Critical Decisions Locked In

These core decisions are **final for MVF** (can revisit post-launch):

✅ **Platform:** Cross-platform (iOS, Android, WebGL)
✅ **Monetization:** Freemium with Premium subscription ($6.99/mo)
✅ **Rules:** Standard Ghanaian Oware (no variants in MVF)
✅ **Player Types:** Achievers (40%) > Socializers (30%) > Explorers (20%) > Killers (10%)
✅ **Tech Stack:** Unity + Photon + PlayFab
✅ **Core Loop:** Hybrid (Match + Lesson + Challenge)
✅ **Progression:** 4 tracks (Level, ELO, Lessons, Streaks)
✅ **Social:** Friends, Leaderboards, Sharing, Private Lobbies
✅ **No Text Chat:** Preset emotes only (safe for ages 7+)

---

## ✅ Next Actions

### Immediate (This Week):
1. ✅ Review all GDD documents (you are here!)
2. ⏭️ Set up GitHub repository for version control
3. ⏭️ Upload GDD docs to GitHub
4. ⏭️ Install Unity 2022 LTS
5. ⏭️ Begin Sprint 1 tasks (Foundation & Setup)

### Short-Term (Next 2 Weeks):
- Complete Sprint 1-2 (Foundation)
- Get first prototype playable (primitive board with game logic)
- Set up Photon and PlayFab accounts

### Medium-Term (Next 16 Weeks):
- Follow MVF roadmap sprint-by-sprint
- Ship MVF to beta testers
- Prepare for App Store / Google Play launch

---

## 📞 Contact & Feedback

**Project Lead:** Benjamin Hinson
**Email:** [Your Email]
**Project Status:** Foundation Phase (GDD Complete)
**Last Updated:** 2025-10-14

**Questions or Suggestions?**
- Open an issue in this GitHub repo
- Email project lead
- Join Discord community (if created)

---

## 📄 License

**Game Design:** © 2025 Benjamin Hinson / Social Oware Academy
**Oware Rules:** Public domain (traditional game)
**Code:** [License TBD - Recommend MIT or Proprietary]

---

## 🙏 Acknowledgments

**Inspired By:**
- **Chess.com** - Competitive gaming platform model
- **Duolingo** - Gamification and retention mechanics
- **Oware Community** - Cultural authenticity and rule standardization
- **MDA Framework** - Hunicke, LeBlanc, Zubek (2004)
- **Bartle's Taxonomy** - Richard Bartle (1996)
- **Playcentric Design** - Tracy Fullerton (USC Games)

**Research Sources:**
- GameRefinery (mobile game analysis)
- Lenny's Newsletter (Duolingo growth tactics)
- Google Play Developer Blog (retention best practices)
- Udemy "Ultimate Guide to Game Development with C# in Unity"

---

## 🎉 Vision Statement

> *"Social Oware Academy will make the world's oldest strategy game accessible to the TikTok generation—bridging 3,000 years of West African heritage with modern social gaming, one match at a time."*

---

**Ready to build? Start with [08-mvf-roadmap.md](08-mvf-roadmap.md) Sprint 1!**

---

**Document Version:** 1.0
**Last Updated:** 2025-10-14
**Status:** Foundation Complete - Ready for Development

*"From conception to code, one document at a time."*
