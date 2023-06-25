using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _mainMusic;
    [SerializeField] private AudioClip _explorationMusic;
    [SerializeField] private AudioClip _buttonClickedAudio;
    [SerializeField] private AudioClip _explorationStartedAudio;
    [SerializeField] private AudioClip _cameraSwitchedAudio;

    private UIStateManager _stateManager;

    private void Start()
    {
        _stateManager = UIStateManager.GetInstance();
        _stateManager.OnStateChanged += OnStateChanged;
    }

    public void PlayButtonClickedAudio()
    {
        _audioSource.PlayOneShot(_buttonClickedAudio, 1);
    }

    public void PlayExplorationStartedAudio()
    {
        _audioSource.PlayOneShot(_explorationStartedAudio, 1);
    }

    public void PlayCameraSwitchedAudio()
    {
        _audioSource.PlayOneShot(_cameraSwitchedAudio, 1);
    }

    private void OnStateChanged(object sender, UIStateManager.GameState newState)
    {
        if (newState == UIStateManager.GameState.EXPLORING)
        {
            _audioSource.clip = _explorationMusic;
            _audioSource.Play();
        }
        else if (_audioSource.clip != _mainMusic)
        {
            _audioSource.clip = _mainMusic;
            _audioSource.Play();
        }
    }
}
