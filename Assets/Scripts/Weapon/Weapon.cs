using UnityEngine;
public abstract class Weapon : MonoBehaviour
{
    [Header("Thông tin vũ khí")]
    public string weaponName;
    public Sprite weaponSprite;
    public int damage;
    public float attackRange;
    public LayerMask enemyLayer;

    [Header("Loại vũ khí")]
    public bool isGun = false; // true = súng, false = melee

    public virtual void Use()
    {
        Debug.Log($"Dùng {weaponName}");
    }

    public virtual void Attack(Vector3 attackPos)
    {
        if (!isGun)
        {
            // Thử tìm bằng LayerMask trước
            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos, attackRange, enemyLayer);
            if (enemies == null || enemies.Length == 0)
            {
                // Fallback: tìm tất cả collider trong phạm vi và lọc theo EnemyHealth
                enemies = Physics2D.OverlapCircleAll(attackPos, attackRange);
            }

            foreach (Collider2D enemy in enemies)
            {
                enemy.GetComponentInParent<EnemyHealth>()?.TakeDamage(damage);
            }
            Debug.Log($"Melee attack tại {attackPos} - gây {damage} sát thương");
        }
        else
        {
            Debug.Log($"Gun attack - cần Fire() để bắn");
        }
    }
}
