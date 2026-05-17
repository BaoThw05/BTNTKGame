using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();
        //for (int i = 0; i < slotCount; i++)
        //{
        //    Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
        //    if(i < itemPrefabs.Length)
        //    {
        //        GameObject item = Instantiate(itemPrefabs[i],slot.transform);
        //        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //        slot.currentItem=item;
        //    }
        //}
    }
    public bool AddItem(GameObject itemPrefab)
    {
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if(slot != null && slot.currentItem == null)
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

            // Kiểm tra chắc chắn slot có tồn tại và đang chứa item
            if (slot != null && slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();

                // Kiểm tra chắc chắn object đó có script Item
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
