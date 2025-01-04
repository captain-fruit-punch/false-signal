using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerUIRadioEntry : MonoBehaviour
{
    [SerializeField] private TMP_InputField messageInputField; // The input field for entering messages
    [SerializeField] private TextMeshProUGUI instructionsText; // Instructions text (optional)
    [SerializeField] private RadioTransmitter radioTransmitter; // Reference to the RadioTransmitter

    private void Start()
    {
        // Set default instructions text (optional)
        if (instructionsText != null)
        {
            instructionsText.text = "Enter a message in the box below and press Enter to transmit.";
        }

        // Add listener to the input field for when the user presses Enter
        messageInputField.onEndEdit.AddListener(OnMessageEntered);
    }

    private void OnDestroy()
    {
        // Remove listener when the script is destroyed
        messageInputField.onEndEdit.RemoveListener(OnMessageEntered);
    }

    private void OnMessageEntered(string input)
    {
        // Prevent empty messages from being sent
        if (string.IsNullOrWhiteSpace(input)) return;

        // Create a new RadioMessage and send it via the RadioTransmitter
        RadioMessage message = new RadioMessage(input);
        radioTransmitter.Transmit(message);

        // Clear the input field
        messageInputField.text = string.Empty;

        Debug.Log($"Message transmitted: {input}");
    }
}
