using UnityEngine;
using UnityEngine.UI;

public class HealthBox : MonoBehaviour
{
    [Header("Cấu hình chung")]
    public string itemName = "Vật phẩm";
    public Sprite itemIcon;
    public int itemID = 1;

    [Header("Loại item")]
    public bool isStaminaItem = false;  // 👈 ĐỔI TỪ isManaItem thành isStaminaItem

    [Header("Giá trị hồi")]
    public int amount = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryController inventory = FindObjectOfType<InventoryController>();

            if (inventory != null)
            {
                GameObject itemToAdd = CreateInventoryItem();
                bool added = inventory.AddItem(itemToAdd);

                if (added)
                {
                    Debug.Log("📦 Đã nhặt " + itemName + " vào túi!");
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Túi đồ đã đầy! Không thể nhặt.");
                }
            }
            else
            {
                Debug.LogError("Không tìm thấy InventoryController!");
            }
        }
    }

    GameObject CreateInventoryItem()
    {
        GameObject itemObj = new GameObject(itemName);

        if (isStaminaItem)  // 👈 ĐỔI TỪ isManaItem thành isStaminaItem
        {
            StaminaItem staminaItem = itemObj.AddComponent<StaminaItem>();
            staminaItem.staminaAmount = amount;
            staminaItem.Name = itemName;
            staminaItem.ID = itemID;
        }
        else
        {
            HealingItem healingItem = itemObj.AddComponent<HealingItem>();
            healingItem.healAmount = amount;
            healingItem.Name = itemName;
            healingItem.ID = itemID;
        }

        // Dùng Image cho UI (hiển thị trong túi)
        Image image = itemObj.AddComponent<Image>();
        if (itemIcon != null)
            image.sprite = itemIcon;

        // Thêm RectTransform cho UI
        RectTransform rt = itemObj.GetComponent<RectTransform>();
        if (rt == null)
            rt = itemObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(80, 80);

        return itemObj;
    }
}