using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MeleeAttack : MonoBehaviour {

    public int abilityID;
    public int strength;

    void execute(ServerMessage.UnitAbilityMessage msg) {
        if (msg.abilityID != abilityID) return;
        GridManager grid = msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid;
        GameObject target = grid.gridData[msg.targetX, msg.targetY].GetComponent<TileInfo>().unit;
        if(target != null){
            target.GetComponent<Unit>().damage(strength, DamageType.MELEE);
        }
    }
}
