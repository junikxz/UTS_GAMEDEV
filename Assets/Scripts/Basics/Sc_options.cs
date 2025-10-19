using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Sc_options : MonoBehaviour
{
    [Header("Komponen")]
    public AudioMixer masterMixer;
    public Toggle fullscreenToggle;

    void Start()
    {
        // Atur toggle ke status fullscreen saat ini
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = Screen.fullScreen;
        }
    }

    // Fungsi untuk Slider "MasterVolumeSlider"
    public void SetMasterVolume(float volume)
    {
        // Gunakan Log10 untuk konversi linear ke desibel (dB)
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    // Fungsi untuk Slider "MusicVolumeSlider"
    public void SetMusicVolume(float volume)
    {
        masterMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    // Fungsi untuk Slider "SFXVolumeSlider"
    public void SetSFXVolume(float volume)
    {
        masterMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    // Fungsi untuk Toggle "FullscreenToggle"
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
