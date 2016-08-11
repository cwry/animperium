using UnityEngine;
using System.Collections;
using System;

public class DestroyUnitAbility : MonoBehaviour {
    public AbilityInfo abilityInfo;

    void Awake() {
        abilityInfo.selfCast = true;
        abilityInfo.owner = gameObject;
        abilityInfo.checkRange = checkRange;
        abilityInfo.checkAoe = AoeChecks.dot;
        abilityInfo.execute = (Vec2i target, bool isMainGrid, Action callback) => {
            AbilityManager.useAbility(abilityInfo, target, isMainGrid, () => {
                ContextMenuSpawn.ClearTarget();
                callback();
            });
        };
        abilityInfo.onExecution = executeAbility;
        abilityInfo.abilityID = GetComponent<Unit>().addAbility(abilityInfo);
    }

    void executeAbility(ServerMessage.UnitAbilityMessage msg) {
        Destroy(gameObject);
    }

    //just to return "true" (and have an actual valid tile returned for consistency)
    GameObject[] checkRange() {
        Unit u = gameObject.GetComponent<Unit>();
        TileInfo ti = u.currentTile.GetComponent<TileInfo>();
        GameObject[] tiles = new GameObject[1];
        tiles[0] = ti.gameObject;
        return tiles;
    }
}
