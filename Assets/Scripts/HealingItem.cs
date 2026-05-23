using UnityEngine;

public class HealingItem : Item
{
    public int healAmount = 20;

    public override void UseItem()
    {
        base.UseItem(); // In ra "Using item: ..."

        // Tìm Player và hồi máu
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                Debug.Log($"❤️ Đã dùng {Name}, hồi {healAmount} máu!");

                // Xóa item khỏi slot sau khi dùng
                Slot parentSlot = GetComponentInParent<Slot>();
                if (parentSlot != null)
                {
                    parentSlot.RemoveItem();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.LogError("Player không có script PlayerHealth!");
            }
        }
        else
        {
            Debug.LogError("Không tìm thấy Player!");
        }
    }
}