using UnityEngine;
using UnityEngine.Events;

public class PlaneRadioListener : RadioListener
{
    [SerializeField] private string transponderName = "ABCD"; // 4-character transponder name
    [SerializeField] private UnityEvent onClearedToLand;
    [SerializeField] private UnityEvent onClearedForTakeoff;
    [SerializeField] private UnityEvent<float, float> onHoldAt;

    public override void OnReceiveMessage(RadioMessage message)
    {
        // Ensure the message content is valid
        if (string.IsNullOrWhiteSpace(message.content)) return;

        // Check if the message starts with the transponder name
        if (!message.content.StartsWith(transponderName)) return;

        // Remove the transponder name from the message
        string command = message.content.Substring(transponderName.Length).Trim();

        // Parse the command
        if (command == "CLEARED TO LAND")
        {
            Debug.Log($"{transponderName}: Received CLEARED TO LAND");
            onClearedToLand?.Invoke();
        }
        else if (command == "CLEARED FOR TAKEOFF")
        {
            Debug.Log($"{transponderName}: Received CLEARED FOR TAKEOFF");
            onClearedForTakeoff?.Invoke();
        }
        else if (command.StartsWith("HOLD AT"))
        {
            ParseHoldAtCommand(command);
        }
        else
        {
            Debug.LogWarning($"{transponderName}: Unrecognized command '{command}'");
        }
    }

    private void ParseHoldAtCommand(string command)
    {
        // Command format: "HOLD AT -+XX.XX -+XX.XX"
        string[] parts = command.Replace("HOLD AT", "").Trim().Split(' ');

        if (parts.Length == 2 && float.TryParse(parts[0], out float latitude) && float.TryParse(parts[1], out float longitude))
        {
            Debug.Log($"{transponderName}: Holding at {latitude}, {longitude}");
            onHoldAt?.Invoke(latitude, longitude);
        }
        else
        {
            Debug.LogWarning($"{transponderName}: Invalid HOLD AT command format");
        }
    }
}
