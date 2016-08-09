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
    CIRCLE,
    BIGCIRCLE
}

public enum UnitType{
    UNDEFINED,
    UNIT,
    BUILDING
}

public class Unit : MonoBehaviour {
    public bool hidden = false;
    public UnitType type;
    public string prefabID;
    public Sprite icon;

    public GameObject currentTile;
    public int playerID;
    public string unitID;

    public int undergroundSightRange = 4;
    public int maxActionPoints = 12;
    [HideInInspector]
    public int actionPoints;

    public float maxHitPoints;
    [HideInInspector]
    public float hitPoints;

    public float magicResist;
    public float meleeResist;
    public float rangedResist;


    [HideInInspector]
    public float magicResistBuff = 0;
    [HideInInspector]
    public float meleeResistBuff = 0;
    [HideInInspector]
    public float rangedResistBuff = 0;
    [HideInInspector]
    private int resistBuffEndTurn;

    [HideInInspector]
    public float attackMultiplier = 1f;

    public UnitFootprintType footprintType;

    [HideInInspector]
    public List<AbilityInfo> abilities = new List<AbilityInfo>();

    public GameEvent onUseAbility = new GameEvent();

    Action removeTurnBegin;

    void Awake(){
        if (hidden) {
            foreach (Renderer render in gameObject.GetComponentsInChildren<Renderer>()) {
                render.enabled = false;
            }
        }
        removeTurnBegin = TurnManager.onTurnBegin.add<int>(onTurnBegin);
        hitPoints = maxHitPoints;
        actionPoints = maxActionPoints;
    }

    public int addAbility(AbilityInfo ai) {
        ai.checkCost = () => {
            if (
            ai.apCost > actionPoints ||
            ai.woodCost > Data.wood + Data.gold ||
            ai.ironCost > Data.iron + Data.gold ||
            ai.stoneCost > Data.stone + Data.gold
            ){
                return false;
            }
            return true;
        };

        Action<ServerMessage.UnitAbilityMessage> onExecution = ai.onExecution;
        Action<ServerMessage.UnitAbilityMessage> handleCost = (ServerMessage.UnitAbilityMessage msg) => {
            actionPoints -= ai.apCost;
            if (playerID != Data.playerID) return;
            Data.wood -= ai.woodCost;
            if (Data.wood < 0) {
                Data.gold += Data.wood;
                Data.wood = 0;
            }
            Data.iron -= ai.ironCost;
            if (Data.iron < 0) {
                Data.gold += Data.iron;
                Data.iron = 0;
            }
            Data.stone -= ai.stoneCost;
            if (Data.stone < 0) {
                Data.gold += Data.stone;
                Data.stone = 0;
            }
        };

        Action<ServerMessage.UnitAbilityMessage> handleParticles = (ServerMessage.UnitAbilityMessage msg) => {
            if (ai.targetParticle != null) {
                GridManager grid = msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid;
                TileInfo target = grid.gridData[msg.targetX, msg.targetY].GetComponent<TileInfo>();
                GameObject[] aoe = ai.checkAoe(target);
                foreach (GameObject go in aoe) {
                    Instantiate(ai.targetParticle, go.transform.position, Quaternion.identity);
                }
            }
        };

        Action<ServerMessage.UnitAbilityMessage> executionAction = (ServerMessage.UnitAbilityMessage msg) => {
            handleCost(msg);
            handleParticles(msg);
            onExecution(msg);
        };

        ai.onExecution = executionAction;

        abilities.Add(ai);
        return abilities.Count - 1;
    }

    public void attach(TileInfo ti) {
        GameObject[] footprint = getFootprint(ti);
        foreach (GameObject go in footprint) {
            go.GetComponent<TileInfo>().unit = gameObject;
        }
    }

    public float getHPPercentage() {
        return hitPoints / maxHitPoints; 
    }

    public void detach() {
        GameObject[] footprint = getFootprint(gameObject.GetComponent<Unit>().currentTile.GetComponent<TileInfo>());
        foreach (GameObject go in footprint) {
            TileInfo ti = go.GetComponent<TileInfo>();
            ti.unit = null;
        }
    }

    void onTurnBegin(int turnID) {
        actionPoints = maxActionPoints;
        resetResistBuff(turnID);
    }

    void OnDestroy(){
        UndergroundTile ut = currentTile.GetComponent<UndergroundTile>();
        if (ut != null) ut.removeSightRange(gameObject);
        detach();
        Data.units.Remove(unitID);

    }

    public GameObject[] getFootprint(TileInfo ti){
        switch (footprintType){
            case UnitFootprintType.DOT:
                return AoeChecks.dot(ti);
            case UnitFootprintType.CIRCLE:
                return AoeChecks.circle(ti);
            case UnitFootprintType.BIGCIRCLE:
                return AoeChecks.getCircle(2, 0)(ti);
        }
        return AoeChecks.dot(ti);
    }

    public void damage(float multiplier, float power, DamageType type){
        float resist;
        switch (type){
            case DamageType.MAGIC:
                resist = magicResist + magicResistBuff;
                break;
            case DamageType.MELEE:
                resist = meleeResist + meleeResistBuff;
                break;
            case DamageType.RANGED:
                resist = rangedResist + rangedResistBuff;
                break;
            default:
                return;
        }
        float damage = power / resist * multiplier;
        hitPoints -= damage;
        Debug.Log("[BATTLE SYSTEM] Unit " + unitID + " took " + damage + " damage.");
        if(hitPoints <= 0){
            Debug.Log("[BATTLE SYSTEM] Unit " + unitID + " died.");
            Destroy(gameObject);
        }
    }

    void resetResistBuff(int turnID){
        if(turnID == resistBuffEndTurn) {
            meleeResistBuff = 0;
            rangedResistBuff = 0;
            magicResistBuff = 0;
        }
    }

    public void buffResistance(float melee, float ranged, float magic) {
        if(meleeResistBuff < melee) meleeResistBuff = melee;
        if(rangedResistBuff < ranged) rangedResistBuff = ranged;
        if(magicResistBuff < magic) magicResistBuff = magic;
        resistBuffEndTurn = TurnManager.turnID + 2;
    }

    public void hide() {
        if (hidden) return;
        hidden = true;
        foreach(Renderer render in gameObject.GetComponentsInChildren<Renderer>()) {
            render.enabled = false;
        }
    }

    public void reveal() {
        if (!hidden) return;
        hidden = false;
        foreach (Renderer render in gameObject.GetComponentsInChildren<Renderer>()) {
            render.enabled = true;
        }
    }
}
