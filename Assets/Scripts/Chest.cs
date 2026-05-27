using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Chest settings")]
    public int minItemCount = 1;
    public int maxItemCount = 3;
    public Transform itemSpawnRoot;

    [Header("Slot settings")]
    public GameObject slotPrefab;
    public Transform chestPanel;

    [HideInInspector]
    public List<GameObject> generatedItems = new List<GameObject>();

    private List<Slot> slots = new List<Slot>();

    private void Awake()
    {
        EnsureSlots();
    }

    public void EnsureSlots()
    {
        Transform parent = chestPanel != null ? chestPanel : (itemSpawnRoot != null ? itemSpawnRoot : transform);

        slots.Clear();

        // cache existing slots
        foreach (Transform child in parent)
        {
            Slot s = child.GetComponent<Slot>();
            if (s != null)
            {
                slots.Add(s);
            }
        }

        if (slotPrefab == null)
        {
            return;
        }
    }

    public void ClearGeneratedItems()
    {
        EnsureSlots();
        foreach (Slot s in slots)
        {
            if (s != null && s.currentItem != null)
            {
                Destroy(s.currentItem);
                s.currentItem = null;
            }
        }
        generatedItems.Clear();
    }
    public bool AddGeneratedItem(GameObject itemInstance)
    {
        if (itemInstance == null)
            return false;

        EnsureSlots();

        foreach (Slot s in slots)
        {
            if (s != null && s.currentItem == null)
            {
                s.AddItem(itemInstance);
                generatedItems.Add(itemInstance);
                return true;
            }
        }

        // No available slot - cleanup
        Destroy(itemInstance);
        return false;
    }
}
