using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------
    
    // General
    private GameManager gameManager;
    private LevelManager levelManager;
    private GameObject playerObject;
    [SerializeField] private InventoryManager inventoryManager;

    private List<GameObject> levelRequirements = new List<GameObject>();

    private GameObject currentTarget;
    private bool nearTarget = false;
    [SerializeField] private bool maintainInventory = false;

    [SerializeField] private float delay = 1f;

    // Movement Focus

    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float rotationSpeed = 100f;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;

    [SerializeField] private Vector2 rotationTimeRange = new Vector2(1, 3);
    [SerializeField] private Vector2 rotationWaitRange = new Vector2(1, 4);

    [SerializeField] private Vector2 walkingTimeRange = new Vector2(1, 3);
    [SerializeField] private Vector2 walkingWaitRange = new Vector2(1, 4); 

    // React Focus

    // Interact Focus
    
    private GameObject selectedItem;

    // Pursue Focus

    public enum Behaviors {
        React,
        Interact,
        Pursue
    }

    [SerializeField] private Behaviors behavior;

    //------------------------------------------------------
    //                  GETTERS/SETTERS
    //------------------------------------------------------

    public InventoryManager GetInventoryManager() {return inventoryManager;}
    public void SetInventoryManager(InventoryManager newManager) {inventoryManager = newManager;}

    //------------------------------------------------------
    //                  COROUTINES
    //------------------------------------------------------

    public IEnumerator Wander() {
        int rotationTime = gameManager.GenerateFromCurrentState((int)rotationTimeRange.x, (int)rotationTimeRange.y);
        int rotationWaitPeriod = gameManager.GenerateFromCurrentState((int)rotationWaitRange.x, (int)rotationWaitRange.y);
        
        // RotationDirection only has two states, doesn't need a specific range
        int rotationDirection = gameManager.GenerateFromCurrentState(0, 3);
        
        int walkingTime = gameManager.GenerateFromCurrentState((int)walkingTimeRange.x, (int)walkingTimeRange.y);
        int walkingWaitPeriod = gameManager.GenerateFromCurrentState((int)walkingWaitRange.x, (int)walkingWaitRange.y);

        isWandering = true;

        yield return new WaitForSeconds(walkingWaitPeriod);
        isWalking = true;
        yield return new WaitForSeconds(walkingTime);
        isWalking = false;
        yield return new WaitForSeconds(rotationWaitPeriod);
        if(rotationDirection == 1) {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingRight = false;
        }
        if(rotationDirection == 2) {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingLeft = false;
        }

        isWandering = false;
        
    }

    public IEnumerator TargetRequirement() {
        yield return new WaitForSeconds(delay);

        Debug.Log("Determining Target based on Behavior");

        if(behavior == Behaviors.React) {
            DetermineTargetReact();
        }
        if(behavior == Behaviors.Interact) {
            DetermineTargetInteract();
        }
        if(behavior == Behaviors.Pursue) {
            DetermineTargetPursuit();
        }
        Debug.Log("Current Target: " + currentTarget);
    }

    public IEnumerator PausePursuit() {
        yield return new WaitForSeconds(delay);
        nearTarget = false;
    }

    public IEnumerator StorePlayerObject() {
        Debug.Log("Grabbing Player Object");
        if(gameManager.GetPlayerObject() == null) {
            yield return new WaitForEndOfFrame();
        }
        playerObject = gameManager.GetPlayerObject();
        Debug.Log("Grabbing Player Object: " + playerObject);
    }

    //------------------------------------------------------
    //                  STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Start() {
        HandleInitialSetup();
    }

    private void Update() {
        if(currentTarget == null) {
            HandleWanderMovement();
            if(behavior == Behaviors.Interact) {
                CheckInventory();
            }
        }
        else {
            ApproachTarget();
            if(behavior == Behaviors.Pursue && nearTarget) {
                StartCoroutine(PausePursuit());
            }
        }
        
    }

    //------------------------------------------------------
    //                  COLLISION FUNCTIONS
    //------------------------------------------------------

    private void OnCollisionEnter(Collision info) {
        if(info.gameObject.tag == "Boundary") {
            TurnAround();
        }

        if(currentTarget != null && info.gameObject == currentTarget) {
            nearTarget = true;
            isWalking = false;
        }

        if(info.gameObject.tag == "Wall") {
            info.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnCollisionExit(Collision info) {
        if(info.gameObject.tag == "Wall" && info.gameObject.GetComponent<BoxCollider>().enabled == false) 
        {
            info.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }

    //------------------------------------------------------
    //                  SETUP FUNCTIONS
    //------------------------------------------------------

    private void HandleInitialSetup() {
        Debug.Log("InitialSetup");
        gameManager = GameManager.Instance;
        levelManager = gameManager.GetLevels()[0];
        levelRequirements = levelManager.GetLevelRequirementList();
        StartCoroutine(StorePlayerObject());
        HandleBehaviorSetup();
    }

    private void HandleBehaviorSetup() {
        if(behavior == Behaviors.React) {
            playerObject.GetComponent<PlayerController>().onActivationEvent += HandleBehaviorChange;
        }
        if(behavior == Behaviors.Pursue) {
            HandleBehaviorChange();
        }
    }

    //------------------------------------------------------
    //                  GENERAL FUNCTIONS
    //------------------------------------------------------

    private void HandleBehaviorChange() {
        if(behavior == Behaviors.React) {
            StartCoroutine(TargetRequirement());
        }
        if(behavior == Behaviors.Pursue) {
            Debug.Log("Determined Pursuit behavior");
            float storeDelay = delay;
            StartCoroutine(TargetRequirement());
            delay = delay * 5;
            StartCoroutine(PausePursuit());
            delay = storeDelay;
        }
    }

    public void HandleWanderMovement() {
        if(!isWandering) {
            StartCoroutine(Wander());
        }
        if(isRotatingRight) {
            transform.Rotate(transform.up * Time.deltaTime * rotationSpeed);
        }
        if(isRotatingLeft) {
            transform.Rotate(transform.up * Time.deltaTime * -rotationSpeed);
        }
        if(isWalking) {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
    }

    private void TurnAround() {
        transform.Rotate(0, 180f, 0, Space.World);
    }

    private void ApproachTarget() {
        transform.LookAt(currentTarget.transform);
        Vector3 currentRotation = transform.eulerAngles;

        Quaternion idealAngle = Quaternion.Euler(0, currentRotation.y, 0);
        transform.rotation = idealAngle;

        if(!nearTarget) {
            isWalking = true;
        }
        
        if(isWalking) {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
    }

    public void HandleInteraction(GameObject interactionOrigin, bool triggered) {
        InteractableController tempController = interactionOrigin.GetComponent<InteractableController>();
        if(!maintainInventory && tempController.GetInteractType() == InteractableController.InteractableType.Block) {
            return;
        }
        // Ignore other interactables
        if((currentTarget != null && currentTarget != interactionOrigin) || currentTarget == null && behavior != Behaviors.Pursue) {
            return;
        }
        if(triggered) {
            tempController.HandleInteraction(inventoryManager);
        }
        else {

        }
    }

    //------------------------------------------------------
    //                  REACT FUNCTIONS
    //------------------------------------------------------

    private void DetermineTargetReact() {
        levelRequirements = levelManager.GetLevelRequirementList();

        foreach(GameObject requirement in levelRequirements) {
            bool currentState = requirement.GetComponent<InteractableController>().GetActiveState();
            if(!currentState) {
                currentTarget = requirement;
                return;
            }
        }
        currentTarget = null;
    }

    //------------------------------------------------------
    //                  INTERACT FUNCTIONS
    //------------------------------------------------------
    private void DetermineTargetInteract() {
        levelRequirements = levelManager.GetLevelRequirementList();

        foreach(GameObject requirement in levelRequirements) {
            bool isButton = requirement.GetComponent<InteractableController>().GetInteractType() == InteractableController.InteractableType.Button;
            bool isActive = requirement.GetComponent<InteractableController>().GetActiveState();

            //Debug.Log("IsButton: " + isButton + ". IsActive: " + isActive);

            if(isButton && !isActive && selectedItem != null)
            {
                currentTarget = requirement;
                return;
            }
        }
        currentTarget = null;

    }

    private void CheckInventory() {
        List<GameObject> currentInventory = inventoryManager.GetInventory();

        if(currentInventory.Count != 0) {
            foreach(GameObject item in currentInventory) {
                if(item.GetComponent<InteractableController>().GetInteractType() == InteractableController.InteractableType.Block) 
                {
                    selectedItem = item;
                    StartCoroutine(TargetRequirement());
                    return;
                }
            }
        }
    }

    //------------------------------------------------------
    //                  PURSUE FUNCTIONS
    //------------------------------------------------------

    private void DetermineTargetPursuit() {
        Debug.Log("Determined Pursuit Behavior, targetting Player");
        currentTarget = playerObject;
    }

}
