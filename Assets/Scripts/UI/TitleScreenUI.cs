using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _transitionScreen;

    private const float TRANSITION_TIME = 1f;

    public void OnNewGameClicked()
    {
        PlayerPrefs.SetInt(SaveSystem.FROM_SAVE_FILE, 0);
        StartCoroutine(LoadGameScene());
    }

    public void OnContinueClicked()
    {
        PlayerPrefs.SetInt(SaveSystem.FROM_SAVE_FILE, 1);
        StartCoroutine(LoadGameScene());
    }

    public void OnExitClicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private IEnumerator LoadGameScene()
    {
        _transitionScreen.gameObject.SetActive(true);
        for (float i = 0; i < 1; i+= Time.deltaTime/TRANSITION_TIME)
        {
            _transitionScreen.alpha = Mathf.Lerp(0f, 1f, i);
            yield return new WaitForFixedUpdate();
        }
        _transitionScreen.alpha = 1f;
        SceneManager.LoadScene("GameScene");
    }
}
