using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FooPeople : Character
{
    public bool isInfected = false;
    public bool isWarned = false;
    public float moveDelay = 0;
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

    const string AcceptWarnVfxPath = "VFX/FooAccept";

    /// <summary>
    /// -1=left, 1=right
    /// </summary>
    int m_avatarXDirection = 0;

    protected override void Start()
    {
        base.Start();
        m_seeker = GetComponent<Seeker>();
        m_rb = GetComponent<Rigidbody2D>();

        Invoke("StartPath", moveDelay);
        AddBuildingStayExitListenter();

        if(targetPoints.Count <= 0)
        {
            m_seeker.enabled = false;
        }
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

        if(m_rb.velocity.magnitude > 0.001f)
        {
            m_avatar.PlayAnimation("walk");
        }
        else
        {
            m_avatar.PlayAnimation("idle");
        }

        if(m_rb.velocity.x > 0.01f)
        {
            FlipAvatar(1);
        } else if(m_rb.velocity.x < -0.01f)
        {
            FlipAvatar(-1);
        }
    }

    void FlipAvatar(int direction)
    {
        if(m_avatarXDirection == direction)
            return;

        m_avatarXDirection = direction;
        m_avatar.transform.rotation = direction == -1 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
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

    /// <summary>
    /// 被感染
    /// </summary>
    public void BeInfected()
    {
        if(isInfected || isWarned)
            return;
        
        if (m_rb.IsSleeping()) 
        {
            m_rb.WakeUp();
        }

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

        if (m_rb.IsSleeping()) 
        {
            m_rb.WakeUp();
        }

        isWarned = true;
        //播放被警示的特效，停止一切移动
        StartCoroutine(PlayVFX());

        AudioController.instance.PlayOneShotSound(AudioController.SoundType.PeopleGetWarn);
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