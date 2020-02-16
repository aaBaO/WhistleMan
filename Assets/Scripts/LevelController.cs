using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : Singleton<LevelController>
{
    string m_currentSceneName;
    public bool currentLevelSuccess = false;

    public void StartLevel(string sceneName)
    {
        GameViewManager.instance.CloseView(GameViewConst.GameResultView);
        GameViewManager.instance.CloseView(GameViewConst.StartMenu);
        GameViewManager.instance.OpenView(GameViewConst.LoadingView);

        currentLevelSuccess = false;

        if(!string.IsNullOrEmpty(m_currentSceneName))
            SceneManager.UnloadSceneAsync(m_currentSceneName);
        m_currentSceneName = sceneName;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncOperation.completed += (async)=>{
            GameViewManager.instance.CloseView(GameViewConst.LoadingView);
            GameViewManager.instance.OpenView(GameViewConst.GameView);
            asyncOperation = null;
        };
    }

    public void RestartLevel()
    {
        GameViewManager.instance.CloseView(GameViewConst.GameResultView);
        GameViewManager.instance.CloseView(GameViewConst.GameView);

        StartLevel(m_currentSceneName);
    }

    public void QuitLevel()
    {
        GameViewManager.instance.CloseView(GameViewConst.GameResultView);
        GameViewManager.instance.CloseView(GameViewConst.GameView);
        GameViewManager.instance.OpenView(GameViewConst.LoadingView);

        if(!string.IsNullOrEmpty(m_currentSceneName))
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(m_currentSceneName);
            asyncOperation.completed += (async)=>{
                GameViewManager.instance.CloseView(GameViewConst.LoadingView);
                GameViewManager.instance.OpenView(GameViewConst.StartMenu);
                asyncOperation = null;
            };
        }
        
        m_currentSceneName = "";
    }
}