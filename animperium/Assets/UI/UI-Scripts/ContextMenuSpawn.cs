using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ContextMenuSpawn : MonoBehaviour {

    public GameObject contextMenuPrefab;
    private GameObject contextMenu; 
    public GameObject canvas;

	// Use this for initialization
	void Start () {
        GUIData.pointerOnGUI = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if(SelectionManager.hoverTile != null && Input.GetMouseButtonDown(1) && !GUIData.pointerOnGUI && SelectionManager.selectedUnit != null)
        {
            Unit unit = SelectionManager.selectedUnit.GetComponent<Unit>();
            if (unit.playerID != Data.playerID) return;

            GUIData.targetTile = SelectionManager.hoverTile;
            if (contextMenu != null)
            {
                GameObject [] arr =  GameObject.FindGameObjectsWithTag("Ui");
                foreach (GameObject g in arr)
                {
                    if(g.name.Contains(contextMenuPrefab.name))
                    {
                        Destroy(g);
                    }
                }
            }
            
            contextMenu = Instantiate(contextMenuPrefab, Camera.main.WorldToScreenPoint(GUIData.targetTile.transform.position), Quaternion.identity) as GameObject;
            contextMenu.transform.SetParent(canvas.transform, false);
        }

        if(Input.GetMouseButtonDown(0) && !GUIData.pointerOnGUI)
        {
            GameObject[] arr = GameObject.FindGameObjectsWithTag("Ui");
            foreach (GameObject g in arr)
            {
                if (g.name.Contains(contextMenuPrefab.name))
                {
                    Destroy(g);
                }
            }
        }

	}
}
