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

        //destroy self if this object is not the instance's object
        if (instance == null)
        {
            //put object in dont destroy on load to persist between scenes && set instance to this
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            //there is already another instance, this object is not needed 
            Destroy(gameObject);
        }
        
        //find the default save path
        savePath = $"{Application.persistentDataPath}/SaveData";
        
        //try to load data
        Load();
    }

    public void Load()
    {
        //settings data
        //cache path
        string thisPath = $"{savePath}/{settingsData.pathName}.json";
        if (File.Exists(thisPath))
        {
            //load data if it exists
            settingsData = JsonUtility.FromJson<SettingsData>(File.ReadAllText(thisPath));
        }
        else
        {
            //create new data if it does not exist already
            settingsData = new SettingsData("SettingsData");
            Save();
        }
        
        //game data
        //cache path
        thisPath = $"{savePath}/{gameData.pathName}.json";
        if (File.Exists(thisPath))
        {
            //load data if it exists
            gameData = JsonUtility.FromJson<GameData>(File.ReadAllText(thisPath));
        }
        else
        {
            //create new data if it does not exist already
            gameData = new GameData("GameData");
            Save();
        }
        
        //player data
        //cache path
        thisPath = $"{savePath}/{playerSaveData.pathName}.json";
        if (File.Exists(thisPath))
        {
            //load data if it exists
            playerSaveData = JsonUtility.FromJson<PlayerSaveData>(File.ReadAllText(thisPath));
        }
        else
        {
            //create new data if it does not exist already
            playerSaveData = new PlayerSaveData("PlayerSaveData");
            Save();
        }
        
    }

    public void Save()
    {
        //check if the folder path to the save path exists && create it if not
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        
        //save settings data
        string thisPath = $"{savePath}/{settingsData.pathName}.json";
        File.WriteAllText(thisPath, JsonUtility.ToJson(settingsData));
        
        //save game data
        thisPath = $"{savePath}/{gameData.pathName}.json";
        File.WriteAllText(thisPath, JsonUtility.ToJson(gameData));
        
        //save player data
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


