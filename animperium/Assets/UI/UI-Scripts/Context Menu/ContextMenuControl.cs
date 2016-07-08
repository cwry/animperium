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
    public GameObject radius;
    public int slotNumber = 10;
    float angle;
    float distance;
   
    public GameObject middle;
    public GameObject costumPivot;
    Vector3[] buttonSlotPositions;
    
    // public System.Collections.Generic.Dictionary<GameObject, GameObject> buttonDic;

    void Awake()
    {
        GUIData.screenHeightRatio = (float) Screen.height / 1080f;
        distance = Vector3.Distance(middle.transform.position, radius.transform.position) * GUIData.screenHeightRatio;
        spawn = GameObject.FindObjectOfType<ContextMenuSpawn>();
    }
    public void AddButton(AbilityInfo ability) {                                 // add button to existing points
        if (slot < buttonSlotPositions.Length){
            Vector3 position = middle.transform.position + (buttonSlotPositions[slot] * distance);
            GameObject g = Instantiate(ability.button, position, costumPivot.transform.localRotation) as GameObject;
            g.transform.SetParent(middle.transform.parent);
            g.transform.localScale *= GUIData.screenHeightRatio;
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
        if(slotNumber == 1)
        {
            float currentAngle = Mathf.Deg2Rad * 90f;
            buttonSlotPositions[0] = new Vector3(Mathf.Cos(currentAngle), -Mathf.Sin(currentAngle), 0);

        }
        else if (slotNumber == 2 || slotNumber == 3)
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