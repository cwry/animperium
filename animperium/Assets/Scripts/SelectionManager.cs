using UnityEngine;
using System.Collections;

public class SelectionManager  {

    public static GameObject selectedItem;

	// Use this for initialization
	public SelectionManager ()
    {
	
	}
	
	public static void SelectObject(GameObject g)
    {
        selectedItem = g;
        selectedItem.transform.position = Data.mainGrid.gridData[12, 10].gameObject.transform.position;
    }

    public static void SelectAction(GameObject g)
    {
        if(selectedItem.tag == "Unit")
        {
                selectedItem.GetComponent<Unit>().functions.move(g);
        }
    }
    
    public static void Deselect()
    {
        selectedItem = null;
    }
    
}
