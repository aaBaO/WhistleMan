using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public GameObject UIRootCanvas;
    // Start is called before the first frame update
    void Start()
    {
        GameViewManager.instance.InitUIRootCanvas(UIRootCanvas);
        GameViewManager.instance.OpenView(GameViewConst.StartMenu); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
