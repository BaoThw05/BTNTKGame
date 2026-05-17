using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public Sprite itemSprite;

    void Start()
    {
        // Nếu chưa có sprite, tự lấy từ SpriteRenderer của item
        if (itemSprite == null)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                itemSprite = sr.sprite;
                Debug.Log("Đã tự lấy sprite: " + itemSprite.name);
            }
            else
            {
                Debug.LogError("❌ Không có sprite! Hãy kéo sprite vào ô Item Sprite");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("🔔 Chạm vào: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ Đúng Player!");

            if (itemSprite == null)
            {
                Debug.LogError("❌ itemSprite bị NULL! Hãy gán sprite trong Inspector");
                return;
            }

            ItemCarrier carrier = other.GetComponent<ItemCarrier>();
            if (carrier != null)
            {
                Debug.Log("🎒 Đang gọi PickUpItem với sprite: " + itemSprite.name);
                carrier.PickUpItem(itemSprite);
                Destroy(gameObject);
            }
        }
    }
}