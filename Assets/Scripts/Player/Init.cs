using Unity.Cinemachine;
using UnityEngine;

public class Init : MonoBehaviour
{
    [SerializeField] private GameObject[] playerModels;
    public Transform spawnPoint;
        [SerializeField] private CinemachineCamera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Lấy Prefab nhân vật được chọn
        GameObject selectedPlayer = playerModels[PlayerSelect.selectedPlayer];

        // Sinh ra nhân vật trên Scene và LƯU LẠI vào biến playerInstance
        GameObject playerInstance = Instantiate(selectedPlayer, spawnPoint.position, Quaternion.identity);
        playerInstance.tag = "Player";
        SaveController save = FindObjectOfType<SaveController>();
        if (save != null)
        {
            save.LoadGame(playerInstance);
        }
        // Gán Camera đi theo nhân vật vừa được sinh ra trên Scene
        if (cam != null)
        {
            cam.Follow = playerInstance.transform;
        }
    }

}
 