using UnityEngine;
using System.Collections;
using System;

public class PathMovement : MonoBehaviour {

	public Vec2i[] path;
    public GridManager grid;
    public float speed;
    public float jumpHeight;
    public bool init = true;
    int lastProgI = 0;
    Action callback;

    float progress = 0;

	void Update () {
	    if(path != null && path.Length > 1 && grid != null){
            if(progress < path.Length - 1){
                int progI = (int) Mathf.Floor(progress);
                Vec2i fromI = path[progI];
                Vec2i toI = path[progI + 1];
                GameObject fromObj = grid.gridData[fromI.x, fromI.y];
                GameObject toObj = grid.gridData[toI.x, toI.y];
                Vector3 from = fromObj.transform.position;
                Vector3 to = toObj.transform.position;

                //sightrange stuff
                if (lastProgI != progI) {
                    lastProgI = progI;
                    if (!grid.isMainGrid) {
                        updateSightRange(grid.gridData[path[progI - 1].x, path[progI - 1].y], fromObj);
                        updateUnitVisibility(fromObj);
                    }
                }

                //animation stuff
                Vector3 dir = to - from;
                float delta = progress - progI;
                Vector3 deltaV = dir * delta;
                float deltaSin = Mathf.Sin(delta * Mathf.PI);
                deltaV.y += deltaSin * jumpHeight;
                Vector3 pos = from + deltaV;
                transform.position = pos;
                progress += Time.deltaTime * speed / dir.magnitude;
            }else{
                Vec2i posI = path[path.Length - 1];
                GameObject goal = grid.gridData[posI.x, posI.y];
                transform.position = goal.transform.position;
                updateSightRange(grid.gridData[path[path.Length - 2].x, path[path.Length - 2].y], goal);
                updateUnitVisibility(goal);
                if (callback != null) callback();
                Destroy(this);
            }
        }
	}

    void updateUnitVisibility(GameObject currentObj) {
        Unit u = gameObject.GetComponent<Unit>();
        if (u.playerID != Data.playerID) {
            UndergroundTile ut = currentObj.GetComponent<UndergroundTile>();
            if (ut == null || ut.isInSight()) {
                u.reveal();
            } else {
                u.hide();
            }
        }
    }

    void updateSightRange(GameObject lastObj, GameObject currObj) {
        UndergroundTile lastUT = lastObj.GetComponent<UndergroundTile>();
        UndergroundTile currUT = currObj.GetComponent<UndergroundTile>();
        if (lastUT != null) lastUT.removeSightRange(gameObject);
        if (currUT != null) currUT.addSightRange(gameObject);
    }

    public static void move(GameObject go, GridManager grid, Vec2i[] path, float speed, float jumpHeight, Action callback = null){
        PathMovement pm = go.AddComponent<PathMovement>();
        pm.grid = grid;
        pm.speed = speed;
        pm.path = path;
        pm.jumpHeight = jumpHeight;
        pm.callback = callback;
        Vec2i start = path[0];
        Vec2i end = path[path.Length - 1];
        TileInfo startTile = grid.gridData[start.x, start.y].GetComponent<TileInfo>();
        TileInfo endTile = grid.gridData[end.x, end.y].GetComponent<TileInfo>();
        startTile.detachUnit();
        endTile.attachUnit(go);
    }
}
