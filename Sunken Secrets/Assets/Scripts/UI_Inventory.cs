using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class UI_Inventory : MonoBehaviour
{
    [Header("Inventory logic")]
    [Tooltip("List to store EVERY collectible item/clue that will be used later in the game")]
    [SerializeField] protected List<GameObject> collectibleItems = new();

    [Tooltip("List for all the images that each item/clue will have in the inventory")]
    [SerializeField] protected List<Sprite> itemSprites = new();

    [SerializeField] private List<Image> inventorySlots = new();
    private List<GameObject> inventoryItems = new();
    
    private int selectedIndex = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SelectItem();
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
