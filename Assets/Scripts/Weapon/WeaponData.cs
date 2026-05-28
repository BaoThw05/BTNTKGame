using UnityEngine;

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    public Sprite weaponSprite;

    public int damage;

    public float attackRange;

    public LayerMask enemyLayer;
}