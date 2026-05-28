using UnityEngine;
[System.Serializable]
public struct SoundEffect       
{
    public string groupID;
    public AudioClip[] clips;
}
public class SoundLibrary : MonoBehaviour
{
    public SoundEffect[] soundEffects;
    public AudioClip GetClipFromName(string name )
    {
       foreach(var s in soundEffects)
        {
            if(s.groupID == name)
            {
                return s.clips[Random.Range(0, s.clips.Length)];
            }
        }
        return null;
    }
}
