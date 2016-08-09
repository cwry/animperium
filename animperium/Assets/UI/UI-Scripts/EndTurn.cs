using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class EndTurn : MonoBehaviour {

    
    EventTrigger eventTrigger;
    EventSprite eventSprite;
    Image image;

	// Use this for initialization
	void Awake () {
        eventSprite = GetComponent<EventSprite>();
        eventTrigger = GetComponent<EventTrigger>();
        TurnManager.onTurnBegin.add<int>(OnTurnBegin);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void EndTurnExecute()
    {
        if (Data.isActivePlayer())
        {
            SoundManager.instance.PlaySound("confirm", SoundManager.effectVolume);
            ContextMenuSpawn.DestroyContextMenu();
            TurnManager.endTurn();
        }

        
    }

    void OnTurnBegin(int turnID)
    {
        eventTrigger.enabled = Data.isActivePlayer();
        if(!Data.isActivePlayer())
        {
            eventSprite.SwitchToDeactivated();
        }
        else
        {
            eventSprite.SwitchToNormal();
        }
    }
    
}
