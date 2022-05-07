using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{

    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

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

    //------------------------------------------------------
    //                  GETTERS/SETTERS
    //------------------------------------------------------

    public ButtonType GetButtonType() {return buttonType;}
    public void SetButtonType(ButtonType newType) {buttonType = newType;}

    //------------------------------------------------------
    //                  STANDARD FUNCTIONS
    //------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    }

    private void HandleOptions() {
        
    }

    private void HandleCredits() {
        
    }

    private void HandleBack() {
        
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
        
    }

    private void HandleRestart() {
        
    }

    private void HandleNextLevel() {
        
    }

    private void HandleSpecificLevelStart() {
        
    }
    
    private void HandlePause() {
        
    }

    private void HandleResume() {
        
    }


}
