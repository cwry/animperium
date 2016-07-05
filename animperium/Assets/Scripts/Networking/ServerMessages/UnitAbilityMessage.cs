﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ServerMessage
{
    public class UnitAbilityMessage : MessageBase{
        public int actionID;
        public int abilityID;
        public string unitID;
        public int targetX;
        public int targetY;
        public bool isTargetMainGrid;
    }
}
