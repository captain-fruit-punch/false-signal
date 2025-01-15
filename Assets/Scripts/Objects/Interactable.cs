using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour  // Inherit from MonoBehaviour
{
    Outline outline;
    public string message;

    public UnityEvent onInteraction;

    void Start()
    {
        outline = GetComponent<Outline>();  // Get the Outline component from the same GameObject
        DisableOutline();
    }

    public void Interact()
    {
        onInteraction.Invoke();  // Call the interaction event
    }

    public void DisableOutline()
    {
        outline.enabled = false;  // Disable the outline effect
    }

    public void EnableOutline()
    {
        outline.enabled = true;  // Enable the outline effect
    }
}
