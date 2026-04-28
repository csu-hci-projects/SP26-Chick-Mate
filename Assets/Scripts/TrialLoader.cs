using System.Collections.Generic;
using UnityEngine;

public class TrialLoader : MonoBehaviour
{
    public List<TrialData> trials = new List<TrialData>();

    void Awake()
    {
        LoadTrials();
    }

    void LoadTrials()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("trials");

        if (csvFile == null)
        {
            Debug.LogError("Could not find trials.csv in Assets/Resources/");
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();

            if (string.IsNullOrEmpty(line))
                continue;

            string[] cols = line.Split(',');

            TrialData t = new TrialData();
            t.trial = int.Parse(cols[0]);
            t.method = cols[1].Trim();
            t.piece = cols[2].Trim();
            t.destination = cols[3].Trim().ToLower();

            trials.Add(t);
        }

        Debug.Log("Loaded " + trials.Count + " trials.");
    }
}

[System.Serializable]
public class TrialData
{
    public int trial;
    public string method;
    public string piece;
    public string destination;
}