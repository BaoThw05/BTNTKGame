using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

public class LocalTeleport : MonoBehaviour
{
    public enum Direction { Up, Down, Left, Right, Teleport }

    CinemachineConfiner confider;

    private void Awake()
    {
        confider = FindFirstObjectByType<CinemachineConfiner>();
    }

    [Header("Cấu hình di chuyển")]
    public Direction direction;

    [Header("Điểm Đến")]
    [Tooltip("Kéo thả GameObject bạn muốn nhân vật bay tới vào đây")]
    public Transform teleportTargetPosition;

    [Header("Khóa cửa")]
    public bool needKey = false;
    public bool consumeKeyOnUse = true;

    private bool isCooldown = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Kiểm tra nếu không phải Player hoặc đang trong quá trình dịch chuyển thì bỏ qua
        if (!collision.CompareTag("Player") || isCooldown) return;

        // 2. Kiểm tra chìa khóa TRƯỚC KHI làm bất cứ hiệu ứng nào
        if (needKey)
        {
            PlayerInventory inventory = collision.GetComponent<PlayerInventory>();

            if (inventory == null)
            {
                Debug.LogError("Nhân vật thiếu script PlayerInventory!");
                return;
            }

            // Nếu cửa khóa và người chơi không có chìa -> Chặn luôn tại đây
            if (!inventory.holdingKey)
            {
                Debug.Log("❌ Cửa khóa rồi! Bạn cần tìm và cầm theo chìa khóa.");
                return;
            }

            // Nếu có chìa và cấu hình dùng xong mất chìa
            if (consumeKeyOnUse)
            {
                inventory.holdingKey = false;
                inventory.hasKey = false;
                Debug.Log("🔑 Chìa khóa đã được sử dụng và biến mất!");
            }
        }

        // 3. Nếu mọi điều kiện đều Thỏa mãn -> Kích hoạt chuỗi dịch chuyển kèm Fade
        Debug.Log("✔ Qua cửa thành công! Bắt đầu dịch chuyển...");
        ExecuteTeleportSequence(collision.gameObject);
    }

    // Hàm bất đồng bộ điều phối toàn bộ quá trình Fade và dịch chuyển theo đúng thứ tự time-line
    private async void ExecuteTeleportSequence(GameObject targetPlayer)
    {
        isCooldown = true; // Bật Cooldown để chặn người chơi spam va chạm liên tục làm lỗi Fade

        // Bước A: Chờ màn hình FADE OUT (Tối đen hoàn toàn)
        if (screenFade.instance != null)
        {
            await screenFade.instance.FadeOut();
        }

        // Bước B: Chỉ dịch chuyển vị trí nhân vật KHI màn hình ĐÃ tối đen
        UpdatePlayerPosition(targetPlayer);

        // Bước C: Chờ thêm 1.5 giây trong bóng tối (tạo cảm giác chuyển cảnh mượt mà)
        await Task.Delay(1500);

        // Bước D: Mở hiệu ứng FADE IN (Màn hình sáng trở lại)
        if (screenFade.instance != null)
        {
            await screenFade.instance.FadeIn();
        }

        isCooldown = false; // Mở lại để có thể dùng cho lần sau
    }

    private void UpdatePlayerPosition(GameObject targetPlayer)
    {
        if (direction == Direction.Teleport)
        {
            if (teleportTargetPosition != null && targetPlayer != null)
            {
                targetPlayer.transform.position = teleportTargetPosition.position;
                Debug.Log("Đã chuyển vị trí tới: " + teleportTargetPosition.name);
            }
        }
    }
}