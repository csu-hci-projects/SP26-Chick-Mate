using UnityEngine;
using UnityEngine.InputSystem;

public class VoiceTestInput : MonoBehaviour
{
    public SimpleBoard board;
    public GameObject piece;

    void Start()
    {
        Debug.Log("VoiceTestInput started");
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Space pressed");

            if (board == null)
            {
                Debug.LogError("Board is not assigned.");
                return;
            }

            if (piece == null)
            {
                Debug.LogError("Piece is not assigned.");
                return;
            }

            Vector3 target = board.GetWorldPosition("e4");
            Debug.Log("Target e4 = " + target);

            piece.transform.position = target;
        }
    }
}