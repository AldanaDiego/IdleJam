using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfigMenuManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _transitionScreen;

    private SaveSystem _saveSystem;
    private const float EXIT_TRANSITION_TIME = 0.5f;

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

    private IEnumerator ExitScene()
    {
        _transitionScreen.gameObject.SetActive(true);
        for (float i = 0; i < 1; i+= Time.deltaTime/EXIT_TRANSITION_TIME)
        {
            _transitionScreen.alpha = Mathf.Lerp(0f, 1f, i);
            yield return new WaitForFixedUpdate();
        }
        _transitionScreen.alpha = 1f;
        SceneManager.LoadScene("TitleScene");
    }
}
