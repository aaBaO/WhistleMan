using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevel : MonoBehaviour
{
    public int levelTotalTime = 12000; //ms
    public int targetSaveCount;
    float m_startTime;
    public float leftTime {private set; get;}

    public int virusCount;
    FooPeople[] m_peopleArray;

    int m_peopleCount;
    public int safePeopleCount;
    public int warnedPeopleCount;

    bool m_isEndOfLevel;

    void Start()
    {
        leftTime = float.MaxValue;
        m_startTime = Time.time;

        virusCount = Object.FindObjectsOfType(typeof(Virus)).Length;
        m_peopleArray = Object.FindObjectsOfType(typeof(FooPeople)) as FooPeople[];
        m_peopleCount = m_peopleArray.Length;

        m_isEndOfLevel = false;
    }

    void Update()
    {
        if(m_isEndOfLevel)
        {
            return;
        }

        if(safePeopleCount <= 0)
        {
            m_isEndOfLevel = true;
            return;
        }

        if(leftTime <= 0)
        {
            //Game over, check safe people count.
            //You'll win if you save more then target count.
            GameViewManager.instance.OpenView(GameViewConst.GameResultView);
            if(safePeopleCount >= targetSaveCount)
            {
                LevelController.instance.currentLevelSuccess = true;
            } else
            {
                LevelController.instance.currentLevelSuccess = false;
            }
            m_isEndOfLevel = true;
            return;
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