using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public WeaponData weaponData;

    private void Start()
    {
        // Nếu không có MeleeWeaponData, tạo từ WeaponBase
        if (weaponData == null)
        {
            Weapon weaponBase = GetComponent<Weapon>();
            if (weaponBase != null)
            {
                // Tạo tạm thời MeleeWeaponData từ WeaponBase
                weaponData = ScriptableObject.CreateInstance<WeaponData>();
                weaponData.weaponSprite = weaponBase.weaponSprite;
                weaponData.damage = weaponBase.damage;
                weaponData.attackRange = weaponBase.attackRange;
                weaponData.enemyLayer = weaponBase.enemyLayer;
            }
        }
    }
}