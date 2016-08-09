using UnityEngine;
using System.Collections;
using System;

public class MovementAbility : MonoBehaviour {
    public AbilityInfo abilityInfo;
    public float animationSpeed = 3;
    public float jumpHeight = 0.5f;

    public int maxMovementPoints;
    [HideInInspector]
    public int movementPoints;

    Action removeTurnBegin;

    Action currentCallback;

    void Awake(){
        abilityInfo.getRangeIndicator = getRangeIndicator;
        movementPoints = maxMovementPoints;
        removeTurnBegin = TurnManager.onTurnBegin.add<int>(onTurnBegin);
        abilityInfo.owner = gameObject;
        abilityInfo.checkRange = checkRange;
        abilityInfo.checkAoe = AoeChecks.dot;
        abilityInfo.execute = (Vec2i target, bool isMainGrid, Action callback) => {
            currentCallback = callback;
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

    bool checkHexTraversability(TileInfo ti){
        UndergroundTile ut = ti.gameObject.GetComponent<UndergroundTile>();
        bool underground = ut != null && ut.state == UndergroundTileState.REVEALED && ut.isInSight();
        return underground && ti.traversable && (ti.unit == null || ti.unit == gameObject);
    }

    void executeAbility(ServerMessage.UnitAbilityMessage msg){
        GridManager grid = msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid;
        Vec2i currentPos = gameObject.GetComponent<Unit>().currentTile.GetComponent<TileInfo>().gridPosition;
        Vec2i[] path = PathFinding.findPath(grid, currentPos.x, currentPos.y, msg.targetX, msg.targetY, movementPoints, (Vec2i hx) => {
            TileInfo ti = grid.gridData[hx.x, hx.y].GetComponent<TileInfo>();
            return checkHexTraversability(ti);
        });
        movementPoints -= path.Length - 1;
        PathMovement.move(gameObject, grid, path, animationSpeed, jumpHeight, currentCallback);
        currentCallback = null;
    }

    GameObject[] checkRange(){
        if (movementPoints == 0) return null;
        Unit u = gameObject.GetComponent<Unit>();
        GameObject[] inRange = u.currentTile.GetComponent<TileInfo>().listTree(1, movementPoints, checkHexTraversability, checkHexTraversability);
        return inRange.Length == 0 ? null : inRange;
    }

    GameObject[] getRangeIndicator(){
        if (movementPoints == 0) return null;
        GameObject[] inRange = gameObject.GetComponent<Unit>().currentTile.GetComponent<TileInfo>().listTree(1, movementPoints,
        (TileInfo ti) => {
            return ti.traversable;
        },
        (TileInfo ti) => {
            return ti.traversable;
        });
        return inRange.Length == 0 ? null : inRange;
    }

}
