using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameViewConst
{
    public static string StartMenu = "StartMenu";
    public static string LoadingView = "LoadingView";
    public static string GameView = "GameView";
    public static string ListView = "ListView";
    public static string GameResultView = "GameResultView";

    public static Dictionary<string, string> ViewDic = new Dictionary<string, string>{
        {StartMenu, "UI/StartMenu"}, 
        {LoadingView, "UI/LoadingView"}, 
        {GameView, "UI/GameView"}, 
        {ListView, "UI/ListView"}, 
        {GameResultView, "UI/GameResultView"}, 
    };
}

public class GameViewManager : Singleton<GameViewManager>
{
    Stack m_viewStack;
    Dictionary<string, IGameView> m_openedViewDic;
    GameObject m_canvasGameObject;
    public Canvas UIRootCanvas { get; private set;}

    public GameViewManager()
    {
        m_viewStack = new Stack();
        m_openedViewDic = new Dictionary<string, IGameView>();
    }

    public void InitUIRootCanvas(GameObject go)
    {   
        m_canvasGameObject = go;
        UIRootCanvas = go.GetComponent<Canvas>();
    }

    public void OpenView(string viewName)
    {
        IGameView openedView = null;
        if(m_openedViewDic.TryGetValue(viewName, out openedView))
        {
            return;
        }

        string viewpath = "";
        if(GameViewConst.ViewDic.TryGetValue(viewName, out viewpath))
        {
            ResourceRequest rr = Resources.LoadAsync<GameObject>(viewpath);
            rr.completed += (async)=>{
                GameObject asset = rr.asset as GameObject;
                GameObject viewgo = GameObject.Instantiate(asset, m_canvasGameObject.transform, false);
                IGameView view = viewgo.GetComponent<IGameView>();
                view.OnViewOpen();
                m_openedViewDic.Add(viewName, view);

                rr = null;
            };
        }
    }

    public void CloseView(string viewName)
    {
        IGameView openedView = null;
        if(m_openedViewDic.TryGetValue(viewName, out openedView))
        {
            openedView.OnViewClose();
            m_openedViewDic.Remove(viewName);
        } else
        {
            return;
        }
    }

    public void Destroy()
    {
        m_openedViewDic.Clear();
        m_openedViewDic = null;
        m_viewStack.Clear();
        m_viewStack = null;
    }
}
