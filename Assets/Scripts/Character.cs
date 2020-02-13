using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum CharacterType
    {
        None = 0,
        WhistlrMan,
        Human,
        Virus,
    }

    protected CharacterAvatar m_avatar;

    public CharacterType characterType = CharacterType.None;
    public float moveSpeed;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_avatar = transform.GetComponentInChildren<CharacterAvatar>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
