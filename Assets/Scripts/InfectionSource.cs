using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionSource : MonoBehaviour
{
    /// <summary>
    /// 感染半径
    /// </summary>
    public float infectRadius = 3.0f;

    /// <summary>
    /// 感染源特效
    /// </summary>
    GameObject m_vfx;
    const string VfxPath = "VFX/FX_Virus";

    IEnumerator Start()
    {
        var rrq = Resources.LoadAsync<GameObject>(VfxPath);
        yield return rrq;

        m_vfx = rrq.asset as GameObject;

        Instantiate<GameObject>(m_vfx, transform, false);
    }

    void Update()
    {
        var center = transform.position;
        var colliders = Physics2D.OverlapCircleAll(center, infectRadius);
        if(colliders.Length > 0)
        {
            foreach (var item in colliders)
            {
                if(item.CompareTag("People"))
                {
                    FooPeople fooPeople = item.GetComponent<FooPeople>();
                    fooPeople.BeInfected();
                } 

                if(item.CompareTag("Player"))
                {
                    Player player = item.GetComponent<Player>();
                    player.BeInfected();
                } 
            }
        }
    }

    void OnDestroy()
    {
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, infectRadius);
    }
}
