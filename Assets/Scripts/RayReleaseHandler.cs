using UnityEngine;

public class RayReleaseHandler : MonoBehaviour
{
    public SimpleBoard board;
    public TrialRunner trialRunner;

    public void OnRelease()
    {
        ChessPiece piece = GetComponent<ChessPiece>();

        if (piece == null)
        {
            Debug.LogError("No ChessPiece found on " + gameObject.name);
            return;
        }

        string square = board.GetClosestSquare(transform.position);

        trialRunner.RegisterMove(piece, square);

        Debug.Log($"Released {piece.pieceName} on {square}");
    }
}