using UnityEngine;

public class FlashlightScript : MonoBehaviour
{
    // Reference to the light component
    private Light flashlight;

    void Start()
    {
        // Get the Light component attached to this GameObject
        flashlight = GetComponent<Light>();

        // Ensure the flashlight starts off
        if (flashlight != null)
        {
            flashlight.enabled = false;
        }
        else
        {
            Debug.LogError("No Light component found on this GameObject!");
        }
    }

    void Update()
    {
        // Toggle light on/off with the F key
        if (Input.GetKeyDown(KeyCode.F) && flashlight != null)
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}
