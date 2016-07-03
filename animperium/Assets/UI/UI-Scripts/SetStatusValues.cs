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

    public void SetText()
    {
        Text text = GetComponent<Text>();
        Unit currentUnit = SelectionManager.selectedUnit.GetComponent<Unit>();
        text.text = String.Format(
            "{0} / {1}\n {2} / {3}\n {4}\n {5}\n {6}"
            , currentUnit.movementPoints, currentUnit.maxMovementPoints
            , currentUnit.hitPoints, currentUnit.maxHitPoints
            , currentUnit.magicResist
            , currentUnit.meleeResist
            , currentUnit.rangedResist);
    }
}
