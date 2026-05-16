using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private InventoryController inventoryController;

    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
        //animator = GetComponentInChildren<Animator>();
    }
    private void Awake()
    {
        inventoryController = FindObjectOfType<InventoryController>(); // hoặc GetComponentInParent nếu cùng object

        if (inventoryController == null)
        {
            Debug.LogError("Không tìm thấy InventoryController!");
        }        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //animator.SetTrigger("PickTrigger");

        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
               bool added = inventoryController.AddItem(collision.gameObject);
                if (added)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
