using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

struct RangeCheckArgs
{
    public string abilityID;
    public Action<GameObject[]> callback;
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

    public static void useAbility(GameObject unit, string abilityID, Vec2i target, bool isTargetMainGrid){
        ServerMessage.UnitAbilityMessage msg = new ServerMessage.UnitAbilityMessage();
        msg.unitID = unit.GetComponent<Unit>().unitID;
        msg.actionID = ActionQueue.getInstance().actionID++;
        msg.abilityID = abilityID;
        msg.targetX = target.x;
        msg.targetY = target.y;
        msg.isTargetMainGrid = isTargetMainGrid;
        NetworkData.client.netClient.Send((short)ServerMessage.Types.UNIT_ABILITY, msg);
        onUnitAbility(msg);
    }

    public static string[] listAbilities(GameObject unit){
        List<string> list = new List<string>();
        Action<string> enlist = (string name) =>{
            list.Add(name);
        };
        unit.SendMessage("enumerateAbility", enlist);
        return list.ToArray();
    }

    public static GameObject[] checkRange(GameObject unit, string abilityID){
        GameObject[] result = null;
        RangeCheckArgs args = new RangeCheckArgs();
        args.abilityID = abilityID;
        args.callback = (GameObject[] res) => {
            result = res;
        };
        unit.SendMessage("rangeCheckAbility", args);
        return result;
    }
}
