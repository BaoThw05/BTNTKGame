using UnityEngine;
[System.Serializable]
public struct MusicTrack
{
    public string trackName;
    public AudioClip audioClip;
}
public class MusicLibrary : MonoBehaviour
{
    public MusicTrack[] musicTracks;
    public AudioClip GetAudioClipByName(string trackName)
    {
        foreach (var track in musicTracks)
        {
            if (track.trackName == trackName)
            {
                return track.audioClip;
            }
        }
        return null;
    }
}