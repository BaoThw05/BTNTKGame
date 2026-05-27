using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ChangeLayerOnTrigger : MonoBehaviour
{
    [Header("Thành phần cần đổi Layer (Kéo Sprite hoặc Tilemap vào đây)")]
    public Renderer targetRenderer;

    [Header("Thiết lập Order in Layer (Số thứ tự hiển thị)")]
    [Tooltip("Số này phải THẤP HƠN Order in Layer của Player để Player đè lên")]
    public int orderWhenPlayerInside = -5;

    [Tooltip("Số mặc định của vật thể khi Player chưa đi vào")]
    public int normalOrder = 5;

    private void Start()
    {
        // Tự động tìm Renderer trên chính Object này nếu bạn quên gán ở Inspector
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Khi Player bước vào vùng Collider
        if (other.CompareTag("Player"))
        {
            if (targetRenderer != null)
            {
                targetRenderer.sortingOrder = orderWhenPlayerInside;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Khi Player bước ra khỏi vùng Collider
        if (other.CompareTag("Player"))
        {
            if (targetRenderer != null)
            {
                targetRenderer.sortingOrder = normalOrder;
            }
        }
    }
}
