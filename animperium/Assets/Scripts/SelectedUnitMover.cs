using UnityEngine;
using System.Collections;

public class SelectedUnitMover : MonoBehaviour {

	void Update(){
        if (SelectionManager.selectedTile == null) return;
        GameObject u = SelectionManager.selectedTile.GetComponent<TileInfo>().unit;
        if (u == null) return;
        TileInfo start = SelectionManager.selectedTile.GetComponent<TileInfo>();
        TileInfo end = SelectionManager.hoverTile.GetComponent<TileInfo>();
        if (Input.GetMouseButtonDown(1)){
            Vec2i[] path = PathFinding.findPath(start.grid, start.gridPosition.x, start.gridPosition.y, end.gridPosition.x, end.gridPosition.y, (Vec2i hx) => {
                return Data.mainGrid.gridData[hx.x, hx.y].GetComponent<TileInfo>().traversable;
            });
            PathMovement.move(u, start.grid, path, 3f);
        }
    }
}
