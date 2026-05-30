using UnityEngine;
using UnityEngine.Tilemaps;

public class Spanwer : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject enemyPrefab;
    public int spawnCount = 3;
    public bool spawnOnStart = true;
    public float spawnDelay = 0f;

    [Header("Spawn Area")]
    [Tooltip("Optional Collider2D objects that define valid spawn regions. If none are set, use custom bounds.")]
    public Collider2D[] spawnAreas;
    [Tooltip("Legacy single spawn area. Assign a map collider here if you only have one region.")]
    public Collider2D spawnArea;
    public Vector2 customMin = new Vector2(-10f, -5f);
    public Vector2 customMax = new Vector2(10f, 5f);

    [Header("Collision Avoidance")]
    public bool avoidExistingColliders = true;
    [Tooltip("LayerMask used to detect colliders and avoid spawning on top of them. Leave empty to avoid all layers.")]
    public LayerMask avoidLayers;
    public float spawnCheckRadius = 0.2f;
    public int maxSpawnAttempts = 50;

    private void Start()
    {
        if (spawnOnStart && enemyPrefab != null)
        {
            if (spawnDelay <= 0f)
                SpawnEnemies();
            else
                Invoke(nameof(SpawnEnemies), spawnDelay);
        }
    }

    public void SpawnEnemies()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner: enemyPrefab chưa được gán.");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2? spawnPosition = FindValidSpawnPosition();
            if (spawnPosition.HasValue)
            {
                Instantiate(enemyPrefab, spawnPosition.Value, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning($"EnemySpawner: Không tìm được vị trí spawn hợp lệ cho enemy #{i + 1} trong {maxSpawnAttempts} lần thử.");
            }
        }
    }

    private Vector2? FindValidSpawnPosition()
    {
        if (spawnAreas != null && spawnAreas.Length > 0)
            return FindValidSpawnPositionInAreas();

        if (spawnArea != null)
            return FindValidSpawnPositionInArea(spawnArea);

        Bounds bounds = GetCustomBounds();
        return FindValidSpawnPositionInBounds(bounds);
    }

    private Vector2? FindValidSpawnPositionInAreas()
    {
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Collider2D selectedArea = spawnAreas[Random.Range(0, spawnAreas.Length)];
            if (selectedArea == null)
                continue;

            Vector2 randomPoint = RandomPointInBounds(selectedArea.bounds);
            if (!selectedArea.OverlapPoint(randomPoint))
                continue;

            if (IsValidSpawnPosition(randomPoint))
                return randomPoint;
        }

        return null;
    }

    private Vector2? FindValidSpawnPositionInArea(Collider2D area)
    {
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Vector2 randomPoint = RandomPointInBounds(area.bounds);
            if (!area.OverlapPoint(randomPoint))
                continue;

            if (IsValidSpawnPosition(randomPoint))
                return randomPoint;
        }

        return null;
    }

    private Vector2? FindValidSpawnPositionInBounds(Bounds bounds)
    {
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Vector2 randomPoint = RandomPointInBounds(bounds);
            if (IsValidSpawnPosition(randomPoint))
                return randomPoint;
        }

        return null;
    }

    private Vector2 RandomPointInBounds(Bounds bounds)
    {
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );
    }

    private bool IsValidSpawnPosition(Vector2 position)
    {
        if (!avoidExistingColliders)
            return true;

        LayerMask mask = avoidLayers;
        if (mask == 0)
            mask = Physics2D.AllLayers;

        Collider2D[] hits = Physics2D.OverlapCircleAll(position, spawnCheckRadius, mask);
        if (hits == null || hits.Length == 0)
            return true;

        foreach (Collider2D hit in hits)
        {
            if (hit == null)
                continue;

            if (IsSpawnAreaCollider(hit))
                continue;

            return false;
        }

        return true;
    }

    private bool IsSpawnAreaCollider(Collider2D collider)
    {
        if (spawnArea != null && collider == spawnArea)
            return true;

        if (spawnAreas != null)
        {
            foreach (Collider2D area in spawnAreas)
            {
                if (area == null)
                    continue;

                if (collider == area)
                    return true;
            }
        }

        return false;
    }

    private Bounds GetCustomBounds()
    {
        Vector2 center = (customMin + customMax) * 0.5f;
        Vector2 size = customMax - customMin;
        return new Bounds(center, size);
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnAreas != null && spawnAreas.Length > 0)
        {
            Gizmos.color = new Color(0f, 1f, 1f, 0.25f);
            foreach (Collider2D area in spawnAreas)
            {
                if (area == null)
                    continue;

                Gizmos.DrawCube(area.bounds.center, area.bounds.size);
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(area.bounds.center, area.bounds.size);
            }
        }
        else if (spawnArea != null)
        {
            Gizmos.color = new Color(0f, 1f, 1f, 0.35f);
            Gizmos.DrawCube(spawnArea.bounds.center, spawnArea.bounds.size);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(spawnArea.bounds.center, spawnArea.bounds.size);
        }
        else
        {
            Bounds bounds = GetCustomBounds();
            Gizmos.color = new Color(0f, 1f, 1f, 0.35f);
            Gizmos.DrawCube(bounds.center, bounds.size);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}
