using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ContextMenuSpawn : MonoBehaviour {

    public GameObject contextMenuPrefab;
    private GameObject contextMenu = null; 
    public GameObject canvas;
    public GameObject currentUnit = null;
    public string[] abilities;

    // Use this for initialization
    void Start () {
        GUIData.pointerOnGUI = false;
        GUIData.canSelectTarget = false;
	}
	
	// Update is called once per frame
	void Update () {
       
	    if( Input.GetMouseButtonDown(0) && SelectionManager.selectedUnit != currentUnit && !GUIData.pointerOnGUI && Data.isEndTurnPossible() && !GUIData.hasContextMenu && !GUIData.canSelectTarget)
        {
            GUIData.targetTile = SelectionManager.selectedTile;
            currentUnit = SelectionManager.selectedUnit;
            SpawnContextMenu();
            GUIData.hasContextMenu = true;
            GUIData.ContextUnit = currentUnit;
        }
        else if(Input.GetMouseButtonDown(0) && SelectionManager.selectedUnit == null && !GUIData.pointerOnGUI && Data.isEndTurnPossible() && GUIData.hasContextMenu && !GUIData.canSelectTarget)
        {
            currentUnit = null;
            Destroy(contextMenu);
            GUIData.hasContextMenu = false;
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
        if (unit.playerID != Data.playerID) return;
        contextMenu = Instantiate(contextMenuPrefab, Camera.main.WorldToScreenPoint(GUIData.targetTile.transform.position), Quaternion.identity) as GameObject;
        contextMenu.transform.SetParent(canvas.transform, false);
        abilities = AbilityManager.listAbilities(SelectionManager.selectedUnit);// returnt string array with ability ids
        foreach (string ability in abilities){
            Debug.Log(ability);
            contextMenu.GetComponent<ContextMenuControl>().AddButton(ability);
        }
        
    }

    public static void DestroyContextMenu()
    {
        GameObject[] arr = GameObject.FindGameObjectsWithTag("Ui");
        foreach (GameObject g in arr)
        {
            if (g.name.Contains("ContextMenu"))
            {
                Destroy(g);
            }
        }
        GUIData.hasContextMenu = false;
    }
}
