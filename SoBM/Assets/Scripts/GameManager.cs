using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

    public static GameManager Instance;

    [SerializeField] private List<LevelManager> levels = new List<LevelManager>();

    //------------------------------------------------------
    //              STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        Instance = this;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
