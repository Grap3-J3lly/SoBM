using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------
    
    // Managers
    private GameManager gameManager;
    private LevelManager levelManager;
    private GameObject playerObject;

    // General

    public enum InteractableType {
        Switch,
        Button,
        Block,
        Exit
    }

    [SerializeField] private InteractableType interactType;

    [SerializeField] private bool activeState = false;
    private bool triggered = false;

    // Switch Only
    [SerializeField] private GameObject swivel;

    // Button Only
    [SerializeField] private GameObject buttonTrigger;
    [SerializeField] private float triggerDepressedHeight = .125f;
    [SerializeField] private float triggerPressedHeight = 0;

    //------------------------------------------------------
    //                  GETTERS/SETTERS
    //------------------------------------------------------

    public InteractableType GetInteractType() {return interactType;}
    public void SetInteractType(InteractableType newType) {interactType = newType;}

    public bool GetActiveState() {return activeState;}
    public void SetActiveState(bool newValue) {activeState = newValue;}

    //------------------------------------------------------
    //                  COROUTINES
    //------------------------------------------------------

    // Works with triggered variable to provide controlled call of trigger when many are ocurring from one object
    public IEnumerator ControlledTrigger(Collider info, bool triggerEnter) {

        if(triggerEnter) {
            // Triggered assists in stopping multiple calls of OnTriggerEnter from one collision
            if(!triggered){
                triggered = true;
                yield return new WaitForEndOfFrame();
                HandleBoxCollider(false);
                CheckTriggerOrigin(info.gameObject);
                yield return new WaitForEndOfFrame();
                HandleBoxCollider(true);
            }
        }
        else {
            triggered = false;
            CheckTriggerOrigin(info.gameObject);
        }

    }

    //------------------------------------------------------
    //                  STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        gameManager = GameManager.Instance;
    }

    private void Start() {
        levelManager = gameManager.GetLevels()[gameManager.GetCurrentLevelNum()];
        playerObject = gameManager.GetPlayerObject();

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
        if(interactType == InteractableType.Exit) {
            if(levelManager == null) {
                StartCoroutine(gameManager.Reset(gameManager.GetBaseResetTime()));
                return;
            }
                if(levelManager.GetRequirementCount() > 0) {ToggleItem(true);}
                else {ToggleItem(false);}
            
        }
    }

    //------------------------------------------------------
    //          STANDARD COLLISION FUNCTIONS
    //------------------------------------------------------

    private void OnTriggerEnter(Collider info) {
        StartCoroutine(ControlledTrigger(info, true));
    }

    private void OnTriggerExit(Collider info) {
        StartCoroutine(ControlledTrigger(info, false));
    }

    //------------------------------------------------------
    //          CUSTOM COLLISION FUNCTIONS
    //------------------------------------------------------

    private void CheckTriggerOrigin(GameObject origin) {
        
        if(origin.tag == "Player") {
            origin.GetComponentInParent<PlayerController>().HandleInteraction(gameObject, triggered);
        }
        else if(origin.tag == "NPC") {
            origin.GetComponentInParent<AIController>().HandleInteraction(gameObject, triggered);
        }
        else {
            Debug.Log("Other Trigger Occurred");
        }
    }

    public void HandleBoxCollider(bool active) {
        gameObject.GetComponent<BoxCollider>().enabled = active;
    }

    //------------------------------------------------------
    //          PRIMARY INTERACTION FUNCTIONS
    //------------------------------------------------------

    public void HandleInteraction(InventoryManager inventoryManager) {
        
        if(this.interactType == InteractableType.Block) {
            HandleBlockInteraction(inventoryManager);
        }

        if(this.interactType == InteractableType.Switch) {
            HandleSwitchInteraction();
        }

        if(this.interactType == InteractableType.Button) {
            HandleButtonInteraction();
        }

        if(this.interactType == InteractableType.Exit) {
            HandleExitInteraction();
        }
    }

    private void HandleBlockInteraction(InventoryManager inventoryManager) {

        if(inventoryManager.transform.parent.gameObject.tag == "Player") {
            inventoryManager.CreateButton(gameObject);
        }
        inventoryManager.inventory.Add(gameObject);
        transform.SetParent(inventoryManager.gameObject.transform);
        gameObject.SetActive(false);
    }

    private void HandleSwitchInteraction() {
        playerObject.GetComponent<PlayerController>().ActivationEvent();
        UpdateLevelManager(activeState);
        ToggleItem(activeState);
    }

    private void HandleButtonInteraction() {
        playerObject.GetComponent<PlayerController>().ActivationEvent();
        UpdateLevelManager(activeState);
        ToggleItem(activeState);
    }

    private void HandleExitInteraction() {
        CheckExit(activeState);
    }

    //------------------------------------------------------
    //          MISCELLANEOUS INTERACTION FUNCTIONS
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

    public void CheckExit(bool unlocked) {
        if(unlocked) {
            Debug.Log("Door is unlocked, level complete");
        }
        else {
            Debug.Log("Door is locked. Requirements remaining: " + levelManager.GetRequirementCount());
        }
    }

    private void UpdateLevelManager(bool active) {
        int currentCount = levelManager.GetRequirementCount();

        Debug.Log("Before Count Change: " + currentCount);
        if(active) {
            levelManager.SetRequirementCount(++currentCount);
        }
        else {
            levelManager.SetRequirementCount(--currentCount);
        }
        Debug.Log("After Count Change: " + currentCount);
    }

}
