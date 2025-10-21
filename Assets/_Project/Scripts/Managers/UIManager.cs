using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ScreenType = SocialOwareAcademy.UI.Screen;

namespace SocialOwareAcademy.Managers
{
    /// <summary>
    /// Centralized UI management for screen navigation and transitions.
    /// Singleton pattern, persists across scenes.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Transition Settings")]
        [SerializeField] private float defaultTransitionDuration = 0.3f;
        [SerializeField] private Ease transitionEase = Ease.InOutQuad;
        [SerializeField] private int maxScreenStackDepth = 10;

        [Header("Canvas Setup")]
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private CanvasGroup canvasGroup;

        // Screen management
        private Dictionary<ScreenType, GameObject> screens = new Dictionary<ScreenType, GameObject>();
        private Stack<ScreenType> screenStack = new Stack<ScreenType>();
        private ScreenType currentScreen = ScreenType.None;
        private bool isTransitioning = false;

        // Events
        public System.Action<ScreenType> OnScreenEnter;
        public System.Action<ScreenType> OnScreenExit;

        void Awake()
        {
            // Singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize canvas
            InitializeCanvas();

            Debug.Log("[UIManager] Initialized");
        }

        private void InitializeCanvas()
        {
            // Create main canvas if not assigned
            if (mainCanvas == null)
            {
                GameObject canvasObj = new GameObject("MainCanvas");
                canvasObj.transform.SetParent(transform);
                mainCanvas = canvasObj.AddComponent<Canvas>();
                mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                mainCanvas.sortingOrder = 0;

                // Add CanvasScaler for responsive design
                var scaler = canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
                scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1080, 1920); // Portrait orientation
                scaler.matchWidthOrHeight = 0.5f;

                // Add GraphicRaycaster for UI interaction
                canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            }

            // Create CanvasGroup for fade transitions
            if (canvasGroup == null)
            {
                canvasGroup = mainCanvas.gameObject.AddComponent<CanvasGroup>();
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }

        /// <summary>
        /// Register a screen for management by UIManager.
        /// Called by screen scripts on Awake.
        /// </summary>
        public void RegisterScreen(ScreenType screenType, GameObject screenObject)
        {
            if (screens.ContainsKey(screenType))
            {
                Debug.LogWarning($"[UIManager] Screen {screenType} already registered. Overwriting.");
            }

            screens[screenType] = screenObject;
            screenObject.SetActive(false); // Hide by default
            screenObject.transform.SetParent(mainCanvas.transform, false);

            Debug.Log($"[UIManager] Registered screen: {screenType}");
        }

        /// <summary>
        /// Unregister a screen (called on screen Destroy).
        /// </summary>
        public void UnregisterScreen(ScreenType screenType)
        {
            if (screens.ContainsKey(screenType))
            {
                screens.Remove(screenType);
                Debug.Log($"[UIManager] Unregistered screen: {screenType}");
            }
        }

        /// <summary>
        /// Show a specific screen with optional animation.
        /// Adds to screen stack for back button navigation.
        /// </summary>
        public void ShowScreen(ScreenType screenType, bool animate = true, float duration = -1f)
        {
            if (isTransitioning)
            {
                Debug.LogWarning("[UIManager] Screen transition already in progress. Ignoring.");
                return;
            }

            if (!screens.ContainsKey(screenType))
            {
                Debug.LogError($"[UIManager] Screen {screenType} not registered. Cannot show.");
                return;
            }

            if (duration < 0)
                duration = defaultTransitionDuration;

            if (animate)
            {
                StartCoroutine(AnimatedTransition(screenType, duration));
            }
            else
            {
                ExecuteScreenSwitch(screenType);
            }
        }

        /// <summary>
        /// Navigate back to previous screen in stack.
        /// </summary>
        public void GoBack()
        {
            if (screenStack.Count <= 1)
            {
                Debug.LogWarning("[UIManager] No previous screen in stack. Cannot go back.");
                return;
            }

            // Remove current screen from stack
            screenStack.Pop();

            // Get previous screen
            ScreenType previousScreen = screenStack.Peek();
            ShowScreen(previousScreen, animate: true);
        }

        /// <summary>
        /// Clear screen stack and show specified screen.
        /// Used for major navigation changes (e.g., logout, main menu).
        /// </summary>
        public void ShowScreenAndClearStack(ScreenType screenType, bool animate = true)
        {
            screenStack.Clear();
            ShowScreen(screenType, animate);
        }

        private IEnumerator AnimatedTransition(ScreenType targetScreen, float duration)
        {
            isTransitioning = true;
            canvasGroup.interactable = false;

            // Fade out
            float elapsed = 0f;
            float startAlpha = canvasGroup.alpha;
            while (elapsed < duration * 0.5f)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / (duration * 0.5f));
                yield return null;
            }
            canvasGroup.alpha = 0f;

            // Switch screen
            ExecuteScreenSwitch(targetScreen);

            // Fade in
            elapsed = 0f;
            while (elapsed < duration * 0.5f)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / (duration * 0.5f));
                yield return null;
            }
            canvasGroup.alpha = 1f;

            canvasGroup.interactable = true;
            isTransitioning = false;
        }

        private void ExecuteScreenSwitch(ScreenType targetScreen)
        {
            // Exit current screen
            if (currentScreen != ScreenType.None && screens.ContainsKey(currentScreen))
            {
                screens[currentScreen].SetActive(false);
                OnScreenExit?.Invoke(currentScreen);
                Debug.Log($"[UIManager] Exited screen: {currentScreen}");
            }

            // Enter new screen
            currentScreen = targetScreen;
            screens[targetScreen].SetActive(true);
            OnScreenEnter?.Invoke(targetScreen);
            Debug.Log($"[UIManager] Entered screen: {targetScreen}");

            // Add to screen stack (avoid duplicates of same screen)
            if (screenStack.Count == 0 || screenStack.Peek() != targetScreen)
            {
                screenStack.Push(targetScreen);

                // Limit stack depth
                if (screenStack.Count > maxScreenStackDepth)
                {
                    Debug.LogWarning($"[UIManager] Screen stack exceeded max depth ({maxScreenStackDepth}). Clearing oldest entries.");
                    var stackList = new List<ScreenType>(screenStack);
                    stackList.RemoveAt(stackList.Count - 1); // Remove oldest
                    screenStack = new Stack<ScreenType>(stackList);
                }
            }
        }

        // Public getters for UI
        public ScreenType CurrentScreen => currentScreen;
        public bool IsTransitioning => isTransitioning;
        public int ScreenStackDepth => screenStack.Count;

        /// <summary>
        /// Check if a screen is registered.
        /// </summary>
        public bool IsScreenRegistered(ScreenType screenType)
        {
            return screens.ContainsKey(screenType);
        }
    }
}
