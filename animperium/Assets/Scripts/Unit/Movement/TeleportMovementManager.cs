using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class TeleportMovementManager : MonoBehaviour{
    void Awake(){
        NetworkData.client.netClient.RegisterHandler((short)ServerMessage.Types.TELEPORT_UNIT, onTeleportUnitMessage);
    }

    static void dig(GameObject unit){
        Unit u = unit.GetComponent<Unit>();
        TileInfo start = u.currentTile.GetComponent<TileInfo>();
        GridManager endGrid = start.grid.isMainGrid ? Data.subGrid : Data.mainGrid;
        TileInfo end = endGrid.gridData[start.gridPosition.x, start.gridPosition.y].GetComponent<TileInfo>();
        ServerMessage.TeleportUnitMessage msg = new ServerMessage.TeleportUnitMessage();
        msg.actionID = ActionQueue.getInstance().actionID++;
        msg.endX = end.gridPosition.x;
        msg.endY = end.gridPosition.y;
        msg.isEndMainGrid = end.grid.isMainGrid;
        msg.unitID = u.unitID;
        NetworkData.client.netClient.Send((short)ServerMessage.Types.TELEPORT_UNIT, msg);
        onTeleportUnit(msg);
        Camera.main.gameObject.GetComponent<CameraFocus>().CameraJump(end.grid.gridData[end.gridPosition.x, end.gridPosition.y]);
    }

    static void onTeleportUnit(ServerMessage.TeleportUnitMessage msg){
        ActionQueue.getInstance().push(msg.actionID, () => {
            GameObject u = Data.units[msg.unitID];
            GridManager endGrid = msg.isEndMainGrid ? Data.mainGrid : Data.subGrid;
            Vec2i endPosition = new Vec2i(msg.endX, msg.endY);

            TeleportMovement.move(u, endGrid, endPosition);
        });
    }

    static void onTeleportUnitMessage(NetworkMessage netMsg){
        ServerMessage.TeleportUnitMessage msg = netMsg.ReadMessage<ServerMessage.TeleportUnitMessage>();
        onTeleportUnit(msg);
    }

}
