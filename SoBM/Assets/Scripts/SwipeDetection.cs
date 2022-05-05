using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeDetection : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

    private GameManager gameManager;

    private InputManager inputManager;

    [SerializeField] private float minimumDistance = .2f;
    [SerializeField] private float maximumTime = 1f;

    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;

    //------------------------------------------------------
    //              STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        HandleSetup();
    }
    
    private void OnEnable() {
        // inputManager.OnStartTouch += SwipeStart;
        // inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable() {
        // inputManager.OnStartTouch -= SwipeStart;
        // inputManager.OnEndTouch -= SwipeEnd;
    }

    //------------------------------------------------------
    //                  SETUP FUNCTIONS
    //------------------------------------------------------

    private void HandleSetup() {
        gameManager = GameManager.Instance;
        inputManager = gameManager.GetInputManager();
    }

    //------------------------------------------------------
    //                  SWIPE FUNCTIONS
    //------------------------------------------------------

    private void SwipeStart(Vector2 position, float time) {
        startPosition = position;
        startTime = time;
    }

    private void SwipeEnd(Vector2 position, float time) {
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe() {
        if(Vector3.Distance(startPosition, endPosition) >= minimumDistance &&
        (endTime - startTime) <= maximumTime) {
            Debug.Log("Swipe Detected");
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
        }
    }



}
