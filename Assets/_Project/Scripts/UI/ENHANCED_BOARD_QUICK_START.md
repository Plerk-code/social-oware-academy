# 🚀 Enhanced Oware Board - Quick Start

Get your beautiful wooden board running in **5 minutes**!

## 🎯 What You Need

✅ Unity 2021.3 or later  
✅ DOTween (free from Package Manager)  
✅ Your existing Oware game (GameManager, OwareBoard, etc.)

## ⚡ Super Quick Setup (5 Minutes)

### Method 1: Use the Setup Wizard (Easiest!)

1. **Open the wizard**: `Tools > Oware > Enhanced Board Setup Wizard`

2. **Click the big green button**: `🚀 SETUP EVERYTHING!`

3. **Press Play** and enjoy! 🎉

That's it! The wizard:
- ✅ Creates the Enhanced Board GameObject
- ✅ Generates marble seed prefab
- ✅ Configures lighting
- ✅ Positions camera
- ✅ Disables old visualizer

---

### Method 2: Manual Setup (5 minutes)

#### Step 1: Create Board GameObject

1. In your scene, create empty GameObject: `Enhanced Oware Board`
2. Add Component: `EnhancedOwareBoardVisualizer`

#### Step 2: Disable Old Visualizer

1. Find your old `OwareBoardVisualizer` component
2. Uncheck the checkbox to disable it

#### Step 3: Press Play!

The enhanced visualizer works with default settings!

---

## 🎨 What You Get

### Beautiful 3D Wooden Board
- Light brown wood texture
- Carved pit holes
- Natural appearance
- Warm lighting

### Colorful Marble Seeds
- Three colors: Red, Beige, Brown
- Randomly distributed
- Smooth, shiny appearance
- Bounce animation on drop

### Professional UI
- Seed counters above/below pits
- Circular backgrounds
- Clean, readable numbers
- Store displays

### Smooth Animations
- Seeds drop with bounce
- Seeds rearrange naturally
- Pits highlight on click
- Score counting animations

---

## 🎮 How to Play

### Controls
- **Click a pit** (bottom row) to make your move
- **Space bar** to start a new game
- **ESC** for menu (if configured)

### Visual Feedback
- **Golden glow** = Selected pit
- **Seed animation** = Move in progress
- **Counter update** = Score change

---

## 🔧 Quick Customization

### Change Wood Color

Select your `Enhanced Oware Board` GameObject:

1. Find `Wood Color` in Inspector
2. Click the color box
3. Choose your color:
   - **Light Maple**: `#F5DEB3`
   - **Dark Walnut**: `#3D2817`
   - **Medium Oak**: `#C19A6B`

### Change Marble Colors

In Inspector, expand `Seed Colors` array:

1. Size: 3 (or more!)
2. Element 0: Red/Terracotta `#E37359`
3. Element 1: Beige/Pink `#F3E0C4`
4. Element 2: Dark Brown `#432717`

Add more colors for variety!

### Adjust Pit Size

- **Pit Radius**: Make pits bigger/smaller
- **Pit Spacing**: Spread pits apart
- **Seed Size**: Change marble size

---

## 🐛 Troubleshooting

### "Seeds not appearing!"

**Check:**
1. Is GameManager in scene?
2. Is GameManager started? (check console)
3. Do pits have initial seeds (should be 4 each)?

**Fix:** Press Space to start new game

---

### "Board looks very dark!"

**Check:**
1. Is "Create Custom Lighting" enabled?
2. Is there a Directional Light in scene?

**Fix:** 
- Use the Setup Wizard to configure lighting
- Or: Add Directional Light, set intensity to 1.2

---

### "Can't click pits!"

**Check:**
1. Is it your turn? (bottom row only)
2. Are pits empty? (need seeds to move)
3. Is game active? (press Space to start)

**Fix:** Check console for `[EnhancedVisualizer]` messages

---

### "DOTween errors!"

**Install DOTween:**
1. Window > Package Manager
2. Search "DOTween"
3. Install "DOTween (HOTween v2)"

Or use Unity Asset Store version (free)

---

## 📖 Need More Help?

### Full Documentation
- **Setup Guide**: `ENHANCED_BOARD_SETUP.md` (detailed customization)
- **Open via**: `Tools > Oware > Documentation > Open Setup Guide`

### Quick Actions Menu
- `Tools > Oware > Quick Actions > Disable Old Visualizer`
- `Tools > Oware > Quick Actions > Enable Old Visualizer`

### Debug Info
Watch the Console for messages:
- `[EnhancedVisualizer] Board creation complete!` = Success!
- `[EnhancedVisualizer] Game started` = Game is running
- `[EnhancedVisualizer] Move made: Pit X` = Move registered

---

## 🎨 Inspiration

This enhanced visualizer is inspired by the beautiful Mancala game on CrazyGames:
- Wooden board aesthetic
- Colorful marble seeds
- Clean, modern UI
- Warm, inviting atmosphere

But with Unity 3D power:
- Smooth animations
- Interactive feedback
- Professional polish
- Mobile-ready

---

## ✨ What's Next?

### Immediate (Do Now)
1. ✅ Play a game and test everything
2. 🎨 Customize colors to your preference
3. 📸 Take screenshots!

### This Week
1. 🎵 Add sound effects (seed drop, click, win)
2. ✨ Add particle effects (confetti on win)
3. 📱 Test on mobile device

### This Month
1. 🖼️ Create custom marble textures
2. 🎭 Add player avatars
3. 🏆 Add trophy displays

---

## 🎉 You're Ready!

Your Oware board now looks professional and inviting.

**Press Play and enjoy your beautiful game!** 🎮✨

---

*Questions? Check the full setup guide or Unity console for debug info.*

**Happy gaming!** 🎲🌟

