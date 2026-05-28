using UnityEngine;

public class BossAnimationEventRelay : MonoBehaviour
{
	public BossMeleeAttack bossMeleeAttack;

	public void DealDamage()
	{
		bossMeleeAttack.DealDamage();
	}

	public void EndAttack()
	{
		bossMeleeAttack.EndAttack();
	}
}