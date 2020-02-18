using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public GameObject UIRootCanvas;

    void Start()
    {
        GameViewManager.instance.InitUIRootCanvas(UIRootCanvas);
        GameViewManager.instance.OpenView(GameViewConst.StartMenu); 
    }

    void Update()
    {
        
    }
}
