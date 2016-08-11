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
        if(currentUnit.GetComponent<Minable>() != null) {
            text.text = currentUnit.GetComponent<Minable>().amt.ToString();
        }
    }

    public void SetDescriptionText(GameObject unit)
    {

        Text text = GetComponent<Text>();
        Unit currentUnit = unit.GetComponent<Unit>();
        string resource = "";
        MovementAbility movementAbility = currentUnit.GetComponent<MovementAbility>();
        text.text = String.Format(
            "{0}{1}{2}{3}{4}"
            , mP 
            , hP
            , magicR
            , meleeR
            , rangeR);
        if (currentUnit.GetComponent<Minable>() != null) {
            switch (currentUnit.GetComponent<Minable>().type) {
                case Resource.GOLD:
                    resource = "gold";
                    break;
                case Resource.IRON:
                    resource = "iron";
                    break;
                case Resource.STONE:
                    resource = "stone";
                    break;
                case Resource.WOOD:
                    resource = "wood";
                    break;
            }
            text.text = "Amount of " + resource;
        }
    }
}
