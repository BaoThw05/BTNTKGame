using UnityEngine;

public class LocalTeleport : MonoBehaviour
{
    [Header("Điểm Đến")]
    [Tooltip("Kéo thả GameObject bạn muốn nhân vật bay tới (ví dụ: RemoveHome) vào đây")]
    public Transform destination;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Dòng này sẽ in ra tên của bất kỳ vật gì chạm vào cửa
        Debug.Log("Có vật chạm vào cửa: " + other.gameObject.name + " | Tag của nó là: " + other.tag);

        if (other.CompareTag("Player"))
        {
            if (destination != null)
            {
                other.transform.position = destination.position;
            }
        }
    }
}