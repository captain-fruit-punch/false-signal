using UnityEngine;
using System.Collections.Generic;

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager Instance { get; private set; }

    private float currentTime = 0f; // The current game time
    private List<Scenario> activeScenarios = new List<Scenario>();

    private void Awake()
    {
        // Ensure there is only one instance of this Singleton.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy the entire GameObject, not just this component.
            return;
        }

        Instance = this;

        // Optional: Make this Singleton persist across scenes.
        DontDestroyOnLoad(gameObject);
    }

    public void AddScenario(Scenario scenario)
    {
        activeScenarios.Add(scenario);
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        // Check for scenarios that should trigger
        for (int i = activeScenarios.Count - 1; i >= 0; i--)
        {
            if (activeScenarios[i].triggerTime <= currentTime)
            {
                activeScenarios[i].Execute();
                activeScenarios.RemoveAt(i); // Remove triggered scenario
            }
        }
    }
}
