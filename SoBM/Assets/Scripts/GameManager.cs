using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

    // General Focus
    public static GameManager Instance;

    [SerializeField] private List<LevelManager> levels = new List<LevelManager>();

    [SerializeField] private float baseResetTime = 2.5f;
    [SerializeField] private int currentLevelNum = 0;

    [SerializeField] private GameObject playerObject;

    // Random Gen Focus
    [SerializeField] private int initialSeed = 1;
    [SerializeField] private int initialLowRange = 0;
    [SerializeField] private int initialHighRange = 999;
    private Random.State currentState;

    //------------------------------------------------------
    //              GETTERS/SETTERS
    //------------------------------------------------------

    // General Focus
    public List<LevelManager> GetLevels() {return levels;}
    public void SetLevels(List<LevelManager> newList) {levels = newList;}

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

    //------------------------------------------------------
    //                  COROUTINES
    //------------------------------------------------------

    public IEnumerator Reset() {
        yield return new WaitForSeconds(baseResetTime);
    }

    //------------------------------------------------------
    //              STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        Instance = this;
        Random.InitState(initialSeed);
        currentState = Random.state;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    //------------------------------------------------------
    //                   RANDOMIZING FUNCTIONS
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

}
