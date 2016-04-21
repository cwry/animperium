using UnityEngine;
using UnityEngine.Networking;

namespace ServerMessage{
    public class InitGameMessage : MessageBase{
        public int mapWidth;
        public int mapHeight;
        public int seed;
        public int playerID;
    }
}