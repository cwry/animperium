using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnMinesRandomly : MonoBehaviour {

    public GameObject minePrefab;
    public int numMines = 10;

    void Awake() {
        TileInfo[] tiles = gameObject.GetComponentsInChildren<TileInfo>();
        List<TileInfo> validTiles = new List<TileInfo>();
        foreach(TileInfo ti in tiles) {
            if(ti.traversable && ti.unit == null) {
                validTiles.Add(ti);
            }
        }
        System.Random rng = new System.Random(0);
        validTiles = validTiles.OrderBy((x) => {
            return rng.Next();
        }).ToList<TileInfo>();
        if (validTiles.Count < numMines) numMines = validTiles.Count;
        for(int i = 0; i < numMines; i++) {
            GameObject mine = Instantiate(minePrefab, validTiles[i].transform.position, Quaternion.identity) as GameObject;
            validTiles[i].attachUnit(mine);
            Unit u = mine.GetComponent<Unit>();
            u.playerID = 0;
            string unitID = "randomly_spawned_mine_" + i;
            u.unitID = unitID;
            u.attach(validTiles[i]);
            Data.units.Add(unitID, mine);
            u.hide();
        }
    }

}
