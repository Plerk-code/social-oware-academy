# Epic 2 - AI Testing Guide

**Stories Covered:** 2.1 (Beginner AI), 2.2 (Intermediate AI)
**Status:** Code Complete - Ready for Testing
**Last Updated:** 2025-10-21

---

## Quick Test Checklist

### Automated Unit Tests (Unity Test Runner)

1. **Open Unity Test Runner**
   - Unity Editor → Window → General → Test Runner
   - Click **EditMode** tab

2. **Run All AI Tests**
   - Click **Run All** button
   - Expected Results:
     - ✅ BeginnerAITests: 8 tests should PASS
     - ✅ IntermediateAITests: 8 tests should PASS
     - Total: 16/16 tests passing

3. **Individual Test Suites**
   - Expand `BeginnerAITests` - verify all green checkmarks
   - Expand `IntermediateAITests` - verify all green checkmarks

---

## Manual PlayMode Testing

### Test 1: Beginner AI (Story 2.1)

**Setup:**
1. Open scene: `Assets/_Project/Scenes/Boot.unity`
2. Select **GameManager** GameObject in Hierarchy
3. In Inspector, verify settings:
   - ✓ Is AI Opponent = **Checked**
   - AI Difficulty = **Beginner**
   - Human Player Index = **0** (you play as Player 1)

**Test Execution:**
1. Enter **Play Mode** (⌘P / Ctrl+P)
2. Play **5 matches** against Beginner AI
3. Click pits on bottom row (0-5) to make your moves
4. AI will respond on top row (6-11)

**What to Verify:**

| Criteria | Expected Result | How to Verify |
|----------|----------------|---------------|
| **Valid Moves** | AI never makes illegal moves | Game never crashes, no error logs |
| **AI Delay** | 1.0-1.5s response time | Feels natural, not instant |
| **Random Play** | AI makes varied moves | Not always same move in same position |
| **Strategic Moves** | Occasional smart captures | See "[BeginnerAI] Strategic move selected" in Console |
| **Performance** | Smooth 60 FPS, no lag | No stuttering during AI turns |
| **Win Rate** | You win 60-70% of matches | Win 3-4 out of 5 games |

**Console Logs to Watch:**
```
[GameManager] Starting new game...
[GameManager] AI opponent initialized: Beginner
[BeginnerAI] Random move selected: Pit X
[BeginnerAI] Strategic move selected: Pit Y  ← Occasionally see this
[GameManager] AI decision time: Xms  ← Should be <100ms
```

---

### Test 2: Intermediate AI (Story 2.2)

**Setup:**
1. Stay in `Boot.unity` scene
2. **Exit Play Mode** if running
3. Select **GameManager** in Hierarchy
4. In Inspector, change:
   - AI Difficulty = **Intermediate**

**Test Execution:**
1. Enter **Play Mode**
2. Play **5 matches** against Intermediate AI
3. Observe AI behavior carefully

**What to Verify:**

| Criteria | Expected Result | How to Verify |
|----------|----------------|---------------|
| **Valid Moves** | AI never makes illegal moves | No crashes or errors |
| **AI Delay** | 1.5-2.0s response time | Slightly longer than Beginner |
| **Tactical Play** | AI plans ahead, makes captures | Better play than Beginner |
| **Performance** | <500ms execution, 60 FPS | Check Console for "AI decision time" |
| **Difficulty** | More challenging than Beginner | Harder to win |
| **Win Rate** | You win 40-50% of matches | Win 2-3 out of 5 games |

**Console Logs to Watch:**
```
[GameManager] AI opponent initialized: Intermediate
[IntermediateAI] Selected move X with score Y
[GameManager] AI decision time: Xms  ← Should be <500ms (likely <250ms)
```

**Tactical Play Indicators:**
- AI sets up multi-move captures
- AI blocks your capture opportunities
- AI maintains seed control on its side
- AI makes better endgame decisions than Beginner

---

### Test 3: Direct AI Comparison

**Setup:**
1. Exit Play Mode
2. Create new empty GameObject: `AI Tester`
3. Attach component: `BeginnerAITest` script

**Test Execution:**
1. Select `AI Tester` GameObject
2. In Inspector, right-click component header
3. Run these context menu tests:

**BeginnerAI Tests:**
- **1. Run Performance Test** - Verifies <100ms execution
  - Expected: Average ~10-50ms
- **2. Run Win Rate Test (100 Games)** - Beginner vs Random
  - Expected: Results show win distribution
- **3. Run Strategic Move Analysis** - Tests capture detection
  - Expected: AI detects capture opportunities

