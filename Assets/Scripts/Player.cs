using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character 
{
    public float whistleRadius = 3.0f;
    public float whistleCD = 2.0f;
    float m_lastWhistleTime;

    Rigidbody2D m_rigidbody;

    /// <summary>
    /// -1=left, 1=right
    /// </summary>
    int m_avatarXDirection = 0;
    /// <summary>
    /// -1=down, 1=up
    /// </summary>
    int m_avatarYDirection = 0;
    public ParticleSystem m_VFX_dustVFX;
    public ParticleSystem m_VFX_shield;
    public ParticleSystem m_VFX_whistleWave;

    /// <summary>
    /// 玩家获得护盾
    /// </summary>
    int m_hitShieldEventId;
    int m_playerShieldEndEventId;
    Shield m_shield;

    /// <summary>
    /// 是否被感染
    /// </summary>
    public bool m_isInfected;

    protected override void Start()
    {
        base.Start();

        m_isInfected = false;
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_lastWhistleTime = float.MinValue;

        AddListener();
    }

    private void AddListener()
    {
        m_hitShieldEventId = SimpleEventSystem.instance.AddEventListener(EventEnum.PlayerHitShield, OnPlayerHitShield);
        m_playerShieldEndEventId = SimpleEventSystem.instance.AddEventListener(EventEnum.PlayerShieldEnd, OnPlayerShieldEnd);
    }

    protected override void OnDestroy()
    {
        RemoveListener();
    }

    private void RemoveListener()
    {
        SimpleEventSystem.instance.RemoveEventListener(EventEnum.PlayerHitShield, m_hitShieldEventId);
        SimpleEventSystem.instance.RemoveEventListener(EventEnum.PlayerShieldEnd, m_playerShieldEndEventId);
    }

    protected override void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            if(Time.time - m_lastWhistleTime < whistleCD)
                return;

            m_lastWhistleTime = Time.time;
            m_VFX_whistleWave.Play();
            AudioController.instance.PlayOneShotSound(AudioController.SoundType.Whistle);
            var center = transform.position;
            var colliders = Physics2D.OverlapCircleAll(center, whistleRadius);
            if(colliders.Length > 0)
            {
                foreach (var item in colliders)
                {
                    if(item.CompareTag("People"))
                    {
                        FooPeople fooPeople = item.GetComponent<FooPeople>();
                        fooPeople.BeWarned();
                    } 
                }
            }
        }
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        var h = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        var v = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        Vector3 targetVelocity = new Vector2(h, v);
        // And then smoothing it out and applying it to the character
        m_rigidbody.velocity = Vector3.SmoothDamp(m_rigidbody.velocity, targetVelocity, ref currentVelocity, movementSmoothing);

        // If the velocity.x > 0.01f, make character turn to right
        // If the velocity.x < -0.01f, make character turn to left
        if(m_rigidbody.velocity.x > 0.01f)
        {
            FlipAvatar(1);
        } else if(m_rigidbody.velocity.x < -0.01f)
        {
            FlipAvatar(-1);
        }

        if(m_rigidbody.velocity.y > 0.01f)
        {
            SetYDirection(1);
        } else if(m_rigidbody.velocity.y < -0.01f)
        {
            SetYDirection(-1);
        }

        if(m_rigidbody.velocity.magnitude > 0.001f)
        {
            m_avatar.PlayAnimation("walk");
        }
        else
        {
            m_avatar.PlayAnimation("idle");
        }
    }

    void FlipAvatar(int direction)
    {
        if(m_avatarXDirection == direction)
            return;

        m_avatarXDirection = direction;
        m_avatar.transform.rotation = direction == -1 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
        PlayDust();
    }

    void SetYDirection(int direction)
    {
        if(m_avatarYDirection == direction)
            return;

        m_avatarYDirection = direction;
        PlayDust();
    }

    void PlayDust()
    {
        if(m_VFX_dustVFX)
            m_VFX_dustVFX.Play();
    }

    void OnPlayerHitShield()
    {
        if(m_VFX_shield)
            m_VFX_shield.Play();

        if(m_shield)
        {
            DestroyImmediate(m_shield);
            m_shield = null;
        }
        m_shield = gameObject.AddComponent<Shield>();

        AudioController.instance.PlayOneShotSound(AudioController.SoundType.GetShield);
    }

    void OnPlayerShieldEnd()
    {
        if(m_VFX_shield)
            m_VFX_shield.Stop();

        m_shield = null;
        AudioController.instance.PlayOneShotSound(AudioController.SoundType.ShieldEnd);
    }

    public float GetWhitleCoolDown()
    {
        float cd = Time.time - m_lastWhistleTime;
        return cd < whistleCD ? 1 - (cd / whistleCD) : 0; 
    }

    public float GetShieldLeftTime()
    {
        if(m_shield)
        {
            return (m_shield.leftTime / m_shield.duration);
        }
        return 0;
    }

    /// <summary>
    /// 被感染
    /// </summary>
    public void BeInfected()
    {
        if(m_shield || m_isInfected)
            return;

        m_isInfected = true;

        gameObject.AddComponent<InfectionSource>();

        SimpleEventSystem.instance.FireEvent(EventEnum.GameOver);

        this.enabled = false;
    }
}
