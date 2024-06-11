using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logText;
    private Queue<string> logQueue = new Queue<string>();
    private const int maxLogCount = 15;

    void Awake()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        logQueue.Enqueue(logString);
        if (logQueue.Count > maxLogCount)
        {
            logQueue.Dequeue();
        }
        logText.text = string.Join("\n", logQueue.ToArray());
    }
}
