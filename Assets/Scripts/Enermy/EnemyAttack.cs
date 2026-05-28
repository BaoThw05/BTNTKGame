using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack")]
    public int damage = 10;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    private float lastAttackTime;

    [Header("References")]
    public Transform attackPointUp;
    public Transform attackPointDown;
    public Transform attackPointLeft;
    public Transform attackPointRight;

    private Transform currentAttackPoint;

    public LayerMask playerLayer;
    public Animator animator;
    public Transform target;

    bool isAttacking;
    bool hasDealtDamage;

    private void Start()
    {
        FindPlayerTarget();
        // SỬA LỖI 1: Gán điểm tấn công mặc định ban đầu tránh bị Null vị trí (0,0)
        currentAttackPoint = attackPointDown != null ? attackPointDown : transform;
    }

    private void Update()
    {
        if (target == null)
        {
            FindPlayerTarget();
            return;
        }

        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null && playerHealth.isDead)
            return;

        // SỬA LỖI 2 (BẢO HIỂM): Nếu bị kẹt trạng thái tấn công (do Animation Event lỗi không chạy),
        // tự động reset trạng thái sau khi hết thời gian Cooldown để quái tiếp tục đánh.
        if (isAttacking && Time.time > lastAttackTime + attackCooldown)
        {
            EndAttack();
        }

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= attackRange)
        {
            TryAttack();
        }
    }

    void FindPlayerTarget()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
    }

    void TryAttack()
    {
        if (isAttacking || animator == null || target == null)
            return;

        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;
        isAttacking = true;
        hasDealtDamage = false; // Reset trạng thái gây sát thương cho đòn mới

        Vector2 dir = (target.position - transform.position).normalized;
        UpdateAttackDirection(dir);

        animator.SetTrigger("Attack");

        // Gọi trực tiếp DealDamage để đảm bảo gây sát thương ngay cả khi
        // Animation Event không chạy hoặc không có animator.
        DealDamage();
        // Kết thúc đòn ngay lập tức; cooldown vẫn áp dụng dựa trên lastAttackTime
        EndAttack();
    }

    // SỬA LỖI 3 (QUAN TRỌNG): Hàm này được gọi từ Animation Event của Unity
    public void DealDamage()
    {
        if (hasDealtDamage || currentAttackPoint == null) return;

        hasDealtDamage = true;

        // Thử dùng LayerMask trước
        Collider2D player = Physics2D.OverlapCircle(
            currentAttackPoint.position,
            attackRange,
            playerLayer
        );

        if (player != null)
        {
            PlayerHealth pHealth = player.GetComponent<PlayerHealth>();
            if (pHealth != null)
            {
                pHealth.TakeDamage(damage);
                return;
            }
            player.GetComponentInParent<PlayerHealth>()?.TakeDamage(damage);
            return;
        }

        // Fallback: nếu LayerMask không tìm được, thử tìm Player theo tag và kiểm tra khoảng cách
        GameObject pObj = GameObject.FindGameObjectWithTag("Player");
        if (pObj != null)
        {
            float dist = Vector2.Distance(pObj.transform.position, currentAttackPoint.position);
            if (dist <= attackRange)
            {
                PlayerHealth pHealth = pObj.GetComponent<PlayerHealth>();
                pHealth?.TakeDamage(damage);
            }
        }
    }

    // Hàm này được gọi từ Animation Event ở Frame cuối của hoạt ảnh Attack
    public void EndAttack()
    {
        isAttacking = false;
        hasDealtDamage = false;
    }

    // Vẽ vòng tròn đỏ trực quan trong Scene để test tầm đánh dễ dàng
    private void OnDrawGizmos()
    {
        if (currentAttackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(currentAttackPoint.position, attackRange);
    }

    void UpdateAttackDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0)
                currentAttackPoint = attackPointRight ?? currentAttackPoint;
            else
                currentAttackPoint = attackPointLeft ?? currentAttackPoint;
        }
        else
        {
            if (dir.y > 0)
                currentAttackPoint = attackPointUp ?? currentAttackPoint;
            else
                currentAttackPoint = attackPointDown ?? currentAttackPoint;
        }
    }
}