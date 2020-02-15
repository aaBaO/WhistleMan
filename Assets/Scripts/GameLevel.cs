using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevel : MonoBehaviour
{
    public int levelTotalTime = 12000; //ms
    float m_startTime;
    public float leftTime {private set; get;}

    public int virusCount;
    FooPeople[] m_peopleArray;

    int peopleCount;
    public int safePeopleCount;
    public int warnedPeopleCount;

    void Start()
    {
        leftTime = 0;
        m_startTime = Time.time;

        virusCount = Object.FindObjectsOfType(typeof(Virus)).Length;
        m_peopleArray = Object.FindObjectsOfType(typeof(FooPeople)) as FooPeople[];
        peopleCount = m_peopleArray.Length;
    }

    void Update()
    {
        if(leftTime <= 0 || safePeopleCount <= 0)
        {
            //failed
        } 

        if(peopleCount - warnedPeopleCount <= 0)
        {
            //success
            Debug.Log("win");
        }

        leftTime = m_startTime + levelTotalTime * 0.001f - Time.time;

        int safeCount = 0;
        int warnedCount = 0;
        foreach(var p in m_peopleArray)
        {
            if(!p.isInfected)
                safeCount++;

            if(p.isWarned)
                warnedCount++;
        }
        safePeopleCount = safeCount;
        warnedPeopleCount = warnedCount;
    }
}