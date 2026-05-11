using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    public string pieceColor;          // "White"
    public string pieceType;           // "Pawn"
    public string startSquare;    // "a2", "b2", etc.

    public string pieceName => pieceColor + " " + pieceType;

    public string pieceID => pieceColor + " " + pieceType + " " + startSquare;
}