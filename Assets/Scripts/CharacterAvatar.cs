using UnityEngine;

public class CharacterAvatar : MonoBehaviour
{
    SpriteMaskInteraction m_maskInteraction = SpriteMaskInteraction.None;

    SpriteRenderer[] m_allSprite;

    Animator m_animator;
    string m_currentState;

    private void Start()
    {
        m_allSprite = GetComponentsInChildren<SpriteRenderer>();    
        m_animator = GetComponentInChildren<Animator>();  
    }

    public SpriteMaskInteraction GetMaskInteraction()
    {
        return m_maskInteraction;
    }

    public void SetMaskInteraction(SpriteMaskInteraction mode)
    {
        Debug.Log("here set begin");
        m_maskInteraction = mode;
        foreach(var r in m_allSprite)
        {
            r.maskInteraction = m_maskInteraction;
        }
        //Also set VFX
        ParticleSystemRenderer[] psrs = transform.parent.GetComponentsInChildren<ParticleSystemRenderer>();
        Debug.Log("here get psrs, length:" + psrs.Length);
        foreach(var psr in psrs)
        {
            Debug.Log("here set psr");
            psr.maskInteraction = mode;   
        }
    }

    public void PlayAnimation(string statename)
    {
        if(m_currentState == statename || m_animator == null)
            return;

        m_animator.Play(statename);
        m_currentState = statename;
    }
}