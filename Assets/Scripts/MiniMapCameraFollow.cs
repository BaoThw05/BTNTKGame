using UnityEngine;

public class MiniMapCameraFollow : MonoBehaviour
{
    [Header("Code sẽ tự tìm Tag 'Player' khi chạy game")]
    public Transform player;

    void Start()
    {
        // Tự động tìm nhân vật trong Map nếu ô Player đang bị trống
        if (player == null)
        {
            // Tìm đối tượng có gắn Tag là "Player"
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void LateUpdate()
    {
        // Nếu trong lúc chơi game vẫn chưa tìm thấy nhân vật thì liên tục quét lại để tránh lỗi
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
            return;
        }

        // Khóa vị trí Camera di chuyển bám theo tọa độ X, Y của nhân vật
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}