# Core Loop & Mechanics Specification

The addictive heart of Social Oware Academy—designed for 88% return rate through research-backed engagement mechanics.

---

## 🔄 Primary Core Loop (Hybrid Model)

```
┌─────────────────────────────────────────┐
│          PLAYER OPENS APP               │
└──────────────┬──────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────┐
│   LOGIN BONUS (+10 XP, Streak Saved)   │
└──────────────┬──────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────┐
│         CHOOSE ACTIVITY:                │
│  [Play Match] [Lesson] [Daily Puzzle]  │
└──────────────┬──────────────────────────┘
               │
         ┌─────┴──────┐
         ▼            ▼
    ┌────────┐   ┌──────────┐
    │ MATCH  │   │  LESSON  │
    │ 5-10min│   │  2-5min  │
    └────┬───┘   └────┬─────┘
         │            │
         └─────┬──────┘
               ▼
┌─────────────────────────────────────────┐
│         EARN REWARDS:                   │
│  • XP (toward Player Level)             │
│  • Rating Change (if ranked match)      │
│  • Stars (if lesson/puzzle)             │
│  • Progress on Daily Challenge          │
└──────────────┬──────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────┐
│      MULTIPLE PROGRESSIONS:             │
│  • Level Up (unlock content)            │
│  • Rank Up (competitive status)         │
│  • Lessons Completed (mastery %)        │
│  • Streak Maintained (daily habit)      │
└──────────────┬──────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────┐
│       SEE NEXT GOALS:                   │
│  "3 more wins → Gold Rank"              │
│  "50 XP → Level 12 (unlock achievement)"│
│  "Complete daily challenge for 3x XP"   │
└──────────────┬──────────────────────────┘
               │
               ▼
        ┌──────────────┐
        │  JUST ONE    │
        │     MORE!    │
        └──────┬───────┘
               │
         (REPEAT ↑)
```

**Target Session Length:** 5-15 minutes (mobile-optimized)
**Target Sessions Per Day:** 2-3 (morning commute, lunch, evening)
**Core Loop Completion Time:** 3-5 minutes (quick match or lesson)

---

## 🎯 Progression Systems (4 Core Tracks)

### Track 1: Player Level (Universal Progression)

**Purpose:** Overarching account progression that rewards ALL activities

**Mechanics:**
- **XP Sources:**
  - Match win: 50 XP (casual), 75 XP (ranked)
  - Match loss: 20 XP (casual), 30 XP (ranked)
  - Lesson completion: 40 XP
  - Daily puzzle solved: 60 XP
  - Daily challenge completed: 100 XP (bonus)
  - Login bonus: 10 XP

- **Level Curve:** Exponential (early levels fast, later levels slower)
  - Level 2: 100 XP
  - Level 5: 500 XP
  - Level 10: 2000 XP
  - Level 20: 8000 XP
  - Level 50: 50,000 XP (prestige territory)

- **Unlocks by Level:**
  - Level 1: Casual AI (Beginner), Training Mode
  - Level 3: Friend system, Private lobbies
  - Level 5: Ranked play unlocked
  - Level 7: Casual AI (Intermediate)
  - Level 10: Daily challenges, Advanced AI
  - Level 15: Friend leaderboard
  - Level 20: Advanced lesson tier
  - Level 25: Profile customization
  - Level 30+: Prestige badges, exclusive cosmetics

**Premium Benefit:** 2x XP multiplier on all activities

---

### Track 2: Competitive Rating (ELO System)

**Purpose:** Skill-based competitive ranking for Achievers and Killers

**Mechanics:**
- **Starting Rating:** 1200 (after 5 placement matches)
- **ELO Formula:** Standard chess ELO (K-factor: 32 for new players, 16 for established)
- **Rating Changes:**
  - Win against equal opponent: +16
  - Win against higher-rated: +24
  - Win against lower-rated: +8
  - Loss inverts above
  - Draw (rare in Oware): ±0

**Rank Tiers:**

