using UnityEngine;
using System.Collections;
using System;

public class UnitData {

    public int health;
    public int moveRange;
    public int damage;

    public UnitData(GameObject unit)
    {
        initFieldsByType(unit);
    }

    private void initFieldsByType(GameObject unit)
    {
       if(GameObjectFilter.TypeOfGameObject(unit) == "Swordfighter")
        {
            health = 100;
            moveRange = 3;
            damage = 25;
        }
    }

    
}
