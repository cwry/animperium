using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class TeleportMovementManager : MonoBehaviour
{

    void Awake()
    {
        NetworkData.client.netClient.RegisterHandler((short)ServerMessage.Types.TELEPORT_UNIT, onTeleportUnit);
    }

    void Update()
    {
        if (SelectionManager.selectedUnit == null) return;

        GameObject u = SelectionManager.selectedUnit;
        if (u == null) return;
        if (u.GetComponent<Unit>().currentTile == null) return;
        TileInfo start = u.GetComponent<Unit>().currentTile.GetComponent<TileInfo>();
        GridManager grid = (start.grid == Data.mainGrid) ? Data.subGrid : Data.mainGrid;
        TileInfo end = grid.gridData[start.gridPosition.x, start.gridPosition.y].GetComponent<TileInfo>();
        if (Input.GetKeyDown("space"))
        {
            ServerMessage.TeleportUnitMessage msg = new ServerMessage.TeleportUnitMessage();
            msg.startX = start.gridPosition.x;
            msg.startY = start.gridPosition.y;
            msg.endX = end.gridPosition.x;
            msg.endY = end.gridPosition.y;
            msg.isStartMainGrid = start.grid.isMainGrid;
            msg.isEndMainGrid = end.grid.isMainGrid;
            msg.unitID = u.GetComponent<Unit>().unitID;
            NetworkData.client.netClient.Send((short)ServerMessage.Types.TELEPORT_UNIT, msg);
        }

    }

    void onTeleportUnit(NetworkMessage netMsg)
    {
        ServerMessage.TeleportUnitMessage msg = netMsg.ReadMessage<ServerMessage.TeleportUnitMessage>();
        UnitActionQueue.getInstance().push(msg.unitID, (Action done) => {
            GameObject u = Data.units[msg.unitID];
            GridManager startGrid = msg.isStartMainGrid ? Data.mainGrid : Data.subGrid;
            GridManager endGrid = msg.isEndMainGrid ? Data.mainGrid : Data.subGrid;
            Vec2i startPosition = new Vec2i(msg.startX, msg.startY);
            Vec2i endPosition = new Vec2i(msg.endX, msg.endY);

            TeleportMovement.move(u, startGrid, startPosition, endGrid, endPosition);
            done();
        });
    }

}
