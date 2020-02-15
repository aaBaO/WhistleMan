using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour, IGameView
{
    public Button btn_Start;
    public Button btn_quit;

    // Start is called before the first frame update
    void Start()
    {
        btn_Start.onClick.AddListener(OnClick_btn_start);         
        btn_quit.onClick.AddListener(OnClick_btn_quit);         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnViewOpen()
    {

    }

    public void OnViewClose()
    {
        Destroy(gameObject);
    }

    void OnClick_btn_start()
    {
        LevelController.instance.StartLevel("Level1");
    }

    void OnClick_btn_quit()
    {
    }
}
