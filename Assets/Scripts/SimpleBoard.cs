using UnityEngine;

public class SimpleBoard : MonoBehaviour
{
    public TrialRunner trialRunner;

    public Transform a1Origin;
    public float cellSize = 0.1f;
    public float pieceHeight = 0.02f;

    private Vector3[,] grid = new Vector3[8, 8];

    void Awake()
    {
        if (a1Origin == null)
        {
            Debug.LogError("A1Origin is not assigned in SimpleBoard.");
            return;
        }

        BuildGrid();
    }

    void BuildGrid()
    {
        if (a1Origin == null)
        {
            Debug.LogError("A1Origin is not assigned in SimpleBoard.");
            return;
        }

        Vector3 letterDirection = a1Origin.forward;
        Vector3 numberDirection = a1Origin.right;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Vector3 offset =
                    letterDirection * (x * cellSize) +
                    numberDirection * (y * cellSize) +
                    a1Origin.up * pieceHeight;

                grid[x, y] = a1Origin.position + offset;
            }
        }
    }

    public Vector3 GetWorldPosition(string square)
    {
        square = square.ToLower().Trim();

        int rankIndex = int.Parse(square[1].ToString()) - 1; // number 1-8
        int fileIndex = square[0] - 'a';                     // letter a-h

        int x = rankIndex;
        int y = fileIndex;

        if (x < 0 || x > 7 || y < 0 || y > 7)
        {
            Debug.LogError("Invalid square: " + square);
            return a1Origin.position;
        }

        Debug.Log($"GetWorldPosition({square}) → grid[{x},{y}]");

        return grid[x, y];
    }

    public string GetClosestSquare(Vector3 worldPosition)
    {
        BuildGrid();

        float closestDistance = Mathf.Infinity;
        string closestSquare = "";

        Vector3 flatWorld = new Vector3(worldPosition.x, 0, worldPosition.z);

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Vector3 flatGrid = new Vector3(grid[x, y].x, 0, grid[x, y].z);

                float distance = Vector3.Distance(flatWorld, flatGrid);

                if (distance < closestDistance)
                {
                    closestDistance = distance;

                    char file = (char)('a' + y);
                    int rank = x + 1;
                    closestSquare = file.ToString() + rank.ToString();

                }
            }
        }

        Debug.Log("Closest square = " + closestSquare);
        return closestSquare;
    }
}