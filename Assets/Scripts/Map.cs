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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Chỉ gọi duy nhất hàm chuỗi bất đồng bộ này
            FadeTrans(collision.gameObject);
        }
    }

    async void FadeTrans(GameObject targetPlayer)
    {
        // 1. Chờ màn hình FADE OUT (Tối đen hoàn toàn)
        await screenFade.instance.FadeOut();
        UpdatePlayerPosition(targetPlayer);
        await Task.Delay(1500);

        await screenFade.instance.FadeIn();
    }

    private void UpdatePlayerPosition(GameObject targetPlayer)
    {
        if (direction == Direction.Teleport)
        {
            if (teleportTargetPosition != null && targetPlayer != null)
            {
                targetPlayer.transform.position = teleportTargetPosition.position;
                Debug.Log("Dịch chuyển thành công tới: " + teleportTargetPosition.name);
            }
        }
    }
}