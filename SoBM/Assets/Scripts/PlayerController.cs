using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerController : MonoBehaviour
{

    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

    // General Related
    private GameManager gameManager;
    [SerializeField] private Camera camera;

    // Player Related

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
    
    public bool zoneEntered = false;
    public GameObject objectInZone;


    //------------------------------------------------------
    //             STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        HandleGeneralSetup();
        HandleMovementSetup();
        HandleInteractionSetup();
    }

    private void OnEnable() {
        
        inputControl.CharacterControls.Move.Enable();
        inputControl.CharacterControls.Interact.Enable();

        //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += OnInteract;
    }

    private void Start() {
        
    }

    private void Update() {
        HandleRotation();
        HandleMovement();

        HandleInteraction();


    }

    private void OnDisable() {

        inputControl.CharacterControls.Move.Disable();
        inputControl.CharacterControls.Interact.Disable();

        //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= OnInteract;
        EnhancedTouchSupport.Disable();
    }

    //------------------------------------------------------
    //             SETUP FUNCTIONS
    //------------------------------------------------------

    private void HandleGeneralSetup() {
        gameManager = GameManager.Instance;
        inputControl = new InputControl();
        charController = GetComponent<CharacterController>();
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

        EnhancedTouchSupport.Enable();
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
        Debug.Log("Interaction Occurred");

        if(zoneEntered) {
            Debug.Log("Interaction ocurring with object in zone");
        }

    }

    private void HandleInteraction() {
        if(interacting) {
            Debug.Log("Interaction Triggered");
        }
    }

    private void OnTriggerEnter(Collider info) {
        Debug.Log("Triggered");
        zoneEntered = true;
        objectInZone = info.gameObject;
    }

    private void OnTriggerExit(Collider info) {
        Debug.Log("Not Triggered");
        zoneEntered = false;
        objectInZone = null;
    }

}

