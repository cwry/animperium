using UnityEngine;
using System.Collections;
using System;

public class MovementAbility : MonoBehaviour {
    public AbilityInfo abilityInfo;
    public float animationSpeed = 3;

    private Unit unit; //just 4 cache

    void Awake(){
        unit = gameObject.GetComponent<Unit>();
        abilityInfo.checkRange = checkRange;
        abilityInfo.checkAoe = AoeChecks.dot;
        abilityInfo.execute = (Vec2i target, bool isMainGrid) => {
            AbilityManager.useAbility(gameObject, abilityInfo.abilityID, target, isMainGrid);
        };
    }

    void executeAbility(ServerMessage.UnitAbilityMessage msg){
        if (msg.abilityID != abilityInfo.abilityID) return;
        GridManager grid = msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid;
        Vec2i currentPos = unit.currentTile.GetComponent<TileInfo>().gridPosition;
        Vec2i[] path = PathFinding.findPath(grid, currentPos.x, currentPos.y, msg.targetX, msg.targetY, unit.movementPoints, (Vec2i hx) => {
            TileInfo ti = grid.gridData[hx.x, hx.y].GetComponent<TileInfo>();
            return ti.traversable && ti.unit == null;
        });
        PathMovement.move(gameObject, grid, path, animationSpeed);
    }

    void getAbilityInfo(Action<AbilityInfo> enlist){
        enlist(abilityInfo);
    }

    GameObject[] checkRange(){
        GameObject[] inRange = unit.currentTile.GetComponent<TileInfo>().listTree(1, unit.movementPoints, (TileInfo ti) => {
            return ti.traversable && ti.unit == null;
        });
        return inRange.Length == 0 ? null : inRange;
    }

}
