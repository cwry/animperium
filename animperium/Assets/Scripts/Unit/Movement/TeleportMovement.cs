using UnityEngine;
using System.Collections;
using System;

public class TeleportMovement : MonoBehaviour {
    public static void move(GameObject go, GridManager startGrid, Vec2i start, GridManager destGrid, Vec2i dest, Action callback = null){
        GameObject destTile = destGrid.gridData[dest.x, dest.y];
        GameObject startTile = startGrid.gridData[start.x, start.y];
        TileInfo currTileInfo = startTile.GetComponent<TileInfo>();
        currTileInfo.detachUnit();
        go.transform.position = destTile.transform.position;
        TileInfo destTileInfo = destTile.GetComponent<TileInfo>();
        destTileInfo.attachUnit(go);
        if (callback != null) callback();
    }
}
 