using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using DG.Tweening;
using SocialOwareAcademy.Gameplay;
using System.Collections.Generic;

namespace SocialOwareAcademy.Gameplay
{
    /// <summary>
    /// Enhanced 3D Oware board visualizer with wooden aesthetic and marble seeds.
    /// Inspired by traditional Mancala games with modern polish.
    /// </summary>
    public class EnhancedOwareBoardVisualizer : MonoBehaviour
    {
        [Header("Board Settings")]
        [SerializeField] private float pitSpacing = 2.2f;
        [SerializeField] private float pitRadius = 0.9f;
        [SerializeField] private float pitDepth = 0.3f;
        [SerializeField] private float boardThickness = 0.5f;
        [SerializeField] private Color woodColor = new Color(0.82f, 0.70f, 0.55f); // Light brown wood
        [SerializeField] private Color pitInteriorColor = new Color(0.4f, 0.3f, 0.2f); // Dark wood

        [Header("Seed Settings")]
        [SerializeField] private GameObject seedPrefab; // Assign a sphere prefab
        [SerializeField] private float seedSize = 0.25f;
        [SerializeField] private Color[] seedColors = new Color[] 
        {
            new Color(0.89f, 0.45f, 0.35f), // Terracotta red
            new Color(0.95f, 0.88f, 0.77f), // Light beige/pink
            new Color(0.26f, 0.15f, 0.09f)  // Dark brown
        };

        [Header("Store Settings")]
        [SerializeField] private float storeWidth = 1.5f;
        [SerializeField] private float storeHeight = 4f;

        [Header("UI Elements")]
        [SerializeField] private Canvas overlayCanvas;
        [SerializeField] private GameObject pitCounterPrefab; // UI Text prefab for seed counts
        [SerializeField] private Color counterTextColor = Color.white;

        [Header("Animation Settings")]
        [SerializeField] private float seedDropDuration = 0.3f;
        [SerializeField] private float seedBounceDuration = 0.2f;

        [Header("Lighting")]
        [SerializeField] private bool createCustomLighting = true;
        [SerializeField] private Color ambientLight = new Color(0.4f, 0.35f, 0.3f);
        [SerializeField] private Color directionalLightColor = new Color(1f, 0.95f, 0.85f);

        // Visual elements
        private GameObject boardBase;
        private GameObject[] pitHoles;
        private GameObject[] storeHoles;
        private List<GameObject>[] seedObjects; // Seeds in each pit
        private TextMeshPro[] pitCountTexts;
        private TextMeshPro[] storeCountTexts;
        private Material woodMaterial;
        private Material pitInteriorMaterial;
        
        // Interaction
        private int hoveredPit = -1;
        private int selectedPit = -1;

        void Start()
        {
            Debug.Log("[EnhancedVisualizer] Initializing beautiful Oware board...");
            
            // Setup lighting
            if (createCustomLighting)
            {
                SetupLighting();
            }

            // Create board
            CreateWoodenBoard();
            CreatePits();
            CreateStores();
            CreateSeedPool();
            
            // Setup camera
            SetupCamera();

            // Subscribe to GameManager events
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStarted += OnGameStarted;
                GameManager.Instance.OnMoveMade += OnMoveMade;
                GameManager.Instance.OnGameEnded += OnGameEnded;
                Debug.Log("[EnhancedVisualizer] Subscribed to GameManager events");
            }
            else
            {
                Debug.LogError("[EnhancedVisualizer] GameManager.Instance is NULL!");
            }

            Debug.Log("[EnhancedVisualizer] Board creation complete!");
        }

        /// <summary>
        /// Setup custom lighting for warm, inviting board appearance
        /// </summary>
        private void SetupLighting()
        {
            // Set ambient lighting
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = ambientLight;

            // Find or create directional light
            Light directionalLight = FindObjectOfType<Light>();
            if (directionalLight == null)
            {
                GameObject lightObj = new GameObject("Directional Light");
                directionalLight = lightObj.AddComponent<Light>();
                directionalLight.type = LightType.Directional;
            }

            directionalLight.color = directionalLightColor;
            directionalLight.intensity = 1.2f;
            directionalLight.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
            directionalLight.shadows = LightShadows.Soft;
        }

