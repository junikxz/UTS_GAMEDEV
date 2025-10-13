using UnityEngine;
using UnityEngine.SceneManagement;

public class Sc_mainMenu : MonoBehaviour
{
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
}
