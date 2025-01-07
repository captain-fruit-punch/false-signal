using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    [SerializeField] Scenario spawnScenario;
    [SerializeField] GameObject planeVisual;
    [SerializeField] Scenario departureScenario;
    [SerializeField] bool hasLanded;
    [SerializeField] float delayUntilDepature;
    [SerializeField] PlanePath planePath;
    
    void Start()
    {
        ScenarioManager.Instance.AddScenario(spawnScenario);
        ScenarioManager.Instance.AddScenario(departureScenario);
    }

    public void activatePlane()
    {
        planeVisual.SetActive(true);
        planePath.gameObject.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
