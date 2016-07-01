using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum UnitFootprintType {
    DOT, CIRCLE
}

public class UnitFootprint : MonoBehaviour {

    public UnitFootprintType type;

    void attach(TileInfo ti){
        GameObject[] footprint = getFootprint(ti);
        foreach (GameObject go in footprint){
            go.GetComponent<TileInfo>().unit = gameObject;
        }
    } 

    void detach(){
        GameObject[] footprint = getFootprint(gameObject.GetComponent<Unit>().currentTile.GetComponent<TileInfo>());
        foreach (GameObject go in footprint){
            go.GetComponent<TileInfo>().unit = null;
        }
    }

    void OnDestroy(){
        detach();
    }

    public GameObject[] getFootprint(TileInfo ti){
        switch (type){
            case UnitFootprintType.CIRCLE:
                return getFootPrintCircle(ti);
            case UnitFootprintType.DOT:
                return getFootPrintDot(ti);
            default:
                return null;
        }
    }

    GameObject[] getFootPrintCircle(TileInfo ti){
        List<GameObject> res = new List<GameObject>();
        res.Add(ti.gameObject);
        foreach(GameObject go in ti.getAdjacent()){
            res.Add(go);
        }
        return res.ToArray();
    }

    GameObject[] getFootPrintDot(TileInfo ti){
        GameObject[] res = new GameObject[1];
        res[0] = ti.gameObject;
        return res;
    }
}
