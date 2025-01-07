using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Windows;

public class PlaneRadioResonder : MonoBehaviour
{
    [SerializeField] RadioTransmitter radioTransmitter;
    [TextArea] public string landingApprovedResponse;
    [TextArea] public string takeoffApprovedResponse;

    public void landingApproved()
    {
        RadioMessage message = new RadioMessage(landingApprovedResponse);
        radioTransmitter.Transmit(message);
    }
    public void takeoffApproved()
    {
        RadioMessage message = new RadioMessage(takeoffApprovedResponse);
        radioTransmitter.Transmit(message);
    }
}
