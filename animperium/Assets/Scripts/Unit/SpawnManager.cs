﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

public class SpawnManager : MonoBehaviour {
    public GameObject[] spawnablePrefabs;
    static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

    static Dictionary<int, Action> callbacks = new Dictionary<int, Action>();


    void Awake () {
        foreach(GameObject nf in spawnablePrefabs){
            prefabs.Add(nf.GetComponent<Unit>().prefabID, nf);
        }
        NetworkData.client.netClient.RegisterHandler((short)ServerMessage.Types.SPAWN_UNIT, onSpawnUnitMessage);
    }

    void onSpawnUnitMessage(NetworkMessage netMsg){
        ServerMessage.SpawnUnitMessage msg = netMsg.ReadMessage<ServerMessage.SpawnUnitMessage>();
        onSpawnUnit(msg);
    }

    static void onSpawnUnit(ServerMessage.SpawnUnitMessage msg){
        if (!prefabs.ContainsKey(msg.unitType)){
            Debug.LogError("[SPAWN MANAGER] prefab with name " + msg.unitType + " not found");
            return;
        }
        GridManager grid = msg.isMainGrid ? Data.mainGrid : Data.subGrid;
        GameObject tile = grid.gridData[msg.tileX, msg.tileY];
        GameObject prefab = prefabs[msg.unitType];
        ActionQueue.getInstance().push(msg.actionID, () => {
            GameObject obj = Instantiate(prefab);
            obj.transform.position = tile.transform.position;
            TileInfo ti = tile.GetComponent<TileInfo>();
            ti.attachUnit(obj);
            Unit u = obj.GetComponent<Unit>();
            u.playerID = msg.playerID;
            u.unitID = msg.unitID;
            u.attach(ti);
            if(msg.ap >= 0) u.actionPoints = msg.ap;
            if(msg.hpPercentage > 0) u.hitPoints = u.maxHitPoints * msg.hpPercentage;
            MovementAbility ma = u.gameObject.GetComponent<MovementAbility>();
            if (msg.mp >= 0 && ma != null) ma.movementPoints = msg.mp;
            Data.units.Add(msg.unitID, obj);
            if (callbacks[msg.actionID] != null) callbacks[msg.actionID]();
            callbacks.Remove(msg.actionID);
        });
    }

    public static void spawnUnit(GridManager grid, Vec2i pos, string prefabID, int ap, int mp, float hpPercentage, Action callback = null){
        if (!Data.isActivePlayer()) {
            Debug.Log("something horrible happened....");
            callback();
            return;
        }
        ServerMessage.SpawnUnitMessage msg = new ServerMessage.SpawnUnitMessage();
        msg.ap = ap;
        msg.mp = mp;
        msg.hpPercentage = hpPercentage;
        msg.actionID = ActionQueue.getInstance().actionID++;
        msg.isMainGrid = grid.isMainGrid;
        msg.tileX = pos.x;
        msg.tileY = pos.y;
        msg.unitType = prefabID;
        msg.playerID = Data.playerID;
        callbacks.Add(msg.actionID, callback);

        NetworkData.client.netClient.Send((short)ServerMessage.Types.SPAWN_UNIT, msg);
    }
}
