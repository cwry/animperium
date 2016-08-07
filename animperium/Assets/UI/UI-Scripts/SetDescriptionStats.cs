using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class SetDescriptionStats : MonoBehaviour {

    // Use this for initialization
    Text text;

    string mP = "Movementpoints/Max:\n";
    string hP = "Hitpoints / Max:\n";
    string aP = "Actionpoints / Max:\n";
    string magicR = "Magic Resist:\n";
    string meleeR = "Melee Resist:\n";
    string rangeR = "Range Resist:\n";

    void Awake () {
        text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void SetValueText(GameObject unit)
    {
        Unit currentUnit = unit.GetComponent<Unit>();
        MovementAbility movementAbility = currentUnit.GetComponent<MovementAbility>();
        text.text = String.Format(
            "{0} / {1}\n {2} / {3}\n {4}\n {5}\n {6}"
            , (movementAbility != null) ? movementAbility.movementPoints : 0
            , (movementAbility != null) ? movementAbility.maxMovementPoints : 0
            , currentUnit.hitPoints, currentUnit.maxHitPoints
            , currentUnit.magicResist
            , currentUnit.meleeResist
            , currentUnit.rangedResist);
    }

    public void SetDescriptionText(GameObject unit)
    {

        Text text = GetComponent<Text>();
        Unit currentUnit = unit.GetComponent<Unit>();
        MovementAbility movementAbility = currentUnit.GetComponent<MovementAbility>();
        text.text = String.Format(
            "{0}{1}{2}{3}{4}"
            , mP 
            , hP
            , magicR
            , meleeR
            , rangeR);
    }
}
