using UnityEngine;

public class YSort : MonoBehaviour
{
	public Transform footPoint; // kéo vào trong Inspector
	SpriteRenderer sr;

	void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
	}

	void LateUpdate()
	{
		if (footPoint == null) return;

		sr.sortingOrder = Mathf.RoundToInt(-footPoint.position.y * 100);
	}
}