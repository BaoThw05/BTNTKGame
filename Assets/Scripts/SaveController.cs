using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    private HotBarController hotBarController;
    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "save.json");
        inventoryController = FindObjectOfType<InventoryController>();
        hotBarController = FindObjectOfType<HotBarController>();
    }

    public void SaveGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Không tìm thấy Player -> chưa spawn hoặc sai tag!");
            return;
        }
        string boundaryName = "";
        CinemachineConfiner confiner = FindObjectOfType<CinemachineConfiner>();
        if (confiner != null && confiner.m_BoundingShape2D != null)
        {
            boundaryName = confiner.m_BoundingShape2D.gameObject.name;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy CinemachineConfiner hoặc BoundingShape2D!");
        }
        List<InventorySaveData> invData = new List<InventorySaveData>();
        List<InventorySaveData> hotBarData = new List<InventorySaveData>();
        if (inventoryController != null && hotBarController != null)
        {
            invData = inventoryController.GetInventoryItem();
            hotBarData = hotBarController.GetHotBarItem();
        }
        else
        {
            Debug.LogError("InventoryController hoặc HotBarController bị null! Hãy chắc chắn script này có mặt trong Scene.");
        }
        SaveData data = new SaveData
        {
            playerPosition = player.transform.position,
            mapBoundary = boundaryName,
            inventorySaveData = invData,
            hotBarSaveData = hotBarData
        };
        File.WriteAllText(saveLocation, JsonUtility.ToJson(data));
        Debug.Log("Save OK");
    }

    public void LoadGame(GameObject player)
    {
        if (File.Exists(saveLocation))
        {
            string json = File.ReadAllText(saveLocation);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            inventoryController.SetInventoryItem(data.inventorySaveData);
            hotBarController.SetHotBarItem(data.hotBarSaveData);
            player.transform.position = data.playerPosition;
            Debug.Log("Load về: " + data.playerPosition);
        }
        else { SaveGame(); }
    }

}
