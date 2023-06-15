using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class SaveSystem : Singleton<SaveSystem>
{
    public const string FROM_SAVE_FILE = "FromSaveFile";

    private AreaManager _areaManager;
    private DroneManager _droneManager;
    private ResourceStock _resourceStock;

    private void Start()
    {
        _areaManager = AreaManager.GetInstance();
        _droneManager = DroneManager.GetInstance();
        _resourceStock = ResourceStock.GetInstance();

        if (PlayerPrefs.GetInt(FROM_SAVE_FILE) != 0)
        {
            SaveFile save = LoadGame();
            Debug.Log($"Loaded: {save.SaveName}");
        }
    }

    public void SaveGame()
    {
        SaveFile save = new SaveFile { SaveName = "Saveeee" };

        string path = Application.persistentDataPath + "/savedata";
        File.WriteAllText(path, Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonUtility.ToJson(save))));
    }

    private SaveFile LoadGame()
    {
        string path = Application.persistentDataPath + "/savedata";
        SaveFile save = JsonUtility.FromJson<SaveFile>(Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(path))));
        return save;
    }
}

[Serializable]
class SaveFile
{
    public string SaveName;
}
