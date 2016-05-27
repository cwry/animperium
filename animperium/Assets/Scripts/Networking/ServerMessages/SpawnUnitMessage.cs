using UnityEngine;
using UnityEngine.Networking;

namespace ServerMessage{
    public class SpawnUnitMessage : MessageBase{
        public int actionID;
        public int tileX;
        public int tileY;
        public bool isMainGrid;
        public string unitType;
        public int playerID;
        public string unitID;
    }
}