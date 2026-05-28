using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField]
    private SoundLibrary sfxLibrary;
    [SerializeField]
    private AudioSource sfxUILibrary;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlaySound(AudioClip clip, Vector3 position)
    {
        if (clip != null)
        {
            GameObject tempAudio = new GameObject("TempSFX");
            tempAudio.transform.position = position;

            AudioSource source = tempAudio.AddComponent<AudioSource>();
            source.clip = clip;
            source.spatialBlend = 0f;
            if (sfxUILibrary != null)
            {
                source.outputAudioMixerGroup = sfxUILibrary.outputAudioMixerGroup;
            }
            source.Play();
            Destroy(tempAudio, clip.length);
        }
    }
    public void PlaySound(string soundName, Vector3 position)
    {
        PlaySound(sfxLibrary.GetClipFromName(soundName), position);
    }
    public void PlaySoundUI(string soundName)
    {
        sfxUILibrary.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
    }
}
