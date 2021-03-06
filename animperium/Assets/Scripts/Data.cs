﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Data {
    public static int playerID;
    public static GridManager mainGrid;
    public static GridManager subGrid;
    public static Dictionary<string, GameObject> units = new Dictionary<string, GameObject>();
    public static float wood = 200;
    public static float stone = 200;
    public static float iron = 200;
    public static float gold = 150;
    public static bool isCameraOnMainGrid;

    public static bool isActivePlayer(){
        return Data.playerID % 2 != TurnManager.turnID % 2;
    }
}