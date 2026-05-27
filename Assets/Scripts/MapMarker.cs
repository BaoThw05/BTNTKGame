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
        float percentX = Mathf.InverseLerp(
            worldBottomLeft.position.x,
            worldTopRight.position.x,
            playerRb.position.x
        );

        float percentY = Mathf.InverseLerp(
            worldBottomLeft.position.y,
            worldTopRight.position.y,
            playerRb.position.y
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