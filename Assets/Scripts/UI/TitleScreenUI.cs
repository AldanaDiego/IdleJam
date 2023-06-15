using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenUI : MonoBehaviour
{
    public void OnNewGameClicked()
    {
        PlayerPrefs.SetInt(SaveSystem.FROM_SAVE_FILE, 0);
        SceneManager.LoadScene("GameScene");
    }

    public void OnContinueClicked()
    {
        PlayerPrefs.SetInt(SaveSystem.FROM_SAVE_FILE, 1);
        SceneManager.LoadScene("GameScene");
    }

    public void OnExitClicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
