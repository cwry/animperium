using UnityEngine;
using System.Collections;
using System;

public class MovementAbility : MonoBehaviour {
    public string abilityID = "move";
    public float animationSpeed = 3;

    private Unit unit; //just 4 cache

    void Awake(){
        unit = gameObject.GetComponent<Unit>();
    }


    void executeAbility(ServerMessage.UnitAbilityMessage msg){
        if (msg.abilityID != abilityID) return;
        GridManager grid = msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid;
        Vec2i currentPos = unit.currentTile.GetComponent<TileInfo>().gridPosition;
        Vec2i[] path = PathFinding.findPath(grid, currentPos.x, currentPos.y, msg.targetX, msg.targetY, unit.movementPoints, (Vec2i hx) => {
            TileInfo ti = grid.gridData[hx.x, hx.y].GetComponent<TileInfo>();
            return ti.traversable && ti.unit == null;
        });
        PathMovement.move(gameObject, grid, path, animationSpeed);
    }

    void enumerateAbility(Action<string> enlist){
        enlist(abilityID);
    }

    void rangeCheckAbility(RangeCheckArgs rca){
        if (rca.abilityID != abilityID) return;
        rca.callback(unit.currentTile.GetComponent<TileInfo>().listTree(unit.movementPoints, false, (TileInfo ti) => {
            return ti.traversable && ti.unit == null;
        }));
    }

}
