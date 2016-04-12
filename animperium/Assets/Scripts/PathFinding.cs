using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding{
    public static void findPath(int startX, int startY, bool isMain, int goalX, int goalY){
        List<PathNode> reachable = new List<PathNode>();
        List<PathNode> visited = new List<PathNode>();
        reachable.Add(new PathNode(startX, startY, isMain));
    }

    private static bool isInBounds(int x, int y){
        var maxW = Data.mainGrid.gridWidthInHexes;
        var maxH = Data.mainGrid.gridHeightInHexes;
        return x >= 0 && x < maxW && y >= 0 && y < maxH;
    }

}
