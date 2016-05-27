using UnityEngine;
using System.Collections;
using System;

public class PathMovement : MonoBehaviour {

	public Vec2i[] path;
    public GridManager grid;
    public float speed;
    public bool init = true;
    Action callback;

    float progress = 0;

	void Update () {
	    if(path != null && path.Length > 1 && grid != null){
            if(progress < path.Length - 1){
                int progI = (int) Mathf.Floor(progress);
                Vec2i fromI = path[progI];
                Vec2i toI = path[progI + 1];
                Vector3 from = grid.gridData[fromI.x, fromI.y].transform.position;
                Vector3 to = grid.gridData[toI.x, toI.y].transform.position;
                Vector3 dir = to - from;
                Vector3 delta = dir * (progress - progI);
                Vector3 pos = from + delta;
                transform.position = pos;
                progress += Time.deltaTime * speed / dir.magnitude;
            }else{
                Vec2i posI = path[path.Length - 1];
                transform.position = grid.gridData[posI.x, posI.y].transform.position;
                if (callback != null) callback();
                Destroy(this);
            }
        }
	}

    public static void move(GameObject go, GridManager grid, Vec2i[] path, float speed, Action callback = null){
        PathMovement pm = go.AddComponent<PathMovement>();
        pm.grid = grid;
        pm.speed = speed;
        pm.path = path;
        Vec2i start = path[0];
        Vec2i end = path[path.Length - 1];
        TileInfo startTile = grid.gridData[start.x, start.y].GetComponent<TileInfo>();
        TileInfo endTile = grid.gridData[end.x, end.y].GetComponent<TileInfo>();
        startTile.detachUnit();
        endTile.attachUnit(go);
        if (callback != null){
            pm.callback = callback;
        }
    }
}
