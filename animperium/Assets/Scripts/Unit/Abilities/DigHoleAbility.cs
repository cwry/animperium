using UnityEngine;
using System.Collections;
using System;

public class DigHoleAbility : MonoBehaviour {
    public AbilityInfo abilityInfo;
    public GameObject holeModel;
    public GameObject holeModelUngerground;
    public bool selfCast = true;
    public int minRange = 1;
    public int maxRange = 1;

    void Awake() {
        if (selfCast) {
            minRange = 0;
            maxRange = 0;
        }
        abilityInfo.selfCast = selfCast;
        abilityInfo.getRangeIndicator = getRangeIndicator;
        abilityInfo.owner = gameObject;
        abilityInfo.checkRange = checkRange;
        abilityInfo.checkAoe = AoeChecks.dot;
        abilityInfo.execute = (Vec2i target, bool isMainGrid, Action callback) => {
            AbilityManager.useAbility(abilityInfo, target, isMainGrid, callback);
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
            target.removeHole = () => { };
            otherTile.removeHole = () => { };
            target.hideHole = () => { };
            otherTile.hideHole = () => { };
            target.revealHole = () => { };
            otherTile.revealHole = () => { };
        };
        target.removeHole = removeHoles;
        target.hideHole = () => {
            foreach(Renderer render in targetModel.GetComponentsInChildren<Renderer>()) {
                render.enabled = false;
            }
        };
        target.revealHole = () => {
            foreach (Renderer render in targetModel.GetComponentsInChildren<Renderer>()) {
                render.enabled = true;
            }
        };

        otherTile.removeHole = removeHoles;
        otherTile.hideHole = () => {
            foreach (Renderer render in otherTargetModel.GetComponentsInChildren<Renderer>()) {
                render.enabled = false;
            }
        };
        otherTile.revealHole = () => {
            foreach (Renderer render in otherTargetModel.GetComponentsInChildren<Renderer>()) {
                render.enabled = true;
            }
        };

        TileInfo undergroundTI = (target.grid.isMainGrid ? otherTile : target);
        UndergroundTile undergroundTile = undergroundTI.GetComponent<UndergroundTile>();
        if (!undergroundTile.isInSight()) undergroundTI.hideHole();
        undergroundTile.state = UndergroundTileState.REVEALED;
        undergroundTile.updateSightRange();
    }

    GameObject[] checkRange() {
        Unit u = gameObject.GetComponent<Unit>();
        GameObject[] inRange = u.currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange, null, (TileInfo ti) => {
            GridManager otherGrid = ti.grid.isMainGrid ? Data.subGrid : Data.mainGrid;
            TileInfo otherTile = otherGrid.gridData[ti.gridPosition.x, ti.gridPosition.y].GetComponent<TileInfo>();
            return ti.traversable && otherTile.traversable && !ti.isHole && !otherTile.isHole;
        });
        return inRange.Length == 0 ? null : inRange;
    }

    GameObject[] getRangeIndicator() {
        return gameObject.GetComponent<Unit>().currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange, null, (TileInfo ti) => {
            return ti.traversable;
        });
    }
}
