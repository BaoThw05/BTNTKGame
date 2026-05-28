using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [SerializeField] private MusicLibrary musicLibrary;
    [SerializeField] private AudioSource musicSource;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayMusic(string trackName, float fadeDuration = 0.5f)
    {
        StartCoroutine(AnimateMusicCrossfade(musicLibrary.GetAudioClipByName(trackName),fadeDuration));
    }
    IEnumerator AnimateMusicCrossfade(AudioClip nexTrack, float fadeDuration = 0.5f)
    {
        float percent = 0f;
        while (percent < 1f)
        {
            percent += Time.deltaTime * 1f / fadeDuration;
            musicSource.volume = Mathf.Lerp(1f, 0f, percent);
            yield return null;
        }
        musicSource.clip = nexTrack;
        musicSource.Play();
        percent = 0f;
        while (percent < 1f)
        {
            percent += Time.deltaTime * 1f / fadeDuration;
            musicSource.volume = Mathf.Lerp(0f, 1f, percent);
            yield return null;
        }
    }
}
