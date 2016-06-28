using UnityEngine;
using System.Collections;
using System;

public class BuildingFootprintCircle : MonoBehaviour {

    void attach(){
        GameObject[] footprint = gameObject.GetComponent<Unit>().currentTile.GetComponent<TileInfo>().getAdjacent();
        foreach (GameObject go in footprint){
            go.GetComponent<TileInfo>().unit = gameObject;
        }
    } 

    void detach(){
        GameObject[] footprint = gameObject.GetComponent<Unit>().currentTile.GetComponent<TileInfo>().getAdjacent();
        foreach (GameObject go in footprint){
            go.GetComponent<TileInfo>().unit = null;
        }
    }

    void Awake(){
        attach();
    }

    void OnDestroy(){
        detach();
    }

    public static bool checkCollision(GameObject tile, Func<GameObject, bool> check){
        if (!check(tile)) return false;
        TileInfo ti = tile.GetComponent<TileInfo>();
        GameObject[] adj = ti.getAdjacent();
        if (adj.Length < 6) return false;
        foreach(GameObject go in adj){
            if (!check(go)) return false;
        }
        return true;
    }
}
