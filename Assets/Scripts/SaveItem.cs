using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


//script does not work
public class SaveItem : MonoBehaviour
{
    [SerializeField, TextArea] private string path;
    [SerializeField] private JunkSaveData savedData = new JunkSaveData();

    private void OnEnable()
    {
        path = $"{Application.persistentDataPath}/SaveData/SavedJunk";
        Load();
    }

    private void OnDisable()
    {
        Save();
    }

    [ContextMenu("Save")]
    private void Save()
    {
        Directory.CreateDirectory(path);
        File.WriteAllText(path + "/" + savedData.pathName + ".json", JsonUtility.ToJson(savedData, true));
    }

    [ContextMenu("Load")]
    private void Load()
    {
        Guid guid = Guid.NewGuid();
        string tempPath = path + "/" + savedData.pathName + ".json";

        if (File.Exists(tempPath))
        {
            print("saveData found");
            Directory.CreateDirectory(path);
            savedData = JsonUtility.FromJson<JunkSaveData>(File.ReadAllText(tempPath));
            savedData.pathName += guid;
        }
        else
        {
            print("saveData not found");
            Directory.CreateDirectory(path);
            savedData = new JunkSaveData { pathName = string.IsNullOrEmpty(savedData.pathName)? gameObject.name : savedData.pathName, data = savedData.data};
            savedData.pathName += guid;
        }
        
    }

    [ContextMenu("Test")]
    public void Test()
    {
        GetComponents<Component>()[savedData.data[0].GetComponentIndex()] = savedData.data[0];
    }

    [Serializable]
    private class JunkSaveData : Data
    { 
        public List<Transform> data;
    }
    
}




