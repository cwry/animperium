using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ServerMessage{
    public class TeleportUnitMessage : MessageBase{
        public int actionID;
        public int endX;
        public int endY;
        public int startX;
        public int startY;
        public bool isEndMainGrid;
        public bool isStartMainGrid;
        public string unitID;
    }
}
