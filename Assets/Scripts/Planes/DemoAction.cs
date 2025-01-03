using UnityEditor.UI;
using UnityEngine;

public class DemoAction : MonoBehaviour
{
    ScenarioManager scenarioManager;
    [SerializeField] Scenario scenario;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scenarioManager = ScenarioManager.Instance;
        print(scenarioManager);
        scenarioManager.AddScenario(scenario);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void printHello()
    {
        Debug.Log("DEMO ACTION CALLED FROM " + gameObject.name);
    }
}
