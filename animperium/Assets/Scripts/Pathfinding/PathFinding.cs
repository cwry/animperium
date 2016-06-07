using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathFinding{
    public static Vec2i[] findPath(GridManager grid, int startX, int startY, int goalX, int goalY, int maxSteps, Func<Vec2i, bool> checkTraversable){
        List<PathNode> reachable = new List<PathNode>();
        List<PathNode> visited = new List<PathNode>();
        reachable.Add(new PathNode(startX, startY, grid));

        while (reachable.Count > 0){
            PathNode cheapest = null;
            float fcost = -1;

            foreach(PathNode node in reachable){
                if(fcost == -1 || node.f <= fcost){
                    fcost = node.f;
                    cheapest = node;
                }
            }

            visited.Add(cheapest);
            reachable.Remove(cheapest);
            GameObject[] adjacent = grid.gridData[cheapest.x, cheapest.y].GetComponent<TileInfo>().getAdjacent();

            foreach(GameObject go in adjacent){
                Vec2i hex = go.GetComponent<TileInfo>().gridPosition;
                bool gotVisited = false;
                foreach(PathNode vNode in visited){
                    if(hex.x == vNode.x && hex.y == vNode.y){
                        gotVisited = true;
                        break;
                    }
                }

                if (!gotVisited && checkTraversable(hex)){ 
                    bool isReachable = false;
                    foreach(PathNode rNode in reachable){
                        if(hex.x == rNode.x && hex.y == rNode.y){
                            isReachable = true;
                            rNode.tryAlternative(cheapest);
                            break;
                        }
                    }

                    if (!isReachable){
                        PathNode nNode = new PathNode(hex.x, hex.y, grid, goalX, goalY, cheapest);
                        if (hex.x == goalX && hex.y == goalY) return nNode.toPath();
                        if (nNode.g <= maxSteps) reachable.Add(nNode);
                    }
                }
            }
        }
        return null;
    }
}
