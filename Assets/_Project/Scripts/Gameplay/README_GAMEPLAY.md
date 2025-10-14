# Gameplay Scripts - Social Oware Academy

Core game logic implementation for Oware.

---

## 📦 What's Been Built (Sprint 1 Progress)

### ✅ Core Classes Created

1. **OwareBoard.cs** - Board state management
   - 12-pit board representation (6 per player)
   - Seed tracking and capture counting
   - Event system for board changes
   - Clone method for AI lookahead
   - Win condition detection (25+ seeds)

2. **OwareRules.cs** - Game rules engine
   - Standard Ghanaian Oware rules implementation
   - Move validation (forced feeding, valid pits)
   - Counter-clockwise sowing logic
   - Capture detection (2-3 seeds in opponent's pit)
   - Grand Slam rule prevention
   - Chain capture support

3. **GameManager.cs** - Game orchestration
   - Singleton pattern for global access
   - Game flow management (start, move, end)
   - Turn management
   - Simple AI opponent (random valid move)
   - Event broadcasting for UI updates

4. **OwareBoardVisualizer.cs** - 3D visualization
   - Unity primitive-based board (cubes for pits)
   - Click-to-move interaction
   - Real-time seed count display
   - Player color coding (cyan/magenta)
   - UI text for game state

---

## 🎮 How to Test (Unity Editor)

### Setup:
1. Open Unity project
2. Create new scene: `Assets/_Project/Scenes/GameTest.unity`
3. Add empty GameObject → Name it "GameManager"
4. Add `GameManager` component to it
5. Add empty GameObject → Name it "BoardVisualizer"
6. Add `OwareBoardVisualizer` component to it

### Optional UI Setup:
1. Create Canvas (right-click Hierarchy → UI → Canvas)
2. Create 2 TextMeshPro text elements:
   - `GameStateText` (top-left, for scores)
   - `InstructionsText` (bottom-center, for instructions)
3. Drag these to the `OwareBoardVisualizer` component's UI fields

### Play:
1. Press Play in Unity Editor
2. Game starts automatically
3. **Click on bottom row pits (cyan)** to make your move
4. AI responds automatically (magenta pits)
5. Press **SPACE** to restart game

---

## 🎯 Game Rules Implemented

### Standard Ghanaian Oware:
- **Setup:** 12 pits (6 per player), 4 seeds per pit, 48 total seeds
- **Sowing:** Counter-clockwise, skip starting pit
- **Capture:** Landing in opponent's pit with 2-3 seeds total
- **Chain Captures:** Continue backwards if previous pits also have 2-3
- **Grand Slam Rule:** Can't capture ALL opponent's seeds
- **Forced Feeding:** Must give opponent seeds if they have none
- **Win Condition:** First to capture 25+ seeds

---

## 🧪 What's Working

✅ Board state management
✅ Move validation
✅ Sowing mechanics
✅ Capture logic
✅ Grand Slam prevention
✅ Turn switching
✅ Win condition detection
✅ Basic AI opponent (random moves)
✅ 3D visualization
✅ Click interaction

---

## 🚧 What's Next (Sprint 2-4)

### Immediate (Week 3-4):
- [ ] **Improved AI** - Minimax algorithm (Beginner/Intermediate/Advanced)
- [ ] **Animation** - Seed sowing animation with DOTween
- [ ] **VFX** - Capture particle effects
- [ ] **SFX** - Click sounds, capture sounds
- [ ] **Game Scene** - Dedicated game scene with proper UI

### Future (Week 5-6):
- [ ] **Multiplayer** - Photon Fusion integration
- [ ] **Matchmaking** - Find online opponents
- [ ] **Private Lobbies** - Room codes for friends

---

## 🐛 Known Issues

1. **AI is too simple** - Just picks random valid moves (intentional for MVP)
2. **No animation** - Moves happen instantly (will add DOTween)
3. **No sound** - Silent game (will add AudioManager integration)
4. **Primitive visuals** - Just cubes (following prototyping-first strategy)

---

## 📚 Code Structure

```
Gameplay/
├── OwareBoard.cs         (Data model - board state)
├── OwareRules.cs         (Business logic - rules engine)
├── GameManager.cs        (Controller - game orchestration)
└── OwareBoardVisualizer.cs (View - Unity visualization)
```

This follows **MVC pattern** (Model-View-Controller):
- **Model:** OwareBoard (pure data)
- **Controller:** GameManager (logic orchestration)
- **View:** OwareBoardVisualizer (Unity-specific visualization)

---

## 🎓 Learning Notes

### Key Unity Concepts Used:
- **Singleton Pattern** - GameManager.Instance for global access
- **Event System** - C# events for loose coupling (OnGameStarted, OnMoveMade, etc.)
- **GameObject Primitives** - CreatePrimitive(Cube) for rapid prototyping
- **TextMeshPro** - Better text rendering than legacy UI Text
- **Raycasting** - OnMouseDown() for pit click detection

### Oware-Specific Logic:
- **Counter-clockwise sowing** - `(currentPit + 1) % 12`
- **Capture chains** - Loop backwards while conditions met
- **Grand Slam check** - Simulate capture, count opponent's remaining seeds
- **Forced feeding** - Check if move gives opponent seeds before allowing it

---

## 🔗 Related Documentation

- [GDD: Core Loop & Mechanics](../../../docs/03-core-loop-mechanics.md) - Full mechanics spec
- [GDD: Feature Specifications](../../../docs/04-feature-specifications.md) - MVP features
- [GDD: MVF Roadmap](../../../docs/08-mvf-roadmap.md) - Sprint plan

---

**Status:** Sprint 1 Core Logic Complete ✅
**Next:** Sprint 2 - AI & Animation
**Last Updated:** 2025-10-14
