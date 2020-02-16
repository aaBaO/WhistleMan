using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameView : MonoBehaviour, IGameView
{
    public TextMeshProUGUI safePeopleCount;
    public Animator safePeopleTextAnimator;
    public TextMeshProUGUI lefttimeTimer;

    public Button btn_restart;
    public Button btn_back;

    public GameLevel gameLevel;

    int m_peopleInfectedEventId;

    void Start()
    {
        var gameLevelObj = Object.FindObjectOfType(typeof(GameLevel));
        if(gameLevelObj)
            gameLevel = gameLevelObj as GameLevel;

        btn_restart.onClick.AddListener(OnClick_btn_restart);
        btn_back.onClick.AddListener(OnClick_btn_back);

        m_peopleInfectedEventId = SimpleEventSystem.instance.AddEventListener(EventEnum.PeopleInfected, OnPeopleInfected);
    }

    void Update()
    {
        lefttimeTimer.text = gameLevel.leftTime.ToString();
        safePeopleCount.text = string.Format("{0}Safe", gameLevel.safePeopleCount);
    }

    public void OnViewOpen()
    {

    }

    public void OnViewClose()
    {
        SimpleEventSystem.instance.RemoveEventListener(EventEnum.PeopleInfected, m_peopleInfectedEventId);
        Destroy(gameObject);
    }

    void OnClick_btn_restart()
    {
        LevelController.instance.RestartLevel();
    }

    void OnClick_btn_back()
    {
        LevelController.instance.QuitLevel();
    }

    void OnPeopleInfected()
    {
        safePeopleTextAnimator.Play("PeopleInfected");
    }
}
