using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PurpleBlockXR : MonoBehaviour
{
    void Start()
    {
        GetComponent<Renderer>().material.color = new Color(0.5f, 0f, 0.5f);
    }

    public void OnHoverEnter(HoverEnterEventArgs args)
    {
        gameObject.SetActive(false);
    }
}
// steps for creation:
// Setup:

// Hook Hover Enter → OnHoverEnter