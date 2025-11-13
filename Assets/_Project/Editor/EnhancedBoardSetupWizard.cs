using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace SocialOwareAcademy.Editor
{
    /// <summary>
    /// Setup wizard to quickly configure the Enhanced Oware Board
    /// </summary>
    public class EnhancedBoardSetupWizard : EditorWindow
    {
        private GameObject visualizerObject;
        private bool createSeedPrefab = true;
        private bool setupLighting = true;
        private bool setupCamera = true;

        [MenuItem("Tools/Oware/Enhanced Board Setup Wizard")]
        public static void ShowWindow()
        {
            var window = GetWindow<EnhancedBoardSetupWizard>("Board Setup");
            window.minSize = new Vector2(400, 500);
            window.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("Enhanced Oware Board Setup", EditorStyles.boldLabel);
            GUILayout.Space(10);

            EditorGUILayout.HelpBox(
                "This wizard will help you set up the beautiful wooden board with marble seeds. " +
                "Make sure you have DOTween installed from the Package Manager!",
                MessageType.Info);

            GUILayout.Space(10);

            // Step 1: Create Visualizer
            DrawSection("Step 1: Board Visualizer", () =>
            {
                visualizerObject = EditorGUILayout.ObjectField(
                    "Visualizer GameObject",
                    visualizerObject,
                    typeof(GameObject),
                    true) as GameObject;

                if (GUILayout.Button("Create Board GameObject", GUILayout.Height(30)))
                {
                    CreateBoardVisualizer();
                }
            });

            GUILayout.Space(10);

            // Step 2: Assets
            DrawSection("Step 2: Create Assets", () =>
            {
                createSeedPrefab = EditorGUILayout.Toggle("Create Seed Prefab", createSeedPrefab);

                if (GUILayout.Button("Generate Seed Prefab", GUILayout.Height(30)))
                {
                    CreateSeedPrefab();
                }
            });

            GUILayout.Space(10);

            // Step 3: Scene Setup
            DrawSection("Step 3: Scene Setup", () =>
            {
                setupLighting = EditorGUILayout.Toggle("Setup Lighting", setupLighting);
                setupCamera = EditorGUILayout.Toggle("Setup Camera", setupCamera);

                if (GUILayout.Button("Configure Scene", GUILayout.Height(30)))
                {
                    ConfigureScene();
                }
            });

            GUILayout.Space(20);

            // All-in-one button
            GUI.backgroundColor = new Color(0.3f, 0.8f, 0.3f);
            if (GUILayout.Button("🚀 SETUP EVERYTHING!", GUILayout.Height(50)))
            {
                SetupEverything();
            }
            GUI.backgroundColor = Color.white;

            GUILayout.Space(10);

            EditorGUILayout.HelpBox(
                "After setup:\n" +
                "1. Press Play to see the beautiful board!\n" +
                "2. Adjust colors in the Inspector if desired\n" +
                "3. Read ENHANCED_BOARD_SETUP.md for customization tips",
                MessageType.Info);
        }

        private void DrawSection(string title, System.Action content)
        {
            GUILayout.Label(title, EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            content();
            EditorGUI.indentLevel--;
        }

        private void CreateBoardVisualizer()
        {
            // Find existing visualizer
            var existing = Object.FindObjectOfType<SocialOwareAcademy.Gameplay.EnhancedOwareBoardVisualizer>();
            if (existing != null)
            {
                if (!EditorUtility.DisplayDialog(
                    "Visualizer Exists",
                    "An Enhanced Board Visualizer already exists. Create another?",
                    "Yes", "No"))
                {
                    visualizerObject = existing.gameObject;
                    return;
                }
            }

            // Create new GameObject
            GameObject boardObj = new GameObject("Enhanced Oware Board");
            boardObj.AddComponent<SocialOwareAcademy.Gameplay.EnhancedOwareBoardVisualizer>();

            visualizerObject = boardObj;
            Selection.activeGameObject = boardObj;

            Debug.Log("[Setup Wizard] Created Enhanced Board Visualizer!");
            EditorUtility.DisplayDialog("Success", "Enhanced Board Visualizer created!", "OK");
        }

        private void CreateSeedPrefab()
        {
            // Check if prefab already exists
            string prefabPath = "Assets/_Project/Prefab/MarbleSeed.prefab";
            GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (existing != null)
            {
                if (!EditorUtility.DisplayDialog(
                    "Prefab Exists",
                    "Marble seed prefab already exists. Recreate it?",
                    "Yes", "No"))
                {
                    return;
                }
            }

            // Create seed sphere
            GameObject seed = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            seed.name = "MarbleSeed";
            seed.transform.localScale = Vector3.one * 0.25f;

            // Remove collider
            DestroyImmediate(seed.GetComponent<Collider>());

            // Create material
            Material seedMat = new Material(Shader.Find("Standard"));
            seedMat.name = "MarbleSeedMaterial";
            seedMat.SetFloat("_Smoothness", 0.8f);
            seedMat.SetFloat("_Metallic", 0.1f);
            seedMat.color = new Color(0.89f, 0.45f, 0.35f); // Terracotta

            // Save material
            string matPath = "Assets/_Project/Resources/MarbleSeedMaterial.mat";
            AssetDatabase.CreateAsset(seedMat, matPath);

            // Apply material
            seed.GetComponent<Renderer>().material = seedMat;

            // Ensure directory exists
            string directory = System.IO.Path.GetDirectoryName(prefabPath);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            // Save as prefab
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(seed, prefabPath);
            DestroyImmediate(seed);

            // Assign to visualizer if present
            if (visualizerObject != null)
            {
                var visualizer = visualizerObject.GetComponent<SocialOwareAcademy.Gameplay.EnhancedOwareBoardVisualizer>();
                if (visualizer != null)
                {
                    SerializedObject so = new SerializedObject(visualizer);
                    SerializedProperty seedPrefabProp = so.FindProperty("seedPrefab");
                    seedPrefabProp.objectReferenceValue = prefab;
                    so.ApplyModifiedProperties();
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("[Setup Wizard] Created marble seed prefab at: " + prefabPath);
            EditorUtility.DisplayDialog("Success", "Marble seed prefab created!", "OK");
        }

        private void ConfigureScene()
        {
            if (setupLighting)
            {
                // Setup lighting
                RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
                RenderSettings.ambientLight = new Color(0.4f, 0.35f, 0.3f);

                // Find or create directional light
                Light dirLight = Object.FindObjectOfType<Light>();
                if (dirLight == null)
                {
                    GameObject lightObj = new GameObject("Directional Light");
                    dirLight = lightObj.AddComponent<Light>();
                    dirLight.type = LightType.Directional;
                }

                dirLight.color = new Color(1f, 0.95f, 0.85f);
                dirLight.intensity = 1.2f;
                dirLight.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
                dirLight.shadows = LightShadows.Soft;

                Debug.Log("[Setup Wizard] Configured lighting");
            }

            if (setupCamera)
            {
                Camera mainCam = Camera.main;
                if (mainCam != null)
                {
                    Vector3 boardCenter = new Vector3(5.5f, 0, 2.2f);
                    mainCam.transform.position = boardCenter + new Vector3(0, 8f, -3f);
                    mainCam.transform.LookAt(boardCenter);
                    mainCam.fieldOfView = 40f;
                    mainCam.backgroundColor = new Color(0.35f, 0.25f, 0.2f);

                    Debug.Log("[Setup Wizard] Configured camera");
                }
            }

            EditorUtility.DisplayDialog("Success", "Scene configured!", "OK");
        }

        private void SetupEverything()
        {
            CreateBoardVisualizer();
            CreateSeedPrefab();
            ConfigureScene();

            EditorUtility.DisplayDialog(
                "Setup Complete! 🎉",
                "Your Enhanced Oware Board is ready!\n\n" +
                "Press Play to see it in action.\n\n" +
                "Check the Inspector to customize colors and settings.",
                "Awesome!");
        }
    }

    /// <summary>
    /// Quick menu items for common operations
    /// </summary>
    public class EnhancedBoardQuickActions
    {
        [MenuItem("Tools/Oware/Quick Actions/Disable Old Visualizer")]
        public static void DisableOldVisualizer()
        {
            var oldVisualizer = Object.FindObjectOfType<OwareBoardVisualizer>();
            if (oldVisualizer != null)
            {
                oldVisualizer.enabled = false;
                Debug.Log("[Quick Actions] Disabled old OwareBoardVisualizer");
                EditorUtility.DisplayDialog("Success", "Old visualizer disabled!", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Not Found", "No old visualizer found in scene", "OK");
            }
        }

        [MenuItem("Tools/Oware/Quick Actions/Enable Old Visualizer")]
        public static void EnableOldVisualizer()
        {
            var oldVisualizer = Object.FindObjectOfType<OwareBoardVisualizer>();
            if (oldVisualizer != null)
            {
                oldVisualizer.enabled = true;
                Debug.Log("[Quick Actions] Enabled old OwareBoardVisualizer");
                EditorUtility.DisplayDialog("Success", "Old visualizer enabled!", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Not Found", "No old visualizer found in scene", "OK");
            }
        }

        [MenuItem("Tools/Oware/Documentation/Open Setup Guide")]
        public static void OpenSetupGuide()
        {
            string guidePath = "Assets/_Project/Scripts/UI/ENHANCED_BOARD_SETUP.md";
            var guide = AssetDatabase.LoadAssetAtPath<TextAsset>(guidePath);
            if (guide != null)
            {
                AssetDatabase.OpenAsset(guide);
            }
            else
            {
                Debug.LogWarning($"Setup guide not found at: {guidePath}");
            }
        }
    }
}

