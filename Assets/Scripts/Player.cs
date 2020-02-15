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
    public ParticleSystem dust;
    public ParticleSystem whistleWave;

    protected override void Start()
    {
        base.Start();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_lastWhistleTime = float.MinValue;
    }

    protected override void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            if(Time.time - m_lastWhistleTime < whistleCD)
                return;

            m_lastWhistleTime = Time.time;
            whistleWave.Play();
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
        PlayerInput();
    }

    private void PlayerInput()
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
    }

    void FlipAvatar(int direction)
    {
        if(m_avatarXDirection == direction)
            return;

        m_avatarXDirection = direction;
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
        if(dust)
            dust.Play();
    }
}
