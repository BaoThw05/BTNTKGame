using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    public GameObject weaponPrefab;
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
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (weaponHolder != null && weaponPrefab != null)
            {
                weaponHolder.EquipWeapon(weaponPrefab);
                Debug.Log($"Click phải để trang bị {weaponPrefab.name}");
            }
        }
    }
}