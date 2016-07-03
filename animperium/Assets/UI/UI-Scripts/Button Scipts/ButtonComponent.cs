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
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { button.OnClick((PointerEventData)data); });
            trigger.triggers.Add(entry);
        }
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerEnter;
        entry2.callback.AddListener((data) => { button.OnPointerEnter((PointerEventData)data); });
        trigger.triggers.Add(entry2);
        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.PointerExit;
        entry3.callback.AddListener((data) => { button.OnPointerExit((PointerEventData)data); });
        trigger.triggers.Add(entry3);
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
    void Start()
    {
        fields = GetComponent<ButtonComponent>().fields;
        txt = fields.descFieldPrefab.GetComponentInChildren<Text>();
    }
    
    public  void OnClick(PointerEventData data){
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
        txt.text = fields.ability.name + "\n\n"+ fields.ability.description;
        descriptionField = Instantiate(fields.descFieldPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
        descriptionField.transform.SetParent(ContextMenuSpawn.contextMenu.transform);
    }

    public void OnPointerExit(PointerEventData data)
    {
        Destroy(descriptionField);
    }
    
    //public void SetFunction(Action v)
    //{
    //   execute = v;
    //}

}
