using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
	// Thời gian tồn tại của hiệu ứng (giây). Bạn có thể chỉnh ngoài Inspector.
	public float lifetime = 0.2f;

	void Start()
	{
		// Hàm này sẽ phá hủy chính GameObject này sau 'lifetime' giây
		Destroy(gameObject, lifetime);
	}
}