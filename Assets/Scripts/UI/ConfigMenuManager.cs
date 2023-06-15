using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfigMenuManager : MonoBehaviour
{
    private SaveSystem _saveSystem;

    private void Awake()
    {
        MainMenuSectionBehaviour menuBehaviour = GetComponent<MainMenuSectionBehaviour>();
        menuBehaviour.OnCreate = OnCreate;
    }

    private void OnCreate()
    {
        _saveSystem = SaveSystem.GetInstance();
    }

    public void OnSaveAndQuitClicked()
    {
        _saveSystem.SaveGame();
        SceneManager.LoadScene("TitleScene");
    }
}
