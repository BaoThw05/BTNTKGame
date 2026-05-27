using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public ItemDictionary itemDictionary;
    public Chest[] chests;

    [Header("Random chest generation")]
    public int defaultMinItemsPerChest = 1;
    public int defaultMaxItemsPerChest = 3;
    public bool generateOnStart = true;
    public bool useDropChance = true;

    private void Start()
    {
        if (generateOnStart)
        {
            GenerateAllChests();
        }
    }

    [ContextMenu("Generate All Chests")]
    public void GenerateAllChests()
    {
        if (itemDictionary == null)
        {
            Debug.LogWarning("ChestManager: ItemDictionary is not assigned.");
            return;
        }

        if (chests == null || chests.Length == 0)
        {
            chests = FindObjectsOfType<Chest>();
        }

        if (chests == null || chests.Length == 0)
        {
            Debug.LogWarning("ChestManager: No Chest objects found in the scene.");
            return;
        }

        foreach (Chest chest in chests)
        {
            GenerateChest(chest);
        }
    }

    public void GenerateChest(Chest chest)
    {
        if (chest == null)
        {
            return;
        }

        if (itemDictionary.itemPrefabs == null || itemDictionary.itemPrefabs.Count == 0)
        {
            Debug.LogWarning("ChestManager: ItemDictionary has no item prefabs.");
            return;
        }

        chest.ClearGeneratedItems();

        int minItems = Mathf.Max(defaultMinItemsPerChest, chest.minItemCount);
        int maxItems = Mathf.Max(defaultMaxItemsPerChest, chest.maxItemCount);
        int itemCount = Random.Range(minItems, maxItems + 1);

        for (int i = 0; i < itemCount; i++)
        {
            Item selected = GetRandomItem(itemDictionary.itemPrefabs);
            if (selected == null)
            {
                continue;
            }

            int quantity = Random.Range(Mathf.Max(1, selected.minQuantity), Mathf.Max(1, selected.maxQuantity) + 1);
            for (int q = 0; q < quantity; q++)
            {
                GameObject itemInstance = Instantiate(selected.gameObject);
                Item itemComponent = itemInstance.GetComponent<Item>();
                if (itemComponent != null)
                {
                    itemComponent.ID = selected.ID;
                    itemComponent.Name = selected.Name;
                }
                chest.AddGeneratedItem(itemInstance);
            }
        }
    }

    private Item GetRandomItem(List<Item> candidates)
    {
        if (candidates == null || candidates.Count == 0)
        {
            return null;
        }

        if (!useDropChance)
        {
            return candidates[Random.Range(0, candidates.Count)];
        }

        float totalWeight = 0f;
        foreach (Item item in candidates)
        {
            if (item == null)
            {
                continue;
            }
            totalWeight += Mathf.Max(0f, item.dropChance);
        }

        if (totalWeight <= 0f)
        {
            return candidates[Random.Range(0, candidates.Count)];
        }

        float value = Random.Range(0f, totalWeight);
        float current = 0f;
        foreach (Item item in candidates)
        {
            if (item == null)
            {
                continue;
            }
            current += Mathf.Max(0f, item.dropChance);
            if (value <= current)
            {
                return item;
            }
        }

        return candidates[candidates.Count - 1];
    }
}
