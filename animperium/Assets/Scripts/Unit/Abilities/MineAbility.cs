using UnityEngine;
using System.Collections;
using System;

public class MineAbility : MonoBehaviour {
    public AbilityInfo abilityInfo;
    public int minRange = 1;
    public int maxRange = 1;

    bool isMining = false;

    Action removeOnTurnBegin;

    void Awake() {
        abilityInfo.owner = gameObject;
        abilityInfo.checkRange = checkRange;
        abilityInfo.checkAoe = AoeChecks.dot;
        abilityInfo.execute = (Vec2i target, bool isMainGrid) => {
            AbilityManager.useAbility(abilityInfo, target, isMainGrid);
        };
        abilityInfo.onExecution = executeAbility;
        abilityInfo.abilityID = GetComponent<Unit>().addAbility(abilityInfo);
    }

    void executeAbility(ServerMessage.UnitAbilityMessage msg) {

    }

    GameObject[] checkRange() {
        Unit u = gameObject.GetComponent<Unit>();
        if (u.actionPoints < abilityInfo.apCost) return null;
        GameObject[] inRange = u.currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange, null, (TileInfo ti) => {
            if (ti.unit == null) return false;
            Minable mine = ti.unit.GetComponent<Minable>();
            if (mine == null) return false;
            return true;
        });

        return inRange.Length == 0 ? null : inRange;
    }
}
