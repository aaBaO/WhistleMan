using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Virus : Character
{
    public Transform target;
    public float nextWaypointDistance = 2.0f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPoint = false;

    Seeker seeker;
    Rigidbody2D rb;

    /// <summary>
    /// 感染半径
    /// </summary>
    public float infectRadius = 3.0f;

    protected override void Start()
    {
        base.Start();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

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

    void OnDrawGizmos()
    {
        if(rb != null)
            Gizmos.DrawWireSphere(rb.position, infectRadius);
    }
}
