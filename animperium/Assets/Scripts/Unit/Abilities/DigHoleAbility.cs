using UnityEngine;
using System.Collections;
using System;

public class DigHoleAbility : MonoBehaviour {
    public AbilityInfo abilityInfo;
    public GameObject holeModel;
    public GameObject holeModelUngerground;
    public int minRange = 1;
    public int maxRange = 1;

    void Awake() {
        abilityInfo.owner = gameObject;
        abilityInfo.checkRange = checkRange;
        abilityInfo.checkAoe = AoeChecks.dot;
        abilityInfo.execute = (Vec2i target, bool isMainGrid) => {
            AbilityManager.useAbility(abilityInfo, target, isMainGrid);
        };
        abilityInfo.onExecution = executeAbility;
        Unit u = GetComponent<Unit>();
        abilityInfo.abilityID = u.addAbility(abilityInfo);
    }

    bool checkTile(TileInfo ti) {
        return ti.traversable;
    }

    void executeAbility(ServerMessage.UnitAbilityMessage msg) {
        GridManager grid = msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid;
        TileInfo target = grid.gridData[msg.targetX, msg.targetY].GetComponent<TileInfo>();
        GridManager otherGrid = msg.isTargetMainGrid ? Data.subGrid : Data.mainGrid;
        TileInfo otherTile = otherGrid.gridData[msg.targetX, msg.targetY].GetComponent<TileInfo>();
        GameObject targetModel = Instantiate(target.grid.isMainGrid ? holeModel : holeModelUngerground, target.transform.position, Quaternion.identity) as GameObject;
        GameObject otherTargetModel = Instantiate(otherTile.grid.isMainGrid ? holeModel : holeModelUngerground, target.transform.position, Quaternion.identity) as GameObject;
        target.makeHole();
        Action removeTarget = target.removeHole;
        otherTile.makeHole();
        Action removeOtherTarget = otherTile.removeHole;
        Action removeHoles = () => {
            Destroy(targetModel);
            Destroy(otherTargetModel);
            removeTarget();
            removeOtherTarget();
        };
        target.removeHole = removeHoles;
        otherTile.removeHole = removeHoles;
    }

    GameObject[] checkRange() {
        Unit u = gameObject.GetComponent<Unit>();
        if (u.actionPoints < abilityInfo.apCost) return null;
        GameObject[] inRange = u.currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange, (TileInfo ti) => {
            GridManager otherGrid = ti.grid.isMainGrid ? Data.subGrid : Data.mainGrid;
            TileInfo otherTile = otherGrid.gridData[ti.gridPosition.x, ti.gridPosition.y].GetComponent<TileInfo>();
            return ti.traversable && otherTile.traversable;
        });
        return inRange.Length == 0 ? null : inRange;
    }
}
