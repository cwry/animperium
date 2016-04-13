using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathFinding{
    public static Vec2i[] findPath(GridManager grid, int startX, int startY, int goalX, int goalY, Func<Vec2i, bool> checkTraversable){
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

            Vec2i[] adjacent = getAdjacent(grid, cheapest.x, cheapest.y);

            foreach(Vec2i hex in adjacent){
                bool gotVisited = false;
                foreach(PathNode vNode in visited){
                    if(hex.x == vNode.x && hex.y == vNode.y){
                        gotVisited = true;
                        break;
                    }
                }

                if (!gotVisited && checkTraversable(hex)){  // && traversable
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
                        reachable.Add(nNode);
                    }
                }
            }
        }
        return null;
    }

    private static bool isInBounds(GridManager grid, int x, int y){
        var maxW = Data.mainGrid.gridWidthInHexes;
        var maxH = Data.mainGrid.gridHeightInHexes;
        return x >= 0 && x < maxW && y >= 0 && y < maxH;
    }

    private static Vec2i getNE(GridManager grid, int x, int y){
        int nx = x + (y % 2 == 1 ? 1 : 0);
        int ny = y + 1;
        isInBounds(grid, nx, ny);
        return isInBounds(grid, nx, ny) ? new Vec2i(nx, ny) : null;
    }

    private static Vec2i getE(GridManager grid, int x, int y){
        int nx = x + 1;
        int ny = y;
        isInBounds(grid, nx, ny);
        return isInBounds(grid, nx, ny) ? new Vec2i(nx, ny) : null;
    }

    private static Vec2i getSE(GridManager grid, int x, int y){
        int nx = x + (y % 2 == 1 ? 1 : 0);
        int ny = y - 1;
        isInBounds(grid, nx, ny);
        return isInBounds(grid, nx, ny) ? new Vec2i(nx, ny) : null;
    }

    private static Vec2i getSW(GridManager grid, int x, int y){
        int nx = x - (y % 2 == 1 ? 1 : 0);
        int ny = y - 1;
        isInBounds(grid, nx, ny);
        return isInBounds(grid, nx, ny) ? new Vec2i(nx, ny) : null;
    }

    private static Vec2i getW(GridManager grid, int x, int y){
        int nx = x - 1;
        int ny = y;
        isInBounds(grid, nx, ny);
        return isInBounds(grid, nx, ny) ? new Vec2i(nx, ny) : null;
    }

    private static Vec2i getNW(GridManager grid, int x, int y){
        int nx = x - (y % 2 == 1 ? 1 : 0);
        int ny = y + 1;
        isInBounds(grid, nx, ny);
        return isInBounds(grid, nx, ny) ? new Vec2i(nx, ny) : null;
    }

    private static Vec2i[] getAdjacent(GridManager grid, int x , int y){
        List<Vec2i> adjacent = new List<Vec2i>();
        Vec2i ne = getNE(grid, x, y);
        Vec2i e = getE(grid, x, y);
        Vec2i se = getSE(grid, x, y);
        Vec2i sw = getSW(grid, x, y);
        Vec2i w = getW(grid, x, y);
        Vec2i nw = getNW(grid, x, y);
        if (ne != null) adjacent.Add(ne);
        if (e != null) adjacent.Add(e);
        if (se != null) adjacent.Add(se);
        if (sw != null) adjacent.Add(sw);
        if (w != null) adjacent.Add(w);
        if (nw != null) adjacent.Add(nw);
        return adjacent.ToArray();
    }

}
