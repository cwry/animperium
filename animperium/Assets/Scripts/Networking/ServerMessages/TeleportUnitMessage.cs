using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ServerMessage{
    public class TeleportUnitMessage : MessageBase{
        public int actionID;
        public int endX;
        public int endY;
        public bool isEndMainGrid;
        public string unitID;
    }
}
