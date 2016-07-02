using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ContextMenuSpawn : MonoBehaviour {

    public GameObject contextMenuPrefab;
    public static  GameObject contextMenu = null; 
    public GameObject canvas;
    public static GameObject currentUnit = null;
    public AbilityInfo[] abilities;

    // Use this for initialization
    void Start () {
        GUIData.pointerOnGUI = false;
        GUIData.canSelectTarget = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if( Input.GetMouseButtonDown(0) 
            && SelectionManager.selectedUnit != currentUnit 
            && !GUIData.pointerOnGUI && Data.isActivePlayer() 
            && !GUIData.hasContextMenu
            && SelectionManager.selectedUnit.GetComponent<Unit>().playerID == Data.playerID
            && !TargetingManager.getActive())
        {
            DestroyContextMenu();
            Debug.Log("Should Spawn");
            GUIData.targetTile = SelectionManager.selectedTile;
            currentUnit = SelectionManager.selectedUnit;
            SpawnContextMenu();
            GUIData.hasContextMenu = true;
            GUIData.ContextUnit = currentUnit;
        }
        else if(Input.GetMouseButtonDown(0) 
            && SelectionManager.selectedUnit == null 
            && !GUIData.pointerOnGUI 
            && Data.isActivePlayer() 
            && GUIData.hasContextMenu)
        {
            DestroyContextMenu();
        }
        //if(Input.GetMouseButtonDown(1) && !GUIData.pointerOnGUI && GUIData.hasContextMenu && GUIData.canSelectTarget && SelectionManager.hoverTile != null && Data.isEndTurnPossible())
        //{
        //    SelectionManager.selectedTarget = SelectionManager.hoverTile;
        //    contextMenu.GetComponent<ContextMenuControl>().attackButton.GetComponent<ButtonComponent>().button.execute();
        //}
    }

    private void SpawnContextMenu()
    {
        
        Unit unit = SelectionManager.selectedUnit.GetComponent<Unit>();
        contextMenu = Instantiate(contextMenuPrefab, Camera.main.WorldToScreenPoint(GUIData.targetTile.transform.position), Quaternion.identity) as GameObject;
        contextMenu.transform.SetParent(canvas.transform, false);
        abilities = AbilityManager.listAbilities(SelectionManager.selectedUnit);// returnt string array with ability ids
        foreach (AbilityInfo ability in abilities){
            contextMenu.GetComponent<ContextMenuControl>().AddButton(ability);
        }
        
    }

    public static void DestroyContextMenu()
    {
        currentUnit = null;
        GUIData.pointerOnGUI = false;
        GUIData.hasContextMenu = false;
        Destroy(contextMenu);
    }
}
