using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActionQueue{
    static ActionQueue instance;
    Dictionary<int, Action> actions;
    public int actionID = 0;

    private ActionQueue(){
        //SINGLETON
        actions = new Dictionary<int, Action>();
        TurnManager.onTurnBegin.add<int>(reset);
    }

    public void reset(int turnID){
        actionID = 0;
        actions = new Dictionary<int, Action>();
    }

    public static ActionQueue getInstance(){
        if (instance == null){
            instance = new ActionQueue();
        }
        return instance;
    }

    public void push(int i, Action f){
        if (!actions.ContainsKey(i)){
            actions.Add(i, f);
        }
    }

    public Action pop(int i){
        if (actions.ContainsKey(i)){
            return actions[i];
        }
        return null;
    }
}
