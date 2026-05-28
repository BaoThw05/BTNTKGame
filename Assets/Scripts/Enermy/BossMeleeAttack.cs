using UnityEngine;

public class BossMeleeAttack : MonoBehaviour
{
    [Header("Attack")]
    public int damage = 10;
    public float detectRange = 3f;
    public float hitRange = 1f;
    public float attackCooldown = 2f;

    float lastAttackTime;

    //------------------------------------------------

    [Header("References")]
    public Transform attackPointLeft;
    public Transform attackPointRight;

    Transform currentAttackPoint;

    public LayerMask playerLayer;

    public Animator animator;

    public Transform target;

    //------------------------------------------------

    bool isAttacking;
    bool hasDealtDamage;

    //------------------------------------------------
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        if (target == null)
            return;

        PlayerHealth playerHealth =
            target.GetComponent<PlayerHealth>();

        if (playerHealth != null && playerHealth.isDead)
            return;

        float distance =
            Vector2.Distance(
                transform.position,
                target.position
            );

        if (distance <= detectRange)
        {
            TryAttack();
        }
    }

    //------------------------------------------------

    public void TryAttack()
    {
        if (isAttacking)
            return;

        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        isAttacking = true;
        hasDealtDamage = false;

        UpdateAttackDirection();

        animator.SetTrigger("Attack");

        // SỬA LỖI: Gọi trực tiếp DealDamage để đảm bảo gây sát thương
        // ngay cả khi Animation Event không chạy hoặc fail
        DealDamage();

        // Kết thúc đòn ngay lập tức
        EndAttack();
    }

    //------------------------------------------------

    void UpdateAttackDirection()
    {
        if (target.position.x > transform.position.x)
        {
            currentAttackPoint = attackPointRight;

            animator.SetFloat("MoveX", 1);
        }
        else
        {
            currentAttackPoint = attackPointLeft;

            animator.SetFloat("MoveX", -1);
        }
    }

    //------------------------------------------------
    // Animation Event

    public void DealDamage()
    {
        if (hasDealtDamage || currentAttackPoint == null)
            return;

        hasDealtDamage = true;

        Debug.Log("[Boss] DealDamage tại " + currentAttackPoint.position);

        // Thử tìm bằng LayerMask trước
        Collider2D player = Physics2D.OverlapCircle(
            currentAttackPoint.position,
            hitRange,
            playerLayer
        );

        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth == null)
            {
                playerHealth = player.GetComponentInParent<PlayerHealth>();
            }

            if (playerHealth != null)
            {
                Debug.Log($"[Boss] Tấn công Player, gây {damage} sát thương");
                playerHealth.TakeDamage(damage);
            }
            return;
        }

        // Fallback: Tìm Player theo tag nếu LayerMask fail
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            float dist = Vector2.Distance(playerObj.transform.position, currentAttackPoint.position);
            if (dist <= hitRange)
            {
                PlayerHealth playerHealth = playerObj.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    Debug.Log($"[Boss] Fallback hit Player, gây {damage} sát thương");
                    playerHealth.TakeDamage(damage);
                }
            }
        }
    }

    //------------------------------------------------
    // Animation Event

    public void EndAttack()
    {
        isAttacking = false;
        hasDealtDamage = false;
    }

    //------------------------------------------------

    private void OnDrawGizmosSelected()
    {
        if (currentAttackPoint == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            currentAttackPoint.position,
            hitRange
        );

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            detectRange
        );
    }
}