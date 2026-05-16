using System.Collections;
using UnityEngine;

public class BounceEffect : MonoBehaviour
{
    public float bounceHeight = 0.1f;
    public float bounceDuration = 0.2f;
    public int bounceCount = 2;

    public void StartBounce()
    {
        StartCoroutine(BounceHandler());
    }
    private IEnumerator BounceHandler()
    {
        Vector3 originalPosition = transform.position;
        float localHeight = bounceHeight;
        float localDuration = bounceDuration;
        for(int i = 0; i < bounceCount; i++)
        {
            yield return Bounce(originalPosition, localHeight, localDuration/2);
            localHeight *= 0.5f;
            localDuration*= 0.8f;
        }
        transform.position = originalPosition;
    }
    private IEnumerator Bounce(Vector3 start, float height, float duration)
    {
        Vector3 peak=start+Vector3.up*height;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(start, peak, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(peak, start, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

}
