using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _transitionScreen;

    private const float ENTER_TRANSITION_TIME = 0.5f;
    private const float EXIT_TRANSITION_TIME = 1f;

    private void OnEnable()
    {
        StartCoroutine(OnShow());
    }

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

    private IEnumerator OnShow()
    {
        for (float i = 0; i < 1; i+= Time.deltaTime/ENTER_TRANSITION_TIME)
        {
            _transitionScreen.alpha = Mathf.Lerp(1f, 0f, i);
            yield return new WaitForFixedUpdate();
        }
        _transitionScreen.alpha = 0f;
        _transitionScreen.gameObject.SetActive(false);
    }

    private IEnumerator LoadGameScene()
    {
        _transitionScreen.gameObject.SetActive(true);
        for (float i = 0; i < 1; i+= Time.deltaTime/EXIT_TRANSITION_TIME)
        {
            _transitionScreen.alpha = Mathf.Lerp(0f, 1f, i);
            yield return new WaitForFixedUpdate();
        }
        _transitionScreen.alpha = 1f;
        SceneManager.LoadScene("GameScene");
    }
}
