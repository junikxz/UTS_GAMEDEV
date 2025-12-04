
using UnityEngine;

public class Sc_soundManager : MonoBehaviour
{
    public static Sc_soundManager Instance;

    [SerializeField] private Sc_soundLibrary sfxLibrary;
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound(string soundName)
    {
        sfxSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
    }
}
