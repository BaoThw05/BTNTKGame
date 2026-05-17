using UnityEngine;
using Pathfinding;
using System.Collections;
using Unity.VisualScripting;
public class EnemyAI : MonoBehaviour
{
	public Seeker seeker;
	public SpriteRenderer characterSR;
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
	private void Start()
	{
		//target = GameObject.FindGameObjectWithTag("Player").transform;
		if (walk.isWalkable)
			InvokeRepeating("CalculatePath", 0f, 0.2f);
	}
    private void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

		if (shoot.isShootable)
		{
			fireCooldown -= Time.deltaTime;

			if (fireCooldown < 0)
			{
				fireCooldown = shoot.timeBtwFire;
				//Shoot
				EnemyFireBullet();
			}
		}
	}
	void EnemyFireBullet()
	{
		var bulletTmp = Instantiate(shoot.bullet, transform.position, Quaternion.identity);

		Rigidbody2D rb = bulletTmp.GetComponent<Rigidbody2D>();
		Vector3 playerPos = target.position;
		Vector3 direction = playerPos - transform.position;
		rb.AddForce(direction.normalized * shoot.bulletSpeed, ForceMode2D.Impulse);
	}
	void CalculatePath()
    {
		if (target == null) return;

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
        int currentWP = 0;

		Vector2 lastDir = Vector2.down; // hướng mặc định

		while (currentWP < path.vectorPath.Count)
		{
			float distToPlayer = Vector2.Distance(transform.position, target.position);

			if (distToPlayer <= walk.stopDistance)
			{
				// dừng animation
				animator.SetFloat("speed", 0f);

				// giữ hướng cuối để idle/attack đúng hướng
				animator.SetFloat("move_X", lastDir.x);
				animator.SetFloat("move_Y", lastDir.y);
				//animator.SetTrigger("Attack");

				yield return null;
				break;
			}

			Vector2 waypoint = path.vectorPath[currentWP];
			Vector2 direction = (waypoint - (Vector2)transform.position);

			float distance = direction.magnitude;

			// Normalize direction để dùng cho animation
			Vector2 dir = direction.normalized;

			// 👉 Di chuyển
			Vector2 velocity = dir * walk.moveSpeed * Time.deltaTime;
			transform.position += (Vector3)velocity;

			// 👉 Chỉ update hướng khi đang di chuyển
			if (velocity.magnitude > 0.001f)
			{
				lastDir = dir;
			}

			// 👉 Animator (QUAN TRỌNG)
			animator.SetFloat("move_X", lastDir.x);
			animator.SetFloat("move_Y", lastDir.y);
			animator.SetFloat("speed", velocity.magnitude > 0.001f ? 1f : 0f);

			// 👉 Flip bằng SpriteRenderer (KHÔNG dùng scale)
			if (lastDir.x > 0.05f)
				characterSR.flipX = false;
			else if (lastDir.x < -0.05f)
				characterSR.flipX = true;

			// 👉 waypoint tiếp theo
			if (distance < walk.nextWPDistance)
			{
				currentWP++;
			}

			yield return null;
		}

		// 👉 Khi dừng hẳn
		animator.SetFloat("speed", 0f);
	}
}
