using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

[System.Serializable]
public struct AbilityInfo {
    public string abilityID;
    public string name;
    public string description;
    public GameObject button;
    public int apCost;
    [HideInInspector]
    public GameObject owner;
    public Func<GameObject[]> checkRange;
    public Func<TileInfo, GameObject[]> checkAoe;
    public Action<Vec2i, bool> execute;
}

public class AbilityManager : MonoBehaviour {

    void Awake(){
        NetworkData.client.netClient.RegisterHandler((short)ServerMessage.Types.UNIT_ABILITY, onUnitAbilityMessage);
    }

    void onUnitAbilityMessage(NetworkMessage netMsg){
        ServerMessage.UnitAbilityMessage msg = netMsg.ReadMessage<ServerMessage.UnitAbilityMessage>();
        onUnitAbility(msg);
    }

    private static void onUnitAbility(ServerMessage.UnitAbilityMessage msg){
        GameObject go = Data.units[msg.unitID];
        ActionQueue.getInstance().push(msg.actionID, () => {
            go.SendMessage("executeAbility", msg);
        });
    }

    public static void useAbility(AbilityInfo abilityInfo, Vec2i target, bool isTargetMainGrid){
        Unit u = abilityInfo.owner.GetComponent<Unit>();
        u.actionPoints -= abilityInfo.apCost;
        ServerMessage.UnitAbilityMessage msg = new ServerMessage.UnitAbilityMessage();
        msg.unitID = u.unitID;
        msg.actionID = ActionQueue.getInstance().actionID++;
        msg.abilityID = abilityInfo.abilityID;
        msg.targetX = target.x;
        msg.targetY = target.y;
        msg.isTargetMainGrid = isTargetMainGrid;
        NetworkData.client.netClient.Send((short)ServerMessage.Types.UNIT_ABILITY, msg);
        onUnitAbility(msg);
    }

    public static AbilityInfo[] listAbilities(GameObject unit){
        List<AbilityInfo> list = new List<AbilityInfo>();
        Action<AbilityInfo> enlist = (AbilityInfo info) =>{
            list.Add(info);
        };
        unit.SendMessage("getAbilityInfo", enlist);
        return list.ToArray();
    }
}
