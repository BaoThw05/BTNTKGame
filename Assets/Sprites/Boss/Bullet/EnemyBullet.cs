using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
	[Header("Hiệu ứng khi vỡ")]
	public GameObject impactEffect; // Kéo thả Prefab hiệu ứng vỡ vào đây
	private void OnTriggerEnter2D(Collider2D hitInfo)
	{
		// Kiểm tra xem mục tiêu chạm vào có Tag là "Player" hoặc "Wall" (Tường) không
		if (hitInfo.CompareTag("Player") || hitInfo.CompareTag("Wall"))
		{
			// 1. Tạo hiệu ứng vỡ ra ngay tại vị trí viên đạn
			if (impactEffect != null)
			{
				Instantiate(impactEffect, transform.position, transform.rotation);
			}

			// (Tùy chọn) Xử lý trừ máu Player tại đây nếu chạm Player
			if (hitInfo.CompareTag("Player"))
			{
				// Ví dụ: hitInfo.GetComponent<PlayerHealth>().TakeDamage(10);
			}

			// 2. Biến mất (Phá hủy viên đạn ngay lập tức)
			Destroy(gameObject);
		}
	}
}
