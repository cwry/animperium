using UnityEngine;
using System.Collections;

public class SelectedUnitGridTeleporter : MonoBehaviour {
    void Update(){
        if (SelectionManager.selectedTile == null) return;
        GameObject u = SelectionManager.selectedUnit;
        if (u == null) return;
        if (u.GetComponent<Unit>().currentTile == null) return;
        TileInfo start = u.GetComponent<Unit>().currentTile.GetComponent<TileInfo>();
        GridManager grid = (start.grid == Data.mainGrid) ? Data.subGrid : Data.mainGrid;
        TileInfo end = grid.gridData[start.gridPosition.x, start.gridPosition.y].GetComponent<TileInfo>();
        if (Input.GetKeyDown("space")){
            TeleportMovement.move(u, start.grid, start.gridPosition, end.grid, end.gridPosition);
        }
    }
}
