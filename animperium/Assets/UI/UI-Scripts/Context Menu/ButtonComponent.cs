using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public struct BC_fields
{
    public AbilityInfo ability;
    public GameObject[] targets;
    public GameObject descFieldPrefab;
    public string abilityDescription;
    public bool isActivated;
}
public class ButtonComponent : MonoBehaviour
{
    public EventTrigger trigger;
    public DynamicButton button;
    public BC_fields fields;
   
    //EventSprite spriteEvent;


    public void Init(AbilityInfo abili, GameObject descrField)
    {
        fields.ability = abili;
        fields.targets = abili.checkRange();
        fields.descFieldPrefab = descrField;
        fields.isActivated = true;
        AddListener();
    }

    void AddListener()
    {
        button = gameObject.AddComponent<DynamicButton>();
        trigger = GetComponent<EventTrigger>();
        if (fields.targets != null)
        {
            EventTrigger.Entry entryClick = new EventTrigger.Entry();
            entryClick.eventID = EventTriggerType.PointerClick;
            entryClick.callback.AddListener((data) => { button.OnClick((PointerEventData)data); });
            trigger.triggers.Add(entryClick);
        }
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => { button.OnPointerEnter((PointerEventData)data); });
        trigger.triggers.Add(entryEnter);
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) => { button.OnPointerExit((PointerEventData)data); });
        trigger.triggers.Add(entryExit);
        
        if(fields.targets == null)
        {
            EventSprite e = GetComponent<EventSprite>();
            e.normal = e.deactivated;
            e.highlighted = e.deactivated;
            e.pressed = e.deactivated;
            GetComponent<Image>().sprite = e.deactivated;
            fields.isActivated = false;
        }
    }

    
}
public  class DynamicButton : MonoBehaviour{

    
    public Action execute;
    BC_fields fields;
    GameObject descriptionField;
    Text txt;
    SetOnGui setOnGui;
    EventSprite eventSprite;
    void Start()
    {
        eventSprite = GetComponent<EventSprite>(); // prefab needs this component
        eventSprite.SwitchToNormal();
        setOnGui = GetComponent<SetOnGui>();
        fields = GetComponent<ButtonComponent>().fields;
        txt = fields.descFieldPrefab.GetComponentInChildren<Text>();
    }
    
    public  void OnClick(PointerEventData data){
        eventSprite.SwitchToPressed();
        TargetingManager.selectTarget(fields.targets, (GameObject target) => {   //select tile and execute callback
            TileInfo tile = target.GetComponent<TileInfo>();
            fields.ability.execute(tile.gridPosition, tile.grid.isMainGrid);
        }, fields.ability.checkAoe);
        GUIData.canSelectTarget = true;
        GUIData.activeButton = gameObject;
        ContextMenuSpawn.DestroyContextMenu();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        eventSprite.SwitchToHighlighted();
        setOnGui.SetOnGUITrue();
        txt.text = fields.ability.name + "\n\n"+ fields.ability.description;
        descriptionField = Instantiate(fields.descFieldPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
        descriptionField.transform.SetParent(ContextMenuSpawn.contextMenu.transform);
    }

    public void OnPointerExit(PointerEventData data)
    {
        eventSprite.SwitchToNormal();
        setOnGui.SetOnGUIFalse();
        Destroy(descriptionField);
    }
    
    //public void SetFunction(Action v)
    //{
    //   execute = v;
    //}

}
