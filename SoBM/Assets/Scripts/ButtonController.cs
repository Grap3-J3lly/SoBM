using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{

    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

    private GameManager gameManager;
    private LevelManager currentLevelManager;

    public enum ButtonType {
        Play,
        Options,
        Credits,
        Back,
        GameVolumeSliderControl,
        MusicToggle,
        MusicVolumeSliderControl,
        SoundToggle,
        SoundVolumeSliderControl,
        MainMenu,
        Restart,
        NextLevel,
        SpecificLevelStart,
        Pause,
        Resume
    }
    [SerializeField] private ButtonType buttonType;

    [SerializeField] private string defaultSceneName = "L";
    private int levelNum = -1;

    //------------------------------------------------------
    //                  GETTERS/SETTERS
    //------------------------------------------------------

    public ButtonType GetButtonType() {return buttonType;}
    public void SetButtonType(ButtonType newType) {buttonType = newType;}

    public int GetLevelNum() {return levelNum;}
    public void SetLevelNum(int newNum) {levelNum = newNum;}

    //------------------------------------------------------
    //                  STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        gameManager = GameManager.Instance;
    }

    private void Start() {

    }

    private void Update() {
        if(this.buttonType != ButtonType.SpecificLevelStart) {
            if(levelNum != gameManager.GetCurrentLevelNum()) {
                levelNum = gameManager.GetCurrentLevelNum();
            }
        }
        
    }

    //------------------------------------------------------
    //                  GENERAL FUNCTIONS
    //------------------------------------------------------

    public int GetToLevelNumber() {
        return 0;
    }

    public void HandleButtonPress() {
        switch(this.buttonType) {
            case ButtonType.Play:
                HandlePlay();
            break;
            case ButtonType.Options:
                HandleOptions();
            break;
            case ButtonType.Credits:
                HandleCredits();
            break;
            case ButtonType.Back:
                HandleBack();
            break;
            case ButtonType.GameVolumeSliderControl:
                HandleGameVolumeSliderControl();
            break;
            case ButtonType.MusicToggle:
                HandleMusicToggle();
            break;
            case ButtonType.MusicVolumeSliderControl:
                HandleMusicVolumeSliderControl();
            break;
            case ButtonType.SoundToggle:
                HandleSoundToggle();
            break;
            case ButtonType.SoundVolumeSliderControl:
                HandleSoundVolumeSliderControl();
            break;
            case ButtonType.MainMenu:
                HandleMainMenu();
            break;
            case ButtonType.Restart:
                HandleRestart();
            break;
            case ButtonType.NextLevel:
                HandleNextLevel();
            break;
            case ButtonType.SpecificLevelStart:
                HandleSpecificLevelStart();
            break;
            case ButtonType.Pause:
                HandlePause();
            break;
            case ButtonType.Resume:
                HandleResume();
            break;
        }
    }

    //------------------------------------------------------
    //              BUTTON SPECIFIC FUNCTIONS
    //------------------------------------------------------

    private void HandlePlay() {
        gameManager.ChangeMenu(MenuController.MenuType.LevelSelect);
    }

    private void HandleOptions() {
        gameManager.ChangeMenu(MenuController.MenuType.Options);
    }

    private void HandleCredits() {
        gameManager.ChangeMenu(MenuController.MenuType.Credits);
    }

    private void HandleBack() {
        GameObject previousMenu = gameManager.GetPreviousMenu();
        gameManager.ChangeMenu(previousMenu.GetComponent<MenuController>().GetMenuType());
    }

    private void HandleGameVolumeSliderControl() {
        
    }

    private void HandleMusicToggle() {
        
    }

    private void HandleMusicVolumeSliderControl() {
        
    }

    private void HandleSoundToggle() {
        
    }

    private void HandleSoundVolumeSliderControl() {
        
    }

    private void HandleMainMenu() {
        if(gameManager.GetCurrentMenu().GetComponent<MenuController>().GetMenuType() == MenuController.MenuType.Pause) {
            Debug.Log("Trying to unload scene: " + defaultSceneName + levelNum);
            StartCoroutine(gameManager.UnloadScene(defaultSceneName + levelNum));
        }
        gameManager.ChangeMenu(MenuController.MenuType.Main);
    }

    private void HandleRestart() {
        Time.timeScale = 1;
        
        StartCoroutine(gameManager.LoadScene(defaultSceneName + levelNum));
        StartCoroutine(gameManager.UnloadScene(defaultSceneName + levelNum));
    }

    private void HandleNextLevel() {
        Time.timeScale = 1;
        gameManager.SetPreviousLevelNum(gameManager.GetCurrentLevelNum());
        gameManager.SetCurrentLevelNum(gameManager.GetCurrentLevelNum() + 1);
        StartCoroutine(gameManager.LoadScene(defaultSceneName + (gameManager.GetCurrentLevelNum()), defaultSceneName + (gameManager.GetPreviousLevelNum())));
    }

    private void HandleSpecificLevelStart() {
        Time.timeScale = 1;
        gameManager.SetCurrentLevelNum(levelNum);
        StartCoroutine(gameManager.LoadScene(defaultSceneName + levelNum));
    }
    
    private void HandlePause() {
        Time.timeScale = 0;
        gameManager.ShiftToMenu();
        gameManager.ChangeMenu(MenuController.MenuType.Pause);
    }

    private void HandleResume() {
        Time.timeScale = 1;
        gameManager.ChangeMenu(MenuController.MenuType.GeneralHud);
        gameManager.ShiftToGame();
    }


}
