using UnityEngine;
using UnityEngine.SceneManagement;

public class Sc_mainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Introduction");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void HowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }
}
