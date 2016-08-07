using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TurnManager : MonoBehaviour {

    public static int turnID = 0;
    public static GameEvent onTurnBegin = new GameEvent();
    public static GameEvent onTurnEnd = new GameEvent();

    void Awake() {
        NetworkData.client.netClient.RegisterHandler((short)ServerMessage.Types.TURN_ENDED, onTurnEndedMessage);
    }

    void onTurnEndedMessage(NetworkMessage netMsg) {
        ServerMessage.TurnEndedMessage msg = netMsg.ReadMessage<ServerMessage.TurnEndedMessage>();
        onTurnEnded(msg);
    }

    static void onTurnEnded(ServerMessage.TurnEndedMessage msg){
        ActionQueue.getInstance().push(msg.actionID, () => {
            onTurnEnd.fire(turnID);
            Debug.Log("[TURN MANAGER] turn " + turnID + " ended");
            turnID++;
            onTurnBegin.fire(turnID);
        });
    }

    public static void endTurn(){
        ServerMessage.TurnEndedMessage msg = new ServerMessage.TurnEndedMessage();
        msg.actionID = ActionQueue.getInstance().actionID++;
        msg.turnID = turnID;
        NetworkData.client.netClient.Send((short)ServerMessage.Types.TURN_ENDED, msg);
        onTurnEnded(msg);
    } 

    public static void init(){
        Debug.Log("[TURN MANAGER] started");
        onTurnBegin.fire(0);
    }
}
