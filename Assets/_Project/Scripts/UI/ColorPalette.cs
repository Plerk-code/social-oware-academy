using UnityEngine;

namespace SocialOwareAcademy.UI
{
    /// <summary>
    /// Centralized color palette for consistent theming across the entire UI.
    /// Based on West African cultural design with warm earth tones.
    /// </summary>
    [CreateAssetMenu(fileName = "ColorPalette", menuName = "Oware/UI/Color Palette")]
    public class ColorPalette : ScriptableObject
    {
        [Header("Primary Colors - Warm Earth Tones")]
        [Tooltip("Terracotta - Primary action color, warm and inviting")]
        public Color terracotta = new Color(0.89f, 0.45f, 0.36f); // #E37359

        [Tooltip("Ochre - Secondary accent, earth pigment")]
        public Color ochre = new Color(0.85f, 0.65f, 0.13f); // #D9A621

        [Tooltip("Deep Brown - Text and borders, traditional Kente cloth")]
        public Color deepBrown = new Color(0.26f, 0.15f, 0.09f); // #432717

        [Header("Secondary Colors - Vibrant Accents")]
        [Tooltip("Gold - Achievements and premium features")]
        public Color gold = new Color(1.00f, 0.84f, 0.00f); // #FFD700

        [Tooltip("Emerald - Success states and positive feedback")]
        public Color emerald = new Color(0.31f, 0.78f, 0.47f); // #4FC878

        [Tooltip("Azure - Information and highlights")]
        public Color azure = new Color(0.25f, 0.52f, 0.96f); // #4085F4

        [Header("Neutrals - Modern Foundation")]
        [Tooltip("Off-White - Light backgrounds")]
        public Color offWhite = new Color(0.98f, 0.96f, 0.94f); // #FAF5F0

        [Tooltip("Charcoal - Primary text color")]
        public Color charcoal = new Color(0.13f, 0.13f, 0.13f); // #212121

        [Tooltip("Light Gray - Borders and dividers")]
        public Color lightGray = new Color(0.85f, 0.85f, 0.85f); // #D9D9D9

        [Tooltip("Dark Gray - Secondary text")]
        public Color darkGray = new Color(0.40f, 0.40f, 0.40f); // #666666

        [Header("Semantic Colors")]
        [Tooltip("Success - Positive feedback, wins")]
        public Color success = new Color(0.30f, 0.69f, 0.31f); // #4CAF50

        [Tooltip("Danger - Errors, destructive actions")]
        public Color danger = new Color(0.96f, 0.26f, 0.21f); // #F44336

        [Tooltip("Info - Informational messages")]
        public Color info = new Color(0.13f, 0.59f, 0.95f); // #2196F3

        [Tooltip("Warning - Cautions and important notices")]
        public Color warning = new Color(1.00f, 0.60f, 0.00f); // #FF9800

        [Header("Tier Colors (ELO Ranks)")]
        public Color bronze = new Color(0.80f, 0.50f, 0.20f); // #CD7F32
        public Color silver = new Color(0.75f, 0.75f, 0.75f); // #C0C0C0
        public Color goldTier = new Color(1.00f, 0.84f, 0.00f); // #FFD700
        public Color platinum = new Color(0.90f, 0.89f, 0.89f); // #E5E4E2
        public Color diamond = new Color(0.72f, 0.85f, 0.92f); // #B7D7EB
        public Color master = new Color(0.86f, 0.44f, 0.58f); // #DC7093

        [Header("Player Colors (Match UI)")]
        [Tooltip("Player 1 / Human player")]
        public Color player1 = new Color(0.31f, 0.78f, 0.47f); // Emerald

        [Tooltip("Player 2 / AI opponent")]
        public Color player2 = new Color(0.96f, 0.26f, 0.21f); // Danger red

        [Tooltip("Selected/highlighted pit")]
        public Color selected = new Color(1.00f, 0.84f, 0.00f); // Gold

        // Singleton instance for easy global access
        private static ColorPalette _instance;
        public static ColorPalette Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<ColorPalette>("ColorPalette");
                    if (_instance == null)
                    {
                        Debug.LogWarning("[ColorPalette] No ColorPalette found in Resources folder. Using default colors.");
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Get color with alpha adjustment
        /// </summary>
        public Color WithAlpha(Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        /// <summary>
        /// Get tier color by ELO rating
        /// </summary>
        public Color GetTierColor(int eloRating)
        {
            if (eloRating < 1200) return bronze;
            if (eloRating < 1400) return silver;
            if (eloRating < 1600) return goldTier;
            if (eloRating < 1800) return platinum;
            if (eloRating < 2000) return diamond;
            return master;
        }
    }
}
