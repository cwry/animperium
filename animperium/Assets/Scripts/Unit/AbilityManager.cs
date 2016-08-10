using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

[System.Serializable]
public struct AbilityInfo {
    [HideInInspector]
    public int abilityID;
    public string name;
    public string description;
    public GameObject button;
    public GameObject[] effectsOnCaster;
    public GameObject[] effectsOnAffected;
    public GameObject[] effectsOnTarget;
    public GameObject[] effectsOnAoe;
    public int apCost;
    public float woodCost;
    public float ironCost;
    public float stoneCost;
    [HideInInspector]
    public bool selfCast;
    [HideInInspector]
    public GameObject owner;
    public Func<GameObject[]> getRangeIndicator;
    public Func<GameObject[]> checkRange;
    public Func<ServerMessage.UnitAbilityMessage, Unit[]> getAffected;
    public Func<bool> checkCost;
    public Func<TileInfo, GameObject[]> checkAoe;
    public Action<Vec2i, bool, Action> execute;
    public Action<ServerMessage.UnitAbilityMessage> onExecution;
}

public class AbilityManager : MonoBehaviour {

    void Awake(){
        NetworkData.client.netClient.RegisterHandler((short)ServerMessage.Types.UNIT_ABILITY, onUnitAbilityMessage);
    }

    void onUnitAbilityMessage(NetworkMessage netMsg){
        ServerMessage.UnitAbilityMessage msg = netMsg.ReadMessage<ServerMessage.UnitAbilityMessage>();
        onUnitAbility(msg);
    }

    private static void onUnitAbility(ServerMessage.UnitAbilityMessage msg, Action callback = null){
        Unit u = Data.units[msg.unitID].GetComponent<Unit>();
        AbilityInfo ai = u.abilities[msg.abilityID];
        ActionQueue.getInstance().push(msg.actionID, () => {
            u.onUseAbility.fire(ai);
            ai.onExecution(msg);
            if(callback != null) callback();
        });
    }

    public static void useAbility(AbilityInfo abilityInfo, Vec2i target, bool isTargetMainGrid, Action callback = null){
        Unit u = abilityInfo.owner.GetComponent<Unit>();
        ServerMessage.UnitAbilityMessage msg = new ServerMessage.UnitAbilityMessage();
        msg.unitID = u.unitID;
        msg.actionID = ActionQueue.getInstance().actionID++;
        msg.abilityID = abilityInfo.abilityID;
        msg.targetX = target.x;
        msg.targetY = target.y;
        msg.isTargetMainGrid = isTargetMainGrid;
        NetworkData.client.netClient.Send((short)ServerMessage.Types.UNIT_ABILITY, msg);
        onUnitAbility(msg, callback);
    }
}
