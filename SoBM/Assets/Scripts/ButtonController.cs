using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    private AudioSource musicSource;
    private AudioManager audioManager;
    private AudioClip buttonClick;
    private Sprite toggleOnImage;
    private Sprite toggleOffImage;
    private Sprite slideControlImage;

    //------------------------------------------------------
    //                  GETTERS/SETTERS
    //------------------------------------------------------

    public ButtonType GetButtonType() {return buttonType;}
    public void SetButtonType(ButtonType newType) {buttonType = newType;}

    public int GetLevelNum() {return levelNum;}
    public void SetLevelNum(int newNum) {levelNum = newNum;}

    //------------------------------------------------------
    //                      COROUTINES
    //------------------------------------------------------

    // private IEnumerator AudioCheck() {
    //     if(audioManager == null) {
    //         yield return new WaitUntil(() => AudioManager.Instance != null);
    //     }
    // }

    //------------------------------------------------------
    //                  STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        HandleSetup();
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
    //                  SETUP FUNCTIONS
    //------------------------------------------------------

    private void HandleSetup() {
        gameManager = GameManager.Instance;
        musicSource = gameManager.GetCurrentMusicSource();
        audioManager = AudioManager.Instance;
        LoadAudio();
        LoadSprites();
    }

    private void LoadAudio() {
        buttonClick = Resources.Load<AudioClip>("Foley/singleButtonClick");
    }

    private void LoadSprites() {
        toggleOnImage = Resources.Load<Sprite>("Sprites/check");
        toggleOffImage = Resources.Load<Sprite>("Sprites/blank");
        slideControlImage = Resources.Load<Sprite>("Sprites/slideControl");
    }   

    //------------------------------------------------------
    //                  GENERAL FUNCTIONS
    //------------------------------------------------------

    public int GetToLevelNumber() {
        return 0;
    }

    public void HandleButtonPress() {
        //StartCoroutine(AudioCheck());
        audioManager.Play(buttonClick.name);
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
        ToggleAudioSource(musicSource);
        ToggleImage(musicSource.gameObject.activeSelf);
    }

    private void HandleMusicVolumeSliderControl() {
        
    }

    private void HandleSoundToggle() {
        ToggleAudioSource(audioManager.GetAudioSource());
        ToggleImage(audioManager.GetAudioSource().gameObject.activeSelf);
    }

    private void HandleSoundVolumeSliderControl() {
        
    }

    private void HandleMainMenu() {
        if(gameManager.GetCurrentMenu().GetComponent<MenuController>().GetMenuType() == MenuController.MenuType.Pause) {
            //Debug.Log("Trying to grab Level Manager: " + currentLevelManager);
            LevelManager tempManager = gameManager.GetLevels()[gameManager.GetCurrentLevelNum()-1];
            tempManager.GetEnemy().SetActive(false);
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
        LevelManager tempManager = gameManager.GetLevels()[gameManager.GetCurrentLevelNum()-1];
            tempManager.GetEnemy().SetActive(false);
        gameManager.SetPreviousLevelNum(gameManager.GetCurrentLevelNum());
        gameManager.SetCurrentLevelNum(gameManager.GetCurrentLevelNum() + 1);
        StartCoroutine(gameManager.LoadScene(defaultSceneName + (gameManager.GetCurrentLevelNum())));
        StartCoroutine(gameManager.UnloadScene(defaultSceneName + (gameManager.GetPreviousLevelNum())));
        
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

    private void ToggleImage(bool status) {
        Image currentButtonImage = GetComponent<Image>();
        if(status) {
            currentButtonImage.sprite = toggleOnImage;
        }
        else {
            currentButtonImage.sprite = toggleOffImage;
        }
    }

    private void ToggleAudioSource(AudioSource thisSource) {
        bool status = thisSource.gameObject.activeSelf;
        status = !status;
        thisSource.gameObject.SetActive(status);
    }

}
