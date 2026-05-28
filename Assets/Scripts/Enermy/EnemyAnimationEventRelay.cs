using UnityEngine;

public class EnemyAnimationEventRelay : MonoBehaviour
{
	public EnemyAttack enemyAttack;

	public void DealDamage()
	{
		enemyAttack.DealDamage();
	}

	public void EndAttack()
	{
		enemyAttack.EndAttack();
	}
}