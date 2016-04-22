using UnityEngine;
using UnityEngine.Networking;

namespace ServerMessage
{
    public class MoveUnitMessage : MessageBase
    {
        public string unitID;
        public int startX;
        public int startY;
        public int endX;
        public int endY;
        public bool isMainGrid;
    }
}