using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

    //public static LevelManager Instance;

    private GameManager gameManager;

    public List<GameObject> levelRequirements = new List<GameObject>();
    private int requirementCount = 0;

    [SerializeField] private GameObject exitDoor;

    //------------------------------------------------------
    //                  GETTERS/SETTERS
    //------------------------------------------------------

    public List<GameObject> GetLevelRequirementList() {return levelRequirements;}
    public void SetLevelRequirementList(List<GameObject> newRequirements) {levelRequirements = newRequirements;}

    public int GetRequirementCount() {return requirementCount;}
    public void SetRequirementCount(int newCount) {requirementCount = newCount;}

    //------------------------------------------------------
    //                  STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        //Instance = this;
        gameManager = GameManager.Instance;
        StartCoroutine(gameManager.Reset(gameManager.GetBaseResetTime()));
        gameManager.GetLevels().Add(this);
        HandleLevelSetup();
    }

    //------------------------------------------------------
    //                  SETUP FUNCTIONS
    //------------------------------------------------------

    private void HandleLevelSetup() {
        exitDoor.GetComponent<InteractableController>().SetActiveState(false);
        foreach(GameObject requirement in levelRequirements) {
            requirement.GetComponent<InteractableController>().SetActiveState(false);
            requirementCount++;
        }
    }

}
