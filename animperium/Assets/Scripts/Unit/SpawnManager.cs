using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

[Serializable]
public struct NamedPrefab{
    public string name;
    public GameObject prefab;
}

public class SpawnManager : MonoBehaviour {
    public NamedPrefab[] namedPrefabs;
    static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();


    void Awake () {
        foreach(NamedPrefab nf in namedPrefabs){
            prefabs.Add(nf.name, nf.prefab);
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
            ti.unit = obj;
            Unit u = obj.GetComponent<Unit>();
            u.playerID = msg.playerID;
            u.unitID = msg.unitID;
            u.currentTile = tile;
            Data.units.Add(msg.unitID, obj);
        });
    }

    public static void spawnUnit(GridManager grid, Vec2i pos, string unitType){
        ServerMessage.SpawnUnitMessage msg = new ServerMessage.SpawnUnitMessage();
        msg.actionID = msg.actionID = ActionQueue.getInstance().actionID++;
        msg.isMainGrid = grid.isMainGrid;
        msg.tileX = pos.x;
        msg.tileY = pos.y;
        msg.unitType = unitType;
        msg.playerID = Data.playerID;

        NetworkData.client.netClient.Send((short)ServerMessage.Types.SPAWN_UNIT, msg);
        onSpawnUnit(msg);
    }
}
