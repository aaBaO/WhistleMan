using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Virus : Character
{
    public List<Transform> targetPoints = new List<Transform>();
    /// <summary>
    /// 是否巡逻
    /// </summary>
    public bool partol;
    int m_currentTargetPoint = 0;

    public float nextWaypointDistance = 2.0f;

    Path m_path;
    int m_currentWaypoint = 0;
    bool m_reachedEndOfPoint = false;

    Seeker m_seeker;
    Rigidbody2D m_rb;

    protected override void Start()
    {
        base.Start();
        m_seeker = GetComponent<Seeker>();
        m_rb = GetComponent<Rigidbody2D>();

        StartPath();
    }

    void StartPath()
    {
        if(m_seeker && targetPoints.Count > 0)
        {
            if(m_currentTargetPoint >= targetPoints.Count)
            {
                if(partol)
                {
                    m_currentTargetPoint = 0;
                } else
                {
                    return;
                }
            } 

            m_seeker.StartPath(m_rb.position, targetPoints[m_currentTargetPoint].position, OnPathComplete);
        }
    }

    void OnPathComplete(Path path)
    {
        if(!path.error)
        {
            this.m_path = path;
            m_currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if(m_path == null)
            return;

        if(m_currentWaypoint >= m_path.vectorPath.Count)
        {
            m_reachedEndOfPoint = true;
            m_path = null;
            return;
        } else
        {
            m_reachedEndOfPoint = false;
        }

        Vector2 direction = ((Vector2)m_path.vectorPath[m_currentWaypoint] - m_rb.position).normalized;
        Vector2 force = direction * moveSpeed * Time.deltaTime;

        m_rb.AddForce(force);

        float distance = Vector2.Distance(m_rb.position, m_path.vectorPath[m_currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            m_currentWaypoint++;
        }
    }

    protected override void Update()
    {
        if(m_reachedEndOfPoint)
        {
            m_reachedEndOfPoint = false;
            m_currentTargetPoint++;
            StartPath();
        }
    }
}
