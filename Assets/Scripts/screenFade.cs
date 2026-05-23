using UnityEngine;
using System.Threading.Tasks;

public class screenFade : MonoBehaviour
{
    public static screenFade instance;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float fadeDuration = 0.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async Task Fade(float targetTransparency)
    {
        if (canvasGroup == null) return;

        float start = canvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime; // Dùng unscaled để không bị ảnh hưởng bởi pause game
            canvasGroup.alpha = Mathf.Lerp(start, targetTransparency, elapsed / fadeDuration);
            await Task.Yield();
        }

        canvasGroup.alpha = targetTransparency;
    }

    public async Task FadeOut() => await Fade(1f);
    public async Task FadeIn() => await Fade(0f);
}