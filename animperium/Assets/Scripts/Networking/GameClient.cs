using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameClient{

    public NetworkClient netClient;

	public GameClient(string host, int port){
        netClient = new NetworkClient();
        netClient.RegisterHandler(MsgType.Connect, onConnected);
        netClient.RegisterHandler((short)ServerMessage.Types.INIT_GAME, onGameInit);
        netClient.Connect(host, port);
    }

    void onConnected(NetworkMessage netMsg){
        Debug.Log("[CLIENT] connected to server");
    }

    void onGameInit(NetworkMessage netMsg){
        SceneManager.LoadScene(1);
        ServerMessage.InitGameMessage msg = netMsg.ReadMessage<ServerMessage.InitGameMessage>();
        MapLoadData.mapH = msg.mapHeight;
        MapLoadData.mapW = msg.mapWidth;
        MapLoadData.seed = msg.seed;
        Data.playerID = msg.playerID;
    }
}
