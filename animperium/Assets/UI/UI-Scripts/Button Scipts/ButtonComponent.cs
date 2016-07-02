using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public struct BC_fields
{
    public string ability;
    public GameObject descFieldPrefab;
    public string abilityDescription;
}
public class ButtonComponent : MonoBehaviour
{
    EventTrigger trigger;
    public DynamicButton button;
    public BC_fields fields;
    //EventSprite spriteEvent;

    void Start()
    {
        
    }

    public void Init(string abili, string abilityDesc, GameObject descrField)
    {
        ;
        fields.ability = abili;
        fields.abilityDescription = abilityDesc;
        fields.descFieldPrefab = descrField; 
        AddListener();
    }

    void AddListener()
    {
        button = gameObject.AddComponent<DynamicButton>();
        trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { button.OnClick((PointerEventData)data); });
        trigger.triggers.Add(entry);
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerEnter;
        entry2.callback.AddListener((data) => { button.OnPointerEnter((PointerEventData)data); });
        trigger.triggers.Add(entry2);
        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.PointerExit;
        entry3.callback.AddListener((data) => { button.OnPointerExit((PointerEventData)data); });
        trigger.triggers.Add(entry3);
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
    
    public  void OnClick(PointerEventData data)
    {
        TargetingManager.selectTarget(AbilityManager.checkRange(GUIData.ContextUnit, fields.ability), (GameObject target) => {   //select tile and execute callback
            TileInfo tile = target.GetComponent<TileInfo>();
            AbilityManager.useAbility(GUIData.ContextUnit, fields.ability, tile.gridPosition, tile.grid.isMainGrid);});
        GUIData.canSelectTarget = true;
        GUIData.activeButton = gameObject;
        ContextMenuSpawn.DestroyContextMenu();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        Debug.Log(txt.gameObject.name);
        txt.text = fields.ability + "\n"+ fields.abilityDescription;
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
