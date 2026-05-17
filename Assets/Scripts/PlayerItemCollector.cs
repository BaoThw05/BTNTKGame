using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerItemCollector : MonoBehaviour
{
    private Animator animator;
    private HotBarController hotBarController;
    private InventoryController inventoryController;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }

        // Tìm Inventory Controller
        inventoryController = FindObjectOfType<InventoryController>();
        if (inventoryController == null)
        {
            Debug.LogError("Không tìm thấy InventoryController!");
        }
        hotBarController = FindObjectOfType<HotBarController>();
        if (hotBarController == null)
            {
            Debug.LogError("Không tìm thấy HotBarController!");
        }
        // Tìm Animator (sẽ tự động tìm trên object hiện tại hoặc các object con)
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Không tìm thấy Animator trên nhân vật!");
        }
    }

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null && /*inventoryController != null &&*/ hotBarController != null)
            {
                bool hotBarAdded = hotBarController.AddItem(collision.gameObject);
                if (hotBarAdded)
                {
                    // Nếu thêm vào hotbar thành công, thì không cần thêm vào inventory nữa
                    // Tắt va chạm của bản gốc dưới đất để lúc chờ animation không bị lụm đúp
                    collision.enabled = false;

                    if (animator != null)
                    {
                        animator.SetTrigger("PickTrigger");
                        await Task.Delay(400);
                    }

                    if (collision != null)
                    {
                        Destroy(collision.gameObject); // Chờ xong thì xóa bản gốc dưới đất
                    }
                    return; // Kết thúc hàm, không cần thêm vào inventory
                }
                else
                {
                    // BƯỚC 1: Cứ để nguyên va chạm, đưa vào túi đồ trước để nó Instantiate bản sao hoàn hảo
                    bool added = inventoryController.AddItem(collision.gameObject);

                    // BƯỚC 2: Nếu đưa vào túi thành công, LÚC NÀY mới tắt va chạm của bản gốc dưới đất
                    if (added)
                    {
                        // Tắt va chạm của bản gốc dưới đất để lúc chờ animation không bị lụm đúp
                        collision.enabled = false;

                        if (animator != null)
                        {
                            animator.SetTrigger("PickTrigger");
                            await Task.Delay(400);
                        }

                        if (collision != null)
                        {
                            Destroy(collision.gameObject); // Chờ xong thì xóa bản gốc dưới đất
                        }
                    }
                    // Nếu túi đầy (added = false) thì không làm gì cả, va chạm vẫn bật, lát quay lại lụm tiếp
                }
            }
        }
    }
}