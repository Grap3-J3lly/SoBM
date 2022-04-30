using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------
    
    private GameManager gameManager;
    private bool inInteractZone = false;

    [SerializeField] private bool activeState = false;
    [SerializeField] private GameObject swivel;

    public enum InteractableType {
        Switch,
        Button,
        Block
    }

    [SerializeField] private InteractableType interactType;

    //------------------------------------------------------
    //                  GETTERS/SETTERS
    //------------------------------------------------------

    public bool GetInInteractZone() {return inInteractZone;}
    public void SetInInteractZone(bool newValue) {inInteractZone = newValue;}

    //------------------------------------------------------
    //                  STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        gameManager = GameManager.Instance;
        if(interactType == InteractableType.Switch) {
            RotateSwivel(activeState);
        }
        
    }

    private void Update() {
        if(interactType == InteractableType.Switch) {
            RotateSwivel(activeState);
        }
    }

    //------------------------------------------------------
    //                  COLLISION FUNCTIONS
    //------------------------------------------------------

    private void OnTriggerEnter(Collider info) {
        Debug.Log("Triggered in Block Controller");
        Debug.Log(info.gameObject.tag);

        if(info.gameObject.tag == "Player") {
            PlayerController tempController = info.gameObject.GetComponentInParent<PlayerController>();
            tempController.SetZoneEntered(true);
            tempController.SetObjectInZone(gameObject);
        }

    }

    private void OnTriggerExit(Collider info) {
        Debug.Log("Leaving Interact Zone");

        if(info.gameObject.tag == "Player") {
            PlayerController tempController = info.gameObject.GetComponentInParent<PlayerController>();
            tempController.SetZoneEntered(false);
            tempController.SetObjectInZone(null);
        }

    }

    //------------------------------------------------------
    //                  INTERACTION FUNCTIONS
    //------------------------------------------------------

    public void HandleInteraction(InventoryManager inventoryManager) {
        
        if(this.interactType == InteractableType.Block) {
            HandleButtonInteraction(inventoryManager);
        }

        if(this.interactType == InteractableType.Switch) {
            HandleSwitchInteraction(inventoryManager);
        }

        if(this.interactType == InteractableType.Block) {
            HandleBlockInteraction(inventoryManager);
        }
    }

    private void HandleSwitchInteraction(InventoryManager inventoryManager) {
        ToggleItem(activeState);
    }

    private void HandleButtonInteraction(InventoryManager inventoryManager) {

    }

    private void HandleBlockInteraction(InventoryManager inventoryManager) {
        Debug.Log("Trying to handle a block");
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

}
