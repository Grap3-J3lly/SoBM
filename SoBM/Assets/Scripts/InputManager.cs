using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

    private GameManager gameManager;
    private InputControl inputControl;
    
    [SerializeField] private GameObject mainCameraObject;
    private Camera mainCamera;

    //------------------------------------------------------
    //                  EVENTS
    //------------------------------------------------------

    public delegate void StartPrimaryTouch(Vector2 position, float time);
    public event StartPrimaryTouch OnStartPrimaryTouch;
    public delegate void EndPrimaryTouch(Vector2 position, float time);
    public event EndPrimaryTouch OnEndPrimaryTouch;

    public delegate void StartSecondaryTouch(Vector2 position, float time);
    public event StartSecondaryTouch OnStartSecondaryTouch;
    public delegate void EndSecondaryTouch(Vector2 position, float time);
    public event EndSecondaryTouch OnEndSecondaryTouch;

    //------------------------------------------------------
    //                  COROUTINES
    //------------------------------------------------------

    public IEnumerator ControlledSetup() {
        inputControl = new InputControl();
        yield return new WaitUntil(() => GameManager.Instance != null);
        gameManager = GameManager.Instance;
        mainCamera = gameManager.GetComponentInChildren<Camera>();
    }

    //------------------------------------------------------
    //              STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        StartCoroutine(ControlledSetup());
    }

    private void OnEnable() {
        inputControl.TouchControls.PrimaryContact.Enable();
        inputControl.TouchControls.PrimaryPosition.Enable();
        inputControl.TouchControls.SecondaryContact.Enable();
        inputControl.TouchControls.SecondaryPosition.Enable();
    }

    private void Start() {
        inputControl.TouchControls.PrimaryContact.started += context => StartTouchPrimary(context);
        inputControl.TouchControls.PrimaryContact.canceled += context => EndTouchPrimary(context);
        
    }

    private void OnDisable() {
        inputControl.TouchControls.PrimaryContact.Disable();
        inputControl.TouchControls.PrimaryPosition.Disable();
        inputControl.TouchControls.SecondaryContact.Disable();
        inputControl.TouchControls.SecondaryPosition.Disable();
    }

    //------------------------------------------------------
    //              TOUCH FUNCTIONS
    //------------------------------------------------------

    private void StartTouchPrimary(InputAction.CallbackContext context) {
        if(OnStartPrimaryTouch != null) {
            OnStartPrimaryTouch(gameManager.ScreenToWorld(mainCamera, inputControl.TouchControls.PrimaryPosition.ReadValue<Vector2>()), (float)context.startTime);
        }
    }

    private void EndTouchPrimary(InputAction.CallbackContext context) {
        if(OnEndPrimaryTouch != null) {
            OnEndPrimaryTouch(gameManager.ScreenToWorld(mainCamera, inputControl.TouchControls.PrimaryPosition.ReadValue<Vector2>()), (float)context.time);
        }
    }

    public Vector2 PrimaryPosition() {
        return gameManager.ScreenToWorld(mainCamera, inputControl.TouchControls.PrimaryPosition.ReadValue<Vector2>());
    }

    private void StartTouchSecondary(InputAction.CallbackContext context) {
        if(OnStartSecondaryTouch != null) {
            OnStartSecondaryTouch(
                gameManager.ScreenToWorld(mainCamera, inputControl.TouchControls.SecondaryPosition.ReadValue<Vector2>()),
                (float)context.time
            );
        }
    }

    private void EndTouchSecondary(InputAction.CallbackContext context) {
        if(OnEndSecondaryTouch != null) {
            OnEndSecondaryTouch(
                gameManager.ScreenToWorld(mainCamera, inputControl.TouchControls.SecondaryPosition.ReadValue<Vector2>()),
                (float)context.time
            );
        }
    }

    public Vector2 SecondaryPosition() {
        return gameManager.ScreenToWorld(mainCamera, inputControl.TouchControls.SecondaryPosition.ReadValue<Vector2>());
    }

}
