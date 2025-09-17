using PuzzleGame;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Grid : MonoBehaviour
{
    [SerializeField] private Vector2Int grid;
    [SerializeField] private Vector2 touchOffset = new Vector2(-1f, -1f);
    [SerializeField] private float cellSize = 2f;
    [SerializeField] private int basePoints = 2;
    [SerializeField] private int jewelPoints = 5;
    [Space]
    [SerializeField] private AudioClip scoreSound;

    [SerializeField] private Tile _tilePrefab;

    private Dictionary<Vector2Int, Tile> _tiles = new Dictionary<Vector2Int, Tile>();

    public static Grid instance;
    public static UnityAction<int> OnScoreAdded;
    public static UnityAction<int> OnLinesDestroyed;

    private int score;
    
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        GenerateGrid();
    }

    private void Update()
    {
        if (PuzzleGame.InputManager.instance.TryGetIntVectorTouch(out Vector2Int toucPos))
        {
            Tile selectedTile = GetTileByGridPos(toucPos);
            if (selectedTile != null)
            {
                //selectedTile.SelectTile();
                Debug.Log(selectedTile.name);
            }
        }
    }

    private void GenerateGrid()
    {
        for(int x = 0; x < grid.x; x++)
        {
            for(int y = 0; y < grid.y; y++)
            {
                Tile tile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                tile.name = $"Tile({x}, {y})";
                _tiles.Add(new Vector2Int(x, y), tile);

                bool isOffset = (x % 2 == 0 && y % 2 != 0) || (y % 2 == 0 && x % 2 != 0);
                tile.SetTile(new Vector2Int(x, y), TileType.Empty, isOffset);
                tile.transform.SetParent(transform);

            }
        }
    }

    public Tile GetTileByGridPos(Vector2Int gridPos)
    {
        if(_tiles.TryGetValue(gridPos, out Tile tile))
            return tile;
        return null;
    }

    public Vector2 GetTouchOffset() => touchOffset;

    public void CheckForCompletedLines()
    {
        List<List<Tile>> completedLines = new List<List<Tile>>();
        int jewelCount = 0;

        // Check rows
        for (int y = 0; y < grid.y; y++)
        {
            List<Tile> row = new List<Tile>();
            bool rowComplete = true;

            for (int x = 0; x < grid.x; x++)
            {
                Tile tile = _tiles[new Vector2Int(x, y)];
                row.Add(tile);
                if (tile.tileType == TileType.Empty) // 👈 adjust depending on your enum
                {
                    rowComplete = false;
                    break;
                }
            }

            if (rowComplete)
                completedLines.Add(row);
        }

        // Check columns
        for (int x = 0; x < grid.x; x++)
        {
            List<Tile> column = new List<Tile>();
            bool colComplete = true;
            for (int y = 0; y < grid.y; y++)
            {
                Tile tile = _tiles[new Vector2Int(x, y)];
                column.Add(tile);
                if (tile.tileType == TileType.Empty)
                {
                    colComplete = false;
                    break;
                }
            }

            if (colComplete)
                completedLines.Add(column);
        }

        // Clear + scoring
        if (completedLines.Count > 0)
        {
            int tilesCleared = 0;

            foreach (var line in completedLines)
            {
                foreach (var tile in line)
                {
                    if(tile.tileType is TileType.Jewel)
                    {
                        jewelCount++;
                    }
                    if(tile.tileType is TileType.Wooden)
                    {
                        tilesCleared++;
                    }
                    tile.SetTileType(TileType.Empty, true);
                }
            }

            // Combo multiplier (e.g., 2 lines = 2x score)
            int points = ((tilesCleared * basePoints) + (jewelCount * jewelPoints)) * completedLines.Count;
            score += points;
            SoundManager.instance.SetAudio(scoreSound);
            OnScoreAdded?.Invoke(score);
            OnLinesDestroyed?.Invoke(tilesCleared);

            Debug.Log($"Cleared {completedLines.Count} lines, +{points} points! Total Score: {score}");
        }
    }
}
