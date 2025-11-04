using UnityEngine;

namespace SocialOwareAcademy.UI
{
    /// <summary>
    /// Centralized layout constants for consistent spacing throughout the UI.
    /// Based on 8pt grid system (industry standard for mobile).
    /// </summary>
    public static class LayoutConstants
    {
        // Base unit - 8pt grid system
        public const float UNIT = 8f;

        // Spacing values
        public const float SPACE_TIGHT = UNIT * 0.5f;  // 4pt
        public const float SPACE_SMALL = UNIT;         // 8pt
        public const float SPACE_MEDIUM = UNIT * 2f;   // 16pt
        public const float SPACE_LARGE = UNIT * 3f;    // 24pt
        public const float SPACE_XLARGE = UNIT * 4f;   // 32pt
        public const float SPACE_XXLARGE = UNIT * 6f;  // 48pt

        // Minimum tap targets (iOS Human Interface Guidelines)
        public const float MIN_TAP_TARGET = 44f;

        // Corner radius (rounded corners)
        public const float RADIUS_SMALL = 8f;
        public const float RADIUS_MEDIUM = 16f;
        public const float RADIUS_LARGE = 24f;
        public const float RADIUS_XLARGE = 32f;

        // Reference resolution (from UIManager)
        public static readonly Vector2 REFERENCE_RES = new Vector2(1080, 1920);

        // Common button sizes
        public const float BUTTON_HEIGHT_SMALL = 40f;
        public const float BUTTON_HEIGHT_MEDIUM = 56f;
        public const float BUTTON_HEIGHT_LARGE = 72f;

        // Icon sizes
        public const float ICON_SIZE_SMALL = 24f;
        public const float ICON_SIZE_MEDIUM = 32f;
        public const float ICON_SIZE_LARGE = 48f;

        // Panel sizes for match UI
        public const float PLAYER_PANEL_WIDTH = 180f;
        public const float PLAYER_PANEL_HEIGHT = 200f;
        public const float SCORE_DISPLAY_HEIGHT = 80f;

        // Animation durations (seconds)
        public const float ANIM_FAST = 0.15f;
        public const float ANIM_MEDIUM = 0.3f;
        public const float ANIM_SLOW = 0.5f;

        // Seed animation
        public const float SEED_HOP_HEIGHT = 50f;
        public const float SEED_HOP_DURATION = 0.15f;
        public const float SEED_CAPTURE_DURATION = 0.5f;

        /// <summary>
        /// Calculate screen-relative size based on reference resolution
        /// </summary>
        public static float ScreenRelativeSize(float size)
        {
            float heightRatio = Screen.height / REFERENCE_RES.y;
            return size * heightRatio;
        }

        /// <summary>
        /// Get safe area insets (for notches)
        /// </summary>
        public static Rect GetSafeArea()
        {
            return Screen.safeArea;
        }
    }
}
