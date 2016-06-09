using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class EndTurn : MonoBehaviour {

    
    EventTrigger eventTrigger;
    public Sprite disabledButton;
    public Sprite normalButton;
    Image image;

	// Use this for initialization
	void Awake () {
        image = GetComponent<Image>();
        eventTrigger = GetComponent<EventTrigger>();
        TurnManager.onTurnBegin.add<int>(OnTurnBegin);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void EndTurnExecute()
    {
        if (IsEndTurnPossible())
        {
            TurnManager.endTurn();
        }

        
    }

    bool IsEndTurnPossible()
    {
        return Data.playerID % 2 != TurnManager.turnID % 2;
    }

    void OnTurnBegin(int turnID)
    {
        eventTrigger.enabled = IsEndTurnPossible();
        if(!IsEndTurnPossible())
        {
            image.sprite = disabledButton;
        }
        else
        {
            image.sprite = normalButton;
        }
    }
    
}
