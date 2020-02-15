using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : Singleton<LevelController>
{
    string m_currentSceneName;

    public void StartLevel(string sceneName)
    {
        GameViewManager.instance.CloseView("StartMenu");
        GameViewManager.instance.OpenView("LoadingView");
        if(!string.IsNullOrEmpty(m_currentSceneName))
            SceneManager.UnloadSceneAsync(m_currentSceneName);
        m_currentSceneName = sceneName;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncOperation.completed += (async)=>{
            GameViewManager.instance.CloseView("LoadingView");
            GameViewManager.instance.OpenView("GameView");
            asyncOperation = null;
        };
    }

    public void RestartLevel()
    {
        GameViewManager.instance.CloseView("GameView");
        StartLevel(m_currentSceneName);
    }

    public void QuitLevel()
    {
        GameViewManager.instance.CloseView("GameView");
        GameViewManager.instance.OpenView("LoadingView");

        if(!string.IsNullOrEmpty(m_currentSceneName))
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(m_currentSceneName);
            asyncOperation.completed += (async)=>{
                GameViewManager.instance.CloseView("LoadingView");
                GameViewManager.instance.OpenView("StartMenu");
                asyncOperation = null;
            };
        }
        
        m_currentSceneName = "";
    }
}