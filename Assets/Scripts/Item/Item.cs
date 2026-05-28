using UnityEngine;
public class Item : MonoBehaviour
{
    public int ID;
    public string Name;
    [Range(0f, 100f)]
    public float dropChance;
    [Header("Cấu hình Random số lượng khi rơi")]
    public int minQuantity = 1;
    public int maxQuantity = 1;
    public virtual void UseItem()
    {
        Debug.Log("Using item: " + Name);
    }
}
