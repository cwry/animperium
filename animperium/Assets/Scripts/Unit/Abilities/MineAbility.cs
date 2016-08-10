using UnityEngine;
using System.Collections;
using System;

public class MineAbility : MonoBehaviour {
    public AbilityInfo abilityInfo;
    public int minRange = 1;
    public int maxRange = 1;

    public float amount;

    bool isMining = false;
    bool initialMine = false;

    Minable currentMine;

    Action removeOnTurnBegin;
    Action removeOnUseAbility;

    void Awake() {
        abilityInfo.getAffected = getAffected;
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
        removeOnUseAbility = u.onUseAbility.add<AbilityInfo>(onUseAbility);
    }

    Unit[] getAffected(ServerMessage.UnitAbilityMessage msg) {
        Unit[] affected = new Unit[1];
        GridManager grid = msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid;
        TileInfo target = grid.gridData[msg.targetX, msg.targetY].GetComponent<TileInfo>();
        affected[0] = target.unit.GetComponent<Unit>();
        return affected;
    }

    void executeAbility(ServerMessage.UnitAbilityMessage msg) {
        Unit[] affected = getAffected(msg);
        removeOnTurnBegin = TurnManager.onTurnEnd.add<int>(onTurnEnd);
        currentMine = affected[0].gameObject.GetComponent<Minable>();
        onTurnEnd(TurnManager.turnID);
        initialMine = true;
    }

    void onTurnEnd(int turnID) {
        if (initialMine) {
            initialMine = false;
            return;
        }
        if (!Data.isActivePlayer()) return;
        currentMine.mine(amount, gameObject);
    }

    void onUseAbility(AbilityInfo ai) {
        if(removeOnTurnBegin != null) removeOnTurnBegin();
    }

    GameObject[] checkRange() {
        Unit u = gameObject.GetComponent<Unit>();
        GameObject[] inRange = u.currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange, null, (TileInfo ti) => {
            if (ti.unit == null) return false;
            UndergroundTile ut = ti.gameObject.GetComponent<UndergroundTile>();
            if (ut != null && (ut.state != UndergroundTileState.REVEALED || !ut.isInSight())) return false;
            Minable mine = ti.unit.GetComponent<Minable>();
            if (mine == null) return false;
            return true;
        });

        return inRange.Length == 0 ? null : inRange;
    }

    GameObject[] getRangeIndicator() {
        return gameObject.GetComponent<Unit>().currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange, null, (TileInfo ti) => {
            return ti.traversable;
        });
    }
}