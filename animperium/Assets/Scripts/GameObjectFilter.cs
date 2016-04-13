using UnityEngine;
using System.Collections;

public class GameObjectFilter {

	// Use this for initialization
	public GameObjectFilter() {
	
	}
	
	public string TypeOfGameObject(GameObject g)
	{
		if(g.name.Contains("Hex"))
			return "Hex";

		return null;
		}
}
