using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerSelect : MonoBehaviour
{
    private int index;
    [SerializeField] private GameObject[] playerModels;
    [SerializeField] GameObject[] playerPrefabs;
    public static int selectedPlayer;
    void Start()
    {
        SelectCharacter(index);
    }

    public void ExitGame()
    {
        Debug.Log("Thoát game!");
        Application.Quit(); // Lệnh này chỉ chạy khi đã xuất ra file .exe
    }
    public void StartGame()
    {
        // Kiểm tra xem bạn đã chọn nhân vật chưa trước khi đi
        int selected = PlayerPrefs.GetInt("SavedCharacter", 1);
        Debug.Log("Đang chuyển sang cảnh playmove với nhân vật số: " + selected);

        // Lệnh chuyển sang Scene tên là playmove
        SceneManager.LoadScene(1);
    }
    public void SelectCharacter(int selectedIndex) // Đổi tên tham số để tránh nhầm với biến global
    {
        // Duyệt qua tất cả nhân vật trong mảng
        for (int i = 0; i < playerModels.Length; i++)
        {
            if (i == selectedIndex)
            {
                // Nhân vật được chọn: Hiện màu trắng và chạy animation
                playerModels[i].GetComponent<Image>().color = Color.white;
                playerModels[i].GetComponent<Animator>().enabled = true;
                selectedPlayer = i;
            }
            else
            {
                // Nhân vật không được chọn: Hiện màu đen và tắt animation
                playerModels[i].GetComponent<Image>().color = Color.black;
                playerModels[i].GetComponent<Animator>().enabled = false;
            }
        }

    }
}
