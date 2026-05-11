using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;

public class TrialRunner : MonoBehaviour
{
    public TrialLoader loader;
    public UIManager ui;

    private int index = 0;
    private float startTime;
    private string filePath;
    private bool trialResolved = false;
    

    private Dictionary<ChessPiece, Vector3> startingPositions = new Dictionary<ChessPiece, Vector3>();
    private Dictionary<ChessPiece, Quaternion> startingRotations = new Dictionary<ChessPiece, Quaternion>();

    void Start()
    {
        if (loader == null)
        {
            Debug.LogError("TrialLoader is not assigned on TrialRunner.");
            return;
        }

        filePath = Path.Combine(
            Application.dataPath,
            $"Participant_{loader.participantID}_Output.csv"
        );

        File.WriteAllText(filePath, "Trial,Method,Piece,Destination,Time,Distance,Size,IndexOfDifficulty\n");

        Debug.Log("Output CSV path: " + filePath);
        
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
            Debug.Log("Experiment complete.");

            ResetPieces();

            if (ui != null)
                ui.ShowFinal(filePath);

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

        if (piece == null)
        {
            Debug.LogError("RegisterMove called with null piece.");
            return;
        }

        TrialData t = GetCurrentTrial();

        if (t == null)
            return;

        string movedPiece = (piece.pieceName + " " + piece.startSquare).ToLower().Trim();
        string expectedPiece = t.piece.ToLower().Trim();

        string movedDestination = destination.ToLower().Trim();
        string expectedDestination = t.destination.ToLower().Trim();

        Debug.Log($"Comparing piece: '{movedPiece}' vs '{expectedPiece}'");
        Debug.Log($"Comparing destination: '{movedDestination}' vs '{expectedDestination}'");

        bool correctPiece = movedPiece == expectedPiece;
        bool correctDestination = movedDestination == expectedDestination;

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

        float distance = Vector3.Distance(
            startingPositions[piece],
            piece.transform.position
        );

        float size = FindObjectOfType<SimpleBoard>().cellSize;

        float indexOfDifficulty = Mathf.Log((distance / size) + 1f, 2f);

        string line =
            $"{t.trial},{t.method},{t.piece},{t.destination},{movementTime:F3},{distance:F4},{size:F4},{indexOfDifficulty:F4}\n";

        File.AppendAllText(filePath, line);

        Debug.Log(
            $"Completed Trial {t.trial} | Time={movementTime:F3} | D={distance:F4} | W={size:F4} | ID={indexOfDifficulty:F4}"
        );

        index++;

        StartCoroutine(AdvanceAfterDelay());
    }

    IEnumerator AdvanceAfterDelay()
    {
        Debug.Log("Waiting before reset...");
        yield return new WaitForSeconds(2f);

        ResetPieces();

        yield return new WaitForSeconds(0.2f);

        StartNextTrial();
    }
}