using UnityEngine;
using System.Collections.Generic;
public class ItemDictionary : MonoBehaviour
{
    public List<Item> itemPrefabs;
    private Dictionary<int, GameObject> itemDictionary;
    private void Awake()
    {
        itemDictionary = new Dictionary<int, GameObject>();
        for(int i = 0; i < itemPrefabs.Count; i++)
        {
             if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].ID = i+1;
            }
        }
        foreach (Item item in itemPrefabs)
        {
            itemDictionary[item.ID] = item.gameObject;
        }

    }
    public GameObject GetItemPrefabByID(int id)
    {
        itemDictionary.TryGetValue(id, out GameObject prefab);
        if(prefab == null)
        {
            Debug.LogWarning("Item with ID " + id + " not found in the dictionary.");
        }
        return prefab;
    }
}


