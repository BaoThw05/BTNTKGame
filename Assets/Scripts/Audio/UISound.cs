using UnityEngine;

public class UISound : MonoBehaviour
{
    public void PlaySFXUISound(string soundName)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySoundUI(soundName);
        }
    }
}
