using System;
using System.Collections.Generic;

public class SimipleEventSystem : Singleton<SimipleEventSystem>
{
    public class EventCallback
    {
        public int id;
        public Action callback;
    }

    private Dictionary<EventEnum, LinkedList<EventCallback>> m_eventsDic = new Dictionary<EventEnum, LinkedList<EventCallback>>();

    private int m_serialNum;

    public int AddEventListener(EventEnum eventEnum, Action callback)
    {
        m_serialNum++;
        EventCallback eventCallback = new EventCallback();
        eventCallback.id = m_serialNum;
        eventCallback.callback = callback;

        LinkedList<EventCallback> list = null;
        if(m_eventsDic.TryGetValue(eventEnum, out list))
        {
            list.AddLast(eventCallback);
        }
        else{
            list = new LinkedList<EventCallback>();
            list.AddLast(eventCallback);
            m_eventsDic.Add(eventEnum, list);
        }
        return m_serialNum;
    }

    public void RemoveEventListener(EventEnum eventEnum, int serialNum)
    {
        LinkedList<EventCallback> list = null;
        if(m_eventsDic.TryGetValue(eventEnum, out list))
        {
            EventCallback delNode = null;
            foreach(var node in list)
            {
                if(node.id == serialNum)
                {
                    delNode = node;
                    break;
                }
            }
            if(delNode != null)
            {
                list.Remove(delNode);
            }
        }
    }

    public void FireEvent(EventEnum eventEnum)
    {
        LinkedList<EventCallback> list = null;
        if(m_eventsDic.TryGetValue(eventEnum, out list))
        {
            foreach(var node in list)
            {
                if(node.callback != null)
                {
                    node.callback();
                }
            }
        }
    }

    public void ClearEventListener()
    {
        m_eventsDic.Clear();
        m_serialNum = 0;
    }
}