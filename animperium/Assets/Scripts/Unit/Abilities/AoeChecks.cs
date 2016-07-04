using UnityEngine;
using System;

public enum AoeType{
    DOT, CIRCLE, BIG_CIRCLE 
}

public class AoeChecks{

    public static Func<TileInfo, GameObject[]> getAoeByType(AoeType t) {
        switch (t) {
            case AoeType.DOT:
                return dot;
            case AoeType.CIRCLE:
                return circle;
            case AoeType.BIG_CIRCLE:
                return getCircle(2, 0);
        }
        return dot;
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
