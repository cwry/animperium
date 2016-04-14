using UnityEngine;
using System.Collections;

public delegate void Attack(GameObject enemy);
public delegate void Move(GameObject targetHex);

public class UnitFunctions {

    public Attack attack;
    public Move move;
   
    private GameObject gameObject;

	// Use this for initialization
	public UnitFunctions (GameObject g) {
        gameObject = g;
        SetFunctions(gameObject);
	}
	
    private void SetFunctions(GameObject g)
    {
        if(GameObjectFilter.TypeOfGameObject(g) == "Swordfighter")
        {
            attack = SwordfighterAttack;
            move = Move;

        }
    }
	
    private void SwordfighterAttack(GameObject enemy)
    {
        GameObject[] gA = enemy.GetComponent<TileInfo>().getAdjacent();
        bool neighbour = false;
        foreach (GameObject g in gA)
        {
            if(g == enemy)
            {
                neighbour = true;
            }
        }
        if (neighbour)
        {
            enemy.GetComponent<Unit>().data.health -= 25;
        }
    }


    //Hier korrigieren ###################################################################
    private void Move(GameObject targetHex)
    {
        Debug.Log("sollte moven");
        TileInfo info = Data.mainGrid.gridData[12, 10].GetComponent<TileInfo>();
        Vec2i position = info.gridPosition;
        Vec2i target = targetHex.GetComponent<TileInfo>().gridPosition;
        Vec2i[] path = PathFinding.findPath(Data.mainGrid, position.x, position.y, target.x, target.y,
//Hier korrigieren
           (Vec2i hex) => {
               return Data.mainGrid.gridData[hex.x, hex.y].GetComponent<TileInfo>().traversable;
           });
        PathMovement.move(gameObject, Data.mainGrid, path, 3f);
    }


}
