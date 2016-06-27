using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class ButtonComponent : MonoBehaviour
{
    ButtonOnClick button;
    EventTrigger trigger;
    //EventSprite spriteEvent;

    void Start()
    {
        //spriteEvent = GetComponent<EventSprite>();
        switch (gameObject.name)
        {
            case "AttackButton":
                button = new AttackButton();
                break;
            case "MoveButton":
                button = new MoveUnitButton();
                break;
            case "DigButton":
                button = new DigButton();
                break;
            default:
                break;
        }
        trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { button.OnClick((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }
}
public abstract class ButtonOnClick : MonoBehaviour{
    
    public abstract void OnClick(PointerEventData data);
}
