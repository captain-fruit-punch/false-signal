using UnityEngine;

public class PlayerInteraction : MonoBehaviour 
{
    public float playerReach = 3f;
    Interactable currentInteractable;

    void Update(){
        CheckInteraction();
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null){
            currentInteractable.Interact();
        }
    }

    void CheckInteraction(){
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hit, playerReach)){
            if (hit.collider.tag == "Interactable"){
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                if (currentInteractable && newInteractable != currentInteractable){
                    currentInteractable.DisableOutline();
                }

                if (newInteractable.enabled){
                    SetNewCurrentInteractable(newInteractable);
                }
                else{
                    DisableCurrentInteractable();
                }
            }
            else{
                DisableCurrentInteractable();
            }
        }
        else{
            DisableCurrentInteractable();
        }
    }

    void SetNewCurrentInteractable(Interactable setInteractable){
        currentInteractable = setInteractable;
        currentInteractable.EnableOutline();
        UIController.instance.EnableInteractionText(currentInteractable.message);
    }

    void DisableCurrentInteractable(){
        UIController.instance.DisableInteractionText();

        if (currentInteractable){
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }
}
