using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    PlayerController playerController;

    public AudioMixer audioMixer;

    public GameObject soundUI;

    public Slider BgmSlider;
    public Slider SfxSlider;

    public AudioSource BgmSound;
    public AudioSource EffectSound;

    public AudioClip[] BgmClips;
    public AudioClip[] EffectClips;

    public AudioClip Day;
    public AudioClip Night;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        playerController = PlayerManager.Instance.Player.controller;
        NightTimeBGM();
    }

    void Update()
    {
        OpenSoundUI();
    }

    public void OpenSoundUI()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (!soundUI.activeSelf)
            {
                soundUI.SetActive(true);
                playerController.ToggleCursor();
            }
            else
            {
                playerController.ToggleCursor();
                soundUI.SetActive(false);               
            }
        }
    }
    public void BgmEnd()
    {
        BgmSound.Stop();
    }
    public void BgmSoundMute()
    {
        BgmSound.mute = !BgmSound.mute;
    }
    public void EffectEnd()
    {
        EffectSound.Stop();
    }
    public void EffectSoundMute()
    {
        EffectSound.mute = !EffectSound.mute;
    }

    public void DayTimeBGM()
    {
        BgmSound.clip = Day;
        BgmSound.Play();
    }

    public void NightTimeBGM()
    {
        BgmSound.clip = Night;
        BgmSound.Play();
    }
    public void MazeBGM()
    {
        BgmSound.clip = BgmClips[0];
        BgmSound.Play();
    }

    public void ShootingSound()
    {
        EffectSound.PlayOneShot(EffectClips[0]);
    }
    public void PlayerWalkingSound()
    {
        EffectSound.PlayOneShot(EffectClips[1]);
    }

/*    public void SetBgmVolume()
    {
        audioMixer.SetFloat("BgmVolume", Mathf.Log10(BgmSlider.value) * 20);
    }*/
    public void SetBgmVolume()
    {
        float volume = BgmSlider.value;
        audioMixer.SetFloat("BgmVolume", Mathf.Log10(volume) * 20);
    }

/*    public void BgmVolume()
    {
        OnSliderValueChanged(BgmSlider.value);
    }

    private void OnSliderValueChanged(float value)
    {
        throw new NotImplementedException();
    }*/

    public void SetSfxVolume()
    {
        float volume = SfxSlider.value;
        audioMixer.SetFloat("SfxVolume", Mathf.Log10(volume) * 20);
    }

}


