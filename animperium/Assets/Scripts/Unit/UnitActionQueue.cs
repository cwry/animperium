using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UnitActionQueue{
    static UnitActionQueue instance;
    Dictionary<string, Queue<Action<Action>>> actions;

    private UnitActionQueue(){
        //SINGLETON
        actions = new Dictionary<string, Queue<Action<Action>>>();
    }

    public static UnitActionQueue getInstance(){
        if (instance == null){
            instance = new UnitActionQueue();
        }
        return instance;
    }

    public void push(string k, Action<Action> f){
        if (!actions.ContainsKey(k)){
            actions.Add(k, new Queue<Action<Action>>());
        }
        Queue<Action<Action>> q = actions[k];
        q.Enqueue(f);
    }

    public Action<Action> pop(string k){
        if (actions.ContainsKey(k)){
            Queue<Action<Action>> q = actions[k];
            if(q.Count > 0){
                return q.Dequeue();
            }
        }
        return null;
    }
}
