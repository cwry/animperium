using UnityEngine;
using System.Collections;

public class ContextMenuInteraction : MonoBehaviour {

    public GameObject contextMenuPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnBreakButtonClick()
    {
        GameObject[] arr = GameObject.FindGameObjectsWithTag("Ui");
        foreach (GameObject g in arr)
        {
            if (g.name.Contains(contextMenuPrefab.name))
            {
                Destroy(g);
                GUIData.pointerOnGUI = false;
            }
        }
    }
}
