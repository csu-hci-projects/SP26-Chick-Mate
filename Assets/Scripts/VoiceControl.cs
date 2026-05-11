using UnityEngine;
using UnityEngine.Windows.Speech;
using System;
using System.Collections.Generic;

public class VoiceControl : MonoBehaviour
{
    public SimpleBoard board;
    public TrialRunner trialRunner;

    private KeywordRecognizer recognizer;
    private Dictionary<string, Action> commands = new Dictionary<string, Action>();

    void Start()
    {
        if (board == null)
        {
            Debug.LogError("VoiceControl: board is not assigned.");
            return;
        }

        ChessPiece[] allPieces = FindObjectsOfType<ChessPiece>();
        Debug.Log("Found pieces: " + allPieces.Length);

        foreach (ChessPiece cp in allPieces)
        {
            Debug.Log("Found piece object: " + cp.gameObject.name +
                      " | pieceID=" + cp.pieceID +
                      " | pieceName=" + cp.pieceName);

            for (char file = 'a'; file <= 'h'; file++)
            {
                for (int rank = 1; rank <= 8; rank++)
                {
                    string square = $"{file}{rank}";

                    // Example: "white pawn e5"
                    string command = $"{cp.pieceName.ToLower().Trim()} {cp.startSquare.ToLower().Trim()} to {square}";

                    ChessPiece capturedPiece = cp;
                    string capturedSquare = square;

                    commands[command] = () => MovePiece(capturedPiece, capturedSquare);
                }
            }
        }

        Debug.Log("Total commands built: " + commands.Count);

        recognizer = new KeywordRecognizer(
            new List<string>(commands.Keys).ToArray(),
            ConfidenceLevel.Low
        );

        recognizer.OnPhraseRecognized += OnPhraseRecognized;
        recognizer.Start();

        Debug.Log("Voice commands ready");
        Debug.Log("Recognizer running: " + recognizer.IsRunning);
    }

    void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        string spoken = args.text.ToLower().Trim();
        Debug.Log("Heard: " + spoken);

        if (commands.TryGetValue(spoken, out Action action))
        {
            Debug.Log("Command matched: " + spoken);
            action.Invoke();
        }
        else
        {
            Debug.LogWarning("Command not found: " + spoken);
        }
    }

    void MovePiece(ChessPiece cp, string square)
    {
        if (cp == null)
        {
            Debug.LogError("MovePiece: ChessPiece was null.");
            return;
        }

        Vector3 target = board.GetWorldPosition(square);

        Debug.Log("Moving object: " + cp.gameObject.name);
        Debug.Log("To square: " + square);
        Debug.Log("Target position: " + target);

        cp.transform.position = target;

        if (trialRunner != null)
        {
            trialRunner.RegisterMove(cp, square);
        }
        else
        {
            Debug.LogError("VoiceControl: trialRunner is not assigned.");
        }

        Debug.Log($"Voice requested square {square}, target {target}");
    }

    void OnApplicationQuit()
    {
        if (recognizer != null)
        {
            if (recognizer.IsRunning)
                recognizer.Stop();

            recognizer.OnPhraseRecognized -= OnPhraseRecognized;
            recognizer.Dispose();
        }
    }
}