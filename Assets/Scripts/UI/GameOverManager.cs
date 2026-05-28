using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void ClickYes()
    {
        Debug.Log("🔄 Đang tải lại màn chơi...");
        Time.timeScale = 1f; 
        SceneManager.LoadScene(1);
    }
    public void ClickNo()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "data.json");

        Debug.Log($"🔍 Đường dẫn file dữ liệu: {filePath}");

        // 2. Kiểm tra xem file có thực sự tồn tại hay không trước khi xóa
        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                Debug.Log("🗑️ Đã xóa file data.json thành công!");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ Lỗi khi xóa file dữ liệu: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ Không tìm thấy file data.json để xóa (Có thể file chưa từng được tạo).");
        }
        Debug.Log("🔄 Đang tải lại menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}