using UnityEngine;
using Pathfinding;
using System.Collections;
using Unity.VisualScripting;
public class EnemyAI : MonoBehaviour
{
    public float moveSpeed;
    public float nextWPDistance;
    public Seeker seeker;
    public SpriteRenderer characterSR;
    public Transform target;
    Path path;
	public Animator anim;

	Coroutine moveCoroutine;
	private void Start()
	{
		//target = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("CalculatePath", 0f, 0.5f);
	}
    private void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void CalculatePath()
    {
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

        while (currentWP < path.vectorPath.Count)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWP] - (Vector2)transform.position).normalized;

			Vector2 velocity = direction * moveSpeed;
			anim.SetFloat("move_X", velocity.x);
			anim.SetFloat("move_Y", velocity.y);
            anim.SetBool("X > Y", Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y));

			Vector3 force = direction * moveSpeed * Time.deltaTime;
            transform.position += force;

            float distance = Vector2.Distance(transform.position, path.vectorPath[currentWP]);
            if (distance < nextWPDistance) 
            {
                currentWP++;
            }

            if (force.x != 0)
                if (force.x < 0)
                    characterSR.transform.localScale = new Vector3(-1, 1, 1);
                else
                    characterSR.transform.localScale = new Vector3(1, 1, 1);
			yield return null;
		}
    }
}
