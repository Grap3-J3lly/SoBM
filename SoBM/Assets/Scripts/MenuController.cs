using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

    private GameManager gameManager;
    private LevelManager levelManager;
    private GameObject player;

    public enum MenuType {
        Main,
        Credits,
        Options,
        LevelComplete,
        LevelSelect,
        GeneralHud,
        Pause,
        Backdrop
    }

    [SerializeField] private MenuType menuType;

    [SerializeField] private List<GameObject> possibleButtons = new List<GameObject>();
    [SerializeField] private List<GameObject> existingButtons = new List<GameObject>();

    [SerializeField] private GameObject buttonArea;

    private int levelCount = 1;
    [SerializeField] private int sceneOffset = 1;
    [SerializeField] private string specificLevelName = "Level ";

    [SerializeField] private Vector2 anchorMaxOffset;
    [SerializeField] private Vector2 anchorMinOffset;

    [SerializeField] private Vector2 positionMaxOffset;
    [SerializeField] private Vector2 positionMinOffset;


    //------------------------------------------------------
    //               GETTERS/SETTERS
    //------------------------------------------------------

    public MenuType GetMenuType() {return menuType;}
    public void SetMenuType(MenuType newType) {menuType = newType;}

    public Vector2 GetAnchorMaxOffset() {return anchorMaxOffset;}
    public void SetAnchorMaxOffset(Vector2 newOffset) {anchorMaxOffset = newOffset;}

    public Vector2 GetAnchorMinOffset() {return anchorMinOffset;}
    public void SetAnchorMinOffset(Vector2 newOffset) {anchorMinOffset = newOffset;}

    public Vector2 GetPositionMaxOffset() {return positionMaxOffset;}
    public void SetPositionMaxOffset(Vector2 newOffset) {positionMaxOffset = newOffset;}

    public Vector2 GetPositionMinOffset() {return positionMinOffset;}
    public void SetPositionMinOffset(Vector2 newOffset) {positionMinOffset = newOffset;}

    //------------------------------------------------------
    //               STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        gameManager = GameManager.Instance;
        player = gameManager.GetPlayerObject();
        HandleButtonSpawn();
        

    }

    private void Start() {
        //GetInfoSection();
        // GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        // GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        // transform.localScale = new Vector3(1, 1, 1);
    }

    //------------------------------------------------------
    //               GENERAL FUNCTIONS
    //------------------------------------------------------

    // public void LoadMenu(MenuType desiredMenu) {
    //     if(this.menuType == MenuType.Backdrop && desiredMenu != MenuType.GeneralHud) {
    //         gameObject.SetActive(true);
    //         return;
    //     }
        
    //     if(this.menuType == desiredMenu) {
    //         gameObject.SetActive(true);
    //     }
    //     else {
    //         gameObject.SetActive(false);
    //     }
    // }

    private void HandleButtonSpawn() {
        if(menuType == MenuType.LevelSelect) {
            DetermineButtonCount();
            foreach(GameObject button in possibleButtons) {
                SpawnButton(button);
            }
        }
    }

    private void DetermineButtonCount() {
        levelCount = gameManager.GetScenes().Count - sceneOffset;
    }

    private void SpawnButton(GameObject button) {
        if(button.GetComponent<ButtonController>().GetButtonType() == ButtonController.ButtonType.SpecificLevelStart) {
            SpawnLevelButton(button);
        }
    }

    private void SpawnLevelButton(GameObject button) {
        for(int index = 1; index <= levelCount; index++) {
            // Creation
             GameObject newButton = (GameObject)Instantiate(button, buttonArea.transform, false);

            // Data Assignment
             newButton.name = specificLevelName + (index);
             AssignNewButtonText(newButton, index);
             newButton.transform.SetParent(buttonArea.transform);
             newButton.GetComponent<ButtonController>().SetLevelNum(index);
             existingButtons.Add(newButton);

            // Location
             if(index > 0) {
                RectTransform tempTransform = newButton.GetComponent<RectTransform>();
                tempTransform.offsetMax += (index * positionMaxOffset);
                tempTransform.offsetMin += (index * positionMinOffset);
             }
            }
    }

    private GameObject SearchPossibleButtons(ButtonController.ButtonType buttonType) {
        return possibleButtons.Find(specificButton => specificButton.GetComponent<ButtonController>().GetButtonType() == buttonType);
    }

    private GameObject SearchExistingButtons(ButtonController.ButtonType buttonType) {
        return existingButtons.Find(specificButton => specificButton.GetComponent<ButtonController>().GetButtonType() == buttonType);
    }

    private void AssignNewButtonText(GameObject newButton, int appendNum) {
        TMP_Text textArea = newButton.GetComponentInChildren<TMP_Text>();
        textArea.text = specificLevelName + appendNum;
    }

    // private void GetInfoSection() {
    //     if(this.menuType == MenuType.GeneralHud) {
    //         Transform tempTransform = GetComponent<RectTransform>();
    //         int childCount = tempTransform.childCount;
    //         for(int index = 0; index < childCount; index++) {
    //             if(tempTransform.GetChild(index).gameObject.name == "Info") {
    //                 GetTextSections(tempTransform.GetChild(index));
    //             }
    //         }
    //     }
    // }

    // private void GetTextSections(Transform parentTransform) {
        
    // }

}
