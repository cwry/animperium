using UnityEngine;
using System.Collections;
using System;

public enum DamageType{
    MAGIC,
    MELEE,
    RANGED
}

public class Unit : MonoBehaviour {
    public GameObject currentTile;
    public int playerID;
    public string unitID;

    public int maxMovementPoints;
    public int movementPoints;

    public float magicResist;
    public float meleeResist;
    public float rangedResist;
    
    Action removeTurnBegin;

    void Awake(){
        onTurnBegin(-1);
        removeTurnBegin = TurnManager.onTurnBegin.add<int>(onTurnBegin);
    }

    private void onTurnBegin(int turnID){
        movementPoints = maxMovementPoints;
    }

    private void damage(float power, DamageType type){
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
    }
}
