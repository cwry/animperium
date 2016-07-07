using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ContextMenuSpawn : MonoBehaviour {

    public GameObject contextMenuPrefab;
    public static  GameObject contextMenu = null; 
    public GameObject canvas;
    public static GameObject currentUnit = null;
    public List<AbilityInfo> abilities;
    bool isButtonSpawning = false;

    // Use this for initialization
    void Awake () {
        SelectionManager.onSelectedUnitChanged.add<GameObject>(onSelectionChanged);
        GUIData.pointerOnGUI = false;
        GUIData.canSelectTarget = false;
	}
	
	// Update is called once per frame
	void Update () {

    }

    void onSelectionChanged(GameObject g)
    {

        if (Input.GetMouseButtonDown(0)
            && SelectionManager.selectedUnit != currentUnit
            && !GUIData.pointerOnGUI
            && Data.isActivePlayer()
            && GUIData.hasContextMenu)
        {
            DestroyContextMenu();
        }
        if (Input.GetMouseButtonDown(0)
            && SelectionManager.selectedUnit != currentUnit
            && !GUIData.pointerOnGUI
            && Data.isActivePlayer()
            && !GUIData.hasContextMenu
            && !TargetingManager.getActive())
        {

            DestroyContextMenu();
            GUIData.targetTile = SelectionManager.selectedTile;
            currentUnit = SelectionManager.selectedUnit;
            if (currentUnit != null)
            {
                if (currentUnit.GetComponent<Unit>().playerID == Data.playerID)
                {
                    SpawnContextMenu();
                    GUIData.hasContextMenu = true;
                    GUIData.ContextUnit = currentUnit;
                }
            }
        }
    }
    private void SpawnContextMenu()
    {
        
        Unit unit = SelectionManager.selectedUnit.GetComponent<Unit>();
        Vector3 initPosition = GUIData.targetTile.transform.position;
        contextMenu = Instantiate(contextMenuPrefab, Camera.main.WorldToScreenPoint(initPosition), Quaternion.identity) as GameObject;
        contextMenu.transform.SetParent(canvas.transform, false);
        abilities = unit.abilities;
        contextMenu.GetComponent<ContextMenuControl>().SetSlotNumber(abilities.Count);
        foreach (AbilityInfo ability in abilities){
            contextMenu.GetComponent<ContextMenuControl>().AddButton(ability);
        }
        
    }

    public static void DestroyContextMenu(){
        currentUnit = null;
        GUIData.pointerOnGUI = false;
        GUIData.hasContextMenu = false;
        Destroy(contextMenu);
    }
}
