using UnityEngine;
using System.Collections;
using System;

public class Unit : MonoBehaviour {
    public GameObject currentTile;
    public int playerID;
    public string unitID;

    /*bool isExecutingAction = false;

	void Update () {
        if (isExecutingAction) return;
        Action<Action> f = UnitActionQueue.getInstance().pop(unitID);
        if(f != null){
            isExecutingAction = true;
            f(() =>{
                isExecutingAction = false;
            });
        }
    }*/
    
}
