using UnityEngine;

public class MapManager : MonoBehaviour
{
    // Kéo Object FullMapUI từ Hierarchy vào ô này trong Inspector
    public GameObject fullMapPanel;

    // Hàm này sẽ chạy khi bấm vào bản đồ nhỏ
    public void ToggleFullMap()
    {
        if (fullMapPanel != null)
        {
            // Nếu đang mở thì tắt, đang tắt thì mở
            bool currentState = fullMapPanel.activeSelf;
            fullMapPanel.SetActive(!currentState);
        }
    }
}