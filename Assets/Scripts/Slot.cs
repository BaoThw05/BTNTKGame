using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public GameObject currentItem;

    void Start()
    {
        currentItem = null;
    }

    public void AddItem(GameObject item)
    {
        if (currentItem == null)
        {
            currentItem = item;
            item.transform.SetParent(transform);
            item.transform.localPosition = Vector3.zero;
        }
    }

    public void RemoveItem()
    {
        if (currentItem != null)
        {
            Destroy(currentItem);
            currentItem = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            // ✅ HealingItem (hồi máu)
            HealingItem healingItem = currentItem.GetComponent<HealingItem>();
            if (healingItem != null)
            {
                Debug.Log("🖱️ Click vào HealingItem: " + healingItem.Name);
                healingItem.UseItem();
                return;
            }

            // ✅ StaminaItem (hồi thể lực - GreenBar)
            StaminaItem staminaItem = currentItem.GetComponent<StaminaItem>();
            if (staminaItem != null)
            {
                Debug.Log("💚 Click vào StaminaItem: " + staminaItem.Name);
                staminaItem.UseItem();
                return;
            }

            // Item thường (base class)
            Item item = currentItem.GetComponent<Item>();
            if (item != null)
            {
                Debug.Log("🖱️ Click vào item: " + item.Name);
                item.UseItem();
            }
        }
    }
}