        /// <summary>
        /// Create the main wooden board base
        /// </summary>
        private void CreateWoodenBoard()
        {
            boardBase = GameObject.CreatePrimitive(PrimitiveType.Cube);
            boardBase.name = "Oware Board Base";
            
            // Size: fits 6 pits + stores
            float boardWidth = pitSpacing * 6 + storeWidth * 2 + 1f;
            float boardLength = pitSpacing * 2 + 2f;
            
            boardBase.transform.localScale = new Vector3(boardWidth, boardThickness, boardLength);
            boardBase.transform.position = new Vector3(pitSpacing * 2.5f, -boardThickness / 2f, pitSpacing);

            // Create wood material with nice texture
            woodMaterial = new Material(Shader.Find("Standard"));
            woodMaterial.color = woodColor;
            woodMaterial.SetFloat("_Smoothness", 0.3f);
            woodMaterial.SetFloat("_Metallic", 0f);
            
            boardBase.GetComponent<Renderer>().material = woodMaterial;
            
            // Remove collider from base (we want to click on pits, not base)
            Destroy(boardBase.GetComponent<Collider>());

            // Add subtle rounding effect
            boardBase.transform.localScale += new Vector3(0, 0.1f, 0);
        }

        /// <summary>
        /// Create pit holes in the board
        /// </summary>
        private void CreatePits()
        {
            pitHoles = new GameObject[OwareBoard.TOTAL_PITS];
            seedObjects = new List<GameObject>[OwareBoard.TOTAL_PITS];
            pitCountTexts = new TextMeshPro[OwareBoard.TOTAL_PITS];

            // Create interior material for pits
            pitInteriorMaterial = new Material(Shader.Find("Standard"));
            pitInteriorMaterial.color = pitInteriorColor;
            pitInteriorMaterial.SetFloat("_Smoothness", 0.2f);

            // Create Player 1 pits (bottom row, left to right: 0-5)
            for (int i = 0; i < OwareBoard.PITS_PER_PLAYER; i++)
            {
                Vector3 position = new Vector3(i * pitSpacing, 0, -pitSpacing * 0.3f);
                pitHoles[i] = CreatePitHole(i, position, true);
                seedObjects[i] = new List<GameObject>();
                
                // Create counter UI
                pitCountTexts[i] = CreatePitCounter(position, -1.5f); // Below pit
            }

            // Create Player 2 pits (top row, right to left: 11-6)
            for (int i = 0; i < OwareBoard.PITS_PER_PLAYER; i++)
            {
                int pitIndex = OwareBoard.TOTAL_PITS - 1 - i;
                Vector3 position = new Vector3(i * pitSpacing, 0, pitSpacing * 2.3f);
                pitHoles[pitIndex] = CreatePitHole(pitIndex, position, false);
                seedObjects[pitIndex] = new List<GameObject>();
                
                // Create counter UI
                pitCountTexts[pitIndex] = CreatePitCounter(position, 1.5f); // Above pit
            }
        }

        /// <summary>
        /// Create a single pit hole
        /// </summary>
        private GameObject CreatePitHole(int pitIndex, Vector3 position, bool isPlayer1)
        {
            // Create pit container
            GameObject pit = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pit.name = $"Pit_{pitIndex}_{(isPlayer1 ? "P1" : "P2")}";
            pit.transform.position = position;
            pit.transform.localScale = new Vector3(pitRadius * 2, pitDepth, pitRadius * 2);

            // Apply dark interior material
            pit.GetComponent<Renderer>().material = new Material(pitInteriorMaterial);

            // Add collider for click detection
            SphereCollider collider = pit.AddComponent<SphereCollider>();
            collider.radius = 0.7f;
            collider.center = Vector3.up * 0.5f;

            // Add click handler
            PitClickHandler clickHandler = pit.AddComponent<PitClickHandler>();
            clickHandler.pitIndex = pitIndex;
            clickHandler.visualizer = this;

            // Add subtle glow effect (emission)
            Material pitMat = pit.GetComponent<Renderer>().material;
            pitMat.EnableKeyword("_EMISSION");
            pitMat.SetColor("_EmissionColor", new Color(0.2f, 0.15f, 0.05f) * 0.3f);

            return pit;
        }

