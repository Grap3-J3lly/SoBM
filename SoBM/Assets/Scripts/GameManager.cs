using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

    // General Focus
    public static GameManager Instance;

    private static InputControl inputControl;

    [SerializeField] private List<LevelManager> levels = new List<LevelManager>();

    [SerializeField] private Vector3 currentLevelSpawnpoint;

    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    [SerializeField] private float baseResetTime = 2.5f;
    [SerializeField] private int currentLevelNum = 0;
    private int previousLevelNum = 0;

    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject cameraControlArea;

    private bool readyToSpawn = false;
    private bool gameComplete = false;

    // Random Gen Focus
    [SerializeField] private int initialSeed = 1;
    [SerializeField] private int initialLowRange = 0;
    [SerializeField] private int initialHighRange = 999;
    private Random.State currentState;

    // Audio Focus
    [SerializeField] private AudioSource generalMusicSource;
    private AudioSource currentMusicSource;
    private AudioManager audioManager;
    private AudioClip gameWinSound;

    // Menu Focus
    [SerializeField] private List<GameObject> possibleMenus = new List<GameObject>();
    private List<GameObject> existingMenus = new List<GameObject>();
    [SerializeField] private bool inMenu;
    [SerializeField] private MenuController.MenuType startUpMenuType;
    [SerializeField] private GameObject menuSection;
    private GameObject previousMenu;
    private GameObject currentMenu;
    private GameObject backDropMenu;
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera menuCamera;

    // Scene Focus
    [SerializeField] private int finalLevelIndex = 3;
    private int gameplaySceneIndex = 0;
    private int currentAreaInitialSceneIndex = 1;
    private int currentSceneIndex = 0;
    private bool sceneLoaded = false;
    private bool sceneUnloaded = false;
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
    public InputControl GetInputControl() {return inputControl;}
    public void SetInputControl(InputControl newManager) {inputControl = newManager;}

    public List<LevelManager> GetLevels() {return levels;}
    public void SetLevels(List<LevelManager> newList) {levels = newList;}

    public List<GameObject> GetEnemies() {return enemies;}
    public void SetEnemies(List<GameObject> newEnemyList) {enemies = newEnemyList;}

    public float GetBaseResetTime() {return baseResetTime;}
    public void SetBaseResetTime(float newResetTime) {baseResetTime = newResetTime;}

    public int GetCurrentLevelNum() {return currentLevelNum;}
    public void SetCurrentLevelNum(int newLevelNum) {currentLevelNum = newLevelNum;}

    public int GetPreviousLevelNum() {return previousLevelNum;}
    public void SetPreviousLevelNum(int newLevelNum) {previousLevelNum = newLevelNum;}

    public GameObject GetPlayerObject() {return playerObject;}
    public void SetPlayerObject(GameObject newPlayer) {playerObject = newPlayer;}

    public GameObject GetCameraControlArea() {return cameraControlArea;}
    public void SetCameraControlArea(GameObject newArea) {cameraControlArea = newArea;}

    // Random Gen Focus
    public int GetInitialSeed() {return initialSeed;}
    public void SetInitialSeed(int newValue) {initialSeed = newValue;}

    public int GetInitialLowRange() {return initialLowRange;}
    public void SetInitialLowRange(int newValue) {initialLowRange = newValue;}

    public int GetInitialHighRange() {return initialHighRange;}
    public void SetInitialHighRange(int newValue) {initialHighRange = newValue;}

    // Audio Focus
    public AudioSource GetCurrentMusicSource() {return currentMusicSource;}
    public void SetCurrentMusicSource(AudioSource newSource) {currentMusicSource = newSource;}

    // Menu Focus
    public List<GameObject> GetExistingMenus() {return existingMenus;}
    public void SetExistingMenus(List<GameObject> newList) {existingMenus = newList;}

    public GameObject GetPreviousMenu() {return previousMenu;}
    public void SetPreviousMenu(GameObject newMenu) {previousMenu = newMenu;}

    public GameObject GetCurrentMenu() {return currentMenu;}
    public void SetCurrentMenu(GameObject newMenu) {currentMenu = newMenu;}

    // Scene Focus
    public List<string> GetScenes() {return scenes;}
    public void SetScenes(List<string> newSceneList) {scenes = newSceneList;}

    //------------------------------------------------------
    //                  COROUTINES
    //------------------------------------------------------

    public IEnumerator LoadScene(string sceneToLoadName, string sceneToUnloadName) {
        sceneLoaded = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(sceneToLoadName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => sceneLoaded);
        SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.UnloadSceneAsync(sceneToUnloadName);
    }

    public IEnumerator LoadScene(string sceneToLoadName) {
        sceneLoaded = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(sceneToLoadName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => sceneLoaded);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public IEnumerator UnloadScene(string sceneToUnloadName) {
        sceneUnloaded = false;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.UnloadSceneAsync(sceneToUnloadName);
        yield return new WaitUntil(() => sceneUnloaded);
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    //------------------------------------------------------
    //              STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        Instance = this;
        Random.InitState(initialSeed);
        currentState = Random.state;
        currentMusicSource = generalMusicSource;
        inputControl = new InputControl();
    }

    private void Start() {
        HandleSetup();
    }

    private void OnEnable() {
        inputControl.Enable();
    }

    private void OnDisable() {
        inputControl.Disable();
    }

    //------------------------------------------------------
    //              SETUP FUNCTIONS
    //------------------------------------------------------

    private void HandleSetup() {
        HandleMenuSetup();

    }

    private  void HandleMenuSetup() {
        
        LoadMenus();
        backDropMenu = CreateMenu(SearchPossibleMenus(MenuController.MenuType.Backdrop));
        currentMenu = CreateMenu(SearchPossibleMenus(startUpMenuType));
        ShiftToMenu();
        
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
    //                  AUDIO FUNCTIONS
    //------------------------------------------------------

    private void HandleAudio() {
        audioManager = AudioManager.Instance;

    }

    public void HandleMusicSource(AudioSource newSource) {
        if(currentMusicSource == generalMusicSource) {
            generalMusicSource.Pause();
        }

        currentMusicSource = newSource;
        currentMusicSource.Play();
    }

    //------------------------------------------------------
    //                   MENU FUNCTIONS
    //------------------------------------------------------

    private GameObject CreateMenu(GameObject newMenu) {
        GameObject spawnedMenu = (GameObject)Instantiate(newMenu, menuSection.transform, false);
        spawnedMenu.transform.SetParent(menuSection.transform);
        existingMenus.Add(spawnedMenu);
        return spawnedMenu;
    }

    private void LoadMenus() {

        possibleMenus = new List<GameObject>() {
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

    private GameObject SearchExistingMenus(MenuController.MenuType menuType) {
        return existingMenus.Find(specificMenu => specificMenu.GetComponent<MenuController>().GetMenuType() == menuType);
    }

    private GameObject SearchPossibleMenus(MenuController.MenuType menuType) {
        return possibleMenus.Find(specificMenu => specificMenu.GetComponent<MenuController>().GetMenuType() == menuType);
    }

    public void ChangeMenu(MenuController.MenuType thisMenuType) {
        GameObject newMenu = SearchExistingMenus(thisMenuType);
        if(newMenu == null) {
            newMenu = CreateMenu(SearchPossibleMenus(thisMenuType));
        }
        previousMenu = currentMenu;
        previousMenu.SetActive(false);
        currentMenu = newMenu;
        currentMenu.SetActive(true);
    }

    //------------------------------------------------------
    //                  SCENE FUNCTIONS
    //------------------------------------------------------

    public void OnSceneLoaded(Scene sceneName, LoadSceneMode mode) {
        UpdateSceneLoaded();
    }

    public void OnSceneUnloaded(Scene sceneName) {
        UpdateSceneUnloaded();
    }

    private void UpdateSceneLoaded() {
        sceneLoaded = !sceneLoaded;
    }

    private void UpdateSceneUnloaded() {
        sceneUnloaded = !sceneUnloaded;
    }

    private int ResetSceneToStartScene(int nextScene) {
        if(nextScene == scenes.Count) {
            nextScene = currentAreaInitialSceneIndex;
        }
        return nextScene;
    } 

    //------------------------------------------------------
    //                  GAME FUNCTIONS
    //------------------------------------------------------

    public void ShiftToGame() {
        mainCamera.gameObject.SetActive(true);
        menuCamera.gameObject.SetActive(false);
        cameraControlArea.SetActive(true);
        backDropMenu.SetActive(false);
        inMenu = false;
    }

    public void ShiftToMenu() {
        mainCamera.gameObject.SetActive(false);
        menuCamera.gameObject.SetActive(true);
        cameraControlArea.SetActive(false);
        backDropMenu.SetActive(true);
        inMenu = true;
    }

    //------------------------------------------------------
    //                  UTILITY FUNCTIONS
    //------------------------------------------------------    

    public Vector3 ScreenToWorld(Camera camera, Vector3 position) {
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);
    }

}
