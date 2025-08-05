using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float cellSize = 1f;
    public bool generateRandomMap = true;
    [Range(0f, 0.4f)]
    public float wallPercentage = 0.3f;

    [Header("Prefabs")]
    public GameObject cellPrefab;

    [Header("Colors")]
    public Color emptyColor = Color.white;
    public Color wallColor = Color.gray;
    public Color npcColor = Color.blue;
    public Color goalColor = Color.red;
    public Color pathColor = Color.yellow;
    public Color visitedColor = Color.cyan;

    private int[,] grid;
    private GameObject[,] cellObjects;
    private Vector2Int npcPosition;
    private Vector2Int goalPosition;
    private PathfindingAgent agent;

    private const int EMPTY = 0;
    private const int WALL = 1;
    private const int NPC = 2;
    private const int GOAL = 3;

    void Start()
    {
        agent = FindAnyObjectByType<PathfindingAgent>();
        GenerateGrid();
        CreateVisualGrid();

        if (agent != null)
        {
            agent.Initialize(this);
        }
    }

    void GenerateGrid()
    {
        grid = new int[gridWidth, gridHeight];

        if (generateRandomMap)
        {
            GenerateRandomMap();
        }
        else
        {
            GenerateHardcodedMap();
        }
    }

    void GenerateRandomMap()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y] = EMPTY;
            }
        }

        int wallCount = Mathf.RoundToInt(gridWidth * gridHeight * wallPercentage);
        for (int i = 0; i < wallCount; i++)
        {
            int x = Random.Range(0, gridWidth);
            int y = Random.Range(0, gridHeight);

            if (grid[x, y] == EMPTY)
            {
                grid[x, y] = WALL;
            }
        }

        do
        {
            npcPosition.x = Random.Range(0, gridWidth);
            npcPosition.y = Random.Range(0, gridHeight);
        } while (grid[npcPosition.x, npcPosition.y] != EMPTY);

        grid[npcPosition.x, npcPosition.y] = NPC;

        do
        {
            goalPosition.x = Random.Range(0, gridWidth);
            goalPosition.y = Random.Range(0, gridHeight);
        } while (grid[goalPosition.x, goalPosition.y] != EMPTY ||
                 Vector2Int.Distance(npcPosition, goalPosition) < 3);

        grid[goalPosition.x, goalPosition.y] = GOAL;
    }

    void GenerateHardcodedMap()
    {
        int[,] hardcodedGrid = new int[,]
        {
            {0,0,0,0,1,0,0,0,0,0},
            {0,1,1,0,1,0,1,1,1,0},
            {0,0,0,0,0,0,0,0,1,0},
            {1,1,1,0,1,1,1,0,1,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,0},
            {1,1,0,1,1,1,0,1,1,1},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,3}
        };

        for (int x = 0; x < gridWidth && x < hardcodedGrid.GetLength(0); x++)
        {
            for (int y = 0; y < gridHeight && y < hardcodedGrid.GetLength(1); y++)
            {
                grid[x, y] = hardcodedGrid[x, y];

                if (grid[x, y] == GOAL)
                {
                    goalPosition = new Vector2Int(x, y);
                }
            }
        }

        npcPosition = new Vector2Int(0, 0);
        grid[0, 0] = NPC;
    }

    void CreateVisualGrid()
    {
        cellObjects = new GameObject[gridWidth, gridHeight];

        SetupMobileCamera();
        CalculateOptimalCellSize();

        float gridPixelWidth = gridWidth * cellSize;
        float gridPixelHeight = gridHeight * cellSize;
        Vector3 gridOffset = new Vector3(-gridPixelWidth * 0.5f + cellSize * 0.5f,
                                        -gridPixelHeight * 0.5f + cellSize * 0.5f, 0);

        Vector3 cellScale = Vector3.one * 5f; 
        if (gridWidth == 20 && gridHeight == 20)
            cellScale = Vector3.one * 2.5f;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 worldPos = new Vector3(x * cellSize, y * cellSize, 0) + gridOffset;
                GameObject cell = Instantiate(cellPrefab, worldPos, Quaternion.identity, transform);
                cell.transform.localScale = cellScale;
                cell.name = $"Cell_{x}_{y}";

                cellObjects[x, y] = cell;
                UpdateCellVisual(x, y);
            }
        }
    }
    void CalculateOptimalCellSize()
    {
        float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        float screenHeight = Camera.main.orthographicSize * 2;

        float availableHeight = screenHeight * 0.7f;
        float availableWidth = screenWidth * 0.9f;

        float cellSizeByWidth = availableWidth / gridWidth;
        float cellSizeByHeight = availableHeight / gridHeight;

        cellSize = Mathf.Min(cellSizeByWidth, cellSizeByHeight);
        cellSize = Mathf.Max(cellSize, 0.4f);
    }

    void SetupMobileCamera()
    {
        if (Camera.main == null) return;

        Camera.main.transform.position = new Vector3(0, 0, -10f);

        Camera.main.orthographicSize = 9.6f; 

        Camera.main.orthographic = true;
    }

    public void UpdateCellVisual(int x, int y)
    {
        if (cellObjects[x, y] == null) return;

        SpriteRenderer renderer = cellObjects[x, y].GetComponent<SpriteRenderer>();
        if (renderer == null) return;

        switch (grid[x, y])
        {
            case EMPTY:
                renderer.color = emptyColor;
                break;
            case WALL:
                renderer.color = wallColor;
                break;
            case NPC:
                renderer.color = npcColor;
                break;
            case GOAL:
                renderer.color = goalColor;
                break;
        }
    }

    public void HighlightPath(List<Vector2Int> path)
    {
        foreach (Vector2Int pos in path)
        {
            if (grid[pos.x, pos.y] != NPC && grid[pos.x, pos.y] != GOAL)
            {
                cellObjects[pos.x, pos.y].GetComponent<SpriteRenderer>().color = pathColor;
            }
        }
    }

    public void HighlightVisited(HashSet<Vector2Int> visited)
    {
        foreach (Vector2Int pos in visited)
        {
            if (grid[pos.x, pos.y] == EMPTY)
            {
                cellObjects[pos.x, pos.y].GetComponent<SpriteRenderer>().color = visitedColor;
            }
        }
    }

    public bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight && grid[x, y] != WALL;
    }

    public bool IsWalkable(int x, int y)
    {
        return IsValidPosition(x, y) && (grid[x, y] == EMPTY || grid[x, y] == GOAL);
    }

    public Vector2Int GetNPCPosition() => npcPosition;
    public Vector2Int GetGoalPosition() => goalPosition;
    public int[,] GetGrid() => grid;
    public float GetCellSize() => cellSize;

    void OnGUI()
    {
        Rect safeArea = Screen.safeArea;

        GUIStyle bigStyle = new GUIStyle(GUI.skin.label);
        bigStyle.fontSize = 40;
        GUIStyle bigButton = new GUIStyle(GUI.skin.button);
        bigButton.fontSize = 45;

        GUILayout.BeginArea(new Rect(
            safeArea.x + 20,
            safeArea.y + 80,
            550,
            570)); 
        GUILayout.BeginVertical("box");

        GUILayout.Label($"Grid Size: {gridWidth}x{gridHeight}", bigStyle);
        GUILayout.Label($"NPC Position: {npcPosition}", bigStyle);
        GUILayout.Label($"Goal Position: {goalPosition}", bigStyle);

        if (GUILayout.Button("Find Path", bigButton))
        {
            if (agent != null)
            {
                agent.FindAndShowPath();
            }
        }

        if (GUILayout.Button("Reset Path", bigButton))
        {
            ResetPathVisualization();
        }

        if(GUILayout.Button("Genrate Hardcoded Map", bigButton))
        {
            ClearOldCells();
            GenerateHardcodedMap();
            CreateVisualGrid();
        }

        if (GUILayout.Button("Generate New Map", bigButton))
        {
            ClearOldCells();
            GenerateGrid();
            CreateVisualGrid();
        }

        if (GUILayout.Button("Generate map 10x10", bigButton))
        {
            gridWidth = 10;
            gridHeight = 10;
            ClearOldCells();
            GenerateGrid();
            CreateVisualGrid();
        }

        if (GUILayout.Button("Generate map 20x20", bigButton))
        {
            gridWidth = 20;
            gridHeight = 20;
            ClearOldCells();
            GenerateGrid();
            CreateVisualGrid();
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
    private void ClearOldCells()
    {
        if (cellObjects != null)
        {
            foreach (var cell in cellObjects)
            {
                if (cell != null)
                    Destroy(cell);
            }
        }
    }

    public void UpdateNPCPosition(Vector2Int newPosition)
    {
        if (cellObjects[newPosition.x, newPosition.y] != null)
        {
            cellObjects[newPosition.x, newPosition.y].GetComponent<SpriteRenderer>().color = npcColor;
        }
    }

    public void ClearNPCFromPosition(Vector2Int position)
    {
        if (cellObjects[position.x, position.y] != null && grid[position.x, position.y] != WALL && grid[position.x, position.y] != GOAL)
        {
            cellObjects[position.x, position.y].GetComponent<SpriteRenderer>().color = emptyColor;
        }
    }

    public void ResetPathVisualization()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                UpdateCellVisual(x, y);
            }
        }
    }
}