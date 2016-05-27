using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class PathMovementManager : MonoBehaviour {
    void Awake(){
        NetworkData.client.netClient.RegisterHandler((short)ServerMessage.Types.MOVE_UNIT, onMoveUnitMessage);
    }

    public static void move(GameObject u, GameObject endTile){
        if (u == null) return;
        Unit unit = u.GetComponent<Unit>();
        if (unit.currentTile != null){
            TileInfo end = endTile.GetComponent<TileInfo>();
            TileInfo start = unit.currentTile.GetComponent<TileInfo>();
            Vec2i[] path = PathFinding.findPath(start.grid, start.gridPosition.x, start.gridPosition.y, end.gridPosition.x, end.gridPosition.y, (Vec2i hx) => {
                return start.grid.gridData[hx.x, hx.y].GetComponent<TileInfo>().traversable;
            });
            if (path != null && path.Length >= 2){
                ServerMessage.MoveUnitMessage msg = new ServerMessage.MoveUnitMessage();
                msg.actionID = ActionQueue.getInstance().actionID++;
                msg.startX = start.gridPosition.x;
                msg.startY = start.gridPosition.y;
                msg.endX = end.gridPosition.x;
                msg.endY = end.gridPosition.y;
                msg.isMainGrid = start.grid.isMainGrid;
                msg.unitID = unit.unitID;
                NetworkData.client.netClient.Send((short)ServerMessage.Types.MOVE_UNIT, msg);
                onMoveUnit(msg);
            }
        }
    }

    static void onMoveUnit(ServerMessage.MoveUnitMessage msg){
        GridManager grid = msg.isMainGrid ? Data.mainGrid : Data.subGrid;
        Vec2i[] path = PathFinding.findPath(grid, msg.startX, msg.startY, msg.endX, msg.endY, (Vec2i hx) => {
            return grid.gridData[hx.x, hx.y].GetComponent<TileInfo>().traversable;
        });
        ActionQueue.getInstance().push(msg.actionID, () => {
            PathMovement.move(Data.units[msg.unitID], grid, path, 3);
        });
    }

    void onMoveUnitMessage(NetworkMessage netMsg){
        ServerMessage.MoveUnitMessage msg = netMsg.ReadMessage<ServerMessage.MoveUnitMessage>();
        onMoveUnit(msg);
    }
}
