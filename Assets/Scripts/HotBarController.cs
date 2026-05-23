using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotBarController : MonoBehaviour
{
    public GameObject hotBarPanel;
    public GameObject slotPrefab;
    public int slotCount = 10;
    private ItemDictionary itemDictionary;
    private Key[] hotBarKeys;
    private void Awake()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();
        hotBarKeys = new Key[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            hotBarKeys[i] = i < 9 ? (Key)((int)Key.Digit1 + i) : Key.Digit0;
        }
    }
    void Update()
    {
        for (int i = 0; i < slotCount; i++)
        {
            if (Keyboard.current[hotBarKeys[i]].wasPressedThisFrame)
            {
                UseItemInSlot(i);
            }
        }
    }
    public bool AddItem(GameObject item)
    {
        foreach (Transform child in hotBarPanel.transform)
        {
            Slot slot = child.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(item, slot.transform, false);
                RectTransform rt = newItem.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(0, 0);
                rt.anchorMax = new Vector2(1, 1);
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                rt.localScale = Vector3.one;
                slot.currentItem = newItem;
                return true;
            }
        }
        return false; // Hotbar đầy
    }
    void UseItemInSlot(int Index)
    {
        Slot slot = hotBarPanel.transform.GetChild(Index).GetComponent<Slot>();
        if (slot.currentItem != null)
        {
            Item item = slot.currentItem.GetComponent<Item>();
            if (item != null)
            {
                item.UseItem();
            }
        }
    }
    public List<InventorySaveData> GetHotBarItem()
    {
        List<InventorySaveData> hotBarData = new List<InventorySaveData>();

        foreach (Transform slotTransform in hotBarPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                if (item != null)
                {
                    hotBarData.Add(new InventorySaveData
                    {
                        itemIDs = item.ID,
                        slotIndex = slotTransform.GetSiblingIndex()
                    });
                }
            }
        }
        return hotBarData;
    }
    public void SetHotBarItem(List<InventorySaveData> hotBarData)
    {
        foreach (Transform child in hotBarPanel.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, hotBarPanel.transform);
        }
        foreach (InventorySaveData data in hotBarData)
        {
            if (data.slotIndex < slotCount)
            {
                Slot slot = hotBarPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefabByID(data.itemIDs);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform, false);
                    RectTransform rt = item.GetComponent<RectTransform>();
                    rt.anchorMin = new Vector2(0, 0);
                    rt.anchorMax = new Vector2(1, 1);
                    rt.offsetMin = Vector2.zero;
                    rt.offsetMax = Vector2.zero;
                    rt.localScale = Vector3.one;
                    slot.currentItem = item;
                }
            }
        }
    }
}
