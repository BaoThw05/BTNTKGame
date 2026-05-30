using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    public GameObject itemPrefab;

    private WeaponHolderController weaponHolder;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            weaponHolder = player.GetComponentInChildren<WeaponHolderController>();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Click trái: tăng HP

            //if (playerHealth != null)
            //{
            //    playerHealth.Heal(healAmount);
            //    Debug.Log($"Click trái để hồi máu +{healAmount}");

            //}

            HealingItem healingItem = itemPrefab.GetComponent<HealingItem>();
            if (healingItem != null)
            {
                healingItem.UseItem();
                Debug.Log($"❤️ Đã sử dụng {itemPrefab.name}");
                return;
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Click phải: trang bị item trên tay
            if (weaponHolder != null && itemPrefab != null)
            {
                weaponHolder.EquipWeapon(itemPrefab);
                Debug.Log($"Click phải để trang bị {itemPrefab.name}");
            }
        }
    }
}