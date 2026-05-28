using UnityEngine;

public class WeaponGun : Weapon
{
    public GameObject bullet;
    public Transform firePos;
    public GameObject muzzleFlash;
    public float TimeBtwFire = 0.2f;
    public float bulletForce;

    private float timeBtwFire;
    private bool isEquipped = false; // Chỉ dùng biến này để kiểm soát

    private void Start()
    {
        isGun = true;
    }

    void Update()
    {
        // Chỉ xử lý khi đã được trang bị
        if (!isEquipped) return;

        RotateGun();
        timeBtwFire -= Time.deltaTime;

        if (Input.GetMouseButton(0) && timeBtwFire <= 0)
        {
            FireBullet();
        }
    }

    void RotateGun()
    {
        if (Camera.main == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270)
                sr.flipY = true;
            else
                sr.flipY = false;
        }
    }

    void FireBullet()
    {
        if (firePos == null) return;

        timeBtwFire = TimeBtwFire;

        GameObject Bullet = Instantiate(bullet, firePos.position, Quaternion.identity);

        BulletController bulletController = Bullet.GetComponent<BulletController>();
        if (bulletController != null)
        {
            bulletController.damage = damage;
        }

        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, firePos.position, transform.rotation, transform);
        }

        Rigidbody2D rb = Bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(transform.right * bulletForce, ForceMode2D.Impulse);
        }
    }

    public override void Attack(Vector3 attackPos)
    {
        // Không dùng
    }

    // Gọi khi trang bị súng từ inventory
    public void OnEquip()
    {
        isEquipped = true;
        Debug.Log($"Súng {weaponName} đã được trang bị, có thể bắn");
    }

    // Gọi khi tháo súng
    public void OnUnequip()
    {
        isEquipped = false;
        Debug.Log($"Súng {weaponName} đã được tháo");
    }
}