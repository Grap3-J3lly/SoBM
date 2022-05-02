using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryButton : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

    private GameManager gameManager;
    private InventoryManager inventoryManager;

    [SerializeField] private string name;
    [SerializeField] private GameObject buttonObject;
    [SerializeField] private GameObject referencedItem;

    [SerializeField] private bool currentlySelected = false;

    private Vector4 backgroundDeselected = new Vector4(1, 1, 1, 1);
    private Vector4 backgroundSelected = new Vector4(0.6509434f, 0.6509434f, 0.6509434f, 1f);

    //------------------------------------------------------
    //                  GETTERS/SETTERS
    //------------------------------------------------------

    public string GetName() {return name;}
    public void SetName(string newName) {name = newName;}

    public GameObject GetButtonObject() {return buttonObject;}
    public void SetButtonObject(GameObject newButton) {buttonObject = newButton;}

    public GameObject GetReferencedItem() {return referencedItem;}
    public void SetReferencedItem(GameObject newItem) {referencedItem = newItem;}

    //------------------------------------------------------
    //                  COROUTINES
    //------------------------------------------------------

    public IEnumerator InitialSetup() {
        gameManager = GameManager.Instance;
        yield return new WaitForEndOfFrame();
        inventoryManager = gameManager.GetComponentInChildren<InventoryManager>();
        GetComponentInChildren<TMP_Text>().text = name;
    }

    //------------------------------------------------------
    //                  STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        StartCoroutine(InitialSetup());
    }

    private void Update() {
        CheckBackgroundColor();
    }

    //------------------------------------------------------
    //                  GENERAL FUNCTIONS
    //------------------------------------------------------

    public void SelectItem() {
        inventoryManager.SetSelectedObject(referencedItem);
        currentlySelected = true;
    }

    private void CheckIfSelected() {
        GameObject selectedObject = inventoryManager.GetSelectedObject();
        if(selectedObject != null && selectedObject != this.referencedItem) {
            currentlySelected = false;   
        }
        
    }

    private void CheckBackgroundColor() {
        Color currentBackgroundColor;
        if(currentlySelected) {
            currentBackgroundColor = backgroundSelected;
        }
        else {
            currentBackgroundColor = backgroundDeselected;
        }
        Image buttonBackground = gameObject.GetComponent<Image>();
        buttonBackground.color = currentBackgroundColor;
    }

}
