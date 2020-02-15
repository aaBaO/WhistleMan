using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingView : MonoBehaviour, IGameView
{
    // Start is called before the first frame update
    void Start()
    {
        
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
}
