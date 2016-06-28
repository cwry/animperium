using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class ButtonComponent : MonoBehaviour
{
    public DynamicButton button;
    EventTrigger trigger;
    public string ability;
    //EventSprite spriteEvent;

    void Start()
    {
        button = gameObject.AddComponent<DynamicButton>();
        trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { button.OnClick((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }
}
public  class DynamicButton : MonoBehaviour{

    
    public Action execute;

    
    public  void OnClick(PointerEventData data)
    {
        GUIData.canSelectTarget = true;
        GUIData.activeButton = gameObject;
        //execute();
    }

    //public void SetFunction(Action v)
    //{
    //   execute = v;
    //}
    
}
