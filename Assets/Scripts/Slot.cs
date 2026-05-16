using UnityEngine;

public class Slot : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
public GameObject currentItem;
     void Start()
    {
        currentItem = null;
    }
    public void AddItem(GameObject item)
    {
        if (currentItem == null)
        {
            currentItem = item;
            item.transform.SetParent(transform);
            item.transform.localPosition = Vector3.zero;
        }
    }
    public void RemoveItem()
    {
        if (currentItem != null)
        {
            Destroy(currentItem);
            currentItem = null;
        }
    }
}
