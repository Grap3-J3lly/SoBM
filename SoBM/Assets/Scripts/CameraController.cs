using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

    private GameManager gameManager;
    
    private InputManager inputManager;
    
    [SerializeField] private GameObject mainCamObject;
    // private Camera mainCamera;

    private Vector2 primaryStartPosition;
    private float primaryStartTime;
    private Vector2 primaryEndPosition;
    private float primaryEndTime;

    private Vector2 secondaryStartPosition;
    private float secondaryStartTime;
    private Vector2 secondaryEndPosition;
    private float secondaryEndTime;

    //------------------------------------------------------
    //                  COROUTINES
    //------------------------------------------------------

    public IEnumerator ControlledSetup() {
        // mainCamera = GetComponentInChildren<Camera>();
        yield return new WaitUntil(() => GameManager.Instance != null);
        gameManager = GameManager.Instance;
        inputManager = gameManager.GetInputManager();
        
        inputManager.OnStartPrimaryTouch += PrimaryTouchStart;
        inputManager.OnEndPrimaryTouch += PrimaryTouchEnd;
        inputManager.OnStartSecondaryTouch += SecondaryTouchStart;
        inputManager.OnEndSecondaryTouch += SecondaryTouchEnd;
    }

    //------------------------------------------------------
    //              STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        StartCoroutine(ControlledSetup());
    }

    private void OnEnable() {
        
    }

    private void Start() {

    }

    private void OnDisable() {
        inputManager.OnStartPrimaryTouch -= PrimaryTouchStart;
        inputManager.OnEndPrimaryTouch -= PrimaryTouchEnd;
        inputManager.OnStartSecondaryTouch -= SecondaryTouchStart;
        inputManager.OnEndSecondaryTouch -= SecondaryTouchEnd;
    }

    private void PrimaryTouchStart(Vector2 position, float time) {
        primaryStartPosition = position;
        primaryStartTime = time;
    }

    private void PrimaryTouchEnd(Vector2 position, float time) {
        primaryEndPosition = position;
        primaryEndTime = time;
    }

    private void SecondaryTouchStart(Vector2 position, float time) {
        secondaryStartPosition = position;
        secondaryStartTime = time;
        DragBegin();
    }

    private void SecondaryTouchEnd(Vector2 position, float time) {
        secondaryEndPosition = position;
        secondaryEndTime = time;
        DragEnd();
    }

    private void DragBegin() {

    }

    private void DragEnd() {
        Vector3 secondaryDragDistance = new Vector3(
            transform.eulerAngles.x + (secondaryEndPosition.x - secondaryStartPosition.x),
            transform.eulerAngles.y + (secondaryEndPosition.y - secondaryStartPosition.y),
            transform.eulerAngles.z
        );

        transform.rotation = Quaternion.Euler(secondaryDragDistance);

    }

    //------------------------------------------------------
    //              ROTATION FUNCTIONS
    //------------------------------------------------------

    // private void OnRotate(InputAction.CallbackContext context) {
    //     Debug.Log("Activated OnRotate: " + context.ReadValue<float>());
        
    // }

    //------------------------------------------------------
    //              ZOOM FUNCTIONS
    //------------------------------------------------------

    // private void OnZoom(InputAction.CallbackContext context) {
    //     Debug.Log("Activated OnZoom: " + context.ReadValue<float>());
    // }

}
