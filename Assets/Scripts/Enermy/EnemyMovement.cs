using UnityEngine;
using Pathfinding;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public Seeker seeker;
    public Transform target;

    public Animator animator;
    public SpriteRenderer sr;

    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float stopDistance = 1.5f;
    public float nextWPDistance = 0.5f;

    private Path path;
    private Coroutine moveCoroutine;

    void Start()
    {
        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
    }
    private void Update()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
                target = player.transform;

            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, target.position);
        if (distanceToPlayer > detectionRange)
        {
            animator?.SetFloat("speed", 0);
            return;
        }
    }
    void UpdatePath()
    {
        if (target == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, target.position);
        if (distanceToPlayer > detectionRange)
            return;

        if (distanceToPlayer <= stopDistance)
            return;

        if (seeker.IsDone())
            seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (p.error) return;

        path = p;

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        int index = 0;
        Vector2 lastDir = Vector2.down;

        while (index < path.vectorPath.Count)
        {
            float dist = Vector2.Distance(transform.position, target.position);

            if (dist <= stopDistance)
            {
                animator.SetFloat("speed", 0);
                yield break;
            }

            Vector2 dir = ((Vector2)path.vectorPath[index] - (Vector2)transform.position).normalized;

            transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);

            lastDir = dir;

            animator.SetFloat("move_X", dir.x);
            animator.SetFloat("move_Y", dir.y);
            animator.SetFloat("speed", 1);

            if (Vector2.Distance(transform.position, path.vectorPath[index]) < nextWPDistance)
                index++;

            yield return null;
        }

        animator.SetFloat("speed", 0);
    }
}