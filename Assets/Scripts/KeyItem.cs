using UnityEngine;

public class KeyItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory =
                other.GetComponent<PlayerInventory>();

            inventory.hasKey = true;
            inventory.holdingKey = true;

            Destroy(gameObject);

            Debug.Log("Đã nhặt chìa khóa");
        }
    }
}