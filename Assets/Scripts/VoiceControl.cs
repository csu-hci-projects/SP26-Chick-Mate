using UnityEngine;
using UnityEngine.Windows.Speech;
using System;
using System.Collections.Generic;

public class VoiceControl : MonoBehaviour
{
    public SimpleBoard board;

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
                      " | color=" + cp.pieceColor +
                      " | piece=" + cp.pieceName);

            for (char file = 'a'; file <= 'h'; file++)
            {
                for (int rank = 1; rank <= 8; rank++)
                {
                    string square = $"{file}{rank}";
                    string command = $"{cp.pieceColor.ToLower().Trim()} {cp.pieceName.ToLower().Trim()} {square}";

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

        if (board == null)
        {
            Debug.LogError("MovePiece: board is null.");
            return;
        }

        Vector3 target = board.GetWorldPosition(square);

        Debug.Log("Moving object: " + cp.gameObject.name);
        Debug.Log("From position: " + cp.transform.position);
        Debug.Log("To square: " + square);
        Debug.Log("Target position: " + target);

        cp.transform.position = target;

        Debug.Log("New actual position: " + cp.transform.position);
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