using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum DamageType{
    MAGIC,
    MELEE,
    RANGED
}

public enum UnitFootprintType{
    DOT,
    CIRCLE
}

public class Unit : MonoBehaviour {
    public string prefabID;

    public GameObject currentTile;
    public int playerID;
    public string unitID;

    public int maxActionPoints = 12;
    [HideInInspector]
    public int actionPoints;

    public float maxHitPoints;
    [HideInInspector]
    public float hitPoints;

    public float magicResist;
    public float meleeResist;
    public float rangedResist;

    public UnitFootprintType footprintType;

    public List<AbilityInfo> abilities = new List<AbilityInfo>();

    Action removeTurnBegin;

    void Awake(){
        removeTurnBegin = TurnManager.onTurnBegin.add<int>(onTurnBegin);
        hitPoints = maxHitPoints;
        actionPoints = maxActionPoints;
    }

    public int addAbility(AbilityInfo ai) {
        abilities.Add(ai);
        return abilities.Count - 1;
    }

    public void attach(TileInfo ti) {
        GameObject[] footprint = getFootprint(ti);
        foreach (GameObject go in footprint) {
            go.GetComponent<TileInfo>().unit = gameObject;
        }
    }

    public void detach() {
        GameObject[] footprint = getFootprint(gameObject.GetComponent<Unit>().currentTile.GetComponent<TileInfo>());
        foreach (GameObject go in footprint) {
            go.GetComponent<TileInfo>().unit = null;
        }
    }

    void onTurnBegin(int turnID) {
        actionPoints = maxActionPoints;
    }

    void OnDestroy(){
        detach();
        Data.units.Remove(unitID);
    }

    public GameObject[] getFootprint(TileInfo ti){
        switch (footprintType){
            case UnitFootprintType.DOT:
                return AoeChecks.dot(ti);
            case UnitFootprintType.CIRCLE:
                return AoeChecks.circle(ti);
        }
        return AoeChecks.dot(ti);
    }

    public void damage(float power, DamageType type){
        float resist;
        switch (type){
            case DamageType.MAGIC:
                resist = magicResist;
                break;
            case DamageType.MELEE:
                resist = meleeResist;
                break;
            case DamageType.RANGED:
                resist = rangedResist;
                break;
            default:
                return;
        }
        float damage = power / resist;
        hitPoints -= damage;
        Debug.Log("[BATTLE SYSTEM] Unit " + unitID + " took " + damage + " damage.");
        if(hitPoints <= 0){
            Debug.Log("[BATTLE SYSTEM] Unit " + unitID + " died.");
            Destroy(gameObject);
        }
    }


}
