using UnityEngine;
using System.Collections;

public delegate void Attack(GameObject heroe, GameObject enemy);
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
        else if (GameObjectFilter.TypeOfGameObject(g) == "Archer")
        {
            attack = ArcherAttack;
            move = Move;
        }
    }
	
    private void SwordfighterAttack(GameObject heroe, GameObject enemy)
    {
        Debug.Log("Attaaaaack!");
        Debug.Log(heroe.GetComponent<Unit>().currentTile.name);
        GameObject[] hexNeighbours = heroe.GetComponent<Unit>().currentTile.GetComponent<TileInfo>().getAdjacent();
        bool neighbour = false;
        
        foreach (GameObject h in hexNeighbours)
        {
            
            if (h == enemy.GetComponent<Unit>().currentTile)
            {
                Debug.Log("true");
                neighbour = true;
            }
        }
        if (neighbour)
        {
            enemy.GetComponent<Unit>().data.health -= 25;
            Debug.Log("dmg dealed");
        }

    }

    private void ArcherAttack(GameObject heroe, GameObject enemy)
    {
        GameObject[] hexNeighbours = enemy.GetComponent<TileInfo>().getAdjacent();
        GameObject[][] hexNeighbours2= new GameObject[hexNeighbours.Length][];

        for (int i = 0; i < hexNeighbours.Length; i++)
        {
            hexNeighbours2[i] = hexNeighbours[i].GetComponent<TileInfo>().getAdjacent();
        }
        bool neighbour = false;
        foreach (GameObject[] gA in hexNeighbours2)
        {
            foreach (GameObject g in gA)
            {
                if (g == enemy)
                {
                    neighbour = true;
                }
            }
        }
        if (neighbour)
        {
            enemy.GetComponent<Unit>().data.health -= 25;
        }
    }

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

    //Hier korrigieren ###################################################################
  



