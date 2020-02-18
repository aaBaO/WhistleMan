using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    static GameAssets m_instance;
    public static GameAssets instance
    {
        get
        {
            return m_instance;
        }
    }

    void Awake()
    {
        m_instance = this;
    }

    [System.Serializable]
    public class SoundAsset
    {
        public AudioController.SoundType soundType;
        public AudioClip clip;
    }
    public SoundAsset[] soundArray;
}
