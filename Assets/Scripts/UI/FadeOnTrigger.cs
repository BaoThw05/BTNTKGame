using UnityEngine;
using UnityEngine.Tilemaps;

public class FadeOnTrigger : MonoBehaviour
{
    [Header("Vật thể cần giảm opacity")]
    [SerializeField] private GameObject targetObject;

    [Header("Độ mờ khi Player đi vào vùng trigger")]
    [Range(0f, 1f)]
    [SerializeField] private float fadeAlpha = 0.4f;

    [Header("Độ mờ bình thường")]
    [Range(0f, 1f)]
    [SerializeField] private float normalAlpha = 1f;

    private SpriteRenderer spriteRenderer;
    private Tilemap tilemap;

    private void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("Chưa gán targetObject!");
            return;
        }

        // Nếu là Sprite
        spriteRenderer = targetObject.GetComponent<SpriteRenderer>();

        // Nếu là Tilemap
        tilemap = targetObject.GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            SetAlpha(fadeAlpha);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            SetAlpha(normalAlpha);
        }
    }

    private void SetAlpha(float alpha)
    {
        // Trường hợp là SpriteRenderer
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }

        // Trường hợp là Tilemap
        if (tilemap != null)
        {
            Color color = tilemap.color;
            color.a = alpha;
            tilemap.color = color;
        }
    }
}