using UnityEngine;

public class SimpleBoard : MonoBehaviour
{
    public Transform a1Origin;
    public float cellSize = 0.1f;
    public float pieceHeight = 0.2f;

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
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Vector3 offset =
                    a1Origin.right * (x * cellSize) +
                    a1Origin.forward * (y * cellSize) +
                    a1Origin.up * pieceHeight;

                grid[x, y] = a1Origin.position + offset;
            }
        }
    }

    public Vector3 GetWorldPosition(string square)
    {
        square = square.ToLower();

        int x = square[0] - 'a';
        int y = int.Parse(square[1].ToString()) - 1;

        return grid[x, y];
    }
}