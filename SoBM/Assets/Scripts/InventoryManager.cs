using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

    private GameManager gameManager;

    private GameObject itemButton;
    [SerializeField] private GameObject itemUIParent;

    [SerializeField] private GameObject selectedObject;

    public List<GameObject> inventory = new List<GameObject>();
    private List<GameObject> inventoryButtons = new List<GameObject>();

    //------------------------------------------------------
    //                  GETTERS/SETTERS
    //------------------------------------------------------

    public GameObject GetSelectedObject() {return selectedObject;}
    public void SetSelectedObject(GameObject newObject) {selectedObject = newObject;}

    public List<GameObject> GetInventory() {return inventory;}
    public void SetInventory(List<GameObject> newList) {inventory = newList;}

    //------------------------------------------------------
    //                  STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        gameManager = GameManager.Instance;
        LoadObjectButton();
    }

    //------------------------------------------------------
    //                  SETUP FUNCTIONS
    //------------------------------------------------------

    private void LoadObjectButton() {
        itemButton = Resources.Load<GameObject>("Prefabs/UI/ItemButton");
    }

    //------------------------------------------------------
    //                  UI FUNCTIONS
    //------------------------------------------------------

    public void CreateButton(GameObject newItem) {
        string buttonLabel = newItem.name;

        RectTransform tempTransform = itemUIParent.GetComponent<RectTransform>();

        GameObject newButtonObject = (GameObject)Instantiate(itemButton, tempTransform.position, tempTransform.rotation);
        newButtonObject.transform.SetParent(itemUIParent.transform);
        newButtonObject.name = buttonLabel + "UIButton";

        InventoryButton newButtonController = newButtonObject.GetComponent<InventoryButton>();
        newButtonController.SetName(buttonLabel);
        newButtonController.SetButtonObject(newButtonObject);
        newButtonController.SetReferencedItem(newItem);

        inventoryButtons.Add(newButtonObject);
    }

}
