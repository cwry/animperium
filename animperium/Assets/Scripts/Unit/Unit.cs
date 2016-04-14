using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

    public UnitData data;
    public UnitFunctions functions;
    public GameObject currentTile;


	// Use this for initialization
	void Start () {
        data = new UnitData(gameObject);
        functions = new UnitFunctions(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	    
    }
    
}
