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
        GameObject otherTargetModel = Instantiate(otherTile.grid.isMainGrid ? holeModel : holeModelUngerground, otherTile.transform.position, Quaternion.identity) as GameObject;
        target.isHole = true;
        otherTile.isHole = true;
        Action removeHoles = () => {
            Destroy(targetModel);
            Destroy(otherTargetModel);
            target.isHole = false;
            otherTile.isHole = false;
        };
        target.removeHole = removeHoles;
        otherTile.removeHole = removeHoles;
    }

    GameObject[] checkRange() {
        Unit u = gameObject.GetComponent<Unit>();
        GameObject[] inRange = u.currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange, (TileInfo ti) => {
            GridManager otherGrid = ti.grid.isMainGrid ? Data.subGrid : Data.mainGrid;
            TileInfo otherTile = otherGrid.gridData[ti.gridPosition.x, ti.gridPosition.y].GetComponent<TileInfo>();
            return ti.traversable && otherTile.traversable && !ti.isHole && !otherTile.isHole;
        });
        return inRange.Length == 0 ? null : inRange;
    }
}
