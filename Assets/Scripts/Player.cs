using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character 
{
    protected override void Start()
    {
        base.Start();
        int hiteventid = SimipleEventSystem.instance.AddEventListener(EventEnum.PlayerHitTrigger, ()=>{Debug.Log("hit");});
    }

    protected override void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {
        var h = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        var v = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        m_avatar.transform.Translate(new Vector3(h, v, 0), Space.World);
    }
}
