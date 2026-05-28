using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Cấu hình đạn")]
    public int damage = 10;              
    public GameObject impactEffect;    

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Xử lý khi chạm vào Wall
        if (hitInfo.CompareTag("Wall"))
        {
            Debug.Log($"Đạn boss chạm vào Wall: {hitInfo.name}");
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, transform.rotation);
            }
            Destroy(gameObject);
            return;
        }
        if (hitInfo.CompareTag("Player"))
        {
            Debug.Log($"Đạn boss bắn trúng Player, gây {damage} sát thương!");
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, transform.rotation);
            }
            PlayerHealth playerHealth = hitInfo.GetComponent<PlayerHealth>();
            if (playerHealth == null)
            {
                playerHealth = hitInfo.GetComponentInParent<PlayerHealth>();
            }

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
