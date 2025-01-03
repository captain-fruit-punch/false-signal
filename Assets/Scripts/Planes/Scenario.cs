using UnityEngine;
using UnityEngine.Events;

public class Scenario : MonoBehaviour
{
    public string scenarioName;
    public float triggerTime; // Time at which this scenario triggers
    [TextArea] public string description; // Optional: Describe what this scenario does

    // List of Unity Events to be executed when the scenario is triggered
    public UnityEvent[] actions;

    public void Execute()
    {
        Debug.Log($"Executing Scenario: {scenarioName}");

        // Invoke all actions in the UnityEvent list
        foreach (UnityEvent action in actions)
        {
            action?.Invoke();
        }
    }
}
