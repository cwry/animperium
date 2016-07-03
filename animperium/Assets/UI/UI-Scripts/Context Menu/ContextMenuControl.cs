using UnityEngine;
using System;
using UnityEngine.EventSystems;

//AbilityManager.listAbilities(Selected unit);// returnt string array with ability ids
//AbilityManager.checkRange(unit, "Melee"); //gameobject array with all possible targets
//AbilityManager.useAbility(unit, "Melee", Vec2i target, ismaingrid) // löst ability melee aus
class ContextMenuControl : MonoBehaviour
{
    int slot = 0;
    public GameObject[] slots = new  GameObject[5];
    public GameObject attackButton;
    public GameObject moveButton;
    public GameObject digButton;
    public ContextMenuSpawn spawn;
    public GameObject descField;
    
    // public System.Collections.Generic.Dictionary<GameObject, GameObject> buttonDic;

    void Start()
    {
        
        spawn = GameObject.FindObjectOfType<ContextMenuSpawn>();
    }
    public void AddButton(AbilityInfo ability) {                                 // add button to existing points
        if (slot < slots.Length){
            GameObject button = GetButtonPrefab(ability.abilityID);
            GameObject g = Instantiate(button,slots[slot].GetComponent<Transform>().position, button.transform.localRotation) as GameObject;
            g.transform.SetParent(slots[slot].transform.parent);
            Destroy(slots[slot]);
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
    
}