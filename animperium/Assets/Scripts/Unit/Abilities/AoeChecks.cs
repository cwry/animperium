using UnityEngine;
using System;
using System.Collections.Generic;

public enum AoeType{
    DOT, CIRCLE, BIG_CIRCLE, MELEE_CONE, FLAME_BREATH, SHORT_LINE, SLASH 
}

public class AoeChecks{

    public static Func<TileInfo, GameObject[]> getAoeByType(AoeType t, Unit origin) {
        switch (t) {
            case AoeType.DOT:
                return dot;
            case AoeType.CIRCLE:
                return circle;
            case AoeType.BIG_CIRCLE:
                return getCircle(2, 0);
            case AoeType.MELEE_CONE:
                return getMeleeCone(origin);
            case AoeType.FLAME_BREATH:
                return getFlameBreath(origin);
            case AoeType.SHORT_LINE:
                return getShortPathStructure(0, origin);
            case AoeType.SLASH:
                return getShortPathStructure(2, origin);
        }
        return dot;
    }

    public static Func<TileInfo, GameObject[]> getShortPathStructure(int deltaDir, Unit origin) {
        return (TileInfo ti) => {
            if (!isMelee(ti, origin)) return new GameObject[0];
            List<GameObject> result = new List<GameObject>();
            result.Add(ti.gameObject);
            GameObject segment = ti.continuePathStructure(deltaDir, origin.currentTile);
            if(segment != null) result.Add(segment);
            return result.ToArray();
        };
    }

    public static Func<TileInfo, GameObject[]> getFlameBreath(Unit origin) {
        return (TileInfo ti) => {
            if(!isMelee(ti, origin)) return new GameObject[0];
            List<GameObject> result = new List<GameObject>();
            GameObject[] originCircle = origin.currentTile.GetComponent<TileInfo>().getAdjacent();
            GameObject[] targetCircle = ti.getAdjacent();
            foreach (GameObject t in targetCircle) {
                bool isInOriginCircle = false;
                foreach (GameObject o in originCircle) {
                    if (t == o || t == origin.currentTile) {
                        isInOriginCircle = true;
                        break;
                    }
                }
                if (!isInOriginCircle) result.Add(t);
            }
            result.Add(ti.gameObject);
            return result.ToArray();
        };
    }

    private static bool isMelee(TileInfo ti, Unit origin) {
        GameObject[] targetCircle = ti.getAdjacent();
        foreach (GameObject t in targetCircle) {
            if (t == origin.currentTile) {
                return true;
            }
        }
        return false;
    }

    public static Func<TileInfo, GameObject[]> getMeleeCone(Unit origin){
        return (TileInfo ti) => {
            if(!isMelee(ti, origin)) return new GameObject[0];
            List<GameObject> result = new List<GameObject>();
            GameObject[] originCircle = origin.currentTile.GetComponent<TileInfo>().getAdjacent();
            GameObject[] targetCircle = ti.getAdjacent();
            foreach (GameObject t in targetCircle) {
                foreach(GameObject o in originCircle) {
                    if (t == o) result.Add(o);
                }
            }
            result.Add(ti.gameObject);
            return result.ToArray();
        };
    }
    
    public static GameObject[] dot(TileInfo ti){
        GameObject[] res = new GameObject[1];
        res[0] = ti.gameObject;
        return res;
    }

    public static GameObject[] circle(TileInfo ti){
        return ti.listTree(0, 1);
    }

    public static Func<TileInfo, GameObject[]> getCircle(int size, int cutout){
        return (TileInfo ti) => {
            return ti.listTree(cutout, size);
        };
    }
}
