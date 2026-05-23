using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject targetPanel; 

    // Hàm này sẽ được gọi khi bấm Button
    public void TogglePanel()
    {
        if (targetPanel != null)
        {
            // Lấy trạng thái hiện tại của Panel và đảo ngược nó lại
            // Nếu đang hiện (true) -> ẩn (false) và ngược lại
            bool currentState = targetPanel.activeSelf;
            targetPanel.SetActive(!currentState);
        }
    }
}
