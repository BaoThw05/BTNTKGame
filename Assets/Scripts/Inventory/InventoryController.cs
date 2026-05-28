using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;
    public GameObject inventory;
    public GameObject panel;
    public GameObject bag;
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;
    

    void Start()
    {

        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SoundManager.Instance.PlaySound("Bag", transform.position);
            ToggleInventory();
        }
    }
    public void ToggleInventory()
    {
        if (inventory != null)
        {
            bool isBagActive = !bag.activeSelf;
            bool isActive = !inventory.activeSelf;
            bool isPanelActive = !panel.activeSelf;
            inventory.SetActive(isActive);
            panel.SetActive(isPanelActive);
            bag.SetActive(isBagActive);
        }
    }

    public bool AddItem(GameObject itemPrefab)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject item = Instantiate(itemPrefab, slotTransform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;
                return true;
            }
        }
        Debug.Log("Inventory is full!");
        return false;
    }

    public List<InventorySaveData> GetInventoryItem()
    {
        List<InventorySaveData> inventoryData = new List<InventorySaveData>();

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot != null && slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();

                if (item != null)
                {
                    inventoryData.Add(new InventorySaveData
                    {
                        itemIDs = item.ID,
                        slotIndex = slotTransform.GetSiblingIndex()
                    });
                }
            }
        }

        return inventoryData;
    }

    public void SetInventoryItem(List<InventorySaveData> inventoryData)
    {
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }
        foreach (InventorySaveData data in inventoryData)
        {
            if (data.slotIndex < slotCount)
            {
                Slot slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefabByID(data.itemIDs);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
            }
        }
    }
}