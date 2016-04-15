using UnityEngine;
using System.Collections;

public class SelectedUnitMover : MonoBehaviour {

	void Update(){
        if (SelectionManager.selectedTile == null) return;
        GameObject u = SelectionManager.selectedUnit;
        if (u == null) return;
        if (u.GetComponent<Unit>().currentTile == null) return;
        TileInfo start = u.GetComponent<Unit>().currentTile.GetComponent<TileInfo>();
        TileInfo end = SelectionManager.hoverTile.GetComponent<TileInfo>();
        if (Input.GetMouseButtonDown(1)){
            Vec2i[] path = PathFinding.findPath(start.grid, start.gridPosition.x, start.gridPosition.y, end.gridPosition.x, end.gridPosition.y, (Vec2i hx) => {
                return start.grid.gridData[hx.x, hx.y].GetComponent<TileInfo>().traversable;
            });
            PathMovement.move(u, start.grid, path, 3f);
        }
    }
}
