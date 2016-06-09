using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

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
            go.SendMessage("execute", msg);
        });
    }

    static void useAbility(GameObject unit, int abilityID, Vec2i target, bool isTargetMainGrid){
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

    void Update(){
        if (Input.GetKeyDown(KeyCode.Q) && SelectionManager.selectedUnit != null && SelectionManager.hoverTile != null) {
            TileInfo target = SelectionManager.hoverTile.GetComponent<TileInfo>();
            useAbility(SelectionManager.selectedUnit, 0, target.gridPosition, target.grid.isMainGrid);
        }
    }

}
