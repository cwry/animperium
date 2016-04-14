using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameEvent {
    private List<Action<object>> callbacks = new List<Action<object>>();

    public void add<T1>(Action<T1> callback){
        callbacks.Add((data) => { callback((T1)data); });
    }

    public void fire(object data) {
        foreach(Action<object> a in callbacks){
            a(data);
        }
    }
}
