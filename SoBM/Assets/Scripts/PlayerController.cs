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

    // Animation Related
    private Animator playerAnimator;

    // Player Related
    private InputControl inputControl;
    private CharacterController charController;

    [SerializeField] private bool useOnPlacement = false;

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

    public bool GetUseOnPlacement() {return useOnPlacement;}
    public void SetUseOnPlacement(bool newValue) {useOnPlacement = newValue;}

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

        inputControl.TouchControls.PrimaryContact.Enable();

        //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += OnInteract;
    }

    private void Start() {
        HandleAnimationSetup();
    }

    private void Update() {
        HandleRotation();
        HandleMovement();
        HandleAnimationChange();
    }

    private void OnDisable() {

        inputControl.CharacterControls.Move.Disable();
        inputControl.CharacterControls.Interact.Disable();
        inputControl.CharacterControls.Place.Disable();

        inputControl.TouchControls.PrimaryContact.Disable();

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
        //inputControl.CharacterControls.Place.performed += OnPlacement;

        
    }

    public void HandleAnimationSetup() {
        playerAnimator = GetComponent<Animator>();
        playerAnimator.updateMode = AnimatorUpdateMode.Normal;
    }

    //------------------------------------------------------
    //          ANIMATION FUNCTIONS
    //------------------------------------------------------

    private void HandleAnimationChange() {
        bool isWalking = playerAnimator.GetBool("isWalking");
        

        if(!isWalking && isMovePressed) {
            playerAnimator.SetBool("isWalking", true);
        }
        if(isWalking && !isMovePressed) {
            playerAnimator.SetBool("isWalking", false);
        }
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

            // if(tempController.GetInteractType() == InteractableController.InteractableType.Button) {
            //     tempController.HandleInteraction(inventoryManager);
            // }
            tempController.HandleInteraction(inventoryManager);
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
            StartCoroutine(selectedObject.GetComponent<InteractableController>().HandleObjectPlaceSound());
            
            PlaceItemInWorld(selectedObject, placement);
            inventoryManager.SetSelectedObject(null);
            Destroy(inventoryManager.GetInventoryButtons()[0]);
            inventoryManager.GetInventory().RemoveAt(0);
            inventoryManager.GetInventoryButtons().RemoveAt(0);

            useOnPlacement = false;
            CheckInteractOrPlacement();
        }
    }

    private Vector3 HandlePlacementLocation() {
        Vector3 placement = (placementOffset.z * interactionZone.transform.forward) + interactionZone.transform.position;

        return placement;
    }

    private void PlaceItemInWorld(GameObject selectedObject, Vector3 placement) {
        selectedObject.SetActive(true);
        LevelManager currentLevelManager = gameManager.GetLevels()[0];
        
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

    // private void DetermineInteractionType() {
    //     if(!inventoryManager.GetSelectedObject()) {
    //         useOnPlacement = false;
    //     }
    //     else {
    //         useOnPlacement = true;
    //     }
    // }

    public void CheckInteractOrPlacement() {
        if(useOnPlacement) {
            inputControl.TouchControls.PrimaryContact.performed += OnPlacement;
            inputControl.TouchControls.PrimaryContact.performed -= OnInteract;
        }
        else {
            inputControl.TouchControls.PrimaryContact.performed += OnInteract;
            inputControl.TouchControls.PrimaryContact.performed -= OnPlacement;
        }
    }

}

