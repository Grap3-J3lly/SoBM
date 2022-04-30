using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------
    
    private GameManager gameManager;
    private bool inInteractZone = false;

    public enum InteractableType {
        Switch,
        Button,
        Block
    }

    [SerializeField] private InteractableType thisType;

    //------------------------------------------------------
    //                  GETTERS/SETTERS
    //------------------------------------------------------

    public bool GetInInteractZone() {return inInteractZone;}
    public void SetInInteractZone(bool newValue) {inInteractZone = newValue;}

    //------------------------------------------------------
    //                  STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        gameManager = GameManager.Instance;
    }

    //------------------------------------------------------
    //                  COLLISION FUNCTIONS
    //------------------------------------------------------

    

}
