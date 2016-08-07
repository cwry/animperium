using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileInfo : MonoBehaviour {
    //hack for unity prefab serialization
    public int initGridX;
    public int initGridY;

    public Vec2i gridPosition;
    public GridManager grid;
    public bool traversable;
    [HideInInspector]
    public bool isHole = false;
    public Action removeHole = () => { };
    public GameObject unit;
    public GameEvent onUnitDetached = new GameEvent();
    public GameEvent onUnitAttached = new GameEvent();

    void Awake(){
        onUnitDetached.add<GameObject>((GameObject go) => {
            unit = null;
            if (go == null) return;
            Unit u = go.GetComponent<Unit>();
            if(u != null){
                u.currentTile = null;
            }
        });
        onUnitAttached.add<GameObject>((GameObject go) => {
            unit = go;
            Unit u = go.GetComponent<Unit>();
            if (u != null){
                u.currentTile = gameObject;
            }
        });
    }

    public void detachUnit(){
        onUnitDetached.fire(unit);
    }

    public void attachUnit(GameObject go){
        onUnitAttached.fire(go);
    }

    public GameObject continuePathStructure(int dirDelta, GameObject origin) {
        TileInfo originTi = origin.GetComponent<TileInfo>();
        Func<GameObject>[] of = new Func<GameObject>[6];
        of[0] = originTi.getNE;
        of[1] = originTi.getE;
        of[2] = originTi.getSE;
        of[3] = originTi.getSW;
        of[4] = originTi.getW;
        of[5] = originTi.getNW;
        Func<GameObject>[] tf = new Func<GameObject>[6];
        tf[0] = getNE;
        tf[1] = getE;
        tf[2] = getSE;
        tf[3] = getSW;
        tf[4] = getW;
        tf[5] = getNW;
        for(int i = 0; i < 6; i++) {
            if (of[i]() == gameObject) return tf[(i + dirDelta) % 6]();
        }
        return null;
    }

    private bool isInBounds(int x, int y){
        var maxW = grid.gridWidthInHexes;
        var maxH = grid.gridHeightInHexes;
        return x >= 0 && x < maxW && y >= 0 && y < maxH;
    }

    private GameObject getNE(){
        int nx = gridPosition.x + (gridPosition.y % 2 == 1 ? 1 : 0);
        int ny = gridPosition.y + 1;
        return isInBounds(nx, ny) ? grid.gridData[nx, ny] : null;
    }

    private GameObject getE(){
        int nx = gridPosition.x + 1;
        int ny = gridPosition.y;
        return isInBounds(nx, ny) ? grid.gridData[nx, ny] : null;
    }

    private GameObject getSE(){
        int nx = gridPosition.x + (gridPosition.y % 2 == 1 ? 1 : 0);
        int ny = gridPosition.y - 1;
        return isInBounds(nx, ny) ? grid.gridData[nx, ny] : null;
    }

    private GameObject getSW(){
        int nx = gridPosition.x - (gridPosition.y % 2 == 0 ? 1 : 0);
        int ny = gridPosition.y - 1;
        return isInBounds(nx, ny) ? grid.gridData[nx, ny] : null;
    }

    private GameObject getW(){
        int nx = gridPosition.x - 1;
        int ny = gridPosition.y;
        return isInBounds(nx, ny) ? grid.gridData[nx, ny] : null;
    }

    private GameObject getNW(){
        int nx = gridPosition.x - (gridPosition.y % 2 == 0 ? 1 : 0);
        int ny = gridPosition.y + 1;
        return isInBounds(nx, ny) ? grid.gridData[nx, ny] : null;
    }

    public GameObject[] getAdjacent(){
        List<GameObject> adjacent = new List<GameObject>();
        GameObject ne = getNE();
        GameObject e = getE();
        GameObject se = getSE();
        GameObject sw = getSW();
        GameObject w = getW();
        GameObject nw = getNW();
        if (ne != null) adjacent.Add(ne);
        if (e != null) adjacent.Add(e);
        if (se != null) adjacent.Add(se);
        if (sw != null) adjacent.Add(sw);
        if (w != null) adjacent.Add(w);
        if (nw != null) adjacent.Add(nw);
        return adjacent.ToArray();
    }

    public GameObject[] listTree(int minRange, int maxRange, Func <TileInfo, bool> shouldTraverse = null, Func<TileInfo, bool> shouldInclude = null){
        if (shouldTraverse == null) shouldTraverse = (TileInfo ti) => { return true; };
        if (shouldInclude == null) shouldInclude = (TileInfo ti) => { return true; };
        HashSet<GameObject> visited = new HashSet<GameObject>();
        List<GameObject> result = new List<GameObject>();
        List<GameObject> current = new List<GameObject>();
        List<GameObject> next = new List<GameObject>();
        current.Add(gameObject);
        visited.Add(gameObject);
        int currDepth = 1;
        if (minRange == 0 && shouldInclude(gameObject.GetComponent<TileInfo>())) result.Add(gameObject);
        while (currDepth <= maxRange){
            foreach(GameObject go in current){
                GameObject[] adj = go.GetComponent<TileInfo>().getAdjacent();
                foreach(GameObject nxt in adj){
                    TileInfo nxtTi = nxt.GetComponent<TileInfo>();
                    if (shouldTraverse(nxtTi)){
                        if (visited.Add(nxt)){
                            next.Add(nxt);
                            if(currDepth >= minRange && shouldInclude(nxtTi)) result.Add(nxt);
                        }
                    }
                }
            }
            current = next;
            next = new List<GameObject>();
            currDepth++;
        }
        return result.ToArray();
    }
    
}
