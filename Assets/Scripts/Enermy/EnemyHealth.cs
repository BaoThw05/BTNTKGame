using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private EnemyAI enemyAI;

    public Animator animator;

    public SpriteRenderer characterSR;

    public int currentHealth = 100;
    private void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
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

        Debug.Log(gameObject.name +
                  " nhận " +
                  damage +
                  " sát thương");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
        enemyAI.target = null;
        Destroy(gameObject, 3f);
    }
}