﻿using UnityEngine;
using System;

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
    public void AddButton(string ability) {                                 // add button to existing points
            if (slot < slots.Length){
                GameObject g = Instantiate(attackButton,slots[slot].GetComponent<Transform>().position, Quaternion.identity) as GameObject;
                g.transform.SetParent(slots[slot].transform.parent);
                Destroy(slots[slot]);
                slots[slot] = g;
                slots[slot].AddComponent<ButtonComponent>();
                slots[slot].GetComponent<ButtonComponent>().Init(ability, "", descField);
        }
        slot++;
    }

    
}