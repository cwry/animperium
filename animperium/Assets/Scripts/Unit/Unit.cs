using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

    public UnitData data;
    public UnitFunctions functions;
    public GameObject currentTile;
    public int playerID;
    public string unitID;
    public bool blockCommands;


	// Use this for initialization
	void Start () {
        data = new UnitData(gameObject);
        functions = new UnitFunctions(gameObject);
	}
	
	// Update is called once per frame
	void Update () {

        if (data.health <= 0)
        {
            Destroy(gameObject);
            
        }
    }
    
}
