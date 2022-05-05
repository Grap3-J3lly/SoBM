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
        WinScreen,
        LoseScreen,
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
        levelManager = gameManager.GetLevels()[gameManager.GetCurrentLevelNum()];
        player = gameManager.GetPlayerObject();
        CreateMenuList();
    }

    private void Start() {
        GetInfoSection();
    }

    //------------------------------------------------------
    //               GENERAL FUNCTIONS
    //------------------------------------------------------

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

    private void CreateMenuList() {
        if(menuType != MenuType.Backdrop) {
            gameManager.GetMenuList().Add(gameObject);
        }
        else {
            ProvideBackdrop();
        }
    }

    private void ProvideBackdrop() {
        gameManager.SetBackdrop(gameObject);
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
