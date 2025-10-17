using UnityEngine;
using System.Collections.Generic;

public class FlagInventoryManager : MonoBehaviour
{
    public static FlagInventoryManager instance;

    [System.Serializable]
    public class FlagData
    {
        public string flagName;
        public Sprite flagIcon; // Gambar bendera
        public bool isUnlocked = false;
    }

    [Header("Flag Database")]
    public List<FlagData> allFlags = new List<FlagData>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadFlagProgress();
    }

    // Unlock bendera baru
    public void UnlockFlag(string flagName)
    {
        FlagData flag = allFlags.Find(f => f.flagName == flagName);
        if (flag != null && !flag.isUnlocked)
        {
            flag.isUnlocked = true;
            SaveFlagProgress();
            Debug.Log($"Bendera {flagName} telah di-unlock!");
        }
    }

    // Cek apakah bendera sudah di-unlock
    public bool IsFlagUnlocked(string flagName)
    {
        FlagData flag = allFlags.Find(f => f.flagName == flagName);
        return flag != null && flag.isUnlocked;
    }

    // Dapatkan semua bendera yang sudah di-unlock
    public List<FlagData> GetUnlockedFlags()
    {
        return allFlags.FindAll(f => f.isUnlocked);
    }

    // Simpan progress ke PlayerPrefs
    void SaveFlagProgress()
    {
        for (int i = 0; i < allFlags.Count; i++)
        {
            PlayerPrefs.SetInt($"Flag_{allFlags[i].flagName}", allFlags[i].isUnlocked ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    // Load progress dari PlayerPrefs
    void LoadFlagProgress()
    {
        for (int i = 0; i < allFlags.Count; i++)
        {
            allFlags[i].isUnlocked = PlayerPrefs.GetInt($"Flag_{allFlags[i].flagName}", 0) == 1;
        }
    }

    // Reset semua bendera (untuk testing)
    public void ResetAllFlags()
    {
        foreach (var flag in allFlags)
        {
            flag.isUnlocked = false;
            PlayerPrefs.DeleteKey($"Flag_{flag.flagName}");
        }
        PlayerPrefs.Save();
        Debug.Log("Semua bendera direset!");
    }
}