using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class YellowBlockXR : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    public void OnHoverEnter(HoverEnterEventArgs args)
    {
        audioSource.Play();
    }
}

//steps for creation:
// Setup:

// Add AudioSource

// Drag your sound into it

// Hook Hover Enter → OnHoverEnter