using UnityEngine;
using UnityEngine.SceneManagement;

public class Sc_mainMenu : MonoBehaviour
{
    [Header("Panel Referensi")]
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;
    public void Play()
    {
        Sc_soundManager.Instance.PlaySound("Play");
        SceneManager.LoadScene("Introduction");
    }

    public void Quit()
    {
        Sc_soundManager.Instance.PlaySound("Quit");
        Application.Quit();
    }

    public void HowToPlay()
    {
        Sc_soundManager.Instance.PlaySound("HowToPlay");
        SceneManager.LoadScene("HowToPlay");
    }

    // Fungsi untuk tombol "Options"
    public void ShowOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    // Fungsi untuk tombol "Kembali" di Options
    public void HideOptions()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    // Fungsi untuk tombol "Credits"
    public void ShowCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    // Fungsi untuk tombol "Kembali" di Credits
    public void HideCredits()
    {
        mainMenuPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }
}
