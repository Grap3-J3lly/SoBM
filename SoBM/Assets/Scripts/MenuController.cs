using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

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

    private GameManager gameManager;
    private LevelManager levelManager;
    private GameObject player;
    //private InputController inputController;

    //------------------------------------------------------
    //               GETTERS/SETTERS
    //------------------------------------------------------

    public MenuType GetMenuType() {return menuType;}
    public void SetMenuType(MenuType newType) {menuType = newType;}

    //------------------------------------------------------
    //               STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        gameManager = GameManager.Instance;
        player = gameManager.GetPlayerObject();

    }

    private void Start() {
        //GetInfoSection();

    }

    //------------------------------------------------------
    //               GENERAL FUNCTIONS
    //------------------------------------------------------

    public void LoadMenu(MenuType desiredMenu) {
        if(this.menuType == MenuType.Backdrop && desiredMenu != MenuType.GeneralHud) {
            gameObject.SetActive(true);
            return;
        }
        
        if(this.menuType == desiredMenu) {
            gameObject.SetActive(true);
        }
        else {
            gameObject.SetActive(false);
        }
    }

    public void ActivateMenu(MenuType showThisType) {
        if(this.menuType == showThisType) {
           gameObject.SetActive(true) ;
        }
    }
    public void DeactivateMenu(MenuType hideThisType) {
        if(this.menuType == hideThisType) {
            gameObject.SetActive(false) ;
        }
    }

    private void GetInfoSection() {
        if(this.menuType == MenuType.GeneralHud) {
            Transform tempTransform = GetComponent<RectTransform>();
            int childCount = tempTransform.childCount;
            for(int index = 0; index < childCount; index++) {
                if(tempTransform.GetChild(index).gameObject.name == "Info") {
                    GetTextSections(tempTransform.GetChild(index));
                }
            }
        }
    }

    private void GetTextSections(Transform parentTransform) {
        
    }

}
