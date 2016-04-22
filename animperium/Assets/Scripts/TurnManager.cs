using UnityEngine;
using System.Collections;

public class TurnManager {

    private static TurnManager instance = null;

    public int turnID = 0;
    public GameEvent onTurnBegin = new GameEvent();
    public GameEvent onTurnEnd = new GameEvent();

    private TurnManager(){
        //SINGLETON
    }

    public TurnManager getInstance(){
        if(instance == null){
            instance = new TurnManager();
        }
        return instance;
    }

    public void endTurn(){
        onTurnEnd.fire(turnID);
        turnID++;
        onTurnBegin.fire(turnID);
    } 
}
