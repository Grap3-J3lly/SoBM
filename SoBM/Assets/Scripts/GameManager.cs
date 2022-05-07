using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

    // General Focus
    public static GameManager Instance;

    [SerializeField] InputManager inputManager;

    [SerializeField] private List<LevelManager> levels = new List<LevelManager>();

    [SerializeField] private Vector3 currentLevelSpawnpoint;

    [SerializeField] private float baseResetTime = 2.5f;
    [SerializeField] private int currentLevelNum = 0;

    [SerializeField] private GameObject playerObject;

    private bool readyToSpawn = false;
    private bool gameComplete = false;

    // Random Gen Focus
    [SerializeField] private int initialSeed = 1;
    [SerializeField] private int initialLowRange = 0;
    [SerializeField] private int initialHighRange = 999;
    private Random.State currentState;

    // Menu Focus
    [SerializeField] private List<GameObject> availableMenus = new List<GameObject>();
    private List<GameObject> menuList = new List<GameObject>();
    [SerializeField] private bool inMenu;
    [SerializeField] private MenuController.MenuType startUpMenuType;
    [SerializeField] private GameObject menuSection;
    private GameObject previousMenu;
    private GameObject currentMenu;
    

    // Scene Focus
    [SerializeField] private int finalLevelIndex = 3;
    private int gameplaySceneIndex = 0;
    private int currentAreaInitialSceneIndex = 1;
    private int currentSceneIndex = 0;
    private bool sceneLoaded = false;
    private List<string> scenes = new List<string>() {
        "GameManager",
        "L1",
        "L2",
        "L3"
    };

    //------------------------------------------------------
    //              GETTERS/SETTERS
    //------------------------------------------------------

    // General Focus
    public InputManager GetInputManager() {return inputManager;}
    public void SetInputManager(InputManager newManager) {inputManager = newManager;}

    public List<LevelManager> GetLevels() {return levels;}
    public void SetLevels(List<LevelManager> newList) {levels = newList;}

    public float GetBaseResetTime() {return baseResetTime;}
    public void SetBaseResetTime(float newResetTime) {baseResetTime = newResetTime;}

    public int GetCurrentLevelNum() {return currentLevelNum;}
    public void SetCurrentLevelNum(int newLevelNum) {currentLevelNum = newLevelNum;}

    public GameObject GetPlayerObject() {return playerObject;}
    public void SetPlayerObject(GameObject newPlayer) {playerObject = newPlayer;}

    // Random Gen Focus
    public int GetInitialSeed() {return initialSeed;}
    public void SetInitialSeed(int newValue) {initialSeed = newValue;}

    public int GetInitialLowRange() {return initialLowRange;}
    public void SetInitialLowRange(int newValue) {initialLowRange = newValue;}

    public int GetInitialHighRange() {return initialHighRange;}
    public void SetInitialHighRange(int newValue) {initialHighRange = newValue;}

    // Menu Focus

    public List<GameObject> GetMenuList() {return menuList;}
    public void SetMenuList(List<GameObject> newList) {menuList = newList;}

    //------------------------------------------------------
    //              STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        Instance = this;
        Random.InitState(initialSeed);
        currentState = Random.state;
    }

    private void Start() {
        HandleSetup();
    }

    //------------------------------------------------------
    //              SETUP FUNCTIONS
    //------------------------------------------------------

    private void HandleSetup() {
        HandleMenuSetup();
    }

    private  void HandleMenuSetup() {
        LoadMenus();
        CreateMenu(SearchMenus(MenuController.MenuType.Backdrop));
        currentMenu = CreateMenu(SearchMenus(startUpMenuType));
        
    }

    //------------------------------------------------------
    //              RANDOMIZING FUNCTIONS
    //------------------------------------------------------

    public int GenerateFromSeed(int seed, int minRange, int maxRange) {
        Random.InitState(initialSeed);

        return Random.Range(minRange, maxRange);

    }

    public int GenerateFromCurrentState(int minRange, int maxRange) {
        Random.state = currentState;
        int generatedNum = Random.Range(minRange, maxRange);
        UpdateCurrentState();
        return generatedNum;
    }

    public void UpdateCurrentState() {
        currentState = Random.state;
    }

    //------------------------------------------------------
    //                   MENU FUNCTIONS
    //------------------------------------------------------

    private GameObject CreateMenu(GameObject newMenu) {
        GameObject spawnedMenu = (GameObject)Instantiate(newMenu, menuSection.transform.position, Quaternion.identity);
        spawnedMenu.transform.SetParent(menuSection.transform);
        menuList.Add(spawnedMenu);
        return spawnedMenu;
    }

    private void LoadMenus() {

        availableMenus = new List<GameObject>() {
            // Credits Menu
            Resources.Load<GameObject>("Prefabs/UI/Menus/Credits"),

            // General HUD
            Resources.Load<GameObject>("Prefabs/UI/Menus/GeneralHUD"),

            // Level Complete
            Resources.Load<GameObject>("Prefabs/UI/Menus/LevelComplete"),

            // Level Select
            Resources.Load<GameObject>("Prefabs/UI/Menus/LevelSelection"),

            // Main Menu
            Resources.Load<GameObject>("Prefabs/UI/Menus/MainMenu"),

            // Backdrop
            Resources.Load<GameObject>("Prefabs/UI/Menus/MenuBackdrop"),

            // Options
            Resources.Load<GameObject>("Prefabs/UI/Menus/Options"),

            // Pause Menu
            Resources.Load<GameObject>("Prefabs/UI/Menus/PauseMenu")
        };

    }

    private GameObject SearchMenus(MenuController.MenuType theMenuType) {
        return availableMenus.Find(specificMenu => specificMenu.GetComponent<MenuController>().GetMenuType() == theMenuType);
    }

    private void ChangeMenu(GameObject thisMenu) {
        currentMenu.GetComponent<MenuController>().DeactivateMenu(currentMenu.GetComponent<MenuController>().GetMenuType());
        thisMenu.GetComponent<MenuController>().ActivateMenu(thisMenu.GetComponent<MenuController>().GetMenuType());
        previousMenu = currentMenu;
        currentMenu = thisMenu;
    }

    // Need to redesign this so it's based off method call to ButtonController for the specific button Types
    // In button controller - HandleButtonPress, then depending on type, call specific functions
    public void ChangeMenuByButton(ButtonController buttonPressed) {
        switch(buttonPressed.GetButtonType()) {
            // "Play" goes to Level Select. Specific level is chosen by SpecificLevelStart
            case ButtonController.ButtonType.Play :
                ChangeMenu(SearchMenus(MenuController.MenuType.LevelSelect));
            break;
            // Options
            case ButtonController.ButtonType.Options : 
                ChangeMenu(SearchMenus(MenuController.MenuType.Options));
                break;
            // Credits
            case ButtonController.ButtonType.Credits :
                ChangeMenu(SearchMenus(MenuController.MenuType.Credits));
                break;
            // Back (PreviousMenu)
            case ButtonController.ButtonType.Back : 
                HandleBackButton();
                break;
            // Start Specific Level / Next Level
            case ButtonController.ButtonType.SpecificLevelStart :
                HandleSpecificLevel(buttonPressed);
                break;
            case ButtonController.ButtonType.NextLevel :
                HandleNextLevel();
                break;
            // Main Menu
            case ButtonController.ButtonType.MainMenu : 
                HandleMainMenuChange();
                break;
            // Pause Menu
            case ButtonController.ButtonType.Pause : 
                HandlePause();
                break;
            // Restart Level
            case ButtonController.ButtonType.Restart :
                RestartGame();
                break;
            case ButtonController.ButtonType.Resume : 
                HandleResume();
                break;
        }
    }

    //------------------------------------------------------
    //                  SCENE FUNCTIONS
    //------------------------------------------------------

    private int ResetSceneToStartScene(int nextScene) {
        if(nextScene == scenes.Count) {
            nextScene = currentAreaInitialSceneIndex;
        }
        return nextScene;
    }

    private void ChangeScene(int nextScene) {
        if(!sceneLoaded) {

            nextScene = ResetSceneToStartScene(nextScene);

            if(currentSceneIndex != gameplaySceneIndex) {
                UnloadScene(scenes[currentSceneIndex]);
            }
            LoadScene(scenes[nextScene]);
            currentSceneIndex = nextScene;
            SceneManager.sceneLoaded += LevelStartSetup;
        }  
    }

    private void LoadScene(string sceneToLoadName) {
        SceneManager.LoadSceneAsync(sceneToLoadName, LoadSceneMode.Additive);
    }
    private void UnloadScene(string sceneToUnloadName) {
        SceneManager.UnloadSceneAsync(sceneToUnloadName);
    }

    //------------------------------------------------------
    //                  GAME FUNCTIONS
    //------------------------------------------------------

    private void HandleMainMenuChange() {
        ChangeMenu(SearchMenus(MenuController.MenuType.Main));
        if(SceneManager.sceneCount > 1) {
            UnloadScene(scenes[currentSceneIndex]);
            currentSceneIndex = 0;
        }
    }

    private void HandleResume() {
        inMenu = false;
        ChangeMenu(SearchMenus(MenuController.MenuType.GeneralHud)); 
        Time.timeScale = 1;
    }

    // Documented reference for better way to pause game, need to implement
    private void HandlePause() {
        inMenu = true;
        ChangeMenu(SearchMenus(MenuController.MenuType.Pause));
        Time.timeScale = 0;
    }

    private void HandleBackButton() {
        if(previousMenu.GetComponent<MenuController>().GetMenuType() == MenuController.MenuType.GeneralHud) {
            HandleResume();
        }
        ChangeMenu(previousMenu);
    }
        
    public void HandleLevelCompletion() {
        if(currentSceneIndex == finalLevelIndex) {
            gameComplete = true;
        }
        inMenu = true;
        
    }

    private void StartLevel(int nextLevelIndex) {
        sceneLoaded = false;
        ChangeScene(nextLevelIndex);
        inMenu = false;
        if(Time.timeScale == 0) {
            Time.timeScale = 1;
        }
    }

    private void HandleSpecificLevel(ButtonController button) {
        int nextLevelIndex = button.GetToLevelNumber();
        StartLevel(nextLevelIndex);
    }

    private void HandleNextLevel() {
        int nextLevelIndex = currentSceneIndex + 1;
        StartLevel(nextLevelIndex);
    }

    private void TogglePlayer(bool status) {
        playerObject.gameObject.SetActive(status);
    }

    private void LevelStartSetup(Scene scene, LoadSceneMode mode) {
        readyToSpawn = true;
        TogglePlayer(readyToSpawn);
        playerObject.transform.position = currentLevelSpawnpoint;
        ChangeMenu(SearchMenus(MenuController.MenuType.GeneralHud));
        sceneLoaded = true;
    }

    public void RestartGame() {
        sceneLoaded = false;
        gameComplete = false;
        ChangeScene(currentAreaInitialSceneIndex);
        HandleResume();
        inMenu = false;
        ChangeMenu(SearchMenus(MenuController.MenuType.GeneralHud));
    }

    //------------------------------------------------------
    //                  UTILITY FUNCTIONS
    //------------------------------------------------------    

    public Vector3 ScreenToWorld(Camera camera, Vector3 position) {
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);
    }

}
