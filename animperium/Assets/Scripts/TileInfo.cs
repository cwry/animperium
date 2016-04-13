using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileInfo : MonoBehaviour {
    public Vec2i gridPosition;
    public GridManager grid;

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
}
