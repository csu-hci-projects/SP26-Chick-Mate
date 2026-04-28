using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TrialRunner : MonoBehaviour
{
    public TrialLoader loader;
    public SimpleBoard board;
    public UIManager ui;

    private int index = 0;
    private float startTime;
    private string filePath;
    private bool trialResolved = false;

    private Dictionary<ChessPiece, Vector3> startingPositions = new Dictionary<ChessPiece, Vector3>();
    private Dictionary<ChessPiece, Quaternion> startingRotations = new Dictionary<ChessPiece, Quaternion>();

    void Start()
    {
        filePath = Path.Combine(Application.dataPath, "ChickMate_OutputFile.csv");

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "Trial,Method,Piece,Destination,Time\n");
        }

        SaveStartingPositions();

        StartNextTrial();
    }

    void SaveStartingPositions()
    {
        ChessPiece[] pieces = FindObjectsOfType<ChessPiece>();

        foreach (ChessPiece piece in pieces)
        {
            startingPositions[piece] = piece.transform.position;
            startingRotations[piece] = piece.transform.rotation;
        }

        Debug.Log("Saved starting positions for " + pieces.Length + " pieces.");
    }

    public void ResetPieces()
    {
        foreach (var item in startingPositions)
        {
            ChessPiece piece = item.Key;

            if (piece != null)
            {
                piece.transform.position = startingPositions[piece];
                piece.transform.rotation = startingRotations[piece];
            }
        }

        Debug.Log("Pieces reset to starting positions.");
    }

    public TrialData GetCurrentTrial()
    {
        if (loader == null || index >= loader.trials.Count)
            return null;

        return loader.trials[index];
    }

    public void StartNextTrial()
    {
        trialResolved = false;

        if (loader == null || loader.trials.Count == 0)
        {
            Debug.LogError("No trials loaded.");
            return;
        }

        if (index >= loader.trials.Count)
        {
            Debug.Log("Experiment complete");

            ResetPieces();

            return;
        }

        TrialData t = loader.trials[index];

        if (ui != null)
            ui.UpdateInstruction(t);

        startTime = Time.time;

        Debug.Log($"Trial {t.trial}: Use {t.method} to move {t.piece} to {t.destination}");
    }

    public void RegisterMove(ChessPiece piece, string destination)
    {
        if (trialResolved)
            return;

        TrialData t = GetCurrentTrial();

        if (t == null)
            return;

        bool correctPiece =
            piece.pieceName.ToLower().Trim() == t.piece.ToLower().Trim();

        bool correctDestination =
            destination.ToLower().Trim() == t.destination.ToLower().Trim();

        if (correctPiece && correctDestination)
        {
            CompleteCurrentTrial(piece, destination);
        }
        else
        {
            Debug.LogWarning(
                $"Wrong move. Expected {t.piece} to {t.destination}, got {piece.pieceName} to {destination}"
            );

        }
    }

    void CompleteCurrentTrial(ChessPiece piece, string destination)
    {
        trialResolved = true;

        TrialData t = loader.trials[index];
        float movementTime = Time.time - startTime;

        string line = $"{t.trial},{t.method},{t.piece},{t.destination},{movementTime:F3}\n";
        File.AppendAllText(filePath, line);

        Debug.Log($"Completed Trial {t.trial} | {piece.pieceName} to {destination} | Time={movementTime:F3}");

        ResetPieces();

        index++;

        Invoke(nameof(StartNextTrial), 1.0f);
    }
}