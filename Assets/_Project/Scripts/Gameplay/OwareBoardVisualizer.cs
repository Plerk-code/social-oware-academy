using UnityEngine;
using TMPro;
using SocialOwareAcademy.Gameplay;

/// <summary>
/// Visualizes the Oware board using Unity primitives (cubes for pits).
/// Creates a 3D representation that updates based on board state.
/// </summary>
public class OwareBoardVisualizer : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private float pitSpacing = 2f;
    [SerializeField] private float pitSize = 1.5f;
    [SerializeField] private Color player1Color = Color.cyan;
    [SerializeField] private Color player2Color = Color.magenta;
    [SerializeField] private Color selectedColor = Color.yellow;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI gameStateText;
    [SerializeField] private TextMeshProUGUI instructionsText;

    // Visual elements
    private GameObject[] pitObjects;
    private TextMeshPro[] seedCountTexts;
    private Material[] pitMaterials;
    private int selectedPit = -1;

    void Start()
    {
        CreateBoardVisualization();

        // Subscribe to GameManager events
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStarted += OnGameStarted;
            GameManager.Instance.OnMoveMade += OnMoveMade;
            GameManager.Instance.OnGameEnded += OnGameEnded;
        }

        UpdateInstructions();
    }

    /// <summary>
    /// Create 3D board visualization using Unity primitives.
    /// </summary>
    private void CreateBoardVisualization()
    {
        pitObjects = new GameObject[OwareBoard.TOTAL_PITS];
        seedCountTexts = new TextMeshPro[OwareBoard.TOTAL_PITS];
        pitMaterials = new Material[OwareBoard.TOTAL_PITS];

        // Create Player 1 pits (bottom row, left to right: 0-5)
        for (int i = 0; i < OwareBoard.PITS_PER_PLAYER; i++)
        {
            Vector3 position = new Vector3(i * pitSpacing, 0, 0);
            pitObjects[i] = CreatePit(i, position, player1Color, "Player 1");
        }

        // Create Player 2 pits (top row, right to left: 11-6)
        for (int i = 0; i < OwareBoard.PITS_PER_PLAYER; i++)
        {
            int pitIndex = OwareBoard.TOTAL_PITS - 1 - i; // 11, 10, 9, 8, 7, 6
            Vector3 position = new Vector3(i * pitSpacing, 0, pitSpacing * 2);
            pitObjects[pitIndex] = CreatePit(pitIndex, position, player2Color, "Player 2");
        }

        // Center camera
        Camera.main.transform.position = new Vector3(pitSpacing * 2.5f, 10f, pitSpacing);
        Camera.main.transform.LookAt(new Vector3(pitSpacing * 2.5f, 0, pitSpacing));
    }

    /// <summary>
    /// Create a single pit visualization.
    /// </summary>
    private GameObject CreatePit(int pitIndex, Vector3 position, Color color, string playerName)
    {
        // Create pit container
        GameObject pitObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pitObj.name = $"Pit_{pitIndex}_{playerName}";
        pitObj.transform.position = position;
        pitObj.transform.localScale = new Vector3(pitSize, 0.5f, pitSize);

        // Add collider for clicking
        pitObj.AddComponent<BoxCollider>();

        // Store pit index in a component for click detection
        PitClickHandler clickHandler = pitObj.AddComponent<PitClickHandler>();
        clickHandler.pitIndex = pitIndex;

        // Set color
        Renderer renderer = pitObj.GetComponent<Renderer>();
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = color;
        renderer.material = mat;
        pitMaterials[pitIndex] = mat;

        // Create text for seed count
        GameObject textObj = new GameObject($"SeedCount_{pitIndex}");
        textObj.transform.SetParent(pitObj.transform);
        textObj.transform.localPosition = new Vector3(0, 1f, 0);
        textObj.transform.localRotation = Quaternion.Euler(90, 0, 0);

        TextMeshPro seedText = textObj.AddComponent<TextMeshPro>();
        seedText.text = "4";
        seedText.fontSize = 10;
        seedText.alignment = TextAlignmentOptions.Center;
        seedText.color = Color.white;
        seedCountTexts[pitIndex] = seedText;

        return pitObj;
    }

    /// <summary>
    /// Update visualization based on current board state.
    /// </summary>
    private void UpdateVisualization()
    {
        if (GameManager.Instance?.Board == null)
            return;

        OwareBoard board = GameManager.Instance.Board;

        // Update seed counts
        for (int i = 0; i < OwareBoard.TOTAL_PITS; i++)
        {
            seedCountTexts[i].text = board.GetSeeds(i).ToString();
        }

        // Update UI text
        if (gameStateText != null)
        {
            gameStateText.text = $"<b>OWARE GAME</b>\n\n" +
                $"Player 1 Captured: {board.Player1Captured}\n" +
                $"Player 2 Captured: {board.Player2Captured}\n\n" +
                $"<b>Current Turn: Player {board.CurrentPlayer + 1}</b>\n" +
                $"({(GameManager.Instance.IsHumanTurn ? "YOUR TURN" : "AI THINKING...")})";
        }

        UpdateInstructions();
    }

    /// <summary>
    /// Update instruction text.
    /// </summary>
    private void UpdateInstructions()
    {
        if (instructionsText == null)
            return;

        if (GameManager.Instance?.IsGameActive == true)
        {
            if (GameManager.Instance.IsHumanTurn)
            {
                instructionsText.text = "Click on your pit (bottom row) to make a move";
            }
            else
            {
                instructionsText.text = "AI is thinking...";
            }
        }
        else
        {
            instructionsText.text = "Press SPACE to start a new game";
        }
    }

    /// <summary>
    /// Handle pit click.
    /// </summary>
    public void OnPitClicked(int pitIndex)
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsGameActive)
            return;

        if (!GameManager.Instance.IsHumanTurn)
        {
            Debug.Log("[Visualizer] Not your turn!");
            return;
        }

        // Highlight selected pit
        selectedPit = pitIndex;
        HighlightPit(pitIndex);

        // Attempt move
        bool success = GameManager.Instance.MakeMove(pitIndex);

        if (success)
        {
            Debug.Log($"[Visualizer] Move successful: Pit {pitIndex}");
        }
        else
        {
            Debug.LogWarning($"[Visualizer] Invalid move: Pit {pitIndex}");
        }

        // Clear highlight after brief delay
        Invoke(nameof(ClearHighlight), 0.5f);
    }

    /// <summary>
    /// Highlight a pit.
    /// </summary>
    private void HighlightPit(int pitIndex)
    {
        if (pitIndex >= 0 && pitIndex < OwareBoard.TOTAL_PITS)
        {
            pitMaterials[pitIndex].color = selectedColor;
        }
    }

    /// <summary>
    /// Clear all highlights.
    /// </summary>
    private void ClearHighlight()
    {
        for (int i = 0; i < OwareBoard.PITS_PER_PLAYER; i++)
        {
            pitMaterials[i].color = player1Color;
        }
        for (int i = OwareBoard.PITS_PER_PLAYER; i < OwareBoard.TOTAL_PITS; i++)
        {
            pitMaterials[i].color = player2Color;
        }
    }

    // Event handlers
    private void OnGameStarted(OwareBoard board)
    {
        Debug.Log("[Visualizer] Game started");
        UpdateVisualization();
    }

    private void OnMoveMade(int pitIndex, int player)
    {
        Debug.Log($"[Visualizer] Move made: Pit {pitIndex} by Player {player + 1}");
        UpdateVisualization();
    }

    private void OnGameEnded(int winner)
    {
        Debug.Log($"[Visualizer] Game ended. Winner: {winner}");
        UpdateVisualization();

        if (gameStateText != null)
        {
            string winnerText = winner == 0 ? "Player 1" : (winner == 1 ? "Player 2" : "Draw");
            gameStateText.text = $"<b>GAME OVER!</b>\n\n" +
                $"Winner: {winnerText}\n\n" +
                $"Final Score:\n" +
                $"Player 1: {GameManager.Instance.Board.Player1Captured}\n" +
                $"Player 2: {GameManager.Instance.Board.Player2Captured}";
        }
    }

    void Update()
    {
        // Press SPACE to restart
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartNewGame();
            }
        }
    }

    void OnDestroy()
    {
        // Unsubscribe
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStarted -= OnGameStarted;
            GameManager.Instance.OnMoveMade -= OnMoveMade;
            GameManager.Instance.OnGameEnded -= OnGameEnded;
        }
    }
}

/// <summary>
/// Helper component to detect clicks on pits.
/// </summary>
public class PitClickHandler : MonoBehaviour
{
    public int pitIndex;

    void OnMouseDown()
    {
        // Find visualizer and notify of click
        OwareBoardVisualizer visualizer = FindObjectOfType<OwareBoardVisualizer>();
        if (visualizer != null)
        {
            visualizer.OnPitClicked(pitIndex);
        }
    }
}
