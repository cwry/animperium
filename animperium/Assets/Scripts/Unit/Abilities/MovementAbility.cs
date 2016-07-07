using UnityEngine;
using System.Collections;
using System;

public class MovementAbility : MonoBehaviour {
    public AbilityInfo abilityInfo;
    public float animationSpeed = 3;
    public float jumpHeight = 0.1f;

    public int maxMovementPoints;
    [HideInInspector]
    public int movementPoints;

    Action removeTurnBegin;

    void Awake(){
        movementPoints = maxMovementPoints;
        removeTurnBegin = TurnManager.onTurnBegin.add<int>(onTurnBegin);
        abilityInfo.owner = gameObject;
        abilityInfo.checkRange = checkRange;
        abilityInfo.checkAoe = AoeChecks.dot;
        abilityInfo.execute = (Vec2i target, bool isMainGrid) => {
            AbilityManager.useAbility(abilityInfo, target, isMainGrid);
        };
        abilityInfo.onExecution = executeAbility;
        abilityInfo.abilityID = GetComponent<Unit>().addAbility(abilityInfo);
    }

    void OnDestroy() {
        removeTurnBegin();
    }

    void onTurnBegin(int turnID) {
        movementPoints = maxMovementPoints;
    }

    void executeAbility(ServerMessage.UnitAbilityMessage msg){
        GridManager grid = msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid;
        Vec2i currentPos = gameObject.GetComponent<Unit>().currentTile.GetComponent<TileInfo>().gridPosition;
        Vec2i[] path = PathFinding.findPath(grid, currentPos.x, currentPos.y, msg.targetX, msg.targetY, movementPoints, (Vec2i hx) => {
            TileInfo ti = grid.gridData[hx.x, hx.y].GetComponent<TileInfo>();
            return ti.traversable && ti.unit == null;
        });
        movementPoints -= path.Length - 1;
        PathMovement.move(gameObject, grid, path, animationSpeed, jumpHeight);
    }

 

    GameObject[] checkRange(){
        Unit u = gameObject.GetComponent<Unit>();
        GameObject[] inRange = u.currentTile.GetComponent<TileInfo>().listTree(1, movementPoints, (TileInfo ti) => {
            return ti.traversable && ti.unit == null;
        });
        return inRange.Length == 0 ? null : inRange;
    }

}
