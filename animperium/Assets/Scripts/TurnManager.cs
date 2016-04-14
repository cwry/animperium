using UnityEngine;
using System.Collections;

public class TurnManager {

    static TurnManager instance;

    public int turnID = 0;
    public GameEvent onTurnBegin = new GameEvent();
    public GameEvent onTurnEnd = new GameEvent();

    private TurnManager(){
        //SINGLETON
    }

    public TurnManager getInstance(){
        return instance == null ? new TurnManager() : instance;
    }

    public void endTurn(){
        onTurnEnd.fire(turnID);
        turnID++;
        onTurnBegin.fire(turnID);
    } 
}
