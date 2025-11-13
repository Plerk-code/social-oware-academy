# 🎮 2D Oware Board - Complete Setup Guide

Build a beautiful CrazyGames-style Oware board in **30 minutes**!

---

## 🚀 Quick Start (10 Minutes - No Assets Needed!)

You can test the system **immediately** without creating any graphics. It auto-generates everything!

### **Step 1: Create the UI Canvas** (2 minutes)

1. **In Unity Hierarchy:**
   ```
   Right-click → UI → Canvas
   ```
   Name it: `Oware 2D Board Canvas`

2. **Configure Canvas:**
   ```
   Select the Canvas
   Inspector → Canvas component:
   - Render Mode: Screen Space - Overlay
   - UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920 x 1080
   ```

3. **Add EventSystem** (if not auto-created):
   ```
   If you don't see "EventSystem" in Hierarchy:
   Right-click → UI → Event System
   ```

---

### **Step 2: Add Board Background** (1 minute)

1. **Create Background Image:**
   ```
   Right-click on Canvas → UI → Image
   Name it: "Board Background"
   ```

2. **Configure Background:**
   ```
   Inspector → Rect Transform:
   - Anchor: Stretch/Stretch (full screen)
   - Left: 0, Right: 0, Top: 0, Bottom: 0
   
   Inspector → Image:
   - Color: Light Brown (#D1B28CFF)
   - Leave Source Image empty for now
   ```

---

### **Step 3: Add the Board Controller** (2 minutes)

1. **Create Board GameObject:**
   ```
   Right-click on Canvas → Create Empty
   Name it: "Oware 2D Board"
   ```

2. **Add Component:**
   ```
   Select "Oware 2D Board"
   Inspector → Add Component
   Type: "Oware2DBoardUI"
   Click it when it appears
   ```

3. **Assign References:**
   ```
   In Inspector → Oware2DBoardUI component:
   
   Board Background:
   - Board Background Image: Drag "Board Background" here
   - Board Tint Color: Leave as default (light brown)
   
   Other fields: Leave empty for now (auto-generates)
   ```

---

### **Step 4: Disable Old Visualizer** (30 seconds)

```
Find your old board GameObject
→ Disable or delete "OwareBoardVisualizer" component
→ Or disable "Enhanced Oware Board" GameObject
```

**You should only have ONE board system active!**

---

### **Step 5: Press Play!** 🎮

```
→ Click Play button
→ You should see a brown background
→ With 12 dark circular pits
→ With colored marbles in each pit
→ With white number counters
```

**It works!** Now let's make it beautiful! ✨

---

## 🎨 Phase 2: Add Custom Graphics (20 Minutes)

Now that it works, let's add beautiful sprites!

### **Step 1: Create Asset Folder**

```
In Unity Project window:
Assets → _Project → UI → Create New Folder → "Sprites"
```

---

### **Step 2: Create/Import Graphics**

**See `ASSET_CREATION_GUIDE.md` for detailed instructions!**

**Quick version:**
1. Create 5 simple images (see guide for specs)
2. Save them to `Assets/_Project/UI/Sprites/`
3. In Unity, select each sprite
4. Set Texture Type to: **Sprite (2D and UI)**
5. Click **Apply**

**Minimum assets:**
- `OwareBoardBackground.png` (1920x1080) - Brown rectangle
- `PitHole.png` (256x256) - Dark circle
- `MarbleRed.png` (128x128) - Red circle
- `MarbleBeige.png` (128x128) - Beige circle
- `MarbleBrown.png` (128x128) - Brown circle

---

### **Step 3: Assign Sprites to Board**

1. **Select "Oware 2D Board" in Hierarchy**

2. **In Inspector → Oware2DBoardUI:**

```
Board Background:
→ Board Background Sprite: Drag your wooden board sprite here

Seed/Marble Sprites:
→ Marble Sprites: Set size to 3
  → Element 0: Drag MarbleRed.png
  → Element 1: Drag MarbleBeige.png
  → Element 2: Drag MarbleBrown.png
```

3. **Stop and Press Play again!**

Now you should see your beautiful custom graphics! 🎨

---

## ⚙️ Customization Options

Once it's working, customize to your liking:

### **Adjust Layout**

```
Select "Oware 2D Board"
Inspector → Oware2DBoardUI:

Layout Settings:
- Pit Size: How big each pit is (default 120x120)
- Pit Spacing: Gap between pits (default 140)
- Player 1 Pits Start Pos: Bottom row position
- Player 2 Pits Start Pos: Top row position
- Marble Size: How big the marbles are (default 40)
```

**Try these values for different layouts:**

**Compact:**
```
Pit Size: 100 x 100
Pit Spacing: 120
Marble Size: 30
```

**Spacious:**
```
Pit Size: 150 x 150
Pit Spacing: 170
Marble Size: 50
```

**CrazyGames Style:**
```
Pit Size: 120 x 120
Pit Spacing: 140
Marble Size: 40
```

---

### **Adjust Colors**

```
Inspector → Oware2DBoardUI → Colors:

Board Tint Color: Overall board tint
Marble Colors: Array of 3 colors
  - Element 0: Red/Terracotta (#E37359)
  - Element 1: Beige/Tan (#F3E0C4)
  - Element 2: Dark Brown (#432717)
```

Change these to match your brand!

---

### **Adjust Animations**

```
Inspector → Oware2DBoardUI → Animation Settings:

Seed Drop Duration: How fast seeds drop in (default 0.3s)
Seed Move Duration: How fast seeds rearrange (default 0.2s)
```

