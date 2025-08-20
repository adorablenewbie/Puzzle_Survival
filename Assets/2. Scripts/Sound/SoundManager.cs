using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

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
        BgmSound.PlayOneShot(Day);
    }

    public void NightTimeBGM()
    {
        BgmSound.PlayOneShot(Night);
    }

    public void ZombieAttack()
    {
        EffectSound.PlayOneShot(EffectClips[0]);
    }

    public void PlayerWalkingSound()
    {
        EffectSound.PlayOneShot(EffectClips[1]);
    }
}


