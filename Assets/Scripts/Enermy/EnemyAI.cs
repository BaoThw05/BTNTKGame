using UnityEngine;
using Pathfinding;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Seeker seeker;
    public SpriteRenderer characterSR;
    public Rigidbody2D rb;
    public Transform target;
    Path path;
    public Animator animator;
    Coroutine moveCoroutine;

    [System.Serializable]
    public struct ShootSettings
    {
        public bool isShootable;
        public GameObject bullet;
        public float bulletSpeed;
        public float timeBtwFire;
    }
    private float fireCooldown;

    public ShootSettings shoot;

    [System.Serializable]
    public struct WalkSettings
    {
        public bool isWalkable;
        public float moveSpeed;
        public float nextWPDistance;
        public float stopDistance;
    }

    public WalkSettings walk;
    [Header("Detection")]
    public float detectRange = 10f;

    private void Start()
    {
        FindPlayerTarget();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogWarning("EnemyAI: Rigidbody2D missing — movement may pass through colliders. Add a Rigidbody2D to this enemy and set Body Type to Dynamic.");

        // Chỉ lặp lại tính toán đường đi nếu có khả năng đi bộ
        if (walk.isWalkable)
            InvokeRepeating("CalculatePath", 0f, 0.5f);
    }

    private void Update()
    {
        if (target != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, target.position);
            if (distanceToPlayer > detectRange)
            {
                ClearTarget();
                return;
            }
        }

        if (target == null)
        {
            FindPlayerTarget();
            return;
        }

        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null && playerHealth.isDead)
        {
            // Nếu player chết, dừng animation chạy
            if (animator != null) animator.SetFloat("speed", 0f);
            return;
        }

        if (shoot.isShootable && shoot.bullet != null)
        {
            fireCooldown -= Time.deltaTime;
            if (fireCooldown < 0)
            {
                fireCooldown = shoot.timeBtwFire;
                EnemyFireBullet();
            }
        }
    }

    // Hàm bổ trợ tự động tìm kiếm Player an toàn trong phạm vi detectRange
    void FindPlayerTarget()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerObj.transform.position);
        if (distanceToPlayer <= detectRange)
        {
            target = playerObj.transform;
        }
    }

    private void ClearTarget()
    {
        target = null;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        if (animator != null)
            animator.SetFloat("speed", 0f);
    }

    void EnemyFireBullet()
    {
        if (shoot.bullet == null || target == null) return;

        var bulletTmp = Instantiate(shoot.bullet, transform.position, Quaternion.identity);
        Rigidbody2D rb = bulletTmp.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector3 playerPos = target.position;
            Vector3 direction = playerPos - transform.position;
            rb.AddForce(direction.normalized * shoot.bulletSpeed, ForceMode2D.Impulse);
        }
    }

    void CalculatePath()
    {
        if (target == null || seeker == null) return;

        if (Vector2.Distance(transform.position, target.position) <= walk.stopDistance)
            return;

        if (seeker.IsDone())
        {
            seeker.StartPath(transform.position, target.position, OnPathCallBack);
        }
    }

    void OnPathCallBack(Path p)
    {
        if (p.error) return;
        path = p;
        MoveToTarget();
    }

    void MoveToTarget()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveToTargetCoroutine());
    }

    IEnumerator MoveToTargetCoroutine()
    {
        if (path == null || animator == null) yield break;

        int currentWP = 0;
        Vector2 lastDir = Vector2.down;

        while (currentWP < path.vectorPath.Count)
        {
            if (target == null) yield break;

            float distToPlayer = Vector2.Distance(transform.position, target.position);

            if (distToPlayer <= walk.stopDistance)
            {
                animator.SetFloat("speed", 0f);
                animator.SetFloat("move_X", lastDir.x);
                animator.SetFloat("move_Y", lastDir.y);
                yield return null;
                break;
            }

            Vector2 waypoint = path.vectorPath[currentWP];
            Vector2 direction = (waypoint - (Vector2)transform.position);
            float distance = direction.magnitude;
            Vector2 dir = direction.normalized;

            Vector2 velocity = dir * walk.moveSpeed * Time.deltaTime;
            if (rb != null)
            {
                rb.MovePosition(rb.position + velocity);
            }
            else
            {
                transform.position += (Vector3)velocity;
            }

            if (velocity.magnitude > 0.001f)
            {
                lastDir = dir;
            }

            animator.SetFloat("move_X", lastDir.x);
            animator.SetFloat("move_Y", lastDir.y);
            animator.SetFloat("speed", 1f);

            if (characterSR != null)
            {
                if (lastDir.x > 0.05f)
                    characterSR.flipX = false;
                else if (lastDir.x < -0.05f)
                    characterSR.flipX = true;
            }

            if (distance < walk.nextWPDistance)
            {
                currentWP++;
            }

            yield return null;
        }

        animator.SetFloat("speed", 0f);
    }
}