        /// <summary>
        /// Create store areas (kalaha/mancala pits)
        /// </summary>
        private void CreateStores()
        {
            storeHoles = new GameObject[2];
            storeCountTexts = new TextMeshPro[2];

            // Player 1 store (right side)
            Vector3 store1Pos = new Vector3(-storeWidth, 0, pitSpacing * 0.5f);
            storeHoles[0] = CreateStore(0, store1Pos);
            storeCountTexts[0] = CreatePitCounter(store1Pos, 0f);

            // Player 2 store (left side)  
            Vector3 store2Pos = new Vector3(pitSpacing * 5 + storeWidth, 0, pitSpacing * 0.5f);
            storeHoles[1] = CreateStore(1, store2Pos);
            storeCountTexts[1] = CreatePitCounter(store2Pos, 0f);
        }

        /// <summary>
        /// Create a store hole
        /// </summary>
        private GameObject CreateStore(int storeIndex, Vector3 position)
        {
            GameObject store = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            store.name = $"Store_{storeIndex}";
            store.transform.position = position;
            store.transform.localScale = new Vector3(storeWidth, storeHeight / 2, storeWidth);
            store.transform.rotation = Quaternion.Euler(0, 0, 90); // Rotate to be horizontal

            // Apply dark interior material
            store.GetComponent<Renderer>().material = new Material(pitInteriorMaterial);
            
            // Add subtle glow
            Material storeMat = store.GetComponent<Renderer>().material;
            storeMat.EnableKeyword("_EMISSION");
            storeMat.SetColor("_EmissionColor", new Color(0.2f, 0.15f, 0.05f) * 0.3f);

            // Remove collider (stores are not clickable)
            Destroy(store.GetComponent<Collider>());

            return store;
        }

        /// <summary>
        /// Create UI text counter for pit/store
        /// </summary>
        private TextMeshPro CreatePitCounter(Vector3 worldPosition, float yOffset)
        {
            // Create world space text
            GameObject counterObj = new GameObject("PitCounter");
            counterObj.transform.position = worldPosition + Vector3.up * 1.2f + Vector3.forward * yOffset;
            counterObj.transform.rotation = Quaternion.Euler(90, 0, 0); // Face up

            TextMeshPro counter = counterObj.AddComponent<TextMeshPro>();
            counter.text = "0";
            counter.fontSize = 5;
            counter.alignment = TextAlignmentOptions.Center;
            counter.color = counterTextColor;
            counter.fontStyle = FontStyles.Bold;
            
            // Add circular background
            GameObject bgObj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            bgObj.name = "CounterBG";
            bgObj.transform.SetParent(counterObj.transform);
            bgObj.transform.localPosition = Vector3.back * 0.1f;
            bgObj.transform.localScale = new Vector3(0.8f, 0.05f, 0.8f);
            
            Material bgMat = new Material(Shader.Find("Standard"));
            bgMat.color = new Color(0.2f, 0.15f, 0.1f, 0.8f);
            bgObj.GetComponent<Renderer>().material = bgMat;
            Destroy(bgObj.GetComponent<Collider>());

            return counter;
        }

        /// <summary>
        /// Create pool of seed objects for reuse
        /// </summary>
        private void CreateSeedPool()
        {
            // If no prefab assigned, create a simple sphere
            if (seedPrefab == null)
            {
                seedPrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                seedPrefab.name = "SeedPrefab";
                seedPrefab.transform.localScale = Vector3.one * seedSize;
                
                // Create shiny seed material
                Material seedMat = new Material(Shader.Find("Standard"));
                seedMat.SetFloat("_Smoothness", 0.8f);
                seedMat.SetFloat("_Metallic", 0.1f);
                seedPrefab.GetComponent<Renderer>().material = seedMat;
                
                // Remove collider
                Destroy(seedPrefab.GetComponent<Collider>());
                
                seedPrefab.SetActive(false);
            }
        }

