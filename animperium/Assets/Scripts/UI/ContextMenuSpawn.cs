using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ContextMenuSpawn : MonoBehaviour {

    public GameObject contextMenuPrefab;
    private GameObject contextMenu; 
    public GameObject canvas;
    

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if(SelectionManager.selectedTile != null && Input.GetMouseButtonDown(0))
        {
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
            contextMenu = Instantiate(contextMenuPrefab, Camera.main.WorldToScreenPoint(SelectionManager.selectedTile.transform.position), Quaternion.identity) as GameObject;
            contextMenu.transform.parent = canvas.transform;
        }

	}
}
