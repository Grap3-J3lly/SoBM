using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerController : MonoBehaviour
{

    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

    // General Related
    private GameManager gameManager;
    [SerializeField] private Camera camera;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private GameObject interactionZone;

    public event Action onActivationEvent;

    // Player Related

    [SerializeField] private InputManager inputManager;
    private InputControl inputControl;
    private CharacterController charController;

    // Horizontal Movement Related
    private Vector2 horizontalInput;
    private Vector3 horizontalForce;
    private bool isMovePressed;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationFactorPerFrame = 1.0f;

    // Interaction Related
    private bool interacting;
    
    private bool zoneEntered = false;
    private GameObject objectInZone;

    // Placement Related
    [SerializeField] Vector3 placementOffset = new Vector3(10f, 0f, 10f);

    //------------------------------------------------------
    //             GETTERS/SETTERS
    //------------------------------------------------------

    public bool GetZoneEntered() {return zoneEntered;} 
    public void SetZoneEntered(bool newValue) {zoneEntered = newValue;}

    public GameObject GetObjectInZone() {return objectInZone;}
    public void SetObjectInZone(GameObject newObject) {objectInZone = newObject;}

    //------------------------------------------------------
    //                  COROUTINES
    //------------------------------------------------------

    public IEnumerator ControlledSetup() {
        inputControl = new InputControl();
        charController = GetComponent<CharacterController>();
        yield return new WaitUntil(() => GameManager.Instance != null);
        gameManager = GameManager.Instance;

        HandleMovementSetup();
        HandleInteractionSetup();
        HandlePlacementSetup();
    }

    //------------------------------------------------------
    //             STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        HandleAllSetup();
    }

    private void OnEnable() {
        
        inputControl.CharacterControls.Move.Enable();
        inputControl.CharacterControls.Interact.Enable();
        inputControl.CharacterControls.Place.Enable();

        //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += OnInteract;
    }

    private void Start() {
        
    }

    private void Update() {
        HandleRotation();
        HandleMovement();
    }

    private void OnDisable() {

        inputControl.CharacterControls.Move.Disable();
        inputControl.CharacterControls.Interact.Disable();
        inputControl.CharacterControls.Place.Disable();

        //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= OnInteract;
        //EnhancedTouchSupport.Disable();
    }

    //------------------------------------------------------
    //             SETUP FUNCTIONS
    //------------------------------------------------------

    private void HandleAllSetup() {
        StartCoroutine(ControlledSetup());
    }

    private void HandleMovementSetup() {
        inputControl.CharacterControls.Move.started += OnMove;
        inputControl.CharacterControls.Move.canceled += OnMove;
        inputControl.CharacterControls.Move.performed += OnMove;
    }

    private void HandleInteractionSetup() {
        // inputControl.CharacterControls.Interact.started += OnInteract;
        // inputControl.CharacterControls.Interact.canceled += OnInteract;
        inputControl.CharacterControls.Interact.performed += OnInteract;

        //EnhancedTouchSupport.Enable();
    }

    private void HandlePlacementSetup() {
        inputControl.CharacterControls.Place.performed += OnPlacement;
    }

    //------------------------------------------------------
    //          MOVEMENT PHYSICS FUNCTIONS
    //------------------------------------------------------

    private void OnMove(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>();
        horizontalForce.x = horizontalInput.x;
        horizontalForce.z = horizontalInput.y;
        isMovePressed = horizontalInput.x != 0 || horizontalInput.y != 0;
    }

    private void HandleMovement() {
        Vector3 currentMovement = horizontalForce * Time.deltaTime * moveSpeed;
        if(transform.position.y > 1) {
            currentMovement.y = -1;
        }
        charController.Move(currentMovement);
    }

    private void HandleRotation() {
        Vector3 desiredDirection;

        desiredDirection.x = horizontalForce.x;
        desiredDirection.y = 0f;
        desiredDirection.z = horizontalForce.z;

        Quaternion currentRotation = transform.rotation;

        if(isMovePressed) {
            Quaternion desiredRotation = Quaternion.LookRotation(desiredDirection);
            transform.rotation = Quaternion.Slerp(currentRotation, desiredRotation, rotationFactorPerFrame);
        }
    }

    //------------------------------------------------------
    //          INTERACTION FUNCTIONS
    //------------------------------------------------------

    // private void OnInteract(InputAction.CallbackContext context) {
    //     interacting = context.ReadValueAsButton();
    // }

    // Touch Screen Movement Links:
    // https://www.youtube.com/watch?v=4HpC--2iowE
    // https://www.youtube.com/watch?v=guOhuhh4CTQ
    // https://www.youtube.com/watch?v=sYtWOsKvqcg

    // private void OnInteract(Finger finger) {
    //     Vector2 newPos = camera.ScreenToWorldPoint(finger.screenPosition);

    //     Vector3 test = new Vector3(newPos.x, newPos.y, 5f);

    //     //Debug.Log("World Location Info: " + newPos);

    //     Debug.Log("Finger Position Info: " + finger.screenPosition);
    // }

    private void OnInteract(InputAction.CallbackContext context) {

        if(zoneEntered) {
            Debug.Log("Interaction ocurring with object in zone");
            if(objectInZone != null && objectInZone.tag == "Interactable") {
                objectInZone.GetComponent<InteractableController>().HandleInteraction(inventoryManager);
            }
        }

    }

    public void HandleInteraction(GameObject interactionOrigin, bool triggered) {
        InteractableController tempController = interactionOrigin.GetComponent<InteractableController>();
        if(triggered) {
            zoneEntered = true;
            objectInZone = interactionOrigin;

            if(tempController.GetInteractType() == InteractableController.InteractableType.Button) {
                tempController.HandleInteraction(inventoryManager);
            }
        }
        else {
            zoneEntered = false;
            objectInZone = null;
        }
    }

    //------------------------------------------------------
    //          PLACEMENT FUNCTIONS
    //------------------------------------------------------

    private void OnPlacement(InputAction.CallbackContext context) {
        if(inventoryManager.GetSelectedObject() != null) {
            Vector3 placement = HandlePlacementLocation();
            GameObject selectedObject = inventoryManager.GetSelectedObject();
            PlaceItemInWorld(selectedObject, placement);
        }
    }

    private Vector3 HandlePlacementLocation() {
        Vector3 placement = (placementOffset.z * interactionZone.transform.forward) + interactionZone.transform.position;

        return placement;
    }

    private void PlaceItemInWorld(GameObject selectedObject, Vector3 placement) {
        selectedObject.SetActive(true);
        LevelManager currentLevelManager = gameManager.GetLevels()[gameManager.GetCurrentLevelNum()];
        
        selectedObject.transform.SetParent(currentLevelManager.transform);
        selectedObject.transform.position = placement;
    }

    //------------------------------------------------------
    //          MISCELLANEOUS FUNCTIONS
    //------------------------------------------------------

    public void ActivationEvent() {
        if(onActivationEvent != null) {
            onActivationEvent();
        }
    }

}

