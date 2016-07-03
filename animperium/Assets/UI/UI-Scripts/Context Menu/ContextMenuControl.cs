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
    public GameObject digButton;
    public ContextMenuSpawn spawn;
    public GameObject descField;
    public int slotNumber = 10;
    public float angle;
    float distance = 55;
    public Transform middle;
    Vector3[] buttonSlotPositions;
    
    // public System.Collections.Generic.Dictionary<GameObject, GameObject> buttonDic;

    void Awake()
    {
        spawn = GameObject.FindObjectOfType<ContextMenuSpawn>();
    }
    public void AddButton(AbilityInfo ability) {                                 // add button to existing points
        if (slot < buttonSlotPositions.Length){
            GameObject button = GetButtonPrefab(ability.abilityID);
            Vector3 position = middle.position + buttonSlotPositions[slot] * distance;
            //GameObject g = Instantiate(button,slots[slot].GetComponent<Transform>().position, button.transform.localRotation) as GameObject;
            GameObject g = Instantiate(button, position, button.transform.localRotation) as GameObject;
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

    public GameObject GetButtonPrefab(string abilityID)
    {
        if(abilityID == "move")
        {
            return moveButton;
        }
        if (abilityID == "melee")
        {
            return attackButton;
        }
        return attackButton;
    }
    
    void GenerateButtonPositions()
    {
        for (int i = 0; i < buttonSlotPositions.Length; i++)
        { 
            buttonSlotPositions[i] = new Vector3(Mathf.Cos(i * angle), -Mathf.Sin(i * angle),0);
        }
    }

    public void SetSlotNumber(int slotN)
    {
        slotNumber = slotN;
        slots = new GameObject[slotNumber];
        angle = 180 / slotNumber;
        buttonSlotPositions = new Vector3[slotNumber];
        GenerateButtonPositions();
    }
}