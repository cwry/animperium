using UnityEngine;
using System.Collections;

public class SelectedUnitGridTeleporter : MonoBehaviour {
    void Update(){
        if (SelectionManager.selectedTile == null) return;
        GameObject u = SelectionManager.selectedTile.GetComponent<TileInfo>().unit;
        if (u == null) return;
        TileInfo start = SelectionManager.selectedTile.GetComponent<TileInfo>();
        GridManager grid = (start.grid == Data.mainGrid) ? Data.subGrid : Data.mainGrid;
        TileInfo end = grid.gridData[start.gridPosition.x, start.gridPosition.y].GetComponent<TileInfo>();
        if (Input.GetKeyDown("space")){
            TeleportMovement.move(u, start.grid, start.gridPosition, end.grid, end.gridPosition);
        }
    }
}
