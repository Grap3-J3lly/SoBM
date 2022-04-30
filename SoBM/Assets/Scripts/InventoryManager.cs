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

    public List<GameObject> inventory = new List<GameObject>();

    private List<GameObject> inventoryButtons = new List<GameObject>();

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

        GameObject newButton = (GameObject)Instantiate(itemButton, tempTransform.position, tempTransform.rotation);
        newButton.transform.SetParent(itemUIParent.transform);
        newButton.name = buttonLabel + "UIButton";

        newButton.GetComponentInChildren<TMP_Text>().text = buttonLabel;
    }

}
