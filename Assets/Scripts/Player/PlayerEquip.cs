using UnityEngine;

public class PlayerEquip : MonoBehaviour
{
    [Header("Cấu hình vị trí tay")]
    public GameObject handPoint;
    private GameObject currentWeapon;
    private Transform gripTransform; // Đây chính là chìa khóa giữ tâm cán kiếm

    private Vector2 lastMoveDirection = Vector2.right;

    private void Start()
    {
        // Nếu handPoint chưa được gán, tìm nó tự động
        if (handPoint == null)
        {
            handPoint = transform.Find("HandPoint")?.gameObject;
            if (handPoint == null)
            {
                Debug.LogWarning("Không tìm thấy HandPoint! Vãy tạo một cái mới tại vị trí gốc.");
                handPoint = new GameObject("HandPoint");
                handPoint.transform.SetParent(transform);
                handPoint.transform.localPosition = Vector3.zero;
            }
        }
    }

    public void Equip(GameObject weaponPrefab)
    {
        if (weaponPrefab == null) return;

        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        // 1. Tạo vũ khí tại vị trí HandPoint của nhân vật
        currentWeapon = Instantiate(weaponPrefab, handPoint.transform.position, Quaternion.identity);

        // 2. Đưa vũ khí làm con của HandPoint để nó di chuyển theo nhân vật
        currentWeapon.transform.SetParent(handPoint.transform);

        // 3. Reset vị trí gốc của vũ khí ngoài cùng về 0 để khít vào tay nhân vật
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
        currentWeapon.transform.localScale = Vector3.one; // Giữ nguyên Scale gốc của cha

        // 4. Tìm kiếm Object "Grip" bên trong Prefab kiếm
        gripTransform = currentWeapon.transform.Find("Grip");

        if (gripTransform == null)
        {
            Debug.LogWarning("Không tìm thấy Object tên là 'Grip' trong Prefab vũ khí!");
        }

        FlipWeapon();
        Debug.Log($"✅ Equip vũ khí từ PlayerEquip: {weaponPrefab.name}");
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        // Chỉ cập nhật hướng khi nhân vật thực sự có di chuyển trái / phải
        if (moveX != 0)
        {
            lastMoveDirection.x = moveX;
            FlipWeapon();
        }
    }

    void FlipWeapon()
    {
        // Chưa có vũ khí thì không làm gì
        if (currentWeapon == null) return;

        // Nếu tìm thấy Grip thì lật Grip, nếu không thì lật Weapon ngoài cùng
        Transform objectToFlip = (gripTransform != null) ? gripTransform : currentWeapon.transform;

        // Đảm bảo góc xoay luôn cố định
        objectToFlip.localRotation = Quaternion.identity;

        // Lấy Scale hiện tại
        Vector3 currentScale = objectToFlip.localScale;

        // Scale của player
        float playerScaleX = transform.localScale.x;

        if (lastMoveDirection.x > 0)
        {
            if (playerScaleX < 0)
                currentScale.x = -Mathf.Abs(currentScale.x);
            else
                currentScale.x = Mathf.Abs(currentScale.x);
        }
        else if (lastMoveDirection.x < 0)
        {
            if (playerScaleX < 0)
                currentScale.x = Mathf.Abs(currentScale.x);
            else
                currentScale.x = -Mathf.Abs(currentScale.x);
        }

        objectToFlip.localScale = currentScale;
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        Equip(weaponPrefab);
    }

    public void Unequip()
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
            currentWeapon = null;
            gripTransform = null;
        }
    }
}