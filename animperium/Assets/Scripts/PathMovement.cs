using UnityEngine;
using System.Collections;
using System;

public class PathMovement : MonoBehaviour {

	public Vec2i[] path;
    public GridManager grid;
    public float speed;
    public bool init = true;
    GameEvent onDone = new GameEvent();

    float progress = 0;
	
	// Update is called once per frame
	void Update () {
	    if(path != null && path.Length > 0 && grid != null){
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
                onDone.fire(gameObject);
                Destroy(this);
            }
        }
	}

    public static void move(GameObject go, GridManager grid, Vec2i[] path, float speed, Action<GameObject> callback = null){
        PathMovement pm = go.AddComponent<PathMovement>();
        pm.grid = grid;
        pm.speed = speed;
        pm.path = path;
        if (callback != null){
            pm.onDone.add(callback);
        }
    }
}