        /// <summary>
        /// Setup camera for optimal board viewing
        /// </summary>
        private void SetupCamera()
        {
            Camera mainCam = Camera.main;
            if (mainCam == null) return;

            // Position camera for top-down angled view
            Vector3 boardCenter = new Vector3(pitSpacing * 2.5f, 0, pitSpacing);
            mainCam.transform.position = boardCenter + new Vector3(0, 8f, -3f);
            mainCam.transform.LookAt(boardCenter);

            // Adjust for better perspective
            mainCam.fieldOfView = 40f;
            mainCam.backgroundColor = new Color(0.35f, 0.25f, 0.2f); // Dark reddish-brown like reference
        }

        /// <summary>
        /// Update visualization based on board state
        /// </summary>
        private void UpdateVisualization()
        {
            if (GameManager.Instance?.Board == null)
                return;

            OwareBoard board = GameManager.Instance.Board;

            // Update pit seeds
            for (int i = 0; i < OwareBoard.TOTAL_PITS; i++)
            {
                int seedCount = board.GetSeeds(i);
                UpdatePitSeeds(i, seedCount);
                
                // Update counter text
                if (pitCountTexts[i] != null)
                {
                    pitCountTexts[i].text = seedCount.ToString();
                }
            }

            // Update store displays
            if (storeCountTexts[0] != null)
                storeCountTexts[0].text = board.Player1Captured.ToString();
            
            if (storeCountTexts[1] != null)
                storeCountTexts[1].text = board.Player2Captured.ToString();
        }

        /// <summary>
        /// Update seeds in a specific pit
        /// </summary>
        private void UpdatePitSeeds(int pitIndex, int targetCount)
        {
            List<GameObject> currentSeeds = seedObjects[pitIndex];
            int currentCount = currentSeeds.Count;

            // Add seeds if needed
            while (currentCount < targetCount)
            {
                GameObject seed = CreateSeed(pitIndex);
                currentSeeds.Add(seed);
                
                // Animate seed dropping in
                AnimateSeedDrop(seed);
                currentCount++;
            }

            // Remove seeds if needed
            while (currentCount > targetCount)
            {
                GameObject seed = currentSeeds[currentCount - 1];
                currentSeeds.RemoveAt(currentCount - 1);
                
                // Animate seed removal
                AnimateSeedRemoval(seed);
                currentCount--;
            }

            // Rearrange seeds in pit
            ArrangeSeedsInPit(pitIndex);
        }

        /// <summary>
        /// Create a seed object with random color
        /// </summary>
        private GameObject CreateSeed(int pitIndex)
        {
            GameObject seed = Instantiate(seedPrefab);
            seed.name = $"Seed_Pit{pitIndex}";
            seed.SetActive(true);

            // Random color from palette
            Color seedColor = seedColors[Random.Range(0, seedColors.Length)];
            seed.GetComponent<Renderer>().material.color = seedColor;

            // Position at pit center (will be arranged)
            Vector3 pitPos = pitHoles[pitIndex].transform.position;
            seed.transform.position = pitPos;

            return seed;
        }

        /// <summary>
        /// Arrange seeds in a pit in a natural cluster
        /// </summary>
        private void ArrangeSeedsInPit(int pitIndex)
        {
            List<GameObject> seeds = seedObjects[pitIndex];
            if (seeds.Count == 0) return;

            Vector3 pitCenter = pitHoles[pitIndex].transform.position;
            float arrangeRadius = pitRadius * 0.6f;

            // Arrange in circular pattern with some randomness
            for (int i = 0; i < seeds.Count; i++)
            {
                float angle = (360f / seeds.Count) * i + Random.Range(-15f, 15f);
                float radius = arrangeRadius * Random.Range(0.3f, 0.9f);
                
                Vector3 offset = new Vector3(
                    Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                    Random.Range(0f, 0.2f),
                    Mathf.Sin(angle * Mathf.Deg2Rad) * radius
                );

                Vector3 targetPos = pitCenter + offset;
                
                // Animate to position
                seeds[i].transform.DOMove(targetPos, 0.3f)
                    .SetEase(Ease.OutQuad);
            }
        }

