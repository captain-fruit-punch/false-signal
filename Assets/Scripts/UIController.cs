using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour {
    public static UIController instance;

    private void Awake(){
        instance = this;
    }

    [SerializeField] TMP_Text interactionText; // Corrected attribute name

    public void EnableInteractionText(string text){
        interactionText.text = text + " (E)";
        interactionText.gameObject.SetActive(true);  // Corrected to use 'gameObject'
    }

    public void DisableInteractionText(){
        interactionText.gameObject.SetActive(false);  // Corrected to use 'gameObject'
    }
}