| Rank | Rating Range | Badge Color | Percentile |
|------|-------------|-------------|------------|
| **Bronze** | 0 - 999 | 🟫 Bronze | 0-30% |
| **Silver** | 1000 - 1399 | ⚪ Silver | 30-60% |
| **Gold** | 1400 - 1799 | 🟡 Gold | 60-85% |
| **Platinum** | 1800 - 2199 | 🔵 Platinum | 85-95% |
| **Diamond** | 2200 - 2599 | 💎 Diamond | 95-99% |
| **Master** | 2600+ | 👑 Master | Top 1% |

**Rank Features:**
- **Promotion:** Reach tier threshold + win 1 more game (promotion match)
- **Demotion Protection:** 3-game grace period when dropping below tier
- **Seasonal Decay:** 1% rating decay per 30 days inactive (prevents rank hoarding)
- **Leaderboards:** Global top 100, Friends, Country/Region

**Premium Benefit:** Rating protection (1 free loss per day doesn't affect rating)

---

### Track 3: Lesson Progress (Mastery System)

**Purpose:** Educational progression for Explorers and learners

**Lesson Structure:**

**Beginner Tier (Free):**
1. "How to Play Oware" - Basic rules, sowing mechanics
2. "Capturing Seeds" - When and how captures happen
3. "The Grand Slam Rule" - Why you can't take all opponent seeds
4. "Opening Moves" - Starting strategy fundamentals
5. "Endgame Basics" - How games end, securing wins

**Intermediate Tier (Level 10+):**
6. "Defensive Play" - Protecting your seeds, denying captures
7. "Setting Traps" - Multi-move capture setups
8. "Counting Seeds" - Mental math for planning ahead
9. "Common Mistakes" - Avoiding beginner errors
10. "Reading Opponents" - Pattern recognition

**Advanced Tier (Premium Only):**
11. "Advanced Openings" - 5 opening strategies with names
12. "Positional Play" - Long-term strategic planning
13. "Endgame Mastery" - Complex endgame scenarios
14. "Regional Variations" - How Oware differs by region
15. "Cultural History" - Deep dive into Oware heritage

**Lesson Mechanics:**
- Each lesson: 3-5 interactive puzzles + explanation text
- Completion: 3 stars (perfect), 2 stars (good), 1 star (pass)
- Replay anytime to improve star rating
- **Mastery %:** (Stars earned / Total possible stars) × 100

**Premium Benefit:** Unlocks Advanced tier (lessons 11-15)

---

### Track 4: Streak System (Daily Engagement)

**Purpose:** Habit formation (Duolingo's 14% retention boost)

**Mechanics:**
- **Streak Counter:** Days played consecutively
- **Streak Saved:** Any activity counts (match, lesson, puzzle)
- **Login Bonus:** Open app = +10 XP + streak maintained
- **Streak Milestones:**
  - 7 days: "Week Warrior" badge + 100 XP bonus
  - 30 days: "Monthly Master" badge + 500 XP + exclusive cosmetic
  - 100 days: "Century Club" badge + 2000 XP + rare cosmetic
  - 365 days: "Year Legend" badge + 10,000 XP + ultra-rare cosmetic

**Streak Freeze:**
- **Free Players:** Can earn 1 freeze per week (play 7 days straight)
- **Premium Players:** 2 freezes per month (automatic if missed day)
- Freeze = 1-day grace period (streak doesn't break)

**Streak Sharing:**
- Auto-generated image: "20-day Oware streak! 🔥"
- Social proof drives viral growth (Metric A: User Growth)

**Premium Benefit:** 2 automatic streak freezes per month

---

## 📅 Daily Engagement Hooks

### 1. Login Bonus (Passive)
- **Trigger:** Open app
- **Reward:** +10 XP, streak maintained
- **Psychology:** Removes barrier ("just open, no pressure")

### 2. Daily Challenge (Active)
- **Refresh:** Midnight UTC
- **Types:**
  - "Win 2 matches today" → 50 bonus XP
  - "Complete 1 lesson" → 40 bonus XP
  - "Solve today's puzzle in under 2 minutes" → 80 bonus XP
  - "Play 1 match with a friend" → 60 bonus XP (social hook)
- **Bonus:** 3x XP multiplier on challenge activity
- **UI:** Prominent banner on home screen

### 3. Daily Puzzle (Featured)
- **Refresh:** Midnight UTC
- **Format:** "Capture 12+ seeds in 3 moves" (tactical puzzle)
- **Leaderboard:** Fastest solve times (global + friends)
- **Reward:** 60 XP base + leaderboard bonus
- **Archive:** Past puzzles unlocked at Level 15 (or Premium)

### 4. Streak Reminder Push Notification
- **Timing:** 8 PM local time (if not played today)
- **Message:** "Don't lose your 12-day streak! Play a quick match 🔥"
- **Psychology:** Loss aversion (Duolingo's secret weapon)

---

## 🎮 Game Modes

### Mode 1: Casual Play
**Purpose:** Low-stakes practice, friend matches

**Rules:**
- No rating changes
- Play vs AI or friends
- No ads (Premium) or interstitial ads between matches (Free)
- XP earned: 50 (win), 20 (loss)

**AI Difficulty:**
- **Beginner** (Level 1+): Makes random moves, occasional good move
- **Intermediate** (Level 7+): Looks ahead 2-3 moves, makes captures
- **Advanced** (Level 10+ or Premium): Looks ahead 5+ moves, strategic play

**Matchmaking:** None (player selects opponent)

---

### Mode 2: Ranked Play
**Purpose:** Competitive ELO-based matches

**Rules:**
- ELO rating changes based on result
- Unlocked at Level 5
- Matchmaking: ±100 rating points
- XP earned: 75 (win), 30 (loss)
- Leaderboard eligibility

**Matchmaking Algorithm:**
1. Search for opponent ±50 rating (30 sec)
2. Expand to ±100 rating (next 30 sec)
3. Expand to ±200 rating (next 30 sec)
4. Match anyone if >90 sec wait (rare)

**Leave Penalty:**
- Abandon match = -50 rating, counted as loss
- 3 abandons in 24 hours = 1-hour ranked ban

---

### Mode 3: Training Mode
**Purpose:** Interactive lessons, tutorial system

**Structure:**
- 15 lessons total (5 free, 5 Level 10+, 5 Premium)
- Each lesson: Explanation → Interactive puzzle → Summary
- Stars earned: 1-3 based on performance
- Replay anytime to improve

**UI Flow:**
1. Lesson list screen (progress tracker)
2. Lesson intro (text + visuals)
3. Interactive board (player solves puzzle)
4. Feedback (correct/incorrect + explanation)
5. Completion screen (stars, XP reward)

---

### Mode 4: Daily Puzzle
**Purpose:** Daily challenge, habit formation

**Mechanics:**
- One puzzle per day (rotates midnight UTC)
- Tactical scenarios: "Capture X seeds in Y moves"
- Difficulty scales gradually (Week 1 = easy, Week 4+ = hard)
- Timer tracks solve speed
- Leaderboards: Global + Friends (fastest solvers)

**Rewards:**
- Solve puzzle: 60 XP
- Top 100 global: Bonus 20 XP
- #1 on friends leaderboard: Bonus 10 XP

**Puzzle Archive (Premium/Level 15+):**
- Access past 365 days of puzzles
- Filter by difficulty
- Practice mode (no leaderboard)

---

### Mode 5: Private Lobbies
**Purpose:** Safe family/friend play

**Mechanics:**
- Create lobby → Generate room code (e.g., "ABCD-1234")
- Share code via WhatsApp, text, etc.
- Lobby settings:
  - Time limit per move (optional)
  - Allow spectators (yes/no)
  - Rematch (yes/no)
- Max 2 players per lobby (1v1 game)

**Safety:**
- Preset emotes only (no text chat)
- Report player option (moderator review)

**Limits:**
- Free: 3 active lobbies max
- Premium: Unlimited

---

## ⚙️ Oware Rules (Standard Ghanaian)

### Setup
- 12 pits total (6 per player, facing each other)
- 4 seeds in each pit at start (48 seeds total)
- No store/mancala pits (captured seeds tracked separately)

### Gameplay
1. **Turn:** Player picks a pit on their side (must contain seeds)
2. **Sowing:** Distribute seeds counter-clockwise, one per pit
3. **Capture:** If final seed lands in opponent's pit with 2 or 3 seeds total (including dropped seed), capture those seeds
4. **Continuing Captures:** If previous pit also has 2-3 seeds in opponent's territory, capture those too (chain backwards)
5. **Grand Slam Rule:** If a move would capture ALL opponent's seeds, the move is legal BUT no seeds are captured (they must leave opponent with seeds)

### Winning
- **Victory Condition:** Capture 25+ seeds (majority)
- **End Conditions:**
  1. One player captures 25+ seeds (immediate win)
  2. Opponent has no legal moves (remaining seeds awarded to moving player)
  3. Infinite loop detected (remaining seeds split, most seeds wins)

### Edge Cases
- **No legal moves:** If all your pits are empty, opponent continues playing until seeds enter your side or game ends
- **Forced feeding:** If opponent has no seeds, you MUST make a move that gives them seeds (if possible)

---

## 🧠 Psychological Hooks (Research-Backed)

### 1. Variable Reward Schedules
**Research:** B.F. Skinner's operant conditioning
**Application:**
- XP varies by performance (20-100 XP per activity)
- Leaderboard position changes unpredictably
- Daily puzzles vary in difficulty (unexpected challenge)

### 2. Loss Aversion (Streaks)
**Research:** Kahneman & Tversky (Prospect Theory)
**Application:**
- Losing 20-day streak hurts more than gaining 1 day feels good
- Push notifications leverage this: "Don't lose your streak!"
- Streak Freeze = paid insurance against loss

### 3. Progress Illusion (Multiple Tracks)
**Research:** Nir Eyal's "Hooked" model
**Application:**
- Always progressing on SOMETHING (level, rank, lessons, streak)
- "Only 50 XP to level up!" (goal proximity)
- Next goal always visible (prevents "now what?" moment)

### 4. Social Proof (Sharing)
**Research:** Cialdini's influence principles
**Application:**
- "10,000 players reached Gold rank today"
- Friend leaderboards (comparison drives competition)
- Shareable achievements (viral loops)

### 5. Endowed Progress Effect
**Research:** Nunes & Drèze (2006)
**Application:**
- Start Level 1 with 10 XP (not 0) → "already made progress"
- Placement matches give provisional rank → "you're Silver already!"
- First lesson completed in tutorial → "1/15 lessons complete"

---

## 🎯 Balancing for Retention

### Target Metrics (Industry Benchmarks):
- **D1 Retention:** 40%+ (industry avg: 25%)
- **D7 Retention:** 20%+ (industry avg: 10%)
- **D30 Retention:** 10%+ (industry avg: 5%)

### Design Principles:
1. **Never let players hit dead ends** → Always show next goal
2. **Reward showing up** → Login bonus reduces friction
3. **Chunk progression** → 5-min sessions feel complete
4. **Multiple paths** → Different player types find their hook
5. **Surprise & delight** → Unexpected rewards (rank up animation, rare achievement)

---

## 📊 Analytics & Tuning

### Key Metrics to Track:
- **Core Loop Completion Rate:** % who finish a match/lesson after starting
- **Average Loops Per Session:** How many activities per play session
- **Return Rate:** % who return within 24 hours
- **Streak Survival Curve:** % maintaining streak at Days 7, 14, 30, 90
- **Progression Bottlenecks:** Where players stop leveling/ranking

### A/B Testing Opportunities:
- XP values (is 50 XP for win optimal?)
- Level curve speed (levels 1-10 too fast/slow?)
- Streak reminder timing (8 PM vs 9 PM push notification)
- Daily challenge difficulty (too easy = boring, too hard = frustration)

---

## 🚀 Future Enhancements (Post-MVF)

### V1.1 - Achievements System
- 30-40 achievements (first win, 100 matches played, reach Diamond, etc.)
- Badge showcase on profile
- Achievement-hunting drives Achievers

### V1.2 - Tournaments
- Weekly tournaments (Swiss system, 5 rounds)
- Prize pool (Premium currency, exclusive cosmetics)
- Tournament rating separate from ladder rating

### V2.0 - Battle Pass
- Seasonal progression (3-month seasons)
- Free track + Premium track
- Cosmetic rewards, XP boosts, exclusive lessons
- Soft rating reset each season

---

**Document Version:** 1.0
**Last Updated:** 2025-10-14
**Status:** Core mechanics finalized

*"The loop that makes them come back for 'just one more'"*