**Results to Document:**
- Average decision time
- Win rate percentages
- Strategic behavior observations

---

## Performance Profiling

### Using Unity Profiler

**Setup:**
1. Window → Analysis → Profiler
2. Enter Play Mode with Beginner AI
3. Let AI make several moves

**What to Monitor:**

**CPU Usage Tab:**
- Find `BeginnerAI.GetMove()` in Hierarchy
  - Should be <5ms per call
- Find `GameManager.TriggerAIMove()`
  - Should show ~1.5s total (includes delay)

**Memory Tab:**
- Check for GC allocations during AI turns
  - Should be minimal (<1KB per move)

**Rendering Tab:**
- Verify frame time stays <16.67ms (60 FPS)
  - Even during AI "thinking"

### Repeat with Intermediate AI

**Expected Metrics:**
- `IntermediateAI.GetMove()` - <250ms typical, <500ms max
- `Minimax()` function shows tree depth 3
- No memory leaks over multiple moves
- 60 FPS maintained

---

## Pass/Fail Criteria

### Story 2.1 (Beginner AI) - Pass if:
- ✅ All 8 unit tests pass
- ✅ AI makes only valid moves (0 errors in 5 games)
- ✅ AI execution <100ms average
- ✅ Win rate: 60-70% player wins
- ✅ 60 FPS maintained
- ✅ No console errors

### Story 2.2 (Intermediate AI) - Pass if:
- ✅ All 8 unit tests pass
- ✅ AI makes only valid moves (0 errors in 5 games)
- ✅ AI execution <500ms (typically <250ms)
- ✅ Win rate: 40-50% player wins (harder than Beginner)
- ✅ AI demonstrates tactical thinking (multi-move planning)
- ✅ 60 FPS maintained
- ✅ No console errors

---

## Troubleshooting

### Issue: Tests won't run in Test Runner
**Solution:**
- Ensure Unity Editor is not in Play Mode
- Reimport test files: Right-click → Reimport
- Check Console for compilation errors

### Issue: AI takes too long to respond
**Check:**
- Console shows "AI decision time"
- If Beginner >100ms or Intermediate >500ms, check Profiler
- Verify `MAX_DEPTH` constants in AI scripts

### Issue: AI makes invalid moves
**Check:**
- Console errors immediately after AI move
- Verify `OwareRules.GetValidMoves()` is called
- Check game state before/after move

### Issue: Can't see Console logs
**Solution:**
- Window → General → Console
- Ensure no filters active (clear search box)
- Check "Collapse" is unchecked to see all logs

---

## Test Results Template

Copy this to record your results:

```markdown
# Epic 2 Testing Results

**Tester:** [Your Name]
**Date:** 2025-10-21
**Unity Version:** 6000.2.6f2

## Automated Tests
- [ ] BeginnerAITests: __/8 passing
- [ ] IntermediateAITests: __/8 passing

## Beginner AI (Story 2.1)
- [ ] Valid moves only (5 games, 0 errors)
- [ ] AI decision time: avg ___ms (target <100ms)
- [ ] Win rate: Won __/5 games (target 60-70%)
- [ ] Performance: 60 FPS maintained
- [ ] Strategic moves observed

**Notes:**


## Intermediate AI (Story 2.2)
- [ ] Valid moves only (5 games, 0 errors)
- [ ] AI decision time: avg ___ms (target <500ms)
- [ ] Win rate: Won __/5 games (target 40-50%)
- [ ] Performance: 60 FPS maintained
- [ ] Tactical play observed (multi-move planning)

**Notes:**


## Profiler Results
- Beginner AI: avg ___ms per GetMove() call
- Intermediate AI: avg ___ms per GetMove() call
- Memory allocations: ___KB per move
- Frame time: ___ms (target <16.67ms)

## Overall Status
- [ ] Story 2.1 PASS / FAIL
- [ ] Story 2.2 PASS / FAIL

**Issues Found:**


**Recommendations:**


```

---

## Next Steps After Testing

1. **If Tests Pass:**
   - Mark stories as "Done" in story files
   - Update Epic 2 progress
   - Proceed to Story 2.3 (Advanced AI)

2. **If Tests Fail:**
   - Document failures in story Debug Log
   - Create bug fix tasks
   - Re-test after fixes

3. **Performance Issues:**
   - Tune AI parameters (depth, heuristic weights)
   - Optimize board cloning
   - Consider caching evaluations

---

**Ready to test!** Start with automated unit tests, then proceed to manual PlayMode testing.
