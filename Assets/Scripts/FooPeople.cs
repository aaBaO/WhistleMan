using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FooPeople : Character
{
    public bool isInfected = false;
    public float moveDelay = 0;

    /// <summary>
    /// 感染半径
    /// </summary>
    public float infectRadius = 3;

    public Transform target;
    public float nextWaypointDistance = 2.0f;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPoint = false;

    Seeker seeker;
    Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        Invoke("StartPath", moveDelay);
    }

    void StartPath()
    {
        if(target != null)
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path path)
    {
        if(!path.error)
        {
            this.path = path;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if(path == null)
            return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPoint = true;
            return;
        } else
        {
            reachedEndOfPoint = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * moveSpeed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    protected override void Update()
    {
        if(isInfected)
        {
            var colliders = Physics2D.OverlapCircleAll(rb.position, infectRadius);
            if(colliders.Length > 0)
            {
                foreach (var item in colliders)
                {
                    if(item.CompareTag("People"))
                    {
                        FooPeople fooPeople = item.GetComponent<FooPeople>();
                        fooPeople.BeInfected();
                    } 
                }
            }
        }
    }

    /// <summary>
    /// 被感染
    /// </summary>
    public void BeInfected()
    {
        isInfected = true;
        m_avatar.transform.localScale = new Vector3(2, 2, 1);
    }
}