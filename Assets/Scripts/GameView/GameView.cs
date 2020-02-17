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

    public Image img_whistleCoolDown;
    public Vector3 whistleImageOffset;
    public Image img_shieldLeftTime;
    public Vector3 shieldImageOffset;

    GameLevel m_gameLevel;
    Player m_player;

    int m_peopleInfectedEventId;

    void Start()
    {
        m_gameLevel = Object.FindObjectOfType<GameLevel>();
        m_player = Object.FindObjectOfType<Player>();

        btn_restart.onClick.AddListener(OnClick_btn_restart);
        btn_back.onClick.AddListener(OnClick_btn_back);

        m_peopleInfectedEventId = SimpleEventSystem.instance.AddEventListener(EventEnum.PeopleInfected, OnPeopleInfected);
    }

    void Update()
    {
        lefttimeTimer.text = string.Format("{0:00}:{1:00}", m_gameLevel.leftTime / 60, m_gameLevel.leftTime % 60);
        safePeopleCount.text = string.Format("{0}Safe", m_gameLevel.safePeopleCount);

        //Update Shield Left time.
        var shieldLeftTime = m_player.GetShieldLeftTime();
        if(shieldLeftTime > 0)
        {
            img_shieldLeftTime.enabled = true;
            img_shieldLeftTime.fillAmount = shieldLeftTime;
        }else
        {
            img_shieldLeftTime.enabled = false;
        }

        //Update Whistle cool down time.
        var whistleCD = m_player.GetWhitleCoolDown();
        if(whistleCD > 0)
        {
            img_whistleCoolDown.enabled = true;
            img_whistleCoolDown.fillAmount = whistleCD;
        }else
        {
            img_whistleCoolDown.enabled = false;
        }
    }

    void LateUpdate()
    {
        if(m_player)
        {
            img_whistleCoolDown.rectTransform.anchoredPosition3D = Camera.main.WorldToScreenPoint(m_player.transform.position) + whistleImageOffset;
            img_shieldLeftTime.rectTransform.anchoredPosition3D = Camera.main.WorldToScreenPoint(m_player.transform.position) + shieldImageOffset;
        }
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
