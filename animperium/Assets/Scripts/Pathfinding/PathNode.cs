using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode{
    public float g = 0;
    public float h = 0;
    public float f = 0;

    public int x;
    public int y;

    public bool isMain;

    PathNode parent;

    public PathNode(int x, int y, bool isMain){
        this.x = x;
        this.y = y;
        this.isMain = isMain;
    }

    public PathNode(int x, int y, bool isMain, int goalX, int goalY, PathNode parent){
        this.x = x;
        this.y = y;
        this.parent = parent;
        this.isMain = isMain;
        evalCost(goalX, goalY);
    }

    void evalCost(int goalX, int goalY){
        g = parent.g + 1;
        Vector3 goal = Data.getGridHex(goalX, goalY, isMain).transform.position;
        Vector3 current = Data.getGridHex(goalX, goalY, isMain).transform.position;
        h = Mathf.Abs(goal.x - current.x) + Mathf.Abs(goal.z - current.z);
        f = g + h;
    }

    void tryAlternative(PathNode alt){
        float altG = alt.g + 1;
        if(altG < g){
            parent = alt;
            g = altG;
            f = g + h;
        }
    }

    Vec2i[] toPath(){
        Stack<Vec2i> path = new Stack<Vec2i>();
        PathNode current = this;
        while(current != null){
            path.Push(new Vec2i(current.x, current.y));
            current = current.parent;
        }
        return path.ToArray();
    }

}
