using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    private BossMeleeAttack bossMeleeAttack;

    public Animator animator;

    public SpriteRenderer characterSR;

    public int currentHealth = 200;  // Boss có nhiều máu hơn quái vật thường

    private void Awake()
    {
        bossMeleeAttack = GetComponent<BossMeleeAttack>();
    }

    IEnumerator Flash()
    {
        characterSR.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        characterSR.color = Color.white;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        StartCoroutine(Flash());

        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        Debug.Log("[Boss] Nhận " + damage + " sát thương, máu còn: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("[Boss] Boss đã chết! Chuyển sang Win Screen...");

        animator.SetTrigger("Die");

        if (bossMeleeAttack != null)
        {
            bossMeleeAttack.enabled = false;
        }

        // Tắt tất cả các component để boss không còn tấn công
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        // Chuyển sang Win Scene (Index 2 - điều chỉnh nếu cần)
        StartCoroutine(LoadWinScreenDelay());
    }

    IEnumerator LoadWinScreenDelay()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Win");  // Hoặc SceneManager.LoadScene(2); nếu dùng index
    }
}
