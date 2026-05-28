using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
	public SpriteRenderer characterSR;
	public int maxHealth;
    public int currentHealth;

    public HealthBar health;
    private Animator animator;

    public GameObject weapon;
	public bool isDead;

    void Start()
    {
        currentHealth = maxHealth;
        if (health == null)
        {
            health = Object.FindFirstObjectByType<HealthBar>();
            if (health == null)
            {
                GameObject redBarObject = GameObject.Find("RedBar");
                if (redBarObject != null)
                {
                    health = redBarObject.GetComponent<HealthBar>();
                }
            }
        }
        if (health != null)
        {
            health.UpdateBar(currentHealth, maxHealth);
        }
        else
        {
            Debug.LogWarning("Player chưa kết nối được với HealthBar. Hãy đảm bảo giao diện RedBar đã được bật (Active) trong Scene!");
        }
        animator = GetComponentInChildren<Animator>();
    }
    IEnumerator Flash()
	{
		characterSR.color = Color.red;

		yield return new WaitForSeconds(0.1f);

		characterSR.color = Color.white;
	}

	// 👉 ĐỔI TỪ void thành public void
	public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

		StartCoroutine(Flash());

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

		isDead = true;
		Destroy(weapon);
        Invoke("LoadFinishScene", 2f); 
    }

    private void LoadFinishScene()
    {
        SceneManager.LoadScene(2);
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