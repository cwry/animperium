using UnityEngine;
using System;
using UnityEngine.EventSystems;

//AbilityManager.listAbilities(Selected unit);// returnt string array with ability ids
//AbilityManager.checkRange(unit, "Melee"); //gameobject array with all possible targets
//AbilityManager.useAbility(unit, "Melee", Vec2i target, ismaingrid) // löst ability melee aus
class ContextMenuControl : MonoBehaviour
{
    int slot = 0;
    public GameObject[] slots;
    public GameObject attackButton;
    public GameObject moveButton;
    public ContextMenuSpawn spawn;
    public GameObject descField;
    public int slotNumber = 10;
    float angle;
    float distance = 55;
    public Transform middle;
    public Transform costumPivot;
    Vector3[] buttonSlotPositions;
    
    // public System.Collections.Generic.Dictionary<GameObject, GameObject> buttonDic;

    void Awake()
    {
        spawn = GameObject.FindObjectOfType<ContextMenuSpawn>();
    }
    public void AddButton(AbilityInfo ability) {                                 // add button to existing points
        if (slot < buttonSlotPositions.Length){
            Vector3 position = middle.position + buttonSlotPositions[slot] * distance;
            Debug.Log("Middle: " + middle.position + "buttonposition+ distance: " + buttonSlotPositions[slot] * distance);
            //GameObject g = Instantiate(button,slots[slot].GetComponent<Transform>().position, button.transform.localRotation) as GameObject;
            GameObject g = Instantiate(ability.button, position, costumPivot.transform.localRotation) as GameObject;
            g.transform.SetParent(middle.transform.parent);
            slots[slot] = g;
            slots[slot].AddComponent<EventTrigger>();
            slots[slot].AddComponent<SetOnGui>();
            slots[slot].AddComponent<ButtonComponent>();
            ButtonComponent bc = slots[slot].GetComponent<ButtonComponent>();
            bc.Init(ability, descField);
        }
        slot++;
    }
    
    void GenerateButtonPositions()
    {
        Debug.Log(buttonSlotPositions.Length);
        Debug.Log(angle);
        if (slotNumber == 2 || slotNumber == 3)
        {
            angle = 180f / slotNumber;
            float offset = angle / 2;
            for (int i = 0; i < buttonSlotPositions.Length; i++)
            {
                float currentAngle = Mathf.Deg2Rad * i * angle + Mathf.Deg2Rad * offset;
                buttonSlotPositions[i] = new Vector3(Mathf.Cos(currentAngle), -Mathf.Sin(currentAngle), 0);
            }
        }
        else
        {
            for (int i = 0; i < buttonSlotPositions.Length; i++)
            {
                float currentAngle = Mathf.Deg2Rad * i * angle;
                buttonSlotPositions[i] = new Vector3(Mathf.Cos(currentAngle), -Mathf.Sin(currentAngle), 0);
            }
        }
    }

    public void SetSlotNumber(int slotN)
    {
        slotNumber = slotN;
        slots = new GameObject[slotNumber];
        angle = 180f / (slotNumber-1);
        buttonSlotPositions = new Vector3[slotNumber];
        GenerateButtonPositions();
    }
}