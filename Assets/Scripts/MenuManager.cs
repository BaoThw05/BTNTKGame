using UnityEngine;
using UnityEngine.SceneManagement; // Dùng để chuyển cảnh

public class MenuManager : MonoBehaviour
{
    public static bool isPaused = false;

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    public void StartGame()
    {
        // Kiểm tra xem bạn đã chọn nhân vật chưa trước khi đi
        int selected = PlayerPrefs.GetInt("SavedCharacter", 1);
        Debug.Log("Đang chuyển sang cảnh playmove với nhân vật số: " + selected);
        SceneManager.LoadScene(1);
    }
    // Hàm này gọi khi nhấn vào vùng nhân vật hoặc nút mũi tên chọn

    // Hàm cho nút EXIT
    public void ExitGame()
    {
        Debug.Log("Thoát game!");
        Application.Quit(); // Lệnh này chỉ chạy khi đã xuất ra file .exe
    }
}