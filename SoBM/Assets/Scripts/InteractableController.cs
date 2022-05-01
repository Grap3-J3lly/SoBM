using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------
    
    private GameManager gameManager;

    [SerializeField] private bool activeState = false;
    private bool triggered = false;

    // Switch Only
    [SerializeField] private GameObject swivel;

    // Button Only
    [SerializeField] private GameObject buttonTrigger;
    [SerializeField] private float triggerDepressedHeight = .125f;
    [SerializeField] private float triggerPressedHeight = 0;


    public enum InteractableType {
        Switch,
        Button,
        Block
    }

    [SerializeField] private InteractableType interactType;

    //------------------------------------------------------
    //                  STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        gameManager = GameManager.Instance;
        if(interactType == InteractableType.Switch) {
            RotateSwivel(activeState);
        }
        if(interactType == InteractableType.Button) {
            ShiftButton(activeState);
        }
        
    }

    private void Update() {
        if(interactType == InteractableType.Switch) {
            RotateSwivel(activeState);
        }
        if(interactType == InteractableType.Button) {
            ShiftButton(activeState);
        }
    }

    //------------------------------------------------------
    //                  COLLISION FUNCTIONS
    //------------------------------------------------------

    private void OnTriggerEnter(Collider info) {
        if(!triggered){
            triggered = true;
            Debug.Log("Triggered by: " + info.gameObject.tag);


            if(info.gameObject.tag == "Player") {
            PlayerController tempController = info.gameObject.GetComponentInParent<PlayerController>();
            tempController.SetZoneEntered(true);
            tempController.SetObjectInZone(gameObject);

            if(this.interactType == InteractableType.Button) {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                HandleButtonInteraction();
            }
            StartCoroutine(Reset());
        }
        }
    }

    private void OnTriggerExit(Collider info) {
        Debug.Log("Leaving Interact Zone");

        if(info.gameObject.tag == "Player") {

            PlayerController tempController = info.gameObject.GetComponentInParent<PlayerController>();
            tempController.SetZoneEntered(false);
            tempController.SetObjectInZone(null);
            
            if(this.interactType == InteractableType.Button) {
                gameObject.GetComponent<BoxCollider>().enabled = true;
            }

        }

    }

    //------------------------------------------------------
    //                  INTERACTION FUNCTIONS
    //------------------------------------------------------

    public void HandleInteraction(InventoryManager inventoryManager) {
        
        if(this.interactType == InteractableType.Block) {
            HandleButtonInteraction();
        }

        if(this.interactType == InteractableType.Switch) {
            HandleSwitchInteraction();
        }

        if(this.interactType == InteractableType.Block) {
            HandleBlockInteraction(inventoryManager);
        }
    }

    private void HandleSwitchInteraction() {
        ToggleItem(activeState);
    }

    private void HandleButtonInteraction() {
        ToggleItem(activeState);
    }

    private void HandleBlockInteraction(InventoryManager inventoryManager) {
        inventoryManager.inventory.Add(gameObject);
        inventoryManager.CreateButton(gameObject);
        transform.SetParent(inventoryManager.gameObject.transform);
        gameObject.SetActive(false);
    }

    //------------------------------------------------------
    //                  MISCELLANEOUS FUNCTIONS
    //------------------------------------------------------

    private void ToggleItem(bool active) {
        if(active) {
            activeState = false;
        } 
        else {
            activeState = true;
        }
    }

    private void RotateSwivel(bool up) {

        Vector3 parentRotation = swivel.transform.parent.transform.eulerAngles;

        if(up) {
            swivel.transform.rotation = Quaternion.Euler(parentRotation.x, parentRotation.y, 45);
        }
        else {
            swivel.transform.rotation = Quaternion.Euler(parentRotation.x, parentRotation.y, -45);
        }
    }

    private void ShiftButton(bool up) {
        Vector3 currentPosition = buttonTrigger.transform.position;
        if(up) {
            Vector3 pressedPosition = new Vector3(currentPosition.x, triggerPressedHeight, currentPosition.z);
            buttonTrigger.transform.position = pressedPosition;
        }
        else {
            Vector3 depressedPosition = new Vector3(currentPosition.x, triggerDepressedHeight, currentPosition.z);
            buttonTrigger.transform.position = depressedPosition;
        }
    }

    IEnumerator Reset() {
        yield return new WaitForSeconds(2.5f);
        triggered = false;
    }

}
