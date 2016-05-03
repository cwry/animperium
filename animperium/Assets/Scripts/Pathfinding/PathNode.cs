using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode{
    public float g = 0;
    public float h = 0;
    public float f = 0;

    public int x;
    public int y;

    public GridManager grid;

    PathNode parent;

    public PathNode(int x, int y, GridManager grid){
        this.x = x;
        this.y = y;
        this.grid = grid;
    }

    public PathNode(int x, int y, GridManager grid, int goalX, int goalY, PathNode parent){
        this.x = x;
        this.y = y;
        this.parent = parent;
        this.grid = grid;
        evalCost(goalX, goalY);
    }

    void evalCost(int goalX, int goalY){
        g = parent.g + 1;
        Vector3 goal = grid.gridData[x, y].transform.position;
        Vector3 current = grid.gridData[goalX, goalY].transform.position;
        h = Mathf.Abs(goal.x - current.x) + Mathf.Abs(goal.z - current.z) * 0.3f;
        f = g + h;
    }

    public void tryAlternative(PathNode alt){
        float altG = alt.g + 1;
        if(altG < g){
            parent = alt;
            g = altG;
            f = g + h;
        }
    }

    public Vec2i[] toPath(){
        Stack<Vec2i> path = new Stack<Vec2i>();
        PathNode current = this;
        while(current != null){
            path.Push(new Vec2i(current.x, current.y));
            current = current.parent;
        }
        return path.ToArray();
    }

}
