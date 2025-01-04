using UnityEngine;

public abstract class RadioListener : MonoBehaviour
{
    // Method to be called by the RadioTransmitter
    public abstract void OnReceiveMessage(RadioMessage message);
}
