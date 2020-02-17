using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FooPeople : Character
{
    public bool isInfected = false;
    public bool isWarned = false;
    public float moveDelay = 0;
    public Transform target;
    public float nextWaypointDistance = 2.0f;
    Path m_path;
    int m_currentWaypoint = 0;
    bool m_reachedEndOfPoint = false;

    Seeker m_seeker;
    Rigidbody2D m_rb;

    const string AcceptWarnVfxPath = "VFX/FooAccept";

    protected override void Start()
    {
        base.Start();
        m_seeker = GetComponent<Seeker>();
        m_rb = GetComponent<Rigidbody2D>();

        Invoke("StartPath", moveDelay);
        AddBuildingStayExitListenter();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        RemoveBuildingStayExitListener(); 
    }

    void StartPath()
    {
        if(isWarned)
            return;

        if(target != null && m_seeker != null)
            m_seeker.StartPath(m_rb.position, target.position, OnPathComplete);
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
    }

    /// <summary>
    /// 被感染
    /// </summary>
    public void BeInfected()
    {
        if(isInfected || isWarned)
            return;

        isInfected = true;
        gameObject.AddComponent<InfectionSource>();

        SimpleEventSystem.instance.FireEvent(EventEnum.PeopleInfected);
    }

    /// <summary>
    /// 被警告
    /// </summary>
    public void BeWarned()
    {
        if(isInfected || isWarned)
            return;

        isWarned = true;
        //播放被警示的特效，停止一切移动
        StartCoroutine(PlayVFX());
    }

    IEnumerator PlayVFX()
    {
        var rrq = Resources.LoadAsync<GameObject>(AcceptWarnVfxPath);
        yield return rrq;

        var go = rrq.asset as GameObject;

        Instantiate<GameObject>(go, transform, false);
    }

#region NPCStayExitWithBuilding
    int m_npcStayBuildingEventId;
    int m_npcExitBuildingEventId;

    void AddBuildingStayExitListenter()
    {
        m_npcStayBuildingEventId = SimpleEventSystem.instance.AddEventListener(EventEnum.NPCStayInBuilding, OnNPCStayBuilding);
        m_npcExitBuildingEventId = SimpleEventSystem.instance.AddEventListener(EventEnum.NPCExitBuilding, OnNPCExitBuilding);
    }

    void RemoveBuildingStayExitListener()
    {
        SimpleEventSystem.instance.RemoveEventListener(EventEnum.NPCStayInBuilding, m_npcStayBuildingEventId);
        SimpleEventSystem.instance.RemoveEventListener(EventEnum.NPCExitBuilding, m_npcExitBuildingEventId);
    }

    void OnNPCStayBuilding()
    {
        if(m_avatar.GetMaskInteraction() == SpriteMaskInteraction.None)
            m_avatar.SetMaskInteraction(SpriteMaskInteraction.VisibleInsideMask);
    }

    void OnNPCExitBuilding()
    {
        m_avatar.SetMaskInteraction(SpriteMaskInteraction.None);
    }
#endregion
}