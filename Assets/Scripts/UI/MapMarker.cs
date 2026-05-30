using UnityEngine;

public class MapMarker : MonoBehaviour
{
    public static MapMarker Instance;

    [Header("Cấu hình Chấm Đỏ UI Map To")]
    public RectTransform markerBig;
    public RectTransform bigMapRect;

    [Header("Cấu hình Đóng/Mở Bản Đồ To")]
    public GameObject fullMapPanel;

    [Header("Cấu hình Tọa độ 4 Rìa Bản đồ thực tế")]
    public Transform worldBottomLeft;
    public Transform worldTopRight;

    [Header("Kích thước UI Map To")]
    public Vector2 bigMapSize = new Vector2(700, 700);

    private Transform currentPlayer;
    private Rigidbody2D playerRb;

    void Start()
    {
        // Nếu chưa được gán từ inspector hoặc từ code khác thì tự tìm Player theo Tag
        if (currentPlayer == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null)
            {
                currentPlayer = p.transform;
                playerRb = p.GetComponent<Rigidbody2D>();
            }
        }

        Canvas.ForceUpdateCanvases();
        UpdateBigMapMarker();
    }

    void Awake()
    {
        Instance = this;
    }

    public void SetCurrentPlayer(Transform playerTransform)
    {
        currentPlayer = playerTransform;
        playerRb = playerTransform.GetComponent<Rigidbody2D>();

        Canvas.ForceUpdateCanvases();
        UpdateBigMapMarker();
    }

    void FixedUpdate()
    {
        if (playerRb == null)
            return;

        UpdateBigMapMarker();
    }

    void UpdateBigMapMarker()
    {
        if (currentPlayer == null) return;

        Vector2 playerPos = playerRb != null ? playerRb.position : (Vector2)currentPlayer.position;

        float percentX = Mathf.InverseLerp(
            worldBottomLeft.position.x,
            worldTopRight.position.x,
            playerPos.x
        );

        float percentY = Mathf.InverseLerp(
            worldBottomLeft.position.y,
            worldTopRight.position.y,
            playerPos.y
        );

        float posX = (percentX - 0.5f) * bigMapSize.x;
        float posY = (percentY - 0.5f) * bigMapSize.y;

        markerBig.anchoredPosition =
            new Vector2(posX, posY);
    }

    public void ToggleFullMap()
    {
        if (fullMapPanel != null)
            fullMapPanel.SetActive(!fullMapPanel.activeSelf);
    }
}