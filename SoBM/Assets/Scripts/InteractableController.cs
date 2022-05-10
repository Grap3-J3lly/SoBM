using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(2)]
public class InteractableController : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------
    
    // Managers
    private GameManager gameManager;
    private LevelManager levelManager;
    private AudioManager audioManager;

    // General

    private GameObject playerObject;

    public enum InteractableType {
        Switch,
        Button,
        Block,
        Exit
    }

    [SerializeField] private InteractableType interactType;

    [SerializeField] private bool activeState = false;
    private bool triggered = false;
    private bool colliding = false;

    private int activeCollisions = 0;

    // Audio
    private AudioClip openDoorSound;
    private AudioClip singleButtonClickSound;
    private AudioClip objectPlaceSound;
    private AudioClip objectLiftSound;
    private AudioClip leverSwitchSound;

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

    // Collisions

    // Works with triggered variable to provide controlled call of trigger when many are ocurring from one object
    public IEnumerator ControlledTrigger(Collider info, bool triggerEnter) {

        if(triggerEnter) {
            // Triggered assists in stopping multiple calls of OnTriggerEnter from one collision
            if(!triggered){
                triggered = true;
                yield return new WaitForEndOfFrame();
                
                if(this.interactType == InteractableType.Button) {
                    Debug.Log("Button Triggered");
                    HandleBoxCollider(false);
                }
                CheckTriggerOrigin(info.gameObject);
                yield return new WaitForSeconds(2f);
                HandleBoxCollider(true);
            }
        }
        else {
            triggered = false;
            CheckTriggerOrigin(info.gameObject);
        }

    }

    public IEnumerator ControlledCollision(Collision info, bool collisionEnter) {
        InteractableType tempType = info.gameObject.GetComponent<InteractableController>().GetInteractType();
        if(collisionEnter) {
            if(!colliding) {
                colliding = true;
                yield return new WaitForEndOfFrame();

                // Turns button off after Block is removed
                if(this.interactType == InteractableType.Button) {
                    if(tempType == InteractableType.Block && activeCollisions == 0) {
                        activeCollisions += 1;
                        HandleBoxCollider(false);
                        HandleButtonInteraction();
                    }
                }
            }
        }
        else {
        
            // Turns button back on after Block is removed
            if(this.interactType == InteractableType.Button) {
                if(tempType == InteractableType.Block && activeCollisions == 1) {
                    activeCollisions -= 1;
                    yield return new WaitForEndOfFrame();
                    HandleButtonInteraction();
                    yield return new WaitForEndOfFrame();
                    HandleBoxCollider(true);
                }
            }
            colliding = false;
        }
    }

    // Audio

    public IEnumerator HandleOpenDoorSound() {
        yield return new WaitUntil(() => audioManager.GetAudioSource().isPlaying == false);
        audioManager.Play(openDoorSound.name);
    }

    public IEnumerator HandleButtonClickSound() {
        yield return new WaitUntil(() => audioManager.GetAudioSource().isPlaying == false);
        audioManager.Play(singleButtonClickSound.name);
    }

    public IEnumerator HandleObjectPlaceSound() {
        yield return new WaitUntil(() => audioManager.GetAudioSource().isPlaying == false);
        audioManager.Play(objectPlaceSound.name);
    }

    public IEnumerator HandleObjectLiftSound() {
        yield return new WaitUntil(() => audioManager.GetAudioSource().isPlaying == false);
        audioManager.Play(objectLiftSound.name);
    }

    public IEnumerator HandleLeverSwitchSound() {
        yield return new WaitUntil(() => audioManager.GetAudioSource().isPlaying == false);
        audioManager.Play(leverSwitchSound.name);
    }

    //------------------------------------------------------
    //                  STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        gameManager = GameManager.Instance;
    }

    private void Start() {
        levelManager = gameManager.GetLevels()[0];
        playerObject = gameManager.GetPlayerObject();
        HandleAudioSetup();

        if(interactType == InteractableType.Switch) {
            RotateSwivel(activeState);
        }
        if(interactType == InteractableType.Button) {
            ShiftButton(activeState);
        }
    }

    private void Update() {
        if(interactType == InteractableType.Block) {
            if(transform.position.y != .5f) {
                transform.position = new Vector3(transform.position.x, .5f, transform.position.z);
            }
        }
        if(interactType == InteractableType.Switch) {
            RotateSwivel(activeState);
        }
        if(interactType == InteractableType.Button) {
            ShiftButton(activeState);
        }
        if(interactType == InteractableType.Exit) {
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

    private void OnCollisionEnter(Collision info) {
        if(info.gameObject.tag == "Interactable") {
            StartCoroutine(ControlledCollision(info, true));
        }
    }

    private void OnCollisionStay(Collision info) {
        
        if(this.interactType == InteractableType.Button && info.gameObject.tag == "Interactable" 
        && info.gameObject.GetComponent<InteractableController>().GetInteractType() == InteractableType.Block) {
            HandleBoxCollider(false);
        }
    }

    private void OnCollisionExit(Collision info) {
        if(info.gameObject.tag == "Interactable") {
            StartCoroutine(ControlledCollision(info, false));
        }
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

    private void HandleBoxCollider(bool active) {
        gameObject.GetComponent<BoxCollider>().enabled = active;
    }

    // Not to be confused with HandleButtonTriggers(), which doesn't exist
    private void HandleButtonCollisions(GameObject origin, bool active) {
        if(origin.tag == "Interactable") {
            // Debug.Log("Showed that origin is an interactable" + gameObject.name);
            InteractableController.InteractableType originType = origin.GetComponent<InteractableController>().GetInteractType();
            if(this.interactType == InteractableType.Button && originType == InteractableType.Block) {
                HandleBoxCollider(active);
                HandleButtonInteraction();
            }
        }
    }

    //------------------------------------------------------
    //                 AUDIO FUNCTIONS
    //------------------------------------------------------

    private void HandleAudioSetup() {
        audioManager = AudioManager.Instance;
        LoadAudio();
    }

    private void LoadAudio() {
        openDoorSound = Resources.Load<AudioClip>("Foley/openCloseDoor");
        singleButtonClickSound = Resources.Load<AudioClip>("Foley/singleButtonClick");
        objectPlaceSound = Resources.Load<AudioClip>("Foley/objectPlacement");
        objectLiftSound = Resources.Load<AudioClip>("Foley/objectLift");
        leverSwitchSound = Resources.Load<AudioClip>("Foley/leverSwitch");
    }

    //------------------------------------------------------
    //          PRIMARY INTERACTION FUNCTIONS
    //------------------------------------------------------

    public void HandleInteraction(InventoryManager inventoryManager) {
        
        if(this.interactType == InteractableType.Block) {
            HandleBlockInteraction(inventoryManager);
        }

        if(this.interactType == InteractableType.Switch) {
            StartCoroutine(HandleLeverSwitchSound());
            HandleSwitchInteraction();
        }

        if(this.interactType == InteractableType.Button) {
            StartCoroutine(HandleButtonClickSound());
            HandleButtonInteraction();
        }

        if(this.interactType == InteractableType.Exit) {
            HandleExitInteraction();
        }
    }

    private void HandleBlockInteraction(InventoryManager inventoryManager) {
        StartCoroutine(HandleObjectLiftSound());
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
            StartCoroutine(HandleOpenDoorSound());
            Debug.Log("Door is unlocked, level complete");
            levelManager.SetLevelComplete(true);
            levelManager.HandleLevelComplete();
        }
        else {
            Debug.Log("Door is locked. Requirements remaining: " + levelManager.GetRequirementCount());
        }
    }

    private void UpdateLevelManager(bool active) {
        int currentCount = levelManager.GetRequirementCount();

        //Debug.Log("Before Count Change: " + currentCount);
        if(active) {
            levelManager.SetRequirementCount(++currentCount);
        }
        else {
            levelManager.SetRequirementCount(--currentCount);
        }
        Debug.Log("After Count Change: " + currentCount);
    }

}
