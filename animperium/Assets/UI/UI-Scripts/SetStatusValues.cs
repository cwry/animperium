using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class SetStatusValues : MonoBehaviour {

    
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetText(GameObject unit)
    {
       
        Text text = GetComponent<Text>();
        Unit currentUnit = unit.GetComponent<Unit>();
        MovementAbility movementAbility = currentUnit.GetComponent<MovementAbility>();
        text.text = String.Format(
            "{0}/{1}\n {2}/{3}\n {4}\n {5}\n {6}"
            , (movementAbility != null) ? movementAbility.movementPoints : 0 
            , (movementAbility != null) ? movementAbility.maxMovementPoints : 0
            , currentUnit.hitPoints, currentUnit.maxHitPoints
            , currentUnit.magicResist
            , currentUnit.meleeResist
            , currentUnit.rangedResist);
    }
}