        /// <summary>
        /// Animate seed dropping into pit
        /// </summary>
        private void AnimateSeedDrop(GameObject seed)
        {
            Vector3 startPos = seed.transform.position + Vector3.up * 2f;
            seed.transform.position = startPos;

            seed.transform.DOMove(seed.transform.position - Vector3.up * 2f, seedDropDuration)
                .SetEase(Ease.OutBounce);
            
            seed.transform.DOPunchScale(Vector3.one * 0.2f, seedBounceDuration)
                .SetDelay(seedDropDuration);
        }

        /// <summary>
        /// Animate seed removal
        /// </summary>
        private void AnimateSeedRemoval(GameObject seed)
        {
            seed.transform.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(() => Destroy(seed));
        }

        /// <summary>
        /// Handle pit click
        /// </summary>
        public void OnPitClicked(int pitIndex)
        {
            if (GameManager.Instance == null || !GameManager.Instance.IsGameActive)
                return;

            if (!GameManager.Instance.IsHumanTurn)
            {
                Debug.Log("[EnhancedVisualizer] Not your turn!");
                return;
            }

            // Highlight selected pit
            selectedPit = pitIndex;
            HighlightPit(pitIndex);

            // Attempt move
            bool success = GameManager.Instance.MakeMove(pitIndex);

            if (success)
            {
                Debug.Log($"[EnhancedVisualizer] Move successful: Pit {pitIndex}");
            }
            else
            {
                Debug.LogWarning($"[EnhancedVisualizer] Invalid move: Pit {pitIndex}");
            }

            // Clear highlight
            Invoke(nameof(ClearHighlight), 0.5f);
        }

        /// <summary>
        /// Highlight a pit
        /// </summary>
        private void HighlightPit(int pitIndex)
        {
            if (pitIndex >= 0 && pitIndex < OwareBoard.TOTAL_PITS)
            {
                Material pitMat = pitHoles[pitIndex].GetComponent<Renderer>().material;
                pitMat.SetColor("_EmissionColor", new Color(0.8f, 0.6f, 0.2f) * 0.5f);
            }
        }

        /// <summary>
        /// Clear pit highlights
        /// </summary>
        private void ClearHighlight()
        {
            for (int i = 0; i < OwareBoard.TOTAL_PITS; i++)
            {
                Material pitMat = pitHoles[i].GetComponent<Renderer>().material;
                pitMat.SetColor("_EmissionColor", new Color(0.2f, 0.15f, 0.05f) * 0.3f);
            }
        }

        // Event handlers
        private void OnGameStarted(OwareBoard board)
        {
            Debug.Log("[EnhancedVisualizer] Game started");
            UpdateVisualization();
        }

        private void OnMoveMade(int pitIndex, int player)
        {
            Debug.Log($"[EnhancedVisualizer] Move made: Pit {pitIndex} by Player {player + 1}");
            UpdateVisualization();
        }

        private void OnGameEnded(int winner)
        {
            Debug.Log($"[EnhancedVisualizer] Game ended. Winner: {winner}");
            UpdateVisualization();
        }

        void Update()
        {
            // Handle mouse clicks
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                HandleMouseClick();
            }

            // Press SPACE to restart
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.StartNewGame();
                }
            }
        }

        /// <summary>
        /// Handle mouse click using raycast
        /// </summary>
        private void HandleMouseClick()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                PitClickHandler clickHandler = hit.collider.GetComponent<PitClickHandler>();
                if (clickHandler != null)
                {
                    OnPitClicked(clickHandler.pitIndex);
                }
            }
        }

        void OnDestroy()
        {
            // Unsubscribe from events
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStarted -= OnGameStarted;
                GameManager.Instance.OnMoveMade -= OnMoveMade;
                GameManager.Instance.OnGameEnded -= OnGameEnded;
            }
        }

        /// <summary>
        /// Helper component for pit click detection
        /// </summary>
        public class PitClickHandler : MonoBehaviour
        {
            public int pitIndex;
            public EnhancedOwareBoardVisualizer visualizer;

            void OnMouseDown()
            {
                if (visualizer != null)
                {
                    visualizer.OnPitClicked(pitIndex);
                }
            }
        }
    }
}

