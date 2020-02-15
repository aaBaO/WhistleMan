using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameViewConst
{
    public static string StartMenuPath = "UI/StartMenu";
    public static string LoadingViewPath = "UI/LoadingView";
    public static string GameViewPath = "UI/GameView";
    public static string ListViewPath = "UI/ListView";

    public static Dictionary<string, string> ViewDic = new Dictionary<string, string>{
        {"StartMenu", StartMenuPath}, 
        {"LoadingView", LoadingViewPath}, 
        {"GameView", GameViewPath}, 
        {"ListView", ListViewPath}, 
    };
}

public class GameViewManager : Singleton<GameViewManager>
{
    Stack m_viewStack;
    Dictionary<string, IGameView> m_openedViewDic;
    GameObject m_UIRootCanvas;

    public GameViewManager()
    {
        m_viewStack = new Stack();
        m_openedViewDic = new Dictionary<string, IGameView>();
    }

    public void InitUIRootCanvas(GameObject go)
    {
        m_UIRootCanvas = go;
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
                GameObject viewgo = GameObject.Instantiate(asset, m_UIRootCanvas.transform, false);
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
