using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public int screen;
    public static bool isPaused = false;
    public Slider musicSlider;
    public Slider sfxSlider;
    private void Start()
    {
        LoadVolume();
        Time.timeScale = 1f;
        isPaused = false;
        if (screen == 0)
            MusicManager.Instance.PlayMusic("Menu");
        else
            MusicManager.Instance.PlayMusic("Play");
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void Menu()
    {
        LoadVolume();
        SceneManager.LoadScene(0);
    }
    public void StartGame()
    {
        LoadVolume();
        int selected = PlayerPrefs.GetInt("SavedCharacter", 1);
        MusicManager.Instance.PlayMusic("Play");
        Debug.Log("Đang chuyển sang cảnh playmove với nhân vật số: " + selected);
        SceneManager.LoadScene(1);

    }
    public void ExitGame()
    {
        SaveVolume();
        Debug.Log("Thoát game!");
        Application.Quit();
    }
    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume",volume);
    }
    public void UpdateSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }
    public void SaveVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        audioMixer.GetFloat("SFXVolume", out float sfxVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }  
    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }
}