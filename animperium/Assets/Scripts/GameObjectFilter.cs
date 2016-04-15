using UnityEngine;
using System.Collections;

public class GameObjectFilter {

	// Use this for initialization
	public GameObjectFilter() {
	
	}
	
	public static string TypeOfGameObject(GameObject g)
	{
        if (g.name.Contains("Hex"))
        {
            return "Hex";
        }
        else if(g.name.Contains("Swordfighter"))
        {
            return "Swordfighter";
        }
        else if(g.name.Contains("Archer"))
        {
            return "Archer";
        }
		return null;
		}
}
