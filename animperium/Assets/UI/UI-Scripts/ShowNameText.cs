using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowNameText : MonoBehaviour {

    Text txt;
    string prefabID;
    GameObject currentUnit;
	// Use this for initialization
	void Start () {
        txt = GetComponent<Text>();
        txt.text = "";
        currentUnit = null;
	}
	
	// Update is called once per frame
	void Update () {
        if (SelectionManager.selectedUnit != currentUnit) { 
            currentUnit = SelectionManager.selectedUnit;
            txt.text = currentUnit.GetComponent<Unit>().prefabID;
        }
        else if(SelectionManager.selectedUnit == null)
            txt.text = "";
	}
}
