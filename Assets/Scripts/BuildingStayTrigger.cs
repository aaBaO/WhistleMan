using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingStayTrigger : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Virus") || other.CompareTag("People"))
        {
            Debug.Log("here stay");
            Character c = other.GetComponent<Character>();
            c.avater.SetMaskInteraction(SpriteMaskInteraction.VisibleInsideMask);
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
            Character c = other.GetComponent<Character>();
            c.avater.SetMaskInteraction(SpriteMaskInteraction.None);
        }

        if(other.CompareTag("BuildingMaskTrigger"))
        {
            SimpleEventSystem.instance.FireEvent(EventEnum.PlayerExitBuilding);
        }
    }
}
