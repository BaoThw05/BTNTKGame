using UnityEngine;

public class WeaponHolderController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer playerSR;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private GameObject defaultWeaponPrefab;
    [SerializeField] private PlayerEquip playerEquip;

    public SpriteRenderer weaponSR;
    public Transform handPoint;
    public Weapon currentWeaponBase;

    private GameObject currentWeaponObject;

    [SerializeField] private Transform weaponPosUp;
    [SerializeField] private Transform weaponPosDown;
    [SerializeField] private Transform weaponPosLeft;
    [SerializeField] private Transform weaponPosRight;

    private void Start()
    {
        if (playerEquip != null && handPoint == null)
            handPoint = playerEquip.handPoint != null ? playerEquip.handPoint.transform : null;

        if (defaultWeaponPrefab != null)
            EquipWeapon(defaultWeaponPrefab);
    }

    private void Update()
    {
        if (currentWeaponObject == null) return;
        UpdateWeaponPosition();
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (weaponPrefab == null) return;

        // Xóa vũ khí cũ
        if (currentWeaponObject != null)
        {
            WeaponGun oldGun = currentWeaponBase as WeaponGun;
            if (oldGun != null) oldGun.OnUnequip();
            Destroy(currentWeaponObject);
        }

        // Tạo vũ khí mới
        if (handPoint != null)
        {
            currentWeaponObject = Instantiate(weaponPrefab, handPoint.position, Quaternion.identity);
            currentWeaponObject.transform.SetParent(handPoint);
        }
        else
        {
            currentWeaponObject = Instantiate(weaponPrefab, transform);
        }

        currentWeaponObject.transform.localPosition = Vector3.zero;
        currentWeaponObject.transform.localRotation = Quaternion.identity;

        currentWeaponBase = currentWeaponObject.GetComponent<Weapon>();
        if (currentWeaponBase == null)
            currentWeaponBase = currentWeaponObject.GetComponentInChildren<Weapon>();

        weaponSR = currentWeaponObject.GetComponent<SpriteRenderer>();
        if (weaponSR == null)
            weaponSR = currentWeaponObject.GetComponentInChildren<SpriteRenderer>();

        // Nếu là súng, gọi OnEquip
        if (currentWeaponBase != null && currentWeaponBase.isGun)
        {
            WeaponGun gun = currentWeaponBase as WeaponGun;
            if (gun != null)
            {
                // Đảm bảo có firePos
                if (gun.firePos == null)
                {
                    Transform firePos = currentWeaponObject.transform.Find("FirePos");
                    if (firePos != null) gun.firePos = firePos;
                }
                gun.OnEquip();
            }
        }

        Debug.Log($"Đã trang bị {weaponPrefab.name}");
    }

    void UpdateWeaponPosition()
    {
        if (weaponSR == null) return;

        Vector2 dir = playerMove.lastMoveDirection;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0)
            {
                transform.localPosition = weaponPosRight.localPosition;
                weaponSR.sortingOrder = playerSR.sortingOrder + 1;
            }
            else
            {
                transform.localPosition = weaponPosLeft.localPosition;
                weaponSR.sortingOrder = playerSR.sortingOrder - 1;
            }
        }
        else
        {
            if (dir.y > 0)
            {
                transform.localPosition = weaponPosUp.localPosition;
                weaponSR.sortingOrder = playerSR.sortingOrder - 1;
            }
            else
            {
                transform.localPosition = weaponPosDown.localPosition;
                weaponSR.sortingOrder = playerSR.sortingOrder + 1;
            }
        }
    }
}