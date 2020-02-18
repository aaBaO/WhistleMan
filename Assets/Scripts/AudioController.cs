using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : Singleton<AudioController>
{
    public enum SoundType
    {
        None = 0,
        MenuBgm,
        GameBgm,
        Whistle,
        PeopleGetWarn,
        Lose,
        Continue,
        GetShield,
        ShieldEnd,
    }

    SoundType m_currentBgmType = SoundType.None;
    AudioSource m_bgmSource;
    AudioSource m_oneshotSource;

    public void PlayBgm(SoundType soundType)
    {
        if(m_bgmSource == null)
        {
            m_bgmSource = new GameObject("BgmSource").AddComponent<AudioSource>();
            m_bgmSource.spatialBlend = 0;
            m_bgmSource.playOnAwake = false;
            m_bgmSource.volume = 0.5f;
            m_bgmSource.loop = true;
        }

        if(m_currentBgmType == soundType)
            return;

        m_bgmSource.clip = GetAudioClip(soundType); 
        if(m_bgmSource.clip)
            m_currentBgmType = soundType;

        m_bgmSource.Play();
    }

    AudioClip GetAudioClip(SoundType soundType)
    {
        if(GameAssets.instance == null)
            return null;

        foreach(var sa in GameAssets.instance.soundArray)
        {
            if(sa.soundType == soundType)
            {
                if(sa.clip == null)
                {
                    Debug.LogError("Sound " + soundType + " missing!");
                }
                return sa.clip;
            }
        }
        Debug.LogError("Sound " + soundType + " not found!");
        return null;    
    }

    public void PlayOneShotSound(SoundType soundType)
    {
        if(m_oneshotSource == null)
        {
            m_oneshotSource = new GameObject("OneShotAudioSource").AddComponent<AudioSource>();
            m_oneshotSource.spatialBlend = 0;
            m_oneshotSource.loop = false;
            m_oneshotSource.playOnAwake = false;
        }
        m_oneshotSource.PlayOneShot(GetAudioClip(soundType));
    }
}