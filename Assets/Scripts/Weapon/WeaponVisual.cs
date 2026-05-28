using UnityEngine;

public class WeaponVisual : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        Weapon weaponBase = GetComponent<Weapon>();
        if (weaponBase != null && weaponBase.weaponSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = weaponBase.weaponSprite;
        }
    }

    public void SetWeapon(WeaponData weapon)
    {
        if (spriteRenderer != null && weapon != null)
        {
            spriteRenderer.sprite = weapon.weaponSprite;
        }
    }

    public void SetWeaponFromBase(Weapon weaponBase)
    {
        if (spriteRenderer != null && weaponBase != null && weaponBase.weaponSprite != null)
        {
            spriteRenderer.sprite = weaponBase.weaponSprite;
        }
    }
}