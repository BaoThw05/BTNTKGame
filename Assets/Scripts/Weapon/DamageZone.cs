using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [Header("Sát thương")]
    public int damage = 10;           // lượng máu mất mỗi lần
    public float damageInterval = 1f; // 1 giây mất máu 1 lần

    private float nextDamageTime;
    private PlayerHealth playerInZone;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("🔥 Có vật chạm vào vùng lửa: " + other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ Player chạm vào vùng lửa!");
            playerInZone = other.GetComponent<PlayerHealth>();

            if (playerInZone != null && Time.time >= nextDamageTime)
            {
                playerInZone.TakeDamage(damage);
                nextDamageTime = Time.time + damageInterval;
            }
            else
            {
                Debug.LogWarning("Không tìm thấy PlayerHealth trên Player!");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerInZone != null)
        {
            if (Time.time >= nextDamageTime)
            {
                playerInZone.TakeDamage(damage);
                nextDamageTime = Time.time + damageInterval;
                Debug.Log($"💔 Gây {damage} sát thương, máu còn: {playerInZone.currentHealth}");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🚪 Player ra khỏi vùng lửa!");
            playerInZone = null;
        }
    }
}