using UnityEngine;

[System.Serializable]
public class RadioMessage
{
    public string content; // The message content

    public RadioMessage(string content)
    {
        this.content = content;
    }
}
