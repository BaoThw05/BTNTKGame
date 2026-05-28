using UnityEngine;

public class ItemCarrier : MonoBehaviour
{
    [Header("Settings")]
    public Vector2 handOffset = new Vector2(0.5f, 0.2f);

    private Transform handPoint;
    private SpriteRenderer currentItemSprite;
    private bool isHoldingItem = false;

    void Awake()
    {
        Debug.Log("1. Awake - Bắt đầu tạo HandPoint");
        CreateHandPoint();
    }

    void CreateHandPoint()
    {
        Debug.Log("2. Đang tạo HandPoint...");
        handPoint = new GameObject("HandPoint").transform;
        handPoint.SetParent(transform);
        handPoint.localPosition = handOffset;
        Debug.Log("3. Đã tạo HandPoint tại vị trí: " + handPoint.localPosition);

        GameObject itemObj = new GameObject("CurrentItem");
        itemObj.transform.SetParent(handPoint);
        itemObj.transform.localPosition = Vector3.zero;

        currentItemSprite = itemObj.AddComponent<SpriteRenderer>();
        currentItemSprite.enabled = false;

        Debug.Log("4. Đã tạo CurrentItem xong! currentItemSprite = " + (currentItemSprite != null ? "OK" : "NULL"));
    }

    public void PickUpItem(Sprite itemSprite)
    {
        Debug.Log("5. PickUpItem được gọi!");

        if (currentItemSprite == null)
        {
            Debug.LogError("❌ LỖI: currentItemSprite bị NULL!");
            return;
        }

        if (itemSprite == null)
        {
            Debug.LogError("❌ LỖI: itemSprite bị NULL!");
            return;
        }

        Debug.Log("6. Gán sprite: " + itemSprite.name);
        currentItemSprite.sprite = itemSprite;
        currentItemSprite.enabled = true;
        isHoldingItem = true;
        Debug.Log("7. Đã bật SpriteRenderer, enabled = " + currentItemSprite.enabled);
    }

    public void DropItem()
    {
        if (currentItemSprite == null) return;
        currentItemSprite.sprite = null;
        currentItemSprite.enabled = false;
        isHoldingItem = false;
    }

    void LateUpdate()
    {
        if (isHoldingItem && handPoint != null && currentItemSprite != null)
        {
            currentItemSprite.transform.position = handPoint.position;
        }
    }
}