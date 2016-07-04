using UnityEngine;
using System.Collections;
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

    public float maxHitPoints;
    public float hitPoints;

    public float magicResist;
    public float meleeResist;
    public float rangedResist;

    public UnitFootprintType footprintType;

    void Awake(){
        hitPoints = maxHitPoints;
    }

    void OnDestroy(){
        Data.units.Remove(unitID);
        currentTile.GetComponent<TileInfo>().unit = null;
    }

    public GameObject[] getFootprint(TileInfo ti){
        switch (footprintType){
            case UnitFootprintType.DOT:
                return AoeChecks.dot(ti);
            case UnitFootprintType.CIRCLE:
                return AoeChecks.circle(ti);
            default:
                return null;
        }
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
