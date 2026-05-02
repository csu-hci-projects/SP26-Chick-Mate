using System.Collections.Generic;
using UnityEngine;
public class TrialLoader : MonoBehaviour
{
    public int participantID = 1; // what YOU enter (1–12)
    private int csvIndex;         // computed (1–3)

    public List<TrialData> trials = new List<TrialData>();

    void Awake()
    {
        csvIndex = ((participantID - 1) % 3) + 1;
        LoadTrials();
    }

    void LoadTrials()
    {
        string fileName = "trials_" + csvIndex;

        TextAsset csvFile = Resources.Load<TextAsset>(fileName);

        if (csvFile == null)
        {
            Debug.LogError("Could not find " + fileName + ".csv in Assets/Resources/");
            return;
        }

        trials.Clear();

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

        Debug.Log($"Participant {participantID} → using trials_{csvIndex}.csv");
    }
}