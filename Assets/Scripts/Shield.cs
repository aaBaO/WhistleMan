using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float duration = 10;

    float m_startTime;
    public float leftTime {private set; get;}

    void Start()
    {
        m_startTime = Time.time; 
    }

    void Update()
    {
        leftTime = duration - (Time.time - m_startTime);
        if(leftTime <= 0)
        {
            leftTime = 0;
            SimpleEventSystem.instance.FireEvent(EventEnum.PlayerShieldEnd);
            Destroy(this);
        }
    }
}
