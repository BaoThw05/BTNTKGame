using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private WeaponHolderController weaponHolder;
    [SerializeField] private PlayerMove playerMove;

    private bool isAttacking;

    private void Update()
    {
        // Nếu đang cầm súng thì không tấn công cận chiến
        if (weaponHolder != null &&
            weaponHolder.currentWeaponBase != null &&
            weaponHolder.currentWeaponBase.isGun)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (isAttacking) return;
        isAttacking = true;

        if (weaponHolder != null && weaponHolder.currentWeaponBase != null)
        {
            Vector3 attackPos = weaponHolder.transform.position;
            weaponHolder.currentWeaponBase.Attack(attackPos);
        }

        isAttacking = false;
    }
}