**Slower animations:**
```
Seed Drop Duration: 0.5
Seed Move Duration: 0.3
```

**Faster animations:**
```
Seed Drop Duration: 0.15
Seed Move Duration: 0.1
```

---

## 🎯 Advanced Setup

### **Create Custom Pit Prefab**

Want more control over pit appearance?

1. **Create Pit Prefab:**
   ```
   Hierarchy → Right-click → UI → Image
   Name: "Pit Prefab"
   ```

2. **Design Your Pit:**
   ```
   Add:
   - Background image (pit hole sprite)
   - Counter text (seed count)
   - Seeds container (for marbles)
   
   Customize colors, fonts, sizes
   ```

3. **Save as Prefab:**
   ```
   Drag "Pit Prefab" to Project window
   Save to: Assets/_Project/UI/Prefabs/
   ```

4. **Assign to Board:**
   ```
   Select "Oware 2D Board"
   Inspector → Pit Prefab: Drag your prefab here
   ```

---

### **Add Store/Mancala Displays**

Stores are auto-created, but you can customize:

```
Inspector → Layout Settings:
- Store Size: Size of capture areas (default 120x300)
```

Or create custom store prefabs similar to pit prefabs!

---

### **Improve Visual Polish**

**Add shadows:**
```
Select Board Background
Component → Add Component → Shadow
- Effect Distance: (2, -2)
- Color: Black, 50% alpha
```

**Add outlines to pits:**
```
Select any pit
Component → Add Component → Outline  
- Effect Distance: (2, 2)
- Color: Dark brown
```

**Add background image:**
```
Create another Image behind everything
Set sprite to decorative background
Lower opacity to 20-30%
```

---

## 🐛 Troubleshooting

### **"I don't see Oware2DBoardUI component!"**

**Fix:**
```
1. Wait for Unity to finish compiling (check bottom-right)
2. If still missing, check Console for errors
3. Make sure DOTween is installed
```

---

### **"Pits appear but no marbles!"**

**Fix:**
```
1. Check Console for errors
2. Make sure GameManager is in scene
3. Press Space to start a new game
4. Check that marble sprites/colors are assigned
```

---

### **"Layout is wrong/pits overlap!"**

**Fix:**
```
Inspector → Layout Settings:
Increase Pit Spacing to 160 or 180
Adjust Player 1/2 Pits Start Pos
```

---

### **"Everything is tiny or huge!"**

**Fix:**
```
Select Canvas
Inspector → Canvas Scaler:
- Reference Resolution: 1920 x 1080
- Match: 0.5 (balance width/height)
```

---

### **"Marbles are all the same color!"**

**Check:**
```
Inspector → Marble Colors array
Make sure you have 3 different colors
Or assign 3 different marble sprites
```

---

## 📱 Mobile Optimization

For mobile devices:

```
Select Canvas
Inspector → Canvas Scaler:
- UI Scale Mode: Scale With Screen Size
- Reference Resolution: 1920 x 1080
- Screen Match Mode: Match Width Or Height
- Match: 0.5
```

**Test different resolutions:**
- Game view → Free Aspect
- Try: 16:9, 4:3, iPhone, iPad, etc.

**Adjust layout if needed:**
```
Reduce Pit Size to 100
Reduce Pit Spacing to 120
Reduce Marble Size to 35
```

---

## ✅ Final Checklist

- [ ] Canvas created with correct settings
- [ ] Board Background added with brown color
- [ ] Oware2DBoardUI component added
- [ ] References assigned (background image)
- [ ] Old visualizer disabled
- [ ] Pressed Play - see pits and marbles
- [ ] (Optional) Created custom sprites
- [ ] (Optional) Assigned custom sprites
- [ ] (Optional) Customized colors/layout
- [ ] Game plays correctly
- [ ] Looks beautiful! 🎨

---

## 🎉 You're Done!

Your 2D Oware board should now look **exactly like the CrazyGames reference**!

### **What You Have:**
- ✅ Beautiful 2D board layout
- ✅ Clear circular pits
- ✅ Colorful marble sprites
- ✅ Number counters
- ✅ Smooth animations
- ✅ Fully playable!

### **Next Steps:**
1. **Play test** - Make sure everything works
2. **Polish graphics** - Replace placeholders with beautiful sprites
3. **Adjust colors** - Match your brand
4. **Add sound effects** - Enhance the experience
5. **Share it!** - Show off your beautiful game

---

## 💡 Pro Tips

**Tip 1:** Start with NO custom sprites
- Let it auto-generate
- Test that it works
- Then add beautiful graphics

**Tip 2:** Iterate on graphics
- Use simple placeholders first
- Polish one sprite at a time
- Easy to swap anytime!

**Tip 3:** Use the Scene view
- While playing, switch to Scene view
- See exact positions of pits
- Adjust layout in real-time

**Tip 4:** Save different layouts
- Create prefab variants
- One for mobile, one for desktop
- Switch between them easily

---

## 📞 Need Help?

**Check Console for messages:**
```
[2D Board UI] Initializing board...
[2D Board UI] Board initialized successfully!
[2D Board UI] Game started
```

If you see these, it's working! ✅

**Common issues:**
- DOTween not installed → Install it
- No marbles visible → Check marble sprites/colors
- Layout wrong → Adjust spacing values
- Can't click pits → Check EventSystem exists

---

**Congratulations! You now have a beautiful 2D Oware board!** 🎉🎨🚀

*Questions? Check ASSET_CREATION_GUIDE.md for graphics help!*

