using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class TeleportMovementManager : MonoBehaviour{
    void Awake(){
        NetworkData.client.netClient.RegisterHandler((short)ServerMessage.Types.TELEPORT_UNIT, onTeleportUnitMessage);
    }

    void Update()
    {
        if (SelectionManager.selectedUnit == null) return;

        GameObject u = SelectionManager.selectedUnit;
        if (u == null) return;
        Unit unit = u.GetComponent<Unit>();
        if (unit == null || unit.playerID != Data.playerID || unit.currentTile == null) return;

        TileInfo start = unit.currentTile.GetComponent<TileInfo>();
        GridManager grid = (start.grid == Data.mainGrid) ? Data.subGrid : Data.mainGrid;
        TileInfo end = grid.gridData[start.gridPosition.x, start.gridPosition.y].GetComponent<TileInfo>();
        if (Input.GetKeyDown("space")){
            ServerMessage.TeleportUnitMessage msg = new ServerMessage.TeleportUnitMessage();
            msg.actionID = ActionQueue.getInstance().actionID++;
            msg.startX = start.gridPosition.x;
            msg.startY = start.gridPosition.y;
            msg.endX = end.gridPosition.x;
            msg.endY = end.gridPosition.y;
            msg.isStartMainGrid = start.grid.isMainGrid;
            msg.isEndMainGrid = end.grid.isMainGrid;
            msg.unitID = unit.unitID;
            NetworkData.client.netClient.Send((short)ServerMessage.Types.TELEPORT_UNIT, msg);
            onTeleportUnit(msg);
            Camera.main.gameObject.GetComponent<CameraFocus>().CameraJump(end.grid.gridData[end.gridPosition.x, end.gridPosition.y]);
        }
    }

    void onTeleportUnit(ServerMessage.TeleportUnitMessage msg){
        ActionQueue.getInstance().push(msg.actionID, () => {
            GameObject u = Data.units[msg.unitID];
            GridManager startGrid = msg.isStartMainGrid ? Data.mainGrid : Data.subGrid;
            GridManager endGrid = msg.isEndMainGrid ? Data.mainGrid : Data.subGrid;
            Vec2i startPosition = new Vec2i(msg.startX, msg.startY);
            Vec2i endPosition = new Vec2i(msg.endX, msg.endY);

            TeleportMovement.move(u, startGrid, startPosition, endGrid, endPosition);
        });
    }

    void onTeleportUnitMessage(NetworkMessage netMsg){
        ServerMessage.TeleportUnitMessage msg = netMsg.ReadMessage<ServerMessage.TeleportUnitMessage>();
        onTeleportUnit(msg);
    }

}
