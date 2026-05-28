using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Cấu hình đạn")]
    public int damage = 10;       // Số sát thương viên đạn gây ra
    [SerializeField] private float lifeTime = 3f;   // Thời gian đạn tự hủy nếu bay trượt (tránh rác bộ nhớ)

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void HandleHit(Collider2D collision)
    {
        // Kiểm tra Boss trước
        BossHealth bossHealth = collision.GetComponent<BossHealth>();

        if (bossHealth == null)
        {
            bossHealth = collision.GetComponentInParent<BossHealth>();
        }

        if (bossHealth == null)
        {
            bossHealth = collision.GetComponentInChildren<BossHealth>();
        }

        if (bossHealth != null)
        {
            bossHealth.TakeDamage(damage);
            Debug.Log($"Đạn bắn trúng Boss, gây {damage} sát thương!");
            Destroy(gameObject);
            return;
        }

        // Kiểm tra Enemy thường
        EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();

        if (enemyHealth == null)
        {
            enemyHealth = collision.GetComponentInParent<EnemyHealth>();
        }

        if (enemyHealth == null)
        {
            enemyHealth = collision.GetComponentInChildren<EnemyHealth>();
        }

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
            Debug.Log($"Đạn bắn trúng {collision.name}, gây {damage} sát thương!");
            Destroy(gameObject);
            return;
        }

        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleHit(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleHit(collision.collider);
    }
}
