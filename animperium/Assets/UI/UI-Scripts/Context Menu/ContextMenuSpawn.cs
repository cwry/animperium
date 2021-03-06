﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ContextMenuSpawn : MonoBehaviour {

    public  GameObject contextMenuPrefab;
    public static  GameObject contextMenu = null; 
    public  GameObject canvas;
    public static GameObject currentUnit = null;
    public static GameObject m_prefab;
    public static GameObject m_canvas;
    public static bool shouldDestroy = false;
    //public static CircleFadeIn fadeComponent;
    // Use this for initialization
    void Awake () {
        SelectionManager.onSelectedUnitChanged.add<GameObject>(SpawnContextMenu);
        GUIData.pointerOnGUI = false;
        GUIData.canSelectTarget = false;
        m_prefab = contextMenuPrefab;
        m_canvas = canvas; 
	}
	
	// Update is called once per frame
	/*void Update () {
        if (shouldDestroy) {
            Destroy(contextMenu);
        }
    }*/

    //target new unit => spawn menu
    //button click => despawn => targeting => respawn on old unit
    //


    public static void SpawnContextMenu(GameObject unit)
    {
        if (unit != null && !GUIData.blockAction && Data.isActivePlayer())
        {
            DestroyContextMenu();
            if (Data.isActivePlayer() && !GUIData.shouldEndTurn && unit.GetComponent<Unit>() != null)
            {
                if (unit.GetComponent<Unit>().playerID == Data.playerID)
                {
                    currentUnit = unit;
                    List<AbilityInfo> abilities;
                    Unit m_unit = unit.GetComponent<Unit>();
                    Vector3 initPosition = unit.transform.position;
                    contextMenu = Instantiate(m_prefab, Camera.main.WorldToScreenPoint(initPosition), Quaternion.identity) as GameObject;
                    contextMenu.transform.SetParent(m_canvas.transform, false);
                    //fadeComponent = contextMenu.GetComponentInChildren<CircleFadeIn>();
                    abilities = m_unit.abilities;
                    contextMenu.GetComponent<ContextMenuControl>().SetSlotNumber(abilities.Count);
                    foreach (AbilityInfo ability in abilities)
                    {
                        contextMenu.GetComponent<ContextMenuControl>().AddButton(ability);
                    }
                    GUIData.hasContextMenu = true;
                }
            }
        }
    }
    

    public static void DestroyContextMenu(){
        Debug.Log("Destroy Menu");
        GUIData.pointerOnGUI = false;
        GUIData.hasContextMenu = false;
        shouldDestroy = true;
        //if(fadeComponent!=null) fadeComponent.shouldClose = true;
        Destroy(contextMenu);
    }

    public static void ClearTarget() {
        currentUnit = null;
    }

    
}
