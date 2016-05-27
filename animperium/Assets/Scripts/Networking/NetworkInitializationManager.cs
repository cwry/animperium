using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkInitializationManager : MonoBehaviour {
    void Awake(){
        NetworkData.client.netClient.RegisterHandler((short)ServerMessage.Types.ALL_LOADED, onAllLoaded);
        NetworkData.client.netClient.Send((short)ServerMessage.Types.CLIENT_LOADED, new UnityEngine.Networking.NetworkSystem.EmptyMessage());
    }

    void onAllLoaded(NetworkMessage netMsg){
        Debug.Log("[CLIENT] All Clients Loaded");
        //SpawnManager.spawnUnit(Data.mainGrid, new Vec2i(Data.playerID * 2, Data.playerID * 2), "Archer");
        TurnManager.endTurn();
    }
}
