using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum UndergroundTileState {
    BLOCKED, REVEALED
}

public enum UndergroundTileAppearanceState {
    BLOCKED = UndergroundTileState.BLOCKED,
    REVEALED = UndergroundTileState.REVEALED,
    HIDDEN
}

public class UndergroundTile : MonoBehaviour {
    public UndergroundTileState state = UndergroundTileState.BLOCKED;
    public UndergroundTileAppearanceState appearance = UndergroundTileAppearanceState.HIDDEN;
    public Dictionary<GameObject, GameObject[]> sightRanges = new Dictionary<GameObject, GameObject[]>();
    GameObject currentObject;

    void Awake(){
        updateAppearance();
    }

    public bool isInSight() {
        if (sightRanges.Count == 0) return false;
        return true;
        /*List<KeyValuePair<GameObject, List<GameObject>>> removals = new List<KeyValuePair<GameObject, List<GameObject>>>();
        bool returnVal = false;
        foreach (KeyValuePair<GameObject, List<GameObject>> entry in sightRanges) {
            if(entry.Key == null || entry.Key.Equals(null)) {
                removals.Add(entry);
            }else {
                returnVal = true;
            }
        }
        //remove on all tiles?
        foreach (KeyValuePair<GameObject, List<GameObject>> entry in removals) {
            sightRanges.Remove(entry.Key);
        }
        return returnVal;*/
    }

    public void addSightRange(GameObject go, GameObject[] tiles = null) {
        if (tiles == null) {
            Unit u = go.GetComponent<Unit>();
            if (u.playerID != Data.playerID) return;
            TileInfo tileInfo = gameObject.GetComponent<TileInfo>();
            tiles = tileInfo.listTree(0, u.undergroundSightRange, (TileInfo ti) => {
                return ti.gameObject.GetComponent<UndergroundTile>().state == UndergroundTileState.REVEALED;
            });
            foreach(GameObject tile in tiles) {
                tile.GetComponent<UndergroundTile>().addSightRange(go, tiles);
            }
            return;
        }
        sightRanges.Add(go, tiles);
        updateAppearance();
    }

    public void removeSightRange(GameObject go, bool recursiveCall = false) {
        if (!recursiveCall && sightRanges.ContainsKey(go)) {
            foreach(GameObject tile in sightRanges[go]) {
                tile.GetComponent<UndergroundTile>().removeSightRange(go, true);
            }
            return;
        }
        sightRanges.Remove(go);
        updateAppearance();
    }

    public void updateSightRange(GameObject go = null){
        if (go == null) {
            List<Action> updates = new List<Action>();
            foreach (KeyValuePair<GameObject, GameObject[]> entry in sightRanges) {
                updates.Add(() => {
                    UndergroundTile ut = entry.Value[0].GetComponent<UndergroundTile>();
                    ut.updateSightRange(entry.Key);
                });
            }
            foreach(Action update in updates) {
                update();
            }
            return;
        }
        removeSightRange(go);
        addSightRange(go);
    }

    public void updateAppearance() {
        TileInfo ti = gameObject.GetComponent<TileInfo>();
        if (!ti.traversable) return;
        if (currentObject != null) Destroy(currentObject);
        bool inSight = isInSight();
        if (inSight) {
            appearance = (UndergroundTileAppearanceState)state;
            ti.revealHole();
        }
        if(ti.unit != null) {
            Unit u = ti.unit.GetComponent<Unit>();
            if (u.playerID != Data.playerID) {
                if (inSight && state == UndergroundTileState.REVEALED) {
                    u.reveal();
                }else {
                    u.hide();
                }
            }
        }
        GameObject prefab = UndergroundManager.getAppearancePrefab(appearance, inSight);
        if (prefab != null) currentObject = (GameObject)Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
