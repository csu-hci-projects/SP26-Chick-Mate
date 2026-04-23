using UnityEngine;

public class TestMove : MonoBehaviour
{
    public SimpleBoard board;
    public GameObject piece;

    void Start()
    {
        if (board == null)
        {
            Debug.LogError("Board is not assigned in TestMove.");
            return;
        }

        if (piece == null)
        {
            Debug.LogError("Piece is not assigned in TestMove.");
            return;
        }

        Vector3 target = board.GetWorldPosition("e4");

        Debug.Log("Board object: " + board.name);
        Debug.Log("Piece object: " + piece.name);
        Debug.Log("Target e4: " + target);

        piece.transform.position = target;
    }
}