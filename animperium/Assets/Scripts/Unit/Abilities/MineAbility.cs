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

    void executeAbility(ServerMessage.UnitAbilityMessage msg) {
        GridManager grid = msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid;
        TileInfo target = grid.gridData[msg.targetX, msg.targetY].GetComponent<TileInfo>();
        removeOnTurnBegin = TurnManager.onTurnEnd.add<int>(onTurnEnd);
        currentMine = target.unit.GetComponent<Minable>();
        onTurnEnd(TurnManager.turnID);
        initialMine = true;
    }

    void onTurnEnd(int turnID) {
        if (initialMine) {
            initialMine = false;
            return;
        }
        if (!Data.isActivePlayer()) return;
        currentMine.mine(amount);
    }

    void onUseAbility(AbilityInfo ai) {
        if(removeOnTurnBegin != null) removeOnTurnBegin();
    }

    GameObject[] checkRange() {
        Unit u = gameObject.GetComponent<Unit>();
        GameObject[] inRange = u.currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange, null, (TileInfo ti) => {
            if (ti.unit == null) return false;
            Minable mine = ti.unit.GetComponent<Minable>();
            if (mine == null) return false;
            return true;
        });

        return inRange.Length == 0 ? null : inRange;
    }
}
