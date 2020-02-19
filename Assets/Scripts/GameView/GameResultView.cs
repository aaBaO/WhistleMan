﻿using System.Collections;
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

        if(LevelController.instance.savedPeopleCount > 15)
        {
            txt_result.text = "厉害啊,救了这么多人,可以拯救世界了!";
        } else if (LevelController.instance.savedPeopleCount > 5)
        {
            txt_result.text = "不错,不错,再接再厉!";
        }else
        {
            txt_result.text = "只能拯救这么多人?怕是人都没有找齐吧?";
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
