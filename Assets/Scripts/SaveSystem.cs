using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class SaveSystem : Singleton<SaveSystem>
{
    [SerializeField] private CanvasGroup _transitionScreen;

    public const string FROM_SAVE_FILE = "FromSaveFile";
    private const float TRANSITION_TIME = 1f;

    private AreaManager _areaManager;
    private DroneManager _droneManager;
    private SquadManager _squadManager;
    private ResourceStock _resourceStock;
    private UIStateManager _uiStateManager;

    private void Start()
    {
        _areaManager = AreaManager.GetInstance();
        _droneManager = DroneManager.GetInstance();
        _squadManager = SquadManager.GetInstance();
        _resourceStock = ResourceStock.GetInstance();
        _uiStateManager = UIStateManager.GetInstance();

        if (PlayerPrefs.GetInt(FROM_SAVE_FILE) != 0)
        {
            SaveFile save = LoadGame();
            _areaManager.SetupFromSave(save.Areas, save.Squads);
            _droneManager.SetupFromSave(save.Squads);
            _squadManager.SetupFromSave(save.Squads);
            _resourceStock.SetupFromSave(save.ResourceStocks);
        }
        StartCoroutine(FinishLoadGame());
    }

    public void SaveGame()
    {
        SaveFile save = new SaveFile()
        {
            Areas = _areaManager.GetAreas(),
            Squads = _squadManager.GetAllSquads(),
            ResourceStocks = _resourceStock.GetAllStocks()
        };

        string path = Application.persistentDataPath + "/savedata";
        File.WriteAllText(path, Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonUtility.ToJson(save))));
    }

    private SaveFile LoadGame()
    {
        string path = Application.persistentDataPath + "/savedata";
        SaveFile save = JsonUtility.FromJson<SaveFile>(Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(path))));
        return save;
    }

    private IEnumerator FinishLoadGame()
    {
        _uiStateManager.ChangeGameState(UIStateManager.GameState.IDLE);
        for (float i = 0; i < 1; i+= Time.deltaTime/TRANSITION_TIME)
        {
            _transitionScreen.alpha = Mathf.Lerp(1f, 0f, i);
            yield return new WaitForFixedUpdate();
        }
        _transitionScreen.alpha = 0f;
        _transitionScreen.gameObject.SetActive(false);
    }
}

[Serializable]
class SaveFile : ISerializationCallbackReceiver
{
    public List<Area> Areas;
    public List<Squad> Squads;
    [NonSerialized] public Dictionary<ResourceData, int> ResourceStocks;
    
    [SerializeField] private List<ResourceAmount> StockList;

    public void OnBeforeSerialize()
    {
        StockList = new List<ResourceAmount>();
        foreach (var amount in ResourceStocks)
        {
            StockList.Add(new ResourceAmount(amount.Key, amount.Value));
        }
    }

    public void OnAfterDeserialize()
    {
        ResourceStocks = new Dictionary<ResourceData, int>();
        foreach (ResourceAmount amount in StockList)
        {
            ResourceStocks[amount.Resource] = amount.Amount;
        }
    }
}
