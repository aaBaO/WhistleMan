using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameResultView : MonoBehaviour, IGameView
{
    public TextMeshProUGUI txt_result;
    public Button btn_restart;
    public Button btn_nextLevel;
    public Button btn_back;

    void Start()
    {
        btn_restart.onClick.AddListener(OnClick_btn_restart);
        btn_nextLevel.onClick.AddListener(OnClick_btn_nextLevel);
        btn_back.onClick.AddListener(OnClick_btn_back);

        if(LevelController.instance.currentLevelSuccess)
        {
            OnGameWin();
        } else
        {
            OnGameLose();
        }
    }

    void Update()
    {
        
    }

    public void OnViewOpen()
    {
    }

    void OnClick_btn_restart()
    {
        LevelController.instance.RestartLevel();
    }

    void OnClick_btn_nextLevel()
    {
    } 

    void OnClick_btn_back()
    {
        LevelController.instance.QuitLevel();
    }

    void OnGameWin()
    {
        txt_result.text = "Win";
        btn_nextLevel.enabled = true;
    }

    void OnGameLose()
    {
        txt_result.text = "Lose";
        btn_nextLevel.enabled = false;
    }

    public void OnViewClose()
    {
        Destroy(gameObject);
    }
}
