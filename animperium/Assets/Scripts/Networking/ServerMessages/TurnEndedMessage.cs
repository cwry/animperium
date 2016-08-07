using UnityEngine;
using UnityEngine.Networking;

namespace ServerMessage {
    public class TurnEndedMessage : MessageBase {
        public int actionID;
        public int turnID;
    }
}