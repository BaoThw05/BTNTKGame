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
    public bool AddItem(GameObject itemPrefab)
    {
        foreach (Transform slotTransform in hotBarPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                // Instantiate item vào slot, false để giữ local transform ổn định
                GameObject item = Instantiate(itemPrefab, slotTransform, false);
                RectTransform rt = item.GetComponent<RectTransform>();
                if (rt != null)
                {
                    // Reset vị trí về giữa slot
                    rt.anchorMin = new Vector2(0.5f, 0.5f);
                    rt.anchorMax = new Vector2(0.5f, 0.5f);
                    rt.pivot = new Vector2(0.5f, 0.5f);
                    rt.anchoredPosition = Vector2.zero;

                    // Giữ nguyên scale gốc
                    rt.localScale = Vector3.one;

                    // FIX kích thước cố định để không bị phóng to
                    rt.sizeDelta = new Vector2(40, 40);
                }

                // Nếu item có Image thì giữ đúng tỉ lệ ảnh
                UnityEngine.UI.Image img = item.GetComponent<UnityEngine.UI.Image>();
                if (img != null)
                {
                    img.preserveAspect = true;
                }

                slot.currentItem = item;
                return true;
            }
        }

        return false;
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
        // Xóa toàn bộ slot cũ
        foreach (Transform child in hotBarPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Tạo lại slot mới
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, hotBarPanel.transform);
        }

        // Load item vào đúng slot và GIỮ NGUYÊN KÍCH CỠ
        foreach (InventorySaveData data in hotBarData)
        {
            if (data.slotIndex < slotCount)
            {
                Slot slot = hotBarPanel.transform
                    .GetChild(data.slotIndex)
                    .GetComponent<Slot>();

                GameObject itemPrefab = itemDictionary.GetItemPrefabByID(data.itemIDs);

                if (itemPrefab != null)
                {
                    // false để giữ nguyên RectTransform prefab
                    GameObject item = Instantiate(itemPrefab, slot.transform, false);

                    RectTransform rt = item.GetComponent<RectTransform>();

                    if (rt != null)
                    {
                        // Giữ nguyên size prefab khi load/drop
                        rt.localScale = Vector3.one;
                        rt.localPosition = Vector3.zero;
                        rt.anchoredPosition = Vector2.zero;

                        // KHÔNG stretch full slot nữa
                        rt.anchorMin = new Vector2(0.5f, 0.5f);
                        rt.anchorMax = new Vector2(0.5f, 0.5f);
                        rt.pivot = new Vector2(0.5f, 0.5f);

                        // giữ nguyên width/height gốc của prefab
                        rt.sizeDelta = itemPrefab.GetComponent<RectTransform>().sizeDelta;
                    }

                    slot.currentItem = item;
                }
            }
        }
    }
}
