using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    public GameObject weaponPrefab;  // Kéo prefab kiếm vào đây
    private PlayerEquip playerEquip;

    void Start()
    {
        // Tìm PlayerEquip trên nhân vật
        playerEquip = FindObjectOfType<PlayerEquip>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Click chuột phải
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            EquipWeapon();
        }
    }

    void EquipWeapon()
    {
        if (playerEquip != null && weaponPrefab != null)
        {
            playerEquip.Equip(weaponPrefab);
            Debug.Log("Đã trang bị: " + weaponPrefab.name);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy PlayerEquip hoặc weaponPrefab chưa gán!");
        }
    }
}