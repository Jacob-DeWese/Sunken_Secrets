using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using System.Reflection;

public class UI_Inventory : MonoBehaviour
{
    [Header("Inventory logic")]
    [Tooltip("List to store EVERY collectible item/clue that will be used later in the game")]
    [SerializeField] protected List<GameObject> collectibleItems = new();

    [Tooltip("List for all the images that each item/clue will have in the inventory")]
    [SerializeField] protected List<Sprite> itemSprites = new();
    [SerializeField] protected List<Sprite> buttonSprites = new();

    [SerializeField] private List<Image> inventorySlots = new();
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Button button;
    
    private List<GameObject> inventoryItems = new();
    
    private int selectedIndex = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryPanel.SetActive(false);
        button.onClick.AddListener(ToggleInventory);
    }

    // Update is called once per frame
    void Update()
    {
        SelectItem();
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        if (inventoryPanel.activeSelf)
        {
            Debug.Log("Inventory Opened");
            button.GetComponent<Image>().sprite = buttonSprites[1];
        }
        else
        {
            Debug.Log("Inventory Closed");
            button.GetComponent<Image>().sprite = buttonSprites[0];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            AddItem(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }

    public void SelectItem()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedIndex = i;
                Debug.Log("Selected Item: " + inventorySlots[i].name);
            }
        }
    }

    public void AddItem(GameObject item)
    {
        if (inventoryItems.Count >= 5)
        {
            Debug.Log("Inventory Full");
            return;
        }

        inventoryItems.Add(item);
        int index = inventoryItems.Count - 1;
        UpdateSlot(index, item);
    }

    private void UpdateSlot(int index, GameObject item)
    {
        if (index < 0 || index >= inventorySlots.Count)
        {
            return;
        }

        int collectibleIndex = collectibleItems.IndexOf(item);

        if (collectibleIndex == -1)
        {
            Debug.Log("Item not found in collectibleItems list.");
            return;
        }

        inventorySlots[index].sprite = itemSprites[collectibleIndex];
        inventorySlots[index].enabled = true;
    }

    public GameObject GetSelectedItem()
    {
        if (selectedIndex < 0 || selectedIndex >= collectibleItems.Count)
        {
            return null;
        }

        return inventoryItems[selectedIndex];
    }
}
