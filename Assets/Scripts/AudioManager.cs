using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance = null;
    public static AudioManager Instance { get { return instance; } }

    [SerializeField] private AudioClip _shotClip;
    [SerializeField] private AudioClip _rechargeClip;
    [SerializeField] private AudioClip _gameOverClip;
    [SerializeField] private AudioClip _nextLevelOrWinClip;
    [SerializeField] private AudioClip _startGameClip;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource _effectsSource;
      
    private bool isMute = false;

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

    }
    public void ShotSound()
    {
        StartCoroutine(ShotSoundCouratine());
    }

    public void GameOverSound()
    {
        _effectsSource.PlayOneShot(_gameOverClip);
    }

    public void NextLevelOrWinSound()
    {
        _effectsSource.PlayOneShot(_nextLevelOrWinClip);
    }

    public void StartGameSound()
    {
        _effectsSource.PlayOneShot(_startGameClip);
    }

    IEnumerator ShotSoundCouratine()
    {
        _effectsSource.PlayOneShot(_shotClip);
        yield return new WaitForSeconds(0.5f);
        _effectsSource.PlayOneShot(_rechargeClip);
    }

    public void isMuteMethod()
    {
        if (isMute)
        {
            audioMixer.SetFloat("Master", 0);
            isMute = false;
        }
        else
        {          
            audioMixer.SetFloat("Master", -80);
            isMute = true;
        }
    }

    public void VolumeMethod(float valueChange)
    {
        float effectsVolume = -80;
        float musicVolume = -45;
        effectsVolume += valueChange * 80;
        musicVolume += valueChange * 20;
        audioMixer.SetFloat("Music", musicVolume);
        audioMixer.SetFloat("Effects", effectsVolume);
    }
}
