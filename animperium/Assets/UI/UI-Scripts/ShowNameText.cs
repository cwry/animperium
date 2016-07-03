using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowNameText : MonoBehaviour {

    Text txt;
    GameObject currentUnit;
	// Use this for initialization
	void Start () {
        txt = GetComponent<Text>();
        txt.text = "";
        currentUnit = null;
	}
	
	// Update is called once per frame
	void Update () {
        if (SelectionManager.selectedUnit == null && SelectionManager.selectedUnit != currentUnit)
            txt.text = "";
        if (SelectionManager.selectedUnit != currentUnit) { 
            currentUnit = SelectionManager.selectedUnit;
            if(currentUnit != null)
            txt.text = currentUnit.GetComponent<Unit>().prefabID;
        }
	}
}
