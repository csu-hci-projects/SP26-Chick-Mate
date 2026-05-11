using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI trialText;
    public TextMeshProUGUI methodText;
    public TextMeshProUGUI instructionText;

    public void UpdateInstruction(TrialData t)
    {
        trialText.text = "Trial: " + t.trial;
        methodText.text = "Use: " + t.method;
        instructionText.text = "Move " + t.piece + " to " + t.destination;
    }

    public void ShowFinal(string path)
    {
        trialText.text = "Experiment Complete";
        methodText.text = "";
        instructionText.text = "Saved to:\n" + path;
    }
}