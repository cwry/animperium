using UnityEngine;
using System.Collections;

public class Data {
    public static GridManager mainGrid;
    public static GridManager subGrid;
    public static GameObject getGridHex(int x, int y, bool main){
        return main ? mainGrid.gridData[x, y] : subGrid.gridData[x, y];
    }
}
