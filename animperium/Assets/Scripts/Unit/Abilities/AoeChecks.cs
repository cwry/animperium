using UnityEngine;
using System;

public class AoeChecks{
    
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
