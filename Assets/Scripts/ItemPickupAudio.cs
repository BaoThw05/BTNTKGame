using UnityEngine;

public class ItemPickupAudio : MonoBehaviour
{
    [Header("Âm thanh khi bị nhặt")]
    // Kéo tiếng "Keng", "Ching" tiếng nhặt đồ vào đây
    public AudioClip pickupSound;

    // Biến để đảm bảo chỉ kêu 1 lần
    private bool wasPickedUp = false;

    // Khi Player chạm vào vật thể này
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem vật thể chạm vào có Tag là "Player" không
        if (!wasPickedUp && collision.CompareTag("Player"))
        {
            wasPickedUp = true; // Đánh dấu đã nhặt để tránh kêu 2 lần

            // Phát âm thanh tại vị trí này. Unity tự tạo loa ảo và tự xóa khi kêu xong.
            // Nhờ vậy, dù Object này bị Destroy ngay sau đó, tiếng kêu vẫn vang lên đủ.
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // --- Thêm code xử lý nhặt đồ của bạn ở đây (VD: Cộng điểm...) ---
            Debug.Log("Đã nhặt: " + gameObject.name);

            // Xóa vật phẩm khỏi màn hình sau 0.1 giây
            Destroy(gameObject, 0.1f);
        }
    }
}