using UnityEngine;
using System.Collections;

public class UnitInfoGui : MonoBehaviour {

    GameObject currentUnit;
    public GameObject prefab;
    GameObject unitInfo;
	// Use this for initialization
	void Start () {
        currentUnit = null;
	}
	
	// Update is called once per frame
	void Update () {
        /*
	    if(SelectionManager.selectedUnit == null)
        {
            Destroy(unitInfo);
        }
        else
        {
            unitInfo = Instantiate(prefab);
        }
        */
	}
}
