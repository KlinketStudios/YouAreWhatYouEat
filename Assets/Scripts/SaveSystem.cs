using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
    public SettingsData settingsData;
    public GameData gameData;
    public PlayerSaveData playerSaveData;
    [SerializeField] private string savePath;
    
    
    private void Awake()
    {
        foreach (var saveSystem in FindObjectsByType<SaveSystem>(FindObjectsSortMode.None))
        {
            if (saveSystem != this)
            {
                Destroy(saveSystem.gameObject);
            }
        }
        
        DontDestroyOnLoad(gameObject);
        instance = this;
        savePath = $"{Application.persistentDataPath}/SaveData";
        Load();
    }

    public void Load()
    {

        string thisPath = $"{savePath}/{settingsData.pathName}.json";
        if (File.Exists(thisPath))
        {
            settingsData = JsonUtility.FromJson<SettingsData>(File.ReadAllText(thisPath));
        }
        else
        {
            settingsData = new SettingsData("SettingsData");
            Save();
        }
        thisPath = $"{savePath}/{gameData.pathName}.json";
        if (File.Exists(thisPath))
        {
            gameData = JsonUtility.FromJson<GameData>(File.ReadAllText(thisPath));
        }
        else
        {
            gameData = new GameData("GameData");
            Save();
        }
        thisPath = $"{savePath}/{playerSaveData.pathName}.json";
        if (File.Exists(thisPath))
        {
            playerSaveData = JsonUtility.FromJson<PlayerSaveData>(File.ReadAllText(thisPath));
        }
        else
        {
            playerSaveData = new PlayerSaveData("PlayerSaveData");
            Save();
        }
        
    }

    public void Save()
    {
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        
        string thisPath = $"{savePath}/{settingsData.pathName}.json";
        File.WriteAllText(thisPath, JsonUtility.ToJson(settingsData));
        thisPath = $"{savePath}/{gameData.pathName}.json";
        File.WriteAllText(thisPath, JsonUtility.ToJson(gameData));
        thisPath = $"{savePath}/{playerSaveData.pathName}.json";
        File.WriteAllText(thisPath, JsonUtility.ToJson(playerSaveData));
    }
}

[Serializable]
public class SettingsData : Data
{
    [Min(0.1f)] public float sensitivity = .3f;
    public int FPS = 60;
    
    public SettingsData(string pathName)
    {
        this.pathName = pathName;
    }
}

[Serializable]
public class GameData : Data
{
    public int currentDay = 1;
    public int customersSpawnedThisDay = 0;
    public List<int> customerOccurences = new List<int>{0,0,0,0,0,0};
    
    public GameData(string pathName)
    {
        this.pathName = pathName;
    }
}
[Serializable]
public class PlayerSaveData : Data
{
    public Transform playerTransform;
    public PlayerData playerData;
    public PlayerInteract playerInteract;
    public PlayerController playerController;

    public PlayerSaveData(string pathName)
    {
        this.pathName = pathName;
    }
}

public class Data
{
    public string pathName;
}


