using UnityEngine;
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;

    public HealthBar health;// Sự kiện khi nhân vật chết
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;

        // 1. Tìm đích danh object có tên là "RedBar" trên Scene
        if (health == null)
        {
            GameObject redBarObject = GameObject.Find("RedBar");

            // Nếu tìm thấy Object tên RedBar, thì lấy script HealthBar của nó
            if (redBarObject != null)
            {
                health = redBarObject.GetComponent<HealthBar>();
            }
        }

        // 2. Cập nhật thanh máu sau khi tìm được
        if (health != null)
        {
            health.UpdateBar(currentHealth, maxHealth);
        }
        else
        {
            Debug.LogError("Không tìm thấy Object nào tên 'RedBar' có gắn script HealthBar!");
        }

        // 3. Tìm Animator
        animator = GetComponentInChildren<Animator>();
    }
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        health.UpdateBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        animator.SetTrigger("DieTrigger"); // gọi animation chết

        // Ví dụ: khóa điều khiển
        GetComponent<PlayerMove>().enabled = false;

        // Optional: tắt collider
        GetComponent<Collider2D>().enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(30);
        }
    }
}