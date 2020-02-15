using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character 
{
    public float whistleRadius = 3.0f;

    Rigidbody2D m_rigidbody;

    protected override void Start()
    {
        base.Start();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
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
    }
}
