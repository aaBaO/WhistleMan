using UnityEngine;

public class CharacterAvatar : MonoBehaviour
{
    SpriteMaskInteraction m_maskInteraction = SpriteMaskInteraction.None;

    SpriteRenderer[] m_allSprite;

    private void Start()
    {
        m_allSprite = GetComponentsInChildren<SpriteRenderer>();    
    }

    public SpriteMaskInteraction GetMaskInteraction()
    {
        return m_maskInteraction;
    }

    public void SetMaskInteraction(SpriteMaskInteraction mode)
    {
        m_maskInteraction = mode;
        foreach(var r in m_allSprite)
        {
            r.maskInteraction = m_maskInteraction;
        }
        //Also set VFX
        ParticleSystemRenderer[] psrs = transform.parent.GetComponentsInChildren<ParticleSystemRenderer>();
        foreach(var psr in psrs)
        {
            psr.maskInteraction = mode;   
        }
    }
}