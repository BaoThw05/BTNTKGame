using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Image staminaFill;
    public float maxStamina = 100f;
    private float currentStamina;
    private PlayerMove playerMove;

    private bool isExhausted = false;
    private float originalMoveSpeed;

    [Header("Cấu hình kiệt sức")]
    [SerializeField] private float exhaustRecoveryThreshold = 10f; // 10% là được đi lại

    void Start()
    {
        currentStamina = maxStamina;
        playerMove = GetComponent<PlayerMove>();

        if (playerMove != null)
            originalMoveSpeed = playerMove.moveSpeed;

        // Tự động tìm "green"
        if (staminaFill == null)
        {
            GameObject greenObj = GameObject.Find("green");
            if (greenObj == null)
            {
                GameObject greenBar = GameObject.Find("GreenBar");
                if (greenBar != null)
                {
                    Transform greenChild = greenBar.transform.Find("green");
                    if (greenChild != null)
                        greenObj = greenChild.gameObject;
                }
            }
            if (greenObj != null)
                staminaFill = greenObj.GetComponent<Image>();
        }

        UpdateStaminaBar();
    }

    void Update()
    {
        if (MenuManager.isPaused) return;

        bool isMoving = (playerMove != null &&
                        (Input.GetAxisRaw("Horizontal") != 0 ||
                         Input.GetAxisRaw("Vertical") != 0));

        // Nếu đang kiệt sức
        if (isExhausted)
        {
            // Hồi stamina khi kiệt sức
            currentStamina += 25f * Time.deltaTime;

            // 👈 KIỂM TRA: Nếu hồi được >= 10% thì thoát khỏi trạng thái kiệt sức
            if (currentStamina >= exhaustRecoveryThreshold)
            {
                isExhausted = false;

                // Khôi phục tốc độ
                if (playerMove != null)
                    playerMove.moveSpeed = originalMoveSpeed;

                Debug.Log($"💪 Đã hồi được {currentStamina:F1}% stamina, có thể đi tiếp!");
            }
            UpdateStaminaBar();
            return;
        }

        // Xử lý stamina bình thường
        if (isMoving && currentStamina > 0)
        {
            currentStamina -= 15f * Time.deltaTime;
            if (currentStamina < 0) currentStamina = 0;
        }
        else if (!isMoving && currentStamina < maxStamina)
        {
            currentStamina += 20f * Time.deltaTime;
            if (currentStamina > maxStamina) currentStamina = maxStamina;
        }

        // 👈 KIỂM TRA: Hết stamina (<= 0%) thì kiệt sức
        if (isMoving && currentStamina <= 0)
        {
            currentStamina = 0;  // Đặt chính xác về 0
            isExhausted = true;
            Debug.Log("😫 Hết stamina! Nhân vật kiệt sức, đứng im chờ hồi 10%!");
        }

        UpdateStaminaBar();
    }

    public void RestoreStamina(float amount)
    {
        currentStamina += amount;
        if (currentStamina > maxStamina) currentStamina = maxStamina;

        // Nếu đang kiệt sức mà được hồi vượt quá ngưỡng 10%
        if (isExhausted && currentStamina >= exhaustRecoveryThreshold)
        {
            isExhausted = false;
            if (playerMove != null)
                playerMove.moveSpeed = originalMoveSpeed;
            Debug.Log("💚 Dùng item hồi stamina, đã tỉnh táo!");
        }

        UpdateStaminaBar();
    }

    public bool IsExhausted()
    {
        return isExhausted;
    }

    void UpdateStaminaBar()
    {
        if (staminaFill != null)
            staminaFill.fillAmount = currentStamina / maxStamina;
    }
}