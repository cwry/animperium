using UnityEngine;
using System.Collections;

public class NetworkLobbyInput : MonoBehaviour {

    bool isConnected = false;
	
	void Update() { 
        if(Input.GetKeyDown(KeyCode.S) && isConnected && NetworkData.server != null){
            NetworkData.server.initGame(100, 100, (int)Random.Range(0, int.MaxValue));
        }
        if (isConnected) return;
        if (Input.GetKeyDown(KeyCode.H)){
            NetworkData.server = new GameServer(7777);
            NetworkData.client = new GameClient("127.0.0.1", 7777);
            isConnected = true;
        } else if (Input.GetKeyDown(KeyCode.J)){
            NetworkData.client = new GameClient("127.0.0.1", 7777);
            isConnected = true;
        }
	}
}
