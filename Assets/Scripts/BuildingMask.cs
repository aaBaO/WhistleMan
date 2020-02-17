using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMask : MonoBehaviour
{
    SpriteMask m_spriteMask;

    int m_stayEventId;
    int m_exitEventId;

    void Start()
    {
        m_spriteMask = GetComponent<SpriteMask>();

        m_stayEventId = SimpleEventSystem.instance.AddEventListener(EventEnum.PlayerStayInBuilding, OnCharacterStayInBuilding);
        m_exitEventId = SimpleEventSystem.instance.AddEventListener(EventEnum.PlayerExitBuilding, OnCharacterExitBuilding);
    }

    void OnCharacterStayInBuilding()
    {
        if(m_spriteMask.enabled == false){
            m_spriteMask.enabled = true;
        }
    }

    void OnCharacterExitBuilding()
    {
        m_spriteMask.enabled = false;
    }

    private void OnDestroy()
    {
        SimpleEventSystem.instance.RemoveEventListener(EventEnum.PlayerStayInBuilding, m_stayEventId);    
        SimpleEventSystem.instance.RemoveEventListener(EventEnum.PlayerExitBuilding, m_exitEventId);    
    }
}
