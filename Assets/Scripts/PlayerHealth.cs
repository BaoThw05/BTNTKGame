using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;  // 👈 ĐỔI TỪ private thành public (hoặc [SerializeField] private)

    public HealthBar health;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;

        if (health == null)
        {
            GameObject redBarObject = GameObject.Find("RedBar");
            if (redBarObject != null)
            {
                health = redBarObject.GetComponent<HealthBar>();
            }
        }

        if (health != null)
        {
            health.UpdateBar(currentHealth, maxHealth);
        }
        else
        {
            Debug.LogError("Không tìm thấy Object nào tên 'RedBar' có gắn script HealthBar!");
        }

        animator = GetComponentInChildren<Animator>();
    }

    // 👉 ĐỔI TỪ void thành public void
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;

        if (health != null)
        {
            health.UpdateBar(currentHealth, maxHealth);
        }

        if (animator != null)
        {
            animator.SetTrigger("HitTrigger");
        }

        Debug.Log("Nhận sát thương: " + damage + ", Máu còn: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (health != null)
        {
            health.UpdateBar(currentHealth, maxHealth);
        }

        Debug.Log("Hồi máu: +" + amount + ", Máu hiện tại: " + currentHealth);
    }

    private void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("DieTrigger");
        }

        PlayerMove playerMove = GetComponent<PlayerMove>();
        if (playerMove != null)
        {
            playerMove.enabled = false;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        Debug.Log("Player đã chết!");
    }

    //Bạn có thể xóa hoặc comment đoạn Update test này
     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(30);
        }
    }
}