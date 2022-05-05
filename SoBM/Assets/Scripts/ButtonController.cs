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
        InvertControlsToggle,
        MainMenu,
        Restart,
        NextLevel,
        SpecificLevelStart,
        Jump,
        Attack,
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

}
