using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BlueBlockXR : MonoBehaviour
{
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = Color.blue;
    }

    public void OnHoverEnter(HoverEnterEventArgs args)
    {
        rend.material.color = Color.white;
    }

    public void OnHoverExit(HoverExitEventArgs args)
    {
        rend.material.color = Color.blue;
    }
}

//Steps for creation: Add XR Simple Interactable

// In Inspector:

// Hover Enter → + → drag object → select OnHoverEnter

// Hover Exit → + → drag object → select OnHoverExit