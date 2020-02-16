using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMaskTrigger : MonoBehaviour
{

    GameObject m_spriteMask;

    bool m_isMaskEnable;

    void Start()
    {
        m_isMaskEnable = false;
        m_spriteMask = transform.GetChild(0).gameObject;

        m_spriteMask.SetActive(m_isMaskEnable);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("BuildingMaskTrigger"))
        {
            m_isMaskEnable = !m_isMaskEnable;
            m_spriteMask.SetActive(m_isMaskEnable);
        }
    }
}
