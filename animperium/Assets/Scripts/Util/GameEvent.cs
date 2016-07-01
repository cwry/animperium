using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameEvent {
    List<Action<object>> callbacks = new List<Action<object>>();

    public Action add<T1>(Action<T1> callback){
        Action<object> boundCallBack = (object data) => { callback((T1)data); };
        callbacks.Add(boundCallBack);
        return () => { callbacks.Remove(boundCallBack); };
    }

    public void fire(object data = null) {
        foreach(Action<object> a in callbacks){
            a(data);
        }
    }
}
