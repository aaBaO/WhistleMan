using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingStayTrigger : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Virus") || other.CompareTag("People"))
        {
            SimpleEventSystem.instance.FireEvent(EventEnum.NPCStayInBuilding);
        }

        if(other.CompareTag("BuildingMaskTrigger"))
        {
            SimpleEventSystem.instance.FireEvent(EventEnum.PlayerStayInBuilding);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Virus") || other.CompareTag("People"))
        {
            SimpleEventSystem.instance.FireEvent(EventEnum.NPCExitBuilding);
        }

        if(other.CompareTag("BuildingMaskTrigger"))
        {
            SimpleEventSystem.instance.FireEvent(EventEnum.PlayerExitBuilding);
        }
    }
}
