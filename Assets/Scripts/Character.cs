using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum CharacterType
    {
        None = 0,
        WhistleMan,
        Human,
        Virus,
    }

    protected CharacterAvatar m_avatar;

    public CharacterType characterType = CharacterType.None;
    public float moveSpeed;
    public float movementSmoothing = 0.2f;
    protected Vector3 currentVelocity;

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
