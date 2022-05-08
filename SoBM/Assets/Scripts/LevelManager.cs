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

    [SerializeField] private GameObject exitDoor;

    [SerializeField] private Vector3 startLocation;

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

    //------------------------------------------------------
    //                  STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        HandleSetup();
        
    }

    //------------------------------------------------------
    //                  SETUP FUNCTIONS
    //------------------------------------------------------

    private void HandleSetup() {
        HandleGameManagerSetup();
        HandleLevelSetup();
        UpdateMenu(true, MenuController.MenuType.GeneralHud);
        HandlePlayerSetup();
    }

    private void HandleGameManagerSetup() {
        gameManager = GameManager.Instance;
        gameManager.GetLevels().Add(this);
        gameManager.SetCurrentLevelNum(levelNumber);
    }

    private void HandleLevelSetup() {
        exitDoor.GetComponent<InteractableController>().SetActiveState(false);
        foreach(GameObject requirement in levelRequirements) {
            requirement.GetComponent<InteractableController>().SetActiveState(false);
            requirementCount++;
        }
    }

    private void HandlePlayerSetup() {
        gameManager.GetPlayerObject().transform.position = startLocation;
        gameManager.GetPlayerObject().GetComponentInChildren<InventoryManager>().LocateButtonLocation();
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
