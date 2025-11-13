# 🚀 Quick Manual Setup - Enhanced Oware Board

**Time:** 2 minutes | **Difficulty:** Super Easy!

---

## ✅ Step-by-Step Instructions

### Step 1: Wait for Unity to Compile (30 seconds)

The new scripts need to compile first:

1. **Look at the bottom-right of Unity**
2. You should see a **spinning circle** that says "Compiling..."
3. **Wait for it to finish** (should take 10-30 seconds)
4. When done, the circle disappears ✅

---

### Step 2: Create Enhanced Board GameObject

In Unity:

1. **In the Hierarchy window** (left side), right-click empty space
2. Select: **Create Empty**
3. **Name it**: `Enhanced Oware Board`

You should now see "Enhanced Oware Board" in your Hierarchy.

---

### Step 3: Add the Visualizer Component

With "Enhanced Oware Board" selected:

1. **Look at the Inspector** (right side)
2. **Click "Add Component"** button at the bottom
3. **Type**: `EnhancedOwareBoardVisualizer`
4. **Click it** when it appears in the list

**If you DON'T see it:**
- Unity might still be compiling - wait a bit longer
- Or try: Window > General > Console (check for red errors)

**If you DO see it:**
- Great! Click it to add ✅

---

### Step 4: Disable Your Old Visualizer (Optional)

Find your old board visualizer:

1. **In Hierarchy**, find the GameObject with your old `OwareBoardVisualizer`
2. **In Inspector**, find the `OwareBoardVisualizer` component
3. **Uncheck the checkbox** next to it (or delete the component)

This prevents both visualizers from running at once.

---

### Step 5: Press Play! 🎮

That's it! Press the **Play button** at the top of Unity.

You should see:
- ✅ Wooden board appear (light brown)
- ✅ 12 pits (6 top, 6 bottom)
- ✅ Colorful marbles (red, beige, brown)
- ✅ Numbers above/below pits

**If you see errors in Console:**
- Red error about "DOTween"? → Install DOTween from Package Manager
- Other errors? → Send me the error message

---

## 🎨 What It Should Look Like

After pressing Play, you should see something like this:

```
        [6]   [6]   [6]   [6]   [6]   [6]    ← Top row (AI)
         ●●    ●●    ●●    ●●    ●●    ●●
    
    [Store]     WOODEN BOARD      [Store]
                (Light Brown)

         ●●    ●●    ●●    ●●    ●●    ●●
        [6]   [6]   [6]   [6]   [6]   [6]    ← Bottom row (You)
```

Where:
- ●● = Colorful marble seeds
- [6] = Seed counter
- Stores = Elongated capsules

---

## 🐛 Troubleshooting

### "I don't see EnhancedOwareBoardVisualizer in Add Component"

**Cause:** Unity hasn't finished compiling

**Fix:**
1. Look bottom-right of Unity for "Compiling..." spinner
2. Wait for it to finish
3. Try again
4. If still missing, go to: Window > General > Console
5. Check for red errors - tell me what they say

---

### "I see red errors about DOTween"

**Cause:** DOTween not installed (required for animations)

**Fix:**
1. Go to: **Window > Package Manager**
2. Click the **+** button (top-left)
3. Select **"Add package from git URL"**
4. Paste: `https://github.com/Demigiant/dotween.git`
5. Click **Add**

OR use Asset Store version:
1. Window > Asset Store
2. Search "DOTween"
3. Download and Import (free!)

---

### "Board appears but looks dark/wrong"

**Fix:** The visualizer will auto-configure on first play:
1. Press **Stop** (stop playing)
2. Press **Play** again
3. It should look better

OR manually:
1. Select "Enhanced Oware Board" in Hierarchy
2. In Inspector, find "Create Custom Lighting"
3. Make sure it's **checked** ✅

---

### "I can't click the pits"

**Fix:**
1. Press **Space** to start a new game
2. Make sure you click **bottom row** pits (your side)
3. Check Console for messages - it will say "Not your turn!" if it's AI's turn

---

## ✨ Customization (Optional)

Once it's working, you can customize:

### Change Wood Color

1. Select "Enhanced Oware Board" in Hierarchy
2. In Inspector, find "**Wood Color**"
3. Click the color box
4. Pick your favorite brown!

### Change Marble Colors

1. In Inspector, find "**Seed Colors**"
2. Click the arrow to expand
3. Size: 3 (or more for variety!)
4. Change Element 0, 1, 2 to any colors you like

### Make Pits Bigger/Smaller

1. In Inspector, find "**Pit Radius**"
2. Try values from 0.7 to 1.2
3. See what looks best!

---

## 🎉 Success Checklist

After setup, verify:

- [ ] Wooden board visible (light brown)
- [ ] 12 pits (6 per row)
- [ ] 2 stores (capsules at ends)
- [ ] Colorful marbles in pits
- [ ] Numbers display correctly
- [ ] Can click bottom row pits
- [ ] Marbles animate smoothly
- [ ] No red errors in Console

**All checked?** Congratulations! 🎊

---

## 📞 Still Having Issues?

### Check These Files Exist:

1. Go to Project window
2. Navigate to: `Assets/_Project/Scripts/Gameplay/`
3. You should see: **EnhancedOwareBoardVisualizer.cs**

If it's there, Unity should compile it automatically.

### Force Recompile:

If Unity seems stuck:
1. Go to: **Assets > Refresh** (or Ctrl/Cmd + R)
2. Wait for recompile
3. Try Step 3 again

### Check Console:

1. Go to: **Window > General > Console**
2. Look for red errors
3. If you see errors, read them - they'll tell you what's missing

---

## 🚀 Next Steps

Once working:

1. **Play a game** - Test everything works
2. **Customize colors** - Make it your own
3. **Take screenshots** - Show off your beautiful board!
4. **Read docs** - Check ENHANCED_BOARD_QUICK_START.md for more

---

## 💡 Pro Tip

If you want to switch back to your old visualizer anytime:

**Old → Enhanced:**
- Disable OwareBoardVisualizer
- Enable EnhancedOwareBoardVisualizer

**Enhanced → Old:**
- Disable EnhancedOwareBoardVisualizer  
- Enable OwareBoardVisualizer

Both work perfectly! Choose whichever you prefer.

---

## 🎮 Ready to Play!

Once you see the beautiful wooden board:

- **Press Space** to start a new game
- **Click bottom row pits** to make moves
- **Enjoy the smooth animations!** ✨

---

**Questions?** Check the Unity Console for helpful messages starting with `[EnhancedVisualizer]`

**Happy gaming!** 🎲🎨🚀

