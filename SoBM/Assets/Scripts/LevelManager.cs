using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class LevelManager : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

    //public static LevelManager Instance;

    private GameManager gameManager;

    public List<GameObject> levelRequirements = new List<GameObject>();
    private int requirementCount = 0;
    private bool levelComplete = false;
    [SerializeField] private int levelNumber;
    [SerializeField] private AudioSource levelMusicSource;

    [SerializeField] private GameObject exitDoor;

    [SerializeField] private Vector3 cameraControlLocation;

    [SerializeField] private Vector3 startLocation;

    [SerializeField] private GameObject enemyNPC;
    [SerializeField] private Vector3 enemySpawnLocation;

    //------------------------------------------------------
    //                  GETTERS/SETTERS
    //------------------------------------------------------

    public List<GameObject> GetLevelRequirementList() {return levelRequirements;}
    public void SetLevelRequirementList(List<GameObject> newRequirements) {levelRequirements = newRequirements;}

    public int GetRequirementCount() {return requirementCount;}
    public void SetRequirementCount(int newCount) {requirementCount = newCount;}

    public int GetLevelNumber() {return levelNumber;}
    public void SetLevelNumber(int newNum) {levelNumber = newNum;}

    public bool GetLevelComplete() {return levelComplete;}
    public void SetLevelComplete(bool newValue) {levelComplete = newValue;}

    public GameObject GetEnemy() {return enemyNPC;}
    public void SetEnemy(GameObject newEnemy) {enemyNPC = newEnemy;}

    public Vector3 GetEnemySpawnLocation() {return enemySpawnLocation;}
    public void SetEnemySpawnLocation(Vector3 newLoc) {enemySpawnLocation = newLoc;}

    //------------------------------------------------------
    //                  COROUTINES
    //------------------------------------------------------

    private IEnumerator MovePlayerToSpawn() {
        yield return new WaitForEndOfFrame();
        Debug.Log("Player Position:" + gameManager.GetPlayerObject().transform.position);
        Debug.Log("Start Location:" + startLocation);
        gameManager.GetPlayerObject().transform.position = startLocation;
        Debug.Log("Player Position:" + gameManager.GetPlayerObject().transform.position);
        
    }

    //------------------------------------------------------
    //                  STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        
        
    }

    private void Start() {
        HandleSetup();
    }

    

    //------------------------------------------------------
    //                  SETUP FUNCTIONS
    //------------------------------------------------------

    private void HandleSetup() {
        HandleGameManagerSetup();
        HandleLevelSetup();
        UpdateMenu(true, MenuController.MenuType.GeneralHud);
        HandleCharacterSetup();
    }

    private void HandleGameManagerSetup() {
        gameManager = GameManager.Instance;
        gameManager.GetLevels().Add(this);
        gameManager.SetCurrentLevelNum(levelNumber);
        gameManager.HandleMusicSource(levelMusicSource);
    }

    private void HandleLevelSetup() {
        exitDoor.GetComponent<InteractableController>().SetActiveState(false);
        foreach(GameObject requirement in levelRequirements) {
            requirement.GetComponent<InteractableController>().SetActiveState(false);
            requirementCount++;
        }
    }

    private void HandleCharacterSetup() {
        gameManager.GetPlayerObject().SetActive(true);
        //gameManager.GetPlayerObject().transform.position = startLocation;
        StartCoroutine(MovePlayerToSpawn());
        gameManager.GetPlayerObject().GetComponentInChildren<InventoryManager>().LocateButtonLocation();

        enemyNPC = gameManager.GetEnemies()[levelNumber - 1];
        enemyNPC.SetActive(true);
        enemyNPC.transform.position = enemySpawnLocation;
    }

    //------------------------------------------------------
    //                  GENERAL FUNCTIONS
    //------------------------------------------------------

    private void UpdateMenu(bool startPlaying, MenuController.MenuType thisMenuType) {
        gameManager.ChangeMenu(thisMenuType);
        if(startPlaying) {
            gameManager.ShiftToGame();
        } 
        else {
            gameManager.ShiftToMenu();
        }
        
    }

    public void HandleLevelComplete() {
        if(levelComplete) {
            MenuController.MenuType winType = MenuController.MenuType.LevelComplete;

            UpdateMenu(false, winType);
            gameManager.ChangeMenu(winType);
        }
    }

}
