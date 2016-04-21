using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class GameServer{

    bool[] loaded = new bool[2];
    bool allLoaded = false;

    public GameServer(int port){
        NetworkServer.Listen(port);
        NetworkServer.RegisterHandler(MsgType.Connect, onConnected);
        NetworkServer.RegisterHandler(MsgType.Disconnect, onDisconnected);
        NetworkServer.RegisterHandler((short)ServerMessage.Types.MOVE_UNIT, onMoveUnit);
        NetworkServer.RegisterHandler((short)ServerMessage.Types.SPAWN_UNIT, onSpawnUnit);
        NetworkServer.RegisterHandler((short)ServerMessage.Types.CLIENT_LOADED, onClientLoaded);
    }

    void onClientLoaded(NetworkMessage netMsg){
        loaded[NetworkServer.connections.IndexOf(netMsg.conn) - 1] = true;
        if(loaded[0] && loaded[1]){
            NetworkServer.SendToAll((short)ServerMessage.Types.ALL_LOADED, new UnityEngine.Networking.NetworkSystem.EmptyMessage());
        }
    }

    void onSpawnUnit(NetworkMessage netMsg){
        ServerMessage.SpawnUnitMessage msg = netMsg.ReadMessage<ServerMessage.SpawnUnitMessage>();
        msg.unitID = Guid.NewGuid().ToString();
        NetworkServer.SendToAll(netMsg.msgType, msg);
    }

    void onMoveUnit(NetworkMessage netMsg) {
        NetworkServer.SendToAll(netMsg.msgType, netMsg.ReadMessage<ServerMessage.MoveUnitMessage>());
    }

    void onConnected(NetworkMessage netMsg){
        Debug.Log("[SERVER] client connected. total connections: " + (NetworkServer.connections.Count - 1));
    }

    void onDisconnected(NetworkMessage netMsg){
        Debug.Log("[SERVER] client disconnected. total connections: " + (NetworkServer.connections.Count - 1));
    }

    public void initGame(int mapW, int mapH, int seed){
        ServerMessage.InitGameMessage msg = new ServerMessage.InitGameMessage();
        msg.mapWidth = mapW;
        msg.mapHeight = mapH;
        msg.seed = seed;

        if(NetworkServer.connections[1] != null){
            msg.playerID = 1;
            NetworkServer.connections[1].Send((short)ServerMessage.Types.INIT_GAME, msg);
        }

        if (NetworkServer.connections[2] != null){
            msg.playerID = 2;
            NetworkServer.connections[2].Send((short)ServerMessage.Types.INIT_GAME, msg);
        }
    }
}
