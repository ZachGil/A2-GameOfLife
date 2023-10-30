using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour
{

    [SerializeField] private Tilemap currentState;
    [SerializeField] private Tilemap nextState;
    [SerializeField] private Tile aliveTile;
    [SerializeField] private Tile deadTile;
    [Header("Interval setting, between 0 - 1")]
    [SerializeField] private float updateInterval = 0.05f;
    [SerializeField] private Pattern[] patterns;
    private bool isRunning = true;
    private int intPattern;

    private HashSet<Vector3Int> aliveCells;
    private HashSet<Vector3Int> cellsToCheck;

    private void Awake()
    {
        aliveCells = new HashSet<Vector3Int>();
        cellsToCheck = new HashSet<Vector3Int>();
    }

    // Start is called before the first frame update
    void Start()
    {
        intPattern = Random.Range(0, patterns.Length);
        SetPattern(patterns[intPattern]);
    }

    private void SetPattern(Pattern pattern)
    {
        Clear();

        Vector2Int center = pattern.GetCenter();

        for(int i = 0; i < pattern.cells.Length; i++)
        {
            Vector3Int cell = (Vector3Int)(pattern.cells[i] - center);
            currentState.SetTile(cell, aliveTile);
            aliveCells.Add(cell);
        }
    }

    private void Clear()
    {
        currentState.ClearAllTiles();
        nextState.ClearAllTiles();
    }

    private void OnEnable()
    {
        StartCoroutine(Simulate());
    }

    private IEnumerator Simulate()
    {
        while (enabled)
        {
            UpdateState();
            yield return  new WaitForSeconds(updateInterval);
        }
    }

    private void UpdateState()
    {
        if (isRunning)
        {
            Debug.Log("button pressed down");
            cellsToCheck.Clear();

            //gather cells to check
            foreach (Vector3Int cell in aliveCells)
            {
                for(int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        cellsToCheck.Add(cell + new Vector3Int(x, y, 0));
                    }
                }
            }

            //transitionaing cells to next state
            foreach (Vector3Int cell in cellsToCheck)
            {
                int neighbors = CountNeighbors(cell);
                bool alive = IsAlive(cell);

                if (!alive && neighbors == 3)
                {
                    //alive
                    nextState.SetTile(cell, aliveTile);
                    aliveCells.Add(cell);
                } else if (alive && (neighbors < 2 || neighbors > 3))
                {
                    //dead
                    nextState.SetTile(cell, deadTile);
                    aliveCells.Remove(cell);
                } else
                {
                    //stays
                    nextState.SetTile(cell, currentState.GetTile(cell));
                }
            }

            Tilemap temp = currentState;
            currentState = nextState;
            nextState = temp;
            nextState.ClearAllTiles();
        }
    }

    private int CountNeighbors(Vector3Int cell)
    {
        int count = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighbor = cell + new Vector3Int(x, y, 0);

                if (x == 0 && y == 0) continue;
                else if (IsAlive(neighbor)) count++;
            }
        }

        return count;
    }

    private bool IsAlive(Vector3Int cell)
    {
        return currentState.GetTile(cell) == aliveTile;
    }

    public void AdjustInterval(float interval)
    {
        updateInterval = interval;
    }

    public void SceneState()
    {
        if (isRunning == true) isRunning = false;
        else isRunning = true;
    }

    public void Gens()
    {
        intPattern++;
        if (intPattern > patterns.Length - 1) intPattern = 0; 
        SetPattern(patterns[intPattern]);
        Debug.Log("Pattern" + intPattern);
    }

    public void Restart()
    {
        SetPattern(patterns[intPattern]);
    }

}
