using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TurnManager : MonoBehaviour {

    public int turnID = 0;
    public static GameEvent onTurnBegin = new GameEvent();
    public static GameEvent onTurnEnd = new GameEvent();

    void Awake() {
        NetworkData.client.netClient.RegisterHandler((short)ServerMessage.Types.TURN_ENDED, onTurnEnded);
    }

    void onTurnEnded(NetworkMessage netMsg){
        onTurnEnd.fire(turnID);
        Debug.Log("[TURN MANAGER] turn " + turnID + " ended");
        turnID++;
        onTurnBegin.fire(turnID);
    }

    public static void endTurn(){
        NetworkData.client.netClient.Send((short)ServerMessage.Types.TURN_ENDED, new UnityEngine.Networking.NetworkSystem.EmptyMessage());
    } 
}
