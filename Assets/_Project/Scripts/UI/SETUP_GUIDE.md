# Oware Match Screen UI - Setup Guide

## Overview
This guide will help you integrate the new beautiful UI components with your existing Oware game.

## What We've Created

### 1. Core Systems
- **ColorPalette.cs** - Centralized color management with West African-inspired palette
- **LayoutConstants.cs** - Consistent spacing and sizing constants
- **ButtonAnimator.cs** - Automatic button press animations
- **MatchScreenUI.cs** - Main match screen controller
- **SeedAnimator.cs** - Beautiful seed hopping and capture animations

## Setup Steps

### Step 1: Create ColorPalette Asset
1. In Unity, go to `Assets/_Project/Data/` (create folder if needed)
2. Right-click → Create → Oware → UI → Color Palette
3. Name it "ColorPalette"
4. Move it to `Assets/_Project/Resources/` so it can be loaded at runtime

### Step 2: Setup Match Screen Canvas

#### A. Create Main Canvas
1. Right-click in Hierarchy → UI → Canvas
2. Rename to "MatchScreenCanvas"
3. Set Canvas Scaler:
   - UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1080 x 1920
   - Match: 0.5 (balance width/height)

#### B. Create Player Panels

**Player 1 Panel (Left Side):**
1. Create → UI → Image, name "Player1Panel"
2. Anchor: Middle Left
3. Position: X=90, Y=0
4. Size: 180 x 200
5. Add CanvasGroup component (for animations)

**Player 1 Components:**
- Add child TextMeshPro - Text (UI): "Player1Name"
  - Text: "YOU"
  - Font Size: 16
  - Bold
  - Alignment: Top Center
- Add child TextMeshPro - Text (UI): "Player1Score"
  - Text: "0"
  - Font Size: 48
  - Bold
  - Alignment: Center

**Player 2 Panel (Right Side):**
- Repeat same structure for Player 2
- Anchor: Middle Right
- Position: X=-90, Y=0

#### C. Create Turn Indicator
1. Create → UI → Image, name "TurnIndicator"
2. Anchor: Top Center
3. Position: X=0, Y=-100
4. Size: 300 x 60
5. Add child TextMeshPro: "TurnText"
   - Text: "YOUR TURN"
   - Font Size: 24
   - Bold

#### D. Create Game Over Panel
1. Create → UI → Panel, name "GameOverPanel"
2. Anchor: Stretch All
3. Add CanvasGroup component
4. Set initially inactive (uncheck in Inspector)

**Game Over Components:**
- Background (dark semi-transparent)
- Title Text: "YOU WIN!" / "AI WINS!" / "DRAW!"
- Score Text: "24 - 20"
- Play Again Button
- Main Menu Button

### Step 3: Add Scripts to Scene

1. **Add MatchScreenUI:**
   - Select MatchScreenCanvas
   - Add Component → MatchScreenUI
   - Assign all references in Inspector:
     - Player panels
     - Name texts
     - Score texts
     - Turn indicator
     - Game over panel elements

2. **Add SeedAnimator:**
   - Create empty GameObject: "SeedAnimator"
   - Add Component → SeedAnimator
   - Create simple seed prefab (sphere or cube)
   - Assign to Seed Prefab field

3. **Keep existing OwareBoardVisualizer:**
   - Your current 3D board visualization works great!
   - It will continue to handle pit creation and clicking
   - The new MatchScreenUI adds the polish on top

### Step 4: Add Animations to Buttons

For every button in your UI:
1. Select button GameObject
2. Add Component → ButtonAnimator
3. That's it! Automatic press animations

### Step 5: Test in Play Mode

1. Press Play
2. You should see:
   - ✅ Colorful player panels
   - ✅ Animated score displays
   - ✅ Turn indicator animations
   - ✅ Smooth button presses
   - ✅ Beautiful game over screen

## Color Palette Reference

Access colors in your scripts:
```csharp
using SocialOwareAcademy.UI;

// Get instance
ColorPalette palette = ColorPalette.Instance;

// Use colors
image.color = palette.terracotta;
text.color = palette.offWhite;
```

### Available Colors:
- **Primary:** terracotta, ochre, deepBrown
- **Accents:** gold, emerald, azure
- **Neutrals:** offWhite, charcoal, lightGray, darkGray
- **Semantic:** success, danger, info, warning
- **Players:** player1 (green), player2 (red), selected (gold)

## Animation Timing Reference

Use LayoutConstants for consistent timing:
```csharp
using SocialOwareAcademy.UI;

// Animation durations
LayoutConstants.ANIM_FAST;    // 0.15s
LayoutConstants.ANIM_MEDIUM;  // 0.3s
LayoutConstants.ANIM_SLOW;    // 0.5s

// Spacing
LayoutConstants.SPACE_SMALL;  // 8pt
LayoutConstants.SPACE_MEDIUM; // 16pt
LayoutConstants.SPACE_LARGE;  // 24pt
```

## Troubleshooting

### "ColorPalette.Instance is null"
- Make sure ColorPalette asset is in `Assets/_Project/Resources/`
- Name must be exactly "ColorPalette"

### "DOTween namespace not found"
- Install DOTween from Asset Store (free)
- Tools → Demigiant → DOTween Utility Panel → Setup DOTween

### "AudioManager.Instance is null"
- Buttons will still work, just no sound
- You can create an AudioManager later

### Animations not smooth
- Check Frame Rate in Stats panel (should be 60 FPS)
- Reduce particle effects if needed
- Check Canvas rebuilds in Profiler

## Next Steps

### Enhance the 3D Board
Your current OwareBoardVisualizer uses colored cubes. To make it stunning:
1. Add rounded corners using Shader Graph
2. Add subtle shadows (Shadow component on pits)
3. Add glow effect on selected pit
4. Add particle trail on seed animations

### Add Sound Effects
Create simple AudioManager:
- "button_press" - Click sound
- "seed_drop" - Seed landing
- "capture" - Capture success
- "victory" - Game win

### Polish Touches
- Add slight screen shake on captures
- Add confetti particles on win
- Add streak fire effect for combos
- Add haptic feedback (already wired up!)

## Questions?

The code is well-commented and follows Unity best practices. Key files to explore:
- `MatchScreenUI.cs` - Main UI controller logic
- `ButtonAnimator.cs` - Simple animation example
- `SeedAnimator.cs` - Advanced object pooling example

Happy building! 🎨
