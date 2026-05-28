using UnityEngine;

public class StaminaItem : Item
{
    public int staminaAmount = 30;

    public override void UseItem()
    {
        base.UseItem();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            StaminaBar staminaBar = player.GetComponent<StaminaBar>();
            if (staminaBar != null)
            {
                staminaBar.RestoreStamina(staminaAmount);
                Debug.Log($"💚 Đã dùng {Name}, hồi {staminaAmount} stamina!");

                Slot parentSlot = GetComponentInParent<Slot>();
                if (parentSlot != null)
                    parentSlot.RemoveItem();
                else
                    Destroy(gameObject);
            }
        }
    }
}