using UnityEngine;
using System.Collections;

public class SelectionManager  {
    public static GameObject selectedTile;
    public static GameObject hoverTile;
    public static GameObject selectedUnit;
    public static GameObject selectedTarget;
    public static GameEvent onSelectedUnitChanged = new GameEvent();